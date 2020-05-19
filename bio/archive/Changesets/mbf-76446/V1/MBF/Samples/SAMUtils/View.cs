// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CommandLine;
using MBF;
using MBF.IO.BAM;
using MBF.IO.SAM;
using SAMUtils.Properties;

namespace SAMUtils
{
    public class View
    {
        #region Public Fields

        /// <summary>
        /// Output in BAM format.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Output BAM", ShortName = "b")]
        public bool BAMOutput;

        /// <summary>
        /// Print header with alignment.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Print header for the SAM output", ShortName = "h")]
        public bool Header;

        /// <summary>
        /// Print only header.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Print header only (no alignments)", ShortName = "H")]
        public bool HeaderOnly;

        /// <summary>
        /// Input file is in SAM format.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Input is SAM format", ShortName = "S")]
        public bool SAMInput;

        /// <summary>
        /// Display uncompressed Bam file.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Uncompressed BAM output", ShortName = "u")]
        public bool UnCompressedBAM;

        /// <summary>
        /// Display flag in HEX.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Output FLAG in HEX", ShortName = "x")]
        public bool FlagInHex;

        /// <summary>
        /// Display flag as string.
        /// </summary>   
        [Argument(ArgumentType.AtMostOnce, HelpText = "Output FLAG in string", ShortName = "X")]
        public bool FlagAsString;

        /// <summary>
        /// Path of file containing reference name and length in tab delimited format.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "List of reference names and lengths in a" +
        "tab limited file rest all field will be ignored.", ShortName = "t")]
        public string ReferenceNamesAndLength;

        /// <summary>
        /// Path of file containing reference sequence.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Reference sequence file", ShortName = "T")]
        public string ReferenceSequenceFile;

        /// <summary>
        /// Path of output file.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Output file path", ShortName = "o")]
        public string OutputFilePath;

        /// <summary>
        /// Only output alignments with all bits in INT present in the FLAG field. 
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Required flag", ShortName = "f")]
        public int FlagRequired;

        /// <summary>
        /// Skip alignments with bits present in INT.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Filtering flag", ShortName = "F")]
        public int FilteringFlag;

        /// <summary>
        /// Skip alignments with MAPQ smaller than INT.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Minimum mapping quality", ShortName = "q")]
        public int QualityMinimumMapping;

        /// <summary>
        /// Only output reads in library STR.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Only output reads in library", ShortName = "l")]
        public string Library;

        /// <summary>
        /// Only output reads in read group STR.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Only output reads in read group", ShortName = "r")]
        public string ReadGroup;

        ///<summary>
        ///If no region is specified, all the alignments will be printed; 
        ///otherwise only alignments overlapping the specified regions will be output. 
        ///</summary>
        [Argument(ArgumentType.AtMostOnce,
            HelpText = "A region can be presented, for example, in the following format:\n" +
                        "          ‘chr2’ (the whole chr2),\n" +
                        "          ‘chr2:1000000’ (region starting from 1,000,000bp)\n" +
                        "          or ‘chr2:1,000,000-2,000,000’\n" +
                        "          (region between 1,000,000 and 2,000,000bp including the end points).\n" +
                        "          The coordinate is 1-based.\n", ShortName = "R")]
        public string Region;

        /// <summary>
        /// Path of input file.
        /// </summary>
        [DefaultArgument(ArgumentType.AtMostOnce, HelpText = "Input SAM/BAM file path")]
        public string InputFilePath;

        /// <summary>
        /// Usage.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce)]
        public bool Usage;

        #endregion

        #region Private Fields

        /// <summary>
        /// Stores Region Variable.
        /// </summary>
        private Region _region;

        /// <summary>
        /// Sequence Alignment Map object after parsing file.
        /// </summary>
        private SequenceAlignmentMap _seqAlignmentMap;

        /// <summary>
        /// Stores information about RG present in header used for storing library information of the reads.
        /// </summary>
        private List<SAMRecordField> _rgRecFields;

        /// <summary>
        /// Writes to Console or File based on user option.
        /// </summary>
        private TextWriter _write;

        #endregion

        #region Public Methods

        /// <summary>
        /// Extract/print all or sub alignments in SAM or BAM format.
        /// By default, this command assumes the file on the command line is in
        /// BAM format and it prints the alignments in SAM.
        /// SAMUtil.exe view in.bam
        /// </summary>
        public void ViewResult()
        {
            if (string.IsNullOrEmpty(InputFilePath))
            {
                throw new InvalidOperationException("Input File Not specified");
            }

            if (!string.IsNullOrEmpty(Region))
            {
                StringToRegionConverter();
            }

            DoParse();
            if (_seqAlignmentMap.Header == null)
            {
                throw new InvalidOperationException("SAM file doesn't contian header");
            }

            if (!string.IsNullOrEmpty(Library))
            {
                _rgRecFields = _seqAlignmentMap.Header.RecordFields.Where(R => R.Typecode.ToUpper().Equals("RG")).ToList();
            }

            InitializeOutputStream();
            if (UnCompressedBAM)
            {
                //Displays Uncompressed BAM.
                DisplayUncompressedBAM();
            }
            else if (BAMOutput)
            {
                //Displays BAM file.
                DisplayBAMFile();
            }
            else
            {
                //Displays SAM as output.
                DisplaySAMOutput();
            }

            _write.Close();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Display OUtput in SAM format.
        /// </summary>
        public void DisplaySAMOutput()
        {
            if (HeaderOnly)
            {
                DisplayHeader();
            }
            else
            {
                if (Header)
                {
                    DisplayHeader();
                }
                foreach (SAMAlignedSequence alignedSequence in _seqAlignmentMap.QuerySequences)
                {
                    if (Filter(alignedSequence))
                    {
                        DisplaySeqAlignments(alignedSequence);
                    }
                }
            }
        }

        /// <summary>
        /// Write BAM file.
        /// </summary>
        private void DisplayBAMFile()
        {
            BAMFormatter format = new BAMFormatter();
            string tempFilename = Path.GetTempFileName();
            string tempFilename1 = Path.GetTempFileName();

            if (HeaderOnly)
            {
                using (FileStream fstemp = new FileStream(tempFilename, FileMode.Create, FileAccess.ReadWrite))
                {
                    format.WriteHeader(_seqAlignmentMap.Header, fstemp);
                    using (FileStream fstemp1 = new FileStream(tempFilename1, FileMode.Create, FileAccess.ReadWrite))
                    {
                        format.CompressBAMFile(fstemp, fstemp1);
                        fstemp1.Seek(0, SeekOrigin.Begin);
                        byte[] bytes = new byte[fstemp1.Length];
                        fstemp1.Read(bytes, 0, (int)fstemp1.Length);
                        string str = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                        _write.Write(str);
                        _write.Flush();
                    }
                }

                File.Delete(tempFilename1);
                File.Delete(tempFilename);
            }
            else
            {
                using (FileStream fstemp = new FileStream(tempFilename, FileMode.Create, FileAccess.ReadWrite))
                {
                    if (Header)
                    {
                        format.WriteHeader(_seqAlignmentMap.Header, fstemp);
                    }

                    foreach (SAMAlignedSequence alignedSequence in _seqAlignmentMap.QuerySequences)
                    {
                        if (Filter(alignedSequence))
                        {
                            format.WriteAlignedSequence(_seqAlignmentMap.Header, alignedSequence, fstemp);
                        }
                    }

                    using (FileStream fstemp1 = new FileStream(tempFilename1, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fstemp.Seek(0, SeekOrigin.Begin);
                        format.CompressBAMFile(fstemp, fstemp1);
                        fstemp1.Seek(0, SeekOrigin.Begin);
                        byte[] bytes = new byte[fstemp1.Length];
                        fstemp1.Read(bytes, 0, (int)fstemp1.Length);
                        string str = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                        _write.Write(str);
                        _write.Flush();
                    }
                }

                File.Delete(tempFilename);
                File.Delete(tempFilename1);
            }
        }

        /// <summary>
        /// Writes Uncompressed BAM file.
        /// </summary>
        private void DisplayUncompressedBAM()
        {
            BAMFormatter format = new BAMFormatter();
            if (HeaderOnly)
            {
                using (MemoryStream mstemp = new MemoryStream())
                {
                    format.WriteHeader(_seqAlignmentMap.Header, mstemp);
                    mstemp.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = new byte[mstemp.Length];
                    mstemp.Read(bytes, 0, (int)mstemp.Length);
                    string str = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                    _write.Write(str);
                    _write.Flush();
                }
            }
            else
            {
                using (MemoryStream mstemp = new MemoryStream())
                {
                    long length;
                    if (Header)
                    {
                        format.WriteHeader(_seqAlignmentMap.Header, mstemp);
                        mstemp.Seek(0, SeekOrigin.Begin);
                        byte[] bytes = new byte[mstemp.Length];
                        mstemp.Read(bytes, 0, (int)mstemp.Length);
                        mstemp.Seek(0, SeekOrigin.Begin);
                        string str = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                        _write.Write(str);
                        _write.Flush();
                    }

                    foreach (SAMAlignedSequence alignedSequence in _seqAlignmentMap.QuerySequences)
                    {
                        if (Filter(alignedSequence))
                        {
                            format.WriteAlignedSequence(_seqAlignmentMap.Header, alignedSequence, mstemp);
                            length = mstemp.Position;
                            mstemp.Seek(0, SeekOrigin.Begin);
                            byte[] bytes = new byte[length];
                            mstemp.Read(bytes, 0, (int)length);
                            mstemp.Seek(0, SeekOrigin.Begin);
                            string str = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                            _write.Write(str);
                            _write.Flush();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Creates stream to write based on user option.
        /// </summary>
        private void InitializeOutputStream()
        {
            if (string.IsNullOrEmpty(OutputFilePath))
            {
                _write = Console.Out;
            }
            else
            {
                _write = new StreamWriter(OutputFilePath);
            }
        }

        /// <summary>
        /// Converts Flag to string.
        /// In a string FLAG, each character represents one bit with
        ///p=0x1 (paired), P=0x2 (properly paired), u=0x4 (unmapped),
        ///U=0x8 (mate unmapped), r=0x10 (reverse), R=0x20 (mate reverse)
        ///1=0x40 (first), 2=0x80 (second), s=0x100 (not primary),
        ///f=0x200 (failure) and d=0x400 (duplicate). 
        /// </summary>
        /// <param name="flag">Sequence Flag.</param>
        /// <returns>String of flag.</returns>
        private string GetFlagDesc(SAMFlags flag)
        {
            string str = string.Empty;
            if ((flag & SAMFlags.PairedRead) == SAMFlags.PairedRead)
            {
                str = str + "p";
            }


            if ((flag & SAMFlags.MappedInPair) == SAMFlags.MappedInPair)
            {
                str = str + "P";
            }

            if ((flag & SAMFlags.UnmappedQuery) == SAMFlags.UnmappedQuery)
            {
                str = str + "u";
            }

            if ((flag & SAMFlags.UnmappedMate) == SAMFlags.UnmappedMate)
            {
                str = str + "U";
            }

            if ((flag & SAMFlags.QueryStrand) == SAMFlags.QueryStrand)
            {
                str = str + "r";
            }

            if ((flag & SAMFlags.MateStrand) == SAMFlags.MateStrand)
            {
                str = str + "R";
            }

            if ((flag & SAMFlags.FirstReadInPair) == SAMFlags.FirstReadInPair)
            {
                str = str + "1";
            }

            if ((flag & SAMFlags.SecondReadInPair) == SAMFlags.SecondReadInPair)
            {
                str = str + "2";
            }

            if ((flag & SAMFlags.NonPrimeAlignment) == SAMFlags.NonPrimeAlignment)
            {
                str = str + "s";
            }

            if ((flag & SAMFlags.QualityCheckFailure)
                == SAMFlags.QualityCheckFailure)
            {
                str = str + "f";
            }

            if ((flag & SAMFlags.Duplicate) == SAMFlags.Duplicate)
            {
                str = str + "d";
            }

            return str;
        }

        /// <summary>
        /// Filters Sequence based on user inputs.
        /// </summary>
        /// <param name="alignedSequence">Aligned Sequence.</param>
        /// <returns>Whether aligned sequence matches user defined options.</returns>
        private bool Filter(SAMAlignedSequence alignedSequence)
        {
            bool filter = true;
            if (filter && FlagRequired != 0)
            {
                filter = (((int)alignedSequence.Flag) & FlagRequired) == FlagRequired;
            }

            if (filter && FilteringFlag != 0)
            {
                filter = ((((int)alignedSequence.Flag) & FilteringFlag) == 0);
            }

            if (filter && QualityMinimumMapping != 0)
            {
                filter = alignedSequence.MapQ == QualityMinimumMapping;
            }

            if (filter && !string.IsNullOrEmpty(Library))
            {
                filter = _rgRecFields.First(
                        a => a.Tags.First(
                        b => b.Tag.Equals("ID")).Value.Equals(alignedSequence.OptionalFields.First(
                        c => c.Tag.Equals("RG")).Value)).Tags.First(
                        d => d.Tag.Equals("LB")).Value.Equals(Library);
            }

            if (filter && !string.IsNullOrEmpty(ReadGroup))
            {
                filter = alignedSequence.OptionalFields.AsParallel().Where(
                   O => O.Tag.ToUpper().Equals("RG")).ToList().Any(a => a.Value.Equals(ReadGroup));
            }

            if (filter && !string.IsNullOrEmpty(Region))
            {
                if (alignedSequence.RName.Equals(_region.Chromosome))
                {
                    if (_region.Start > -1)
                    {
                        if (alignedSequence.Pos >= _region.Start)
                        {
                            if (_region.End > -1)
                            {
                                if (alignedSequence.Pos <= _region.End)
                                {
                                    filter = true;
                                }
                                else
                                {
                                    filter = false;
                                }
                            }
                            else
                            {
                                filter = true;
                            }
                        }
                        else
                        {
                            filter = false;
                        }
                    }
                    else
                    {
                        filter = true;
                    }
                }
                else
                {
                    filter = false;
                }
            }

            return filter;
        }

        /// <summary>
        /// Displays the Aligned sequence
        /// </summary>
        private void DisplaySeqAlignments(SAMAlignedSequence alignedSequence, FileStream stream = null)
        {
            // Get Aligned sequences
            _write.Write("\n");
            string seq = "*";
            if (alignedSequence.QuerySequence.Count > 0)
            {
                seq = alignedSequence.QuerySequence.ToString();
            }

            string qualValues = "*";

            QualitativeSequence qualSeq = alignedSequence.QuerySequence as QualitativeSequence;
            if (qualSeq != null)
            {
                byte[] bytes = qualSeq.Scores;
                qualValues = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            }

            string flag = string.Empty;
            if (FlagInHex)
            {
                flag = String.Format("0x" + "{0:x2}", (int)alignedSequence.Flag);
            }
            else if (FlagAsString)
            {
                flag = GetFlagDesc(alignedSequence.Flag);
            }
            else
            {
                flag = ((int)alignedSequence.Flag).ToString();
            }

            _write.Write("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}",
            alignedSequence.QName, flag, alignedSequence.RName,
            alignedSequence.Pos, alignedSequence.MapQ, alignedSequence.CIGAR,
            alignedSequence.MRNM.Equals(alignedSequence.RName) ? "=" : alignedSequence.MRNM,
            alignedSequence.MPos, alignedSequence.ISize, seq, qualValues);

            for (int j = 0; j < alignedSequence.OptionalFields.Count; j++)
            {
                _write.Write("\t{0}:{1}:{2}", alignedSequence.OptionalFields[j].Tag,
                    alignedSequence.OptionalFields[j].VType, alignedSequence.OptionalFields[j].Value);
            }
        }

        /// <summary>
        /// Parse SAM or BAM file based on user input.
        /// </summary>
        private void DoParse()
        {
            if (!SAMInput)
            {
                BAMParser parse = new BAMParser();
                parse.EnforceDataVirtualization = true;
                try
                {
                    _seqAlignmentMap = parse.Parse(InputFilePath);
                }
                catch
                {
                    throw new InvalidOperationException(Resources.InvalidBAMFile);
                }
            }
            else
            {
                SAMParser parse = new SAMParser();
                parse.EnforceDataVirtualization = true;
                try
                {
                    _seqAlignmentMap = parse.Parse(InputFilePath);
                }
                catch
                {
                    throw new InvalidOperationException(Resources.InvalidSAMFile);
                }
            }
        }

        /// <summary>
        /// Dispalys the headers present in the BAM file
        /// </summary>
        /// <param name="seqAlignmentMap">SeqAlignment map</param>
        private void DisplayHeader()
        {
            // Get Header
            SAMAlignmentHeader header = _seqAlignmentMap.Header;
            IList<SAMRecordField> recordField = header.RecordFields;
            IList<string> commenstList = header.Comments;

            if (recordField.Count > 0)
            {
                // Read Header Lines
                for (int i = 0; i < recordField.Count; i++)
                {
                    _write.Write("\n@{0}", recordField[i].Typecode);
                    _write.Flush();
                    for (int tags = 0; tags < recordField[i].Tags.Count; tags++)
                    {
                        _write.Write("\t{0}:{1}", recordField[i].Tags[tags].Tag,
                            recordField[i].Tags[tags].Value);
                        _write.Flush();
                    }
                }
            }

            // Displays the comments if any
            if (commenstList.Count > 0)
            {
                for (int i = 0; i < commenstList.Count; i++)
                {
                    _write.Write("\n@CO\t{0}\n", commenstList[i].ToString());
                    _write.Flush();
                }
            }
        }

        /// <summary>
        /// Converts region passed as command line argument to Region strucure.
        /// </summary>
        /// <param name="region">String passed as command line argument.</param>
        /// <returns>Region structure.</returns>
        private void StringToRegionConverter()
        {
            string[] splitRegion = Region.Split(new char[] { ':', '-' });
            if (splitRegion.Length == 1)
            {
                _region = new Region()
                {
                    Chromosome = splitRegion[0],
                    Start = -1,
                    End = -1
                };
            }
            else if (splitRegion.Length == 2)
            {
                _region = new Region()
                {
                    Chromosome = splitRegion[0],
                    Start = uint.Parse(splitRegion[1], CultureInfo.InvariantCulture),
                    End = -1
                };
            }
            else if (splitRegion.Length == 3)
            {
                _region = new Region()
                {
                    Chromosome = splitRegion[0],
                    Start = uint.Parse(splitRegion[1], CultureInfo.InvariantCulture),
                    End = uint.Parse(splitRegion[2], CultureInfo.InvariantCulture)
                };
            }
            else
            {
                throw new InvalidOperationException("Region cannot be parsed");
            }
        }

        #endregion
    }

    #region Public Structures

    /// <summary>
    /// An alignment may be given multiple times if it is overlapping several regions. 
    /// A region can be presented, for example, in the following format: 
    /// ‘chr2’ (the whole chr2), 
    /// ‘chr2:1000000’ (region starting from 1,000,000bp) 
    /// or ‘chr2:1,000,000-2,000,000’ (region between 1,000,000 and 2,000,000bp including the end points). 
    /// The coordinate is 1-based. 
    /// </summary>
    public struct Region
    {
        /// <summary>
        /// Chromosome Number.
        /// </summary>
        public string Chromosome { get; set; }

        /// <summary>
        /// Start position of alignment.
        /// </summary>
        public long Start { get; set; }

        /// <summary>
        /// End position of alignment.
        /// </summary>
        public long End { get; set; }
    }

    #endregion
}