//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Bio.Util;
using Bio.Matrix;
using MBT.Escience;
using MBT.Escience.Matrix;
using MBT.Escience.Parse;
using MBT.Escience.Biology;


namespace MBT.Escience.Biology
{
    public enum SequenceFileFormat
    {
        Phylip, Fasta, Tab, Matrix
    };

    public abstract class SequenceFormatter
    {
        //protected ArgumentCollection argumentCollection = new CommandArguments(new string[] { });
        SequenceFileFormat _formatType;


        protected SequenceFormatter() { }

        public SequenceFileFormat FileFormat
        {
            get { return _formatType; }
        }

        public static SequenceFormatter GetLikelyFormatterForFile(FileInfo file)
        {
            SequenceFormatter formatter;
            if (!TryGetLikelyFormatterForFile(file, out formatter))
            {
                throw new FormatException("Could not find a suitable SequenceFormatter that could parse the first line: " + Bio.Util.FileUtils.ReadLine(file));
            }
            return formatter;
        }

        public static SequenceFormatter GetLikelyFormatterForFile(string filename)
        {
            FileInfo file = new FileInfo(filename);
            return GetLikelyFormatterForFile(file);
        }

        public static bool TryGetLikelyFormatterForFile(FileInfo file, out SequenceFormatter formatter)
        {
            string firstLine = Bio.Util.FileUtils.ReadLine(file);
            Helper.CheckCondition(!string.IsNullOrEmpty(firstLine), "First line of file cannot be empty.");
            return TryGetLikelyFormatterForFirstLineOfFile(firstLine, out formatter);
        }

        public static bool TryGetLikelyFormatterForFile(string filename, out SequenceFormatter formatter)
        {
            string firstLine = Bio.Util.FileUtils.ReadLine(filename);
            Helper.CheckCondition(!string.IsNullOrEmpty(firstLine), "First line of file cannot be empty.");
            return TryGetLikelyFormatterForFirstLineOfFile(firstLine, out formatter);
        }


        public static bool TryGetLikelyFormatterForFirstLineOfFile(string firstLine, out SequenceFormatter formatter)
        {
            foreach (string format in Enum.GetNames(typeof(SequenceFileFormat)))
            {
                formatter = GetInstance(format);
                if (formatter.ConformsToFileFormat(firstLine))
                {
                    return true;
                }
            }

            formatter = null;
            return false;
        }




        public static SequenceFormatter GetInstance(string formatName)
        {
            SequenceFileFormat format = (SequenceFileFormat)Enum.Parse(typeof(SequenceFileFormat), formatName, true);
            return GetInstance(format);
        }

        public static SequenceFormatter GetInstance(SequenceFileFormat format)
        {
            SequenceFormatter formatter = null;

            switch (format)
            {
                case SequenceFileFormat.Phylip:
                    formatter = new Phylip();
                    break;
                case SequenceFileFormat.Fasta:
                    formatter = new Fasta();
                    break;
                case SequenceFileFormat.Tab:
                    formatter = new Tab();
                    break;
                //case SequenceFileFormat.Sparse:
                //    formatter = new SequenceFormatterSparse();
                //    break;
                case SequenceFileFormat.Matrix:
                    formatter = new SequenceMatrix();
                    break;
                default:
                    throw new Exception("Should never get here.");
            }
            formatter._formatType = format;

            return formatter;
        }

        /// <summary>
        /// Will convert from Tab, Fasta or Phylip format to a SparseMatrix. Does so using a temp file. Assumes our common settings, but does NOT convert DNA to AA.
        /// All positions with only a single value are ignored.
        /// </summary>
        public static bool TryGetMatrixFromSequenceFileIgnoreOneValueVariables<TRow, TCol, TVal>(string filename, TVal missing, ParallelOptions parallelOptions, out Matrix<TRow, TCol, TVal> matrix)
        {
            return TryGetMatrixFromSequenceFile(filename, missing, false, parallelOptions, out matrix);
        }

        /// <summary>
        /// Will convert from Tab, Fasta or Phylip format to a SparseMatrix. Does so using a temp file. Assumes our common settings, but does NOT convert DNA to AA.
        /// Positions with a single value are kept.
        /// </summary>
        public static bool TryGetMatrixFromSequenceFileKeepOneValueVariables<TRow, TCol, TVal>(string filename, TVal missing, ParallelOptions parallelOptions, out Matrix<TRow, TCol, TVal> matrix)
        {
            return TryGetMatrixFromSequenceFile(filename, missing, true, parallelOptions, out matrix);
        }

        public static bool TryGetMatrixFromSequenceFile<TRow, TCol, TVal>(string filename, TVal missing, bool keepOneValueVariables, ParallelOptions parallelOptions, out Matrix<TRow, TCol, TVal> matrix)
        {
            SequenceFormatter formatter;
            matrix = null;
            if (SequenceFormatter.TryGetLikelyFormatterForFile(filename, out formatter))
            {
                // only support true sequence formats.
                if (formatter is SequenceMatrix)// || formatter is SequenceFormatterSparse)
                    return false;

                var seqs = formatter.Parse(filename);
                if (seqs.Count == 0)
                {
                    Console.Error.WriteLine("No sequences in sequence file of format {0}", formatter.FileFormat);
                    return false;
                }
                matrix = NamedSequencesToMatrix<TRow, TCol, TVal>(seqs, missing, keepOneValueVariables, parallelOptions);
                return true;
                //if (IsDna(seqs[0].Sequence))
                //{
                //    seqs = NucSeqToAaSeq(seqs, true);
                //}
            }
            Console.Error.WriteLine("No SequenceFormatter matches this file type.");
            return false;
        }

        /// <summary>
        /// Will convert from Tab, Fasta or Phylip format to a SparseMatrix. If seqs are DNA, will convert to AA, interpreting dash as missing. 
        /// Does so using a temp file. Assumes our common settings. All positions with only a single value are ignored.
        /// </summary>
        public static bool TryGetMatrixFromSequenceFileAndPossiblyConvertDna2AaIgnoreOneValueVariables<TRow, TCol, TVal>(string filename, TVal missing, ParallelOptions parallelOptions, out Matrix<TRow, TCol, TVal> matrix)
        {
            return TryGetMatrixFromSequenceFileAndPossiblyConvertDna2Aa(filename, missing, false, parallelOptions, out matrix);
        }

        /// <summary>
        /// Will convert from Tab, Fasta or Phylip format to a SparseMatrix. If seqs are DNA, will convert to AA, interpreting dash as missing. 
        /// Does so using a temp file. Assumes our common settings. Positions with a single value are kept.
        /// </summary>
        public static bool TryGetMatrixFromSequenceFileAndPossiblyConvertDna2AaKeepOneValueVariables<TRow, TCol, TVal>(string filename, TVal missing, ParallelOptions parallelOptions, out Matrix<TRow, TCol, TVal> matrix)
        {
            return TryGetMatrixFromSequenceFileAndPossiblyConvertDna2Aa(filename, missing, true, parallelOptions, out matrix);
        }

        public static bool TryGetMatrixFromSequenceFileAndPossiblyConvertDna2Aa<TRow, TCol, TVal>(string filename, TVal missing, bool keepOneValueVariables, ParallelOptions parallelOptions, out Matrix<TRow, TCol, TVal> matrix)
        {
            SequenceFormatter formatter;
            matrix = null;
            if (SequenceFormatter.TryGetLikelyFormatterForFile(filename, out formatter))
            {
                // only support true sequence formats.
                if (formatter is SequenceMatrix)
                    return false;

                var seqs = formatter.Parse(filename);
                if (seqs.Count == 0)
                    return false;

                if (seqs[0].IsDna())
                {
                    seqs = Translate(seqs, true);
                }

                matrix = NamedSequencesToMatrix<TRow, TCol, TVal>(seqs, missing, keepOneValueVariables, parallelOptions);
                return true;

            }
            return false;
        }

        public static Matrix<TRow, TCol, TVal> NamedSequencesToMatrix<TRow, TCol, TVal>(List<NamedSequence> seqs, TVal missing, bool keepOneValueVariables, ParallelOptions parallelOptions)
        {
            Helper.CheckCondition(seqs.Count > 0, "There are no sequences");

            // can't do this because of the generics
            //var matrix = SequenceMatrix.ConvertToMatrix(seqs, MixtureSemantics.Uncertainty, SequenceMatrix.BinaryOrMultistate.Binary, keepOneValueVariables);
            string tempFile = Path.GetTempFileName();

            SequenceMatrix tableFormatter = new SequenceMatrix();
            tableFormatter.KeepOneValueVariables = keepOneValueVariables;
            ////if (keepOneValueVariables)
            ////{
            ////    tableFormatter.argumentCollection = new CommandArguments("-keepOneValueVariables");
            ////}
            tableFormatter.Write(seqs, tempFile);

            Matrix<TRow, TCol, TVal> matrix;
            bool success = DenseMatrix<TRow, TCol, TVal>.TryParseRFileWithDefaultMissing(tempFile, missing, parallelOptions, out matrix);
            File.Delete(tempFile);

            if (!success)
                throw new Exception("Unexpected error parsing the table into a matrix. This exception should not happen.");

            return matrix;
        }

        //public static Dictionary<string, Dictionary<string, SufficientStatistics>> LoadSparseFile(string hlaSequenceOrSparseFileName,
        //    bool keepOneValuedVariables, int merLength, MixtureSemantics semantics, bool dna2aa, bool dashAsMissing)
        //{
        //    SequenceFormatter formatter;
        //    HlaFormatter hlaFormatter;
        //    if (TryGetLikelyFormatterForFile(hlaSequenceOrSparseFileName, out formatter))
        //    {
        //        if (formatter.FileFormat == SequenceFileFormat.Sparse)
        //        {
        //            return SparseFormatter.LoadSparseFileIntoMemory(hlaSequenceOrSparseFileName);
        //        }
        //        else
        //        {
        //            List<NamedSequence> seqs = formatter.Parse(hlaSequenceOrSparseFileName);
        //            if (dna2aa)
        //            {
        //                seqs = NucSeqToAaSeq(seqs, dashAsMissing);
        //            }
        //            string args = string.Format("{0} {1} -MixtureSemantics {2}",
        //                keepOneValuedVariables ? "-keepOneValueVariables" : "",
        //                merLength > 1 ? "-merLength " + merLength : "",
        //                semantics);

        //            SequenceFormatterSparse sparseFormatter = new SequenceFormatterSparse();
        //            sparseFormatter.SetArgs(new CommandArguments(args));

        //            return sparseFormatter.ConvertToSparseMapping(seqs);
        //        }
        //    }
        //    else if (HlaFormatter.TryGetLikelyFormatterForFile(hlaSequenceOrSparseFileName, out hlaFormatter))
        //    {
        //        return HlaFormatter.LoadSparseFile(hlaSequenceOrSparseFileName);
        //    }
        //    else
        //    {
        //        throw new FormatException("Could not find a suitable sequence or hla formatter for " + hlaSequenceOrSparseFileName);
        //    }
        //}

        public static List<NamedSequence> ParseWithMostLikelyFormatter(string filename)
        {
            using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(filename))
            {
                return ParseWithMostLikelyFormatter(reader);
            }
        }

        public static List<NamedSequence> ParseWithMostLikelyFormatter(FileInfo file)
        {
            using (TextReader reader = file.OpenTextStripComments())
            {
                return ParseWithMostLikelyFormatter(reader);
            }
        }

        public static List<NamedSequence> ParseWithMostLikelyFormatter(TextReader reader)
        {
            string firstLine = reader.ReadLine();
            string allButFirstLine = reader.ReadToEnd();
            SequenceFormatter formatter = null;
            TryGetLikelyFormatterForFirstLineOfFile(firstLine, out formatter).Enforce("Could not find a suitable sequence formatter for first line " + firstLine);

            TextReader newReader = new StringReader(firstLine + "\n" + allButFirstLine);
            List<NamedSequence> result = formatter.Parse(newReader);
            return result;
        }

        //public void SetArgs(argumentCollection args)
        //{
        //    argumentCollection = args;
        //}

        public override string ToString()
        {
            return _formatType.ToString();
        }

        public override bool Equals(object obj)
        {
            SequenceFormatter other = obj as SequenceFormatter;
            return other != null && _formatType == other._formatType;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public List<NamedSequence> Parse(FileInfo file)
        {
            using (TextReader reader = file.OpenTextStripComments())
            {
                return Parse(reader);
            }
        }

        public List<NamedSequence> Parse(string filename)
        {
            return Parse(new FileInfo(filename));
        }

        public void Write(List<NamedSequence> sequences, string filename)
        {
            using (TextWriter writer = File.CreateText(filename))
            {
                Write(sequences, writer);
            }
        }

        static public List<NamedSequence> RemoveDuplicateSeqs(List<NamedSequence> seqs)
        {
            Set<string> observedSeqs = new Set<string>();
            List<NamedSequence> result = new List<NamedSequence>(seqs.Count);

            foreach (NamedSequence seq in seqs)
            {
                if (!observedSeqs.Contains(seq.Sequence.ToLower()))
                {
                    result.Add(seq);
                    observedSeqs.AddNew(seq.Sequence.ToLower());
                }
            }
            return result;
        }



        /// <param name="readingFrameToTranslate">Will start translation at start+readingFrameToTranslate-1. If RF>3, will take the reverse
        /// commplement, then translate the revComp using rf -= 3. The sequence will be padded at the end to make it the same length as the
        /// original, and the final result will be reversed, so that the sequence is read in the same order as the original string!
        /// ie RF is 1-based.</param>
        public static List<NamedSequence> Translate(List<NamedSequence> seqs, bool nucToAaDashAsMissing = true, int readingFrameToTranslate = 1)
        {
            Helper.CheckCondition(readingFrameToTranslate > 0 && readingFrameToTranslate <= 6, "readingFrameToTranslate must be between 0 and 6. " + readingFrameToTranslate + " is not valid.");
            bool isAntisense = false;
            if (readingFrameToTranslate > 3)
            {
                seqs = NucSeqReverseComplement(seqs);
                readingFrameToTranslate -= 3;
                isAntisense = true;
            }
            List<NamedSequence> result = new List<NamedSequence>(seqs.Count);
            int lastLen = -1;
            foreach (NamedSequence seq in seqs)
            {
                string seqToTranslate = seq.Sequence.Substring(readingFrameToTranslate - 1);
                if (seqToTranslate.Length != lastLen)
                {
                    lastLen = seqToTranslate.Length;
                    if (lastLen % 3 != 0)
                    {
                        Console.Error.WriteLine("WARNING: Sequence of length {0} is not divisible by 3. Cutting off the end for translation.", lastLen);
                    }
                }
                if (seqToTranslate.Length % 3 != 0)
                    seqToTranslate = seqToTranslate.Substring(0, seqToTranslate.Length - seqToTranslate.Length % 3);
                //seqToTranslate += Enumerable.Repeat("-", 3 - (seqToTranslate.Length % 3)).StringJoin();

                var translatedSeq = new NamedSequence(seq.Name, seq.Protein, NamedSequence.Translate(seqToTranslate, nucToAaDashAsMissing));
                result.Add(isAntisense ? translatedSeq.ReverseSequence() : translatedSeq);
            }
            return result;
        }

        /// <summary>
        /// Reverses all the sequences. No reverse complement is taken. AA mixtures are handled correctly.
        /// </summary>
        public static List<NamedSequence> ReverseSequences(List<NamedSequence> seqs)
        {
            return seqs.Select(seq => seq.ReverseSequence()).ToList();
            //List<NamedSequence> result = new List<NamedSequence>(seqs.Count);
            //foreach (var seq in seqs)
            //{
            //    StringBuilder rev = new StringBuilder(seq.Sequence.Reverse());
            //    rev.Replace("{", "TEMP-{-TEMP");
            //    rev.Replace('}', '{');
            //    rev.Replace("TEMP-{-TEMP", "}");
            //    result.Add(new NamedSequence(seq.Name, rev.ToString()));
            //}
            //return result;
        }

        public static List<NamedSequence> NucSeqReverseComplement(List<NamedSequence> seqs)
        {
            return seqs.Select(seq => seq.ReverseCompelement()).ToList();
            //List<NamedSequence> result = new List<NamedSequence>(seqs.Count);
            //Biology Biology = Biology.GetInstance();

            //foreach (NamedSequence seq in seqs)
            //{
            //    StringBuilder revComp = new StringBuilder(seq.Sequence.Length);
            //    for (int i = seq.Sequence.Length - 1; i >= 0; i--)
            //    {
            //        Helper.CheckCondition(Biology.DnaComplementarityCode.ContainsKey(seq.Sequence[i]), "Cannot compute complement for " + seq.Sequence[i]);
            //        char comp = Biology.DnaComplementarityCode[seq.Sequence[i]];
            //        revComp.Append(comp);
            //    }
            //    result.Add(new NamedSequence(seq.Name, revComp.ToString()));
            //}

            //return result;
        }

        public static AASeq GetAaSeqConsensus(List<AASeq> aaSeqs)
        {
            int len = -1;
            StringBuilder consensusSeq = new StringBuilder();
            foreach (AASeq seq in aaSeqs)
            {
                len = Math.Max(len, seq.Count);
            }

            for (int pos0 = 0; pos0 < len; pos0++)
            {
                consensusSeq.Append(AASeq.CountAasAtPos(aaSeqs, pos0)[0].Key);
            }
            //for (int i = 0; i < len; i++)
            //{
            //    Dictionary<string, int> charToCount = new Dictionary<string, int>();
            //    KeyValuePair<string, int> currentConsensus = new KeyValuePair<string, int>("z", -1);
            //    foreach (AASeq aaSeq in aaSeqs)
            //    {
            //        if (i < aaSeq.Count)
            //        {
            //            string residue = aaSeq.SubSeqAA0Pos(i, 1).ToString();
            //            charToCount[residue] = charToCount.GetValueOrDefault(residue) + 1;
            //            if (charToCount[residue] > currentConsensus.Value)
            //            {
            //                currentConsensus = new KeyValuePair<string, int>(residue, charToCount[residue]);
            //            }
            //        }
            //    }
            //    consensusSeq.Append(currentConsensus.Key);
            //}

            AASeq consensus = AASeq.GetInstance(consensusSeq.ToString(), MixtureSemantics.Uncertainty);
            //NamedSequence consensus = new NamedSequence("consensus", consensusSeq.ToString());
            return consensus;
        }

        public static NamedSequence GetConsensus(List<NamedSequence> seqs)
        {
            int len = -1;
            //StringBuilder consensusSeq = new StringBuilder();
            List<AASeq> aaSeqs = new List<AASeq>();
            bool isDna = seqs[0].IsDna();
            foreach (NamedSequence seq in seqs)
            {
                AASeq aaSeq = isDna ? DnaSeq.GetInstance(seq.Sequence, MixtureSemantics.Uncertainty) : AASeq.GetInstance(seq.Sequence, MixtureSemantics.Uncertainty);

                len = Math.Max(len, aaSeq.Count);
                //if (len < 0)
                //{
                //    len = aaSeq.Count;
                //}
                //else
                //{
                //    Helper.CheckCondition(len == aaSeq.Count, String.Format("Sequence {0} is a different length from previous sequences", seq.Name));
                //}
                aaSeqs.Add(aaSeq);
            }

            //for (int i = 0; i < len; i++)
            //{
            //    Dictionary<string, int> charToCount = new Dictionary<string, int>();
            //    KeyValuePair<string, int> currentConsensus = new KeyValuePair<string, int>("z", -1);
            //    foreach (AASeq aaSeq in aaSeqs)
            //    {
            //        if (i < aaSeq.Count)
            //        {
            //            string residue = aaSeq.SubSeqAA0Pos(i, 1).ToString();
            //            charToCount[residue] = SpecialFunctions.GetValueOrDefault(charToCount, residue) + 1;
            //            if (charToCount[residue] > currentConsensus.Value)
            //            {
            //                currentConsensus = new KeyValuePair<string, int>(residue, charToCount[residue]);
            //            }
            //        }
            //    }
            //    consensusSeq.Append(currentConsensus.Key);
            //}
            AASeq consensusAaSeq = GetAaSeqConsensus(aaSeqs);
            NamedSequence consensus = new NamedSequence("consensus", consensusAaSeq.ToString());
            return consensus;
        }



        abstract public bool ConformsToFileFormat(string firstLine);
        abstract public List<NamedSequence> Parse(TextReader reader);
        abstract public void Write(List<NamedSequence> sequences, TextWriter writer);

        protected static string RemoveWhiteSpace(string s)
        {
            s = s.Trim();
            string[] charsBetweenWhiteSpace = Regex.Split(s, @"\s");
            StringBuilder seqWithoutWS = new StringBuilder(s.Length);

            foreach (string section in charsBetweenWhiteSpace)
            {
                seqWithoutWS.Append(section);
            }
            return seqWithoutWS.ToString();
        }



        public virtual void ParseReadArgs(ArgumentCollection argumentCollection)
        {
            // default does nothing.
        }
        public virtual void ParseWriteArgs(ArgumentCollection argumentCollection)
        {
            // default does nothing.
        }
    }

    public class Phylip : SequenceFormatter
    {
        public override bool ConformsToFileFormat(string firstLine)
        {
            return Regex.IsMatch(firstLine, @"^\s*\d+\s+\d+\s*$");
        }

        public override List<NamedSequence> Parse(TextReader reader)
        {
            string line;
            string[] fields;

            line = reader.ReadLine().Trim();
            if (!ConformsToFileFormat(line))
            {
                throw new FormatException("File does not conform to Phylip format. First line must be two integers specifying number of sequences and sequence length.");
            }

            fields = Regex.Split(line, @"\s+");

            int lineCount, lineLength;
            try
            {
                lineCount = int.Parse(fields[0]);
                lineLength = int.Parse(fields[1]);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Error parsing Phylip header: {0} split to {1} and {2}", line, fields[0], fields[1]);
                throw e;
            }

            List<NamedSequence> seqs = new List<NamedSequence>(lineCount);

            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    int endOfName = Math.Max(10, line.IndexOfAny(new char[] { ' ', '\t' }, 0, Math.Min(50, line.Length)));

                    string name = line.Substring(0, endOfName).Trim();
                    string seq = RemoveWhiteSpace(line.Substring(endOfName));

                    //NamedSequence sequence = new NamedSequence(name, seq);
                    NamedSequence sequence = NamedSequence.Parse(name);
                    sequence.Sequence = seq;
                    if (sequence.Sequence.Length != lineLength)
                    {
                        throw new FormatException(string.Format("Expected {0} to be {1} chars long, but its {2} long.",
                            sequence.Name, lineLength, sequence.Sequence.Length));
                    }

                    seqs.Add(sequence);
                }
            }
            if (seqs.Count != lineCount)
            {
                throw new FormatException(string.Format("Expected {0} sequences. Read {1}.", lineCount, seqs.Count));
            }

            return seqs;
        }

        public override void Write(List<NamedSequence> sequences, TextWriter writer)
        {
            int lineCount = sequences.Count;
            int lineLength = sequences[0].Sequence.Length;

            writer.WriteLine("{0} {1}", lineCount, lineLength);
            int longestNameLen = sequences.Select(seq => seq.ToParsableNameString().Length).Max();
            bool repaceNameWS = false;
            int padLen = 13;
            if (longestNameLen > 10)
            {
                Console.Error.WriteLine("WARNING: Names are longer than 10. Using the relaxed Phylip format that PHYML uses, which uses the first whitespace as the break. Will change all whitespace in the name to '_'");
                repaceNameWS = true;
                padLen = longestNameLen + 3;
            }

            foreach (NamedSequence sequence in sequences)
            {
                string seq = sequence.Sequence;
                if (sequence.Sequence.Length != lineLength)
                {
                    Console.Error.WriteLine("WARNING: Not a valid Phylip format. {0} is not the expected length of {1}. It's {2} bases long. Padding with -",
                        sequence.Name, lineLength, sequence.Sequence.Length);
                    seq = sequence.Sequence.PadRight(lineLength, '-');
                }
                string name = sequence.ToParsableNameString();

                if (repaceNameWS)
                {
                    name.Replace(' ', '_');
                    name.Replace('\t', '_');
                }
                //if (name.Length > 10)
                //{
                //    string shortName = name.Substring(0, 2) + "~" + name.Substring(name.Length - 7);
                //    Console.Error.WriteLine("WARNING: {0} is longer than 10 characters. Truncating to {1}", name, shortName);
                //    name = shortName;
                //}
                name = name.PadRight(padLen);
                writer.WriteLine(name + seq);
            }
        }
    }

    public class Fasta : SequenceFormatter
    {

        public override bool ConformsToFileFormat(string firstLine)
        {
            return firstLine.StartsWith(">");
        }

        public override List<NamedSequence> Parse(TextReader reader)
        {
            List<NamedSequence> seqs = new List<NamedSequence>();
            string line;

            //string name = null;
            NamedSequence seqToAdd = null;
            StringBuilder sequence = new StringBuilder();
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(">"))
                {
                    if (seqToAdd != null)
                    {
                        //seqs.Add(new NamedSequence(name, RemoveWhiteSpace(sequence.ToString())));
                        seqToAdd.Sequence = sequence.ToString();
                        seqs.Add(seqToAdd);
                    }

                    seqToAdd = NamedSequence.Parse(line.Substring(1).Trim());
                    //name = line.Substring(1).Trim();
                    sequence = new StringBuilder(sequence.Length);
                }
                else
                {
                    sequence.Append(line.Trim());
                }
            }
            //seqs.Add(new NamedSequence(name, RemoveWhiteSpace(sequence.ToString())));
            seqToAdd.Sequence = sequence.ToString();
            seqs.Add(seqToAdd);

            return seqs;
        }

        public override void Write(List<NamedSequence> sequences, TextWriter writer)
        {
            foreach (NamedSequence seq in sequences)
            {
                writer.WriteLine(">{0}", seq.ToParsableNameString());
                writer.WriteLine(seq.Sequence);
            }
        }
    }

    public class Tab : SequenceFormatter
    {
        public static readonly string OUTPUT_HEADER = "name\tseq";
        public bool NoHeader = false;

        public override bool ConformsToFileFormat(string firstLine)
        {
            return firstLine.Split('\t').Length == 2;
        }

        public override List<NamedSequence> Parse(TextReader reader)
        {
            if (NoHeader)
            {
                reader = new StringReader(OUTPUT_HEADER + "\n" + reader.ReadToEnd());
            }

            List<NamedSequence> seqs = new List<NamedSequence>();

            string[] header = null;
            foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(reader, "", true, false, true))
            {
                if (header == null)
                {
                    header = row[""].Split('\t');
                    Helper.CheckCondition(header.Length == 2, "Header does not conform to tab file format: " + row[""]);
                    continue;
                }

                string name = row[header[0]];
                string seq = row[header[1]];
                NamedSequence namedSeq = NamedSequence.Parse(name);
                namedSeq.Sequence = RemoveWhiteSpace(seq);
                seqs.Add(namedSeq);
            }
            return seqs;
        }


        public override void Write(List<NamedSequence> sequences, TextWriter writer)
        {
            if (!NoHeader)
                writer.WriteLine(OUTPUT_HEADER);
            foreach (NamedSequence seq in sequences)
            {
                writer.WriteLine(seq.ToParsableNameString() + '\t' + seq.Sequence);
            }
        }
    }

    public class SequenceMatrix : SequenceFormatter
    {
        /// <summary>
        /// if true, will keep invariant positions. Otherwise, will ignore them.
        /// </summary>
        public bool KeepOneValueVariables = true;

        /// <summary>
        /// Specifies the missing character to use
        /// </summary>
        public char MissingChar = '?';

        /// <summary>
        /// Specifies the mixture semantics that will be used.
        /// </summary>
        public MixtureSemantics MixtureSemantics = MixtureSemantics.Uncertainty;

        public enum BinaryOrMultistate { Binary, Multistate };
        public enum WriteFormat { Dense, DenseAnsi, Sparse };

        [Parse(ParseAction.Required)]
        public WriteFormat Format;
        [Parse(ParseAction.Required)]
        public BinaryOrMultistate DataType;


        public override bool ConformsToFileFormat(string firstLine)
        {
            return firstLine.Split('\t').Length > 2;
        }

        //public void ParseArgs(ArgumentCollection argumentCollection)
        //{
        //    KeepOneValueVariables = argumentCollection.ExtractOptionalFlag("keepOneValueVariables");
        //    //MissingChar = argumentCollection.ExtractOptional<char>("missingChar", '?');

        //    //int? merLengthOrNull = argumentCollection.ExtractOptional<int?>("Mer", null); // we don't seem to actually use this...
        //    CurrentMixtureSemantics = argumentCollection.ExtractOptional<MixtureSemantics>("MixtureSemantics", MixtureSemantics.Uncertainty);
        //    //Helper.CheckCondition(null == merLengthOrNull || mixtureSemantics == MixtureSemantics.Uncertainty, "The 'Mer' option cannot be used with '-MixtureSemantics pure' or '-MixtureSemantics any'");

        //}

        //public override void ParseReadArgs(ArgumentCollection argumentCollection)
        //{
        //    base.ParseReadArgs(argumentCollection);
        //    ParseArgs(argumentCollection);
        //}

        //public override void ParseWriteArgs(ArgumentCollection argumentCollection)
        //{
        //    base.ParseWriteArgs(argumentCollection);
        //    ParseArgs(argumentCollection);
        //    Format = argumentCollection.ExtractNext<SequenceMatrix.WriteFormat>("WriteFormat");
        //    Type = argumentCollection.ExtractNext<SequenceMatrix.WriteType>("WriteFormat");
        //}

        public override List<NamedSequence> Parse(TextReader reader)
        {

            MatrixFactory<string, string, SufficientStatistics> mf = MatrixFactory<string, string, SufficientStatistics>.GetInstance();
            string filename = MBT.Escience.FileUtils.WriteTextReaderToTempFile(reader);
            var matrix = mf.Parse(filename, MissingStatistics.GetInstance(), new ParallelOptions());

            return ConvertToSequences(matrix);
        }

        public List<NamedSequence> ConvertToSequences(Matrix<string, string, SufficientStatistics> matrix)
        {
            bool isMultistate = matrix.RowKeys.Any(key => key.Contains('#'));
            if (!isMultistate)
            {
                matrix = new BinaryToMultistateView<string, string, SufficientStatistics>(matrix, Tabulate.BinaryToMultistateMapping(matrix), MBT.Escience.ValueConverter.SufficientStatisticsToInt);
            }

            var rowKeysAsMerAndPos = (from key in matrix.RowKeys
                                      let merAndPos = Tabulate.GetMerAndPos(key)
                                      orderby merAndPos.Value
                                      select new
                                      {
                                          Position0 = (int)merAndPos.Value - 1,
                                          Chars = merAndPos.Key.Split('#'),
                                          Key = key
                                      }).ToList();

            List<NamedSequence> result = new List<NamedSequence>(matrix.ColCount);
            for (int pid = 0; pid < matrix.ColCount; pid++)
            {
                StringBuilder seq = new StringBuilder(matrix.RowCount);
                foreach (var rowKey in rowKeysAsMerAndPos)
                {
                    Helper.CheckCondition(seq.Length <= rowKey.Position0, "There appears to be multiple keys with the same position.");
                    while (seq.Length < rowKey.Position0)
                        seq.Append(MissingChar);
                    DiscreteStatistics stats = matrix.GetValueOrMissing(rowKey.Key, matrix.ColKeys[pid]).AsDiscreteStatistics();
                    if (stats.IsMissing())
                        seq.Append(MissingChar);
                    else
                    {
                        string thisChar;
                        if (rowKey.Chars.Length == 1)
                        {
                            thisChar = rowKey.Chars[0];
                            Helper.CheckCondition((int)stats == 1);
                        }
                        else
                        {
                            thisChar = rowKey.Chars[stats];
                        }
                        Helper.CheckCondition(thisChar.Length == 1, "State {0} is too long. Must be a single character.", thisChar);
                        seq.Append(thisChar);
                    }
                }
                result.Add(new NamedSequence(matrix.ColKeys[pid], seq.ToString()));
            }

            return result;
        }

        //public List<NamedSequence> ParseOld(TextReader reader)
        //{
        //    string header = reader.ReadLine();
        //    List<string> pids = header.Split('\t').ToList();
        //    bool hasReadFirstLine = false;
        //    List<List<Set<char>>> seqs = new List<List<Set<char>>>(pids.Count);
        //    string line;

        //    while (null != (line = reader.ReadLine()))
        //    {
        //        string[] fields = line.Split('\t');

        //        if (!hasReadFirstLine)
        //        {
        //            hasReadFirstLine = true;
        //            Helper.CheckCondition(fields.Length == pids.Count || fields.Length == pids.Count + 1, "Header doesn't match the rows. Wrong number of fields.");
        //            if (pids.Count == fields.Length)
        //            {
        //                pids.RemoveAt(0);
        //            }
        //            for (int i = 0; i < pids.Count; i++)
        //            {
        //                seqs.Add(new List<Set<char>>());
        //            }
        //        }

        //        // parse the row name into an amino acid and position. Assume aa@pos or pos@aa.
        //        char residue;
        //        int pos0;
        //        string[] merAndPos = fields[0].Split('@');
        //        if (int.TryParse(merAndPos[1], out pos0))
        //        {
        //            Helper.CheckCondition(merAndPos[0].Length == 1, "Currently only support strings of the type aa@pos or pos@aa. Appears aa is a string.");
        //            residue = merAndPos[0][0];
        //        }
        //        else
        //        {
        //            pos0 = int.Parse(merAndPos[0]);
        //            Helper.CheckCondition(merAndPos[1].Length == 1, "Currently only support strings of the type aa@pos or pos@aa. Appears aa is a string.");
        //            residue = merAndPos[1][0];
        //        }

        //        pos0--; // the parsed position is pos1...

        //        // for each pid, add the mer at the pos
        //        for (int pidIdx = 0; pidIdx < pids.Count; pidIdx++)
        //        {
        //            while (pos0 >= seqs[pidIdx].Count)
        //                seqs[pidIdx].Add(new Set<char>());
        //            switch (fields[pidIdx + 1][0]) // +1 because the fields also contains the row name.
        //            {
        //                case '1':
        //                    seqs[pidIdx][pos0].AddNew(residue); break;
        //                case '0':
        //                    break;
        //                case MissingStatistics.MissingChar:
        //                    seqs[pidIdx][pos0].AddNewOrOld(MissingStatistics.MissingChar); break;
        //                default:
        //                    throw new ArgumentException("Unknown boolean character " + fields[pidIdx + 1]);
        //            }
        //        }
        //    }
        //    List<NamedSequence> result = new List<NamedSequence>(pids.Count);
        //    for (int pidIdx = 0; pidIdx < pids.Count; pidIdx++)
        //    {
        //        string seq = seqs[pidIdx].Aggregate(new StringBuilder(2 * seqs[0].Count), (sb, set) => sb.Append(AASeq.AaAsString(set))).ToString();
        //        result.Add(new NamedSequence(pids[pidIdx], seq));
        //    }
        //    return result;
        //}


        public override void Write(List<NamedSequence> sequences, TextWriter writer)
        {
            var m = ConvertToMatrix(sequences, this.MixtureSemantics, this.DataType, KeepOneValueVariables);
            switch (Format)
            {
                case WriteFormat.Dense:
                    m.WriteDense(writer); break;
                case WriteFormat.DenseAnsi:
                    m.WriteDenseAnsi(writer, new ParallelOptions() { MaxDegreeOfParallelism = 1 }); break;
                case WriteFormat.Sparse:
                    m.WriteSparse(writer); break;
                default:
                    throw new NotImplementedException("Missing a case");
            }


            //if (Type == WriteType.Binary && Format == WriteFormat.Dense)
            //{
            //    WriteAsTable(sequences, writer);
            //}
            //else
            //{
            //    string tempFile = Path.GetTempFileName();
            //    using (TextWriter tempWriter = File.CreateText(tempFile))
            //    {
            //        WriteAsTable(sequences, tempWriter);
            //    }
            //    var mf = MatrixFactory<string, string, SufficientStatistics>.GetInstance();
            //    var m = mf.Parse(tempFile, MissingStatistics.GetInstance(), new ParallelOptions());

            //    if (Type == WriteType.Multistate)
            //        m = new BinaryToMultistateView<string, string, SufficientStatistics>(m, Tabulate.BinaryToMultistateMapping(m), MBT.Escience.ValueConverter.SufficientStatisticsToInt);

            //    switch (Format)
            //    {
            //        case WriteFormat.Dense:
            //            m.WriteDense(writer); break;
            //        case WriteFormat.DenseAnsi:
            //            m.WriteDenseAnsi(writer, new ParallelOptions()); break;
            //        case WriteFormat.Sparse:
            //            m.WriteSparse(writer); break;
            //        default:
            //            throw new Exception("Should not be able to get here.");
            //    }
            //}
        }

        public void WriteAsTable(List<NamedSequence> sequences, TextWriter writer)
        {


            CaseIdToAASeq cidToAASeq = CaseIdToAASeq.GetInstance();
            bool isDna = sequences[0].IsDna();

            foreach (NamedSequence seq in sequences)
            {
                cidToAASeq.Add(seq.Name,
                    isDna ?
                        DnaSeq.GetInstance(seq.Sequence, MixtureSemantics) :
                        AASeq.GetInstance(seq.Sequence, MixtureSemantics));
            }

            List<string> header = new List<string>(sequences.Count + 1);
            header.Add("Var");
            header.AddRange(sequences.Select(seq => seq.Name));

            writer.WriteLine(header.StringJoin("\t"));

            int maxLen = cidToAASeq.Dictionary.Values.Select(aaSeq => aaSeq.Count).Max();
            for (int pos0 = 0; pos0 < maxLen; pos0++)
            {
                foreach (char aa in cidToAASeq.EveryAminoAcid(pos0))
                {
                    string merAndPos = (pos0 + 1) + "@" + aa;
                    int?[] values = new int?[sequences.Count];
                    HashSet<int> nonMissingValues = new HashSet<int>();
                    for (int pidIdx = 0; pidIdx < sequences.Count; pidIdx++)
                    {
                        int? value;
                        Set<char> observedAAs = cidToAASeq.Dictionary[sequences[pidIdx].Name][pos0];
                        if (observedAAs.Contains('?') || observedAAs.Count == 0 ||
                            (observedAAs.Count > 1 && MixtureSemantics == MixtureSemantics.Uncertainty && observedAAs.Contains(aa)))
                        {
                            value = null;
                        }
                        else if (observedAAs.Contains(aa) && (MixtureSemantics != MixtureSemantics.Pure || observedAAs.Count == 1))
                            value = 1;
                        else
                            value = 0;

                        values[pidIdx] = value;
                        if (value != null)
                            nonMissingValues.Add((int)value);
                    }
                    if (nonMissingValues.Count > 1 || (KeepOneValueVariables && nonMissingValues.Count == 1 && nonMissingValues.First() == 1))
                    {
                        writer.WriteLine(Helper.CreateTabString(merAndPos, values.Select(v => v.HasValue ? v.ToString() : MissingStatistics.GetInstance().ToString()).StringJoin("\t")));
                    }
                }
            }


            writer.Flush();
        }

        public static Matrix<string, string, SufficientStatistics> ConvertToMatrix(List<NamedSequence> sequences, MixtureSemantics mix, BinaryOrMultistate dataType, bool keepOneValueVariables)
        {
            var colNames = sequences.Select(s => s.Name);
            //var rowNames = (from posAndAa in sequences.SelectMany(s => s.AASeq)
            //                where !AASeq.IsMissing(posAndAa.Value)
            //                let pos = posAndAa.Key
            //                let aas = posAndAa.Value
            //                from c in aas
            //                let merAndPos = pos + "@" + c
            //                orderby pos, c
            //                select merAndPos).Distinct();
            var rowNames = (from posAndAa in sequences.SelectMany(s => s.AASeq)
                            where posAndAa.Value.Count == 1 && !AASeq.IsMissing(posAndAa.Value)
                            let pos = posAndAa.Key
                            let aas = posAndAa.Value
                            let c = aas.First()
                            let merAndPos = pos + "@" + c
                            orderby pos, c
                            select merAndPos).Distinct();

            var posToRowNames = (from row in rowNames
                                 let pos = (int)Tabulate.GetMerAndPos(row).Value
                                 group row by pos into g
                                 select new KeyValuePair<int, List<string>>(g.Key, g.ToList())).ToDictionary();

            Matrix<string, string, SufficientStatistics> m = DenseMatrix<string, string, SufficientStatistics>.CreateDefaultInstance(rowNames, colNames, MissingStatistics.GetInstance());

            foreach (var seq in sequences)
            {
                foreach (var posAndAa in seq.AASeq)
                {
                    int pos = posAndAa.Key;
                    if (!posToRowNames.ContainsKey(pos))
                    {
                        Helper.CheckCondition(AASeq.IsMissing(posAndAa.Value), "Something's wrong. We thinking everyone is missing at position {0}, but {1} has {2}", pos, seq.Name, posAndAa.Value);
                        continue;
                    }

                    var relevantRows = posToRowNames[pos];

                    bool isMissing = AASeq.IsMissing(posAndAa.Value);
                    var myRows = posAndAa.Value.Select(c => pos + "@" + c).ToList();
                    foreach (var row in relevantRows)
                    {
                        SufficientStatistics value;
                        if (isMissing)
                            value = MissingStatistics.GetInstance();
                        else if (!myRows.Contains(row))
                            value = (BooleanStatistics)false;   //in all cases, this is false
                        else if (myRows.Count == 1)
                            value = (BooleanStatistics)true;
                        else
                        {
                            switch (mix)
                            {
                                case MixtureSemantics.Any:
                                    value = (BooleanStatistics)true; break;   //Any means we say you have both
                                case MixtureSemantics.Pure:
                                    value = (BooleanStatistics)false; break;   //Pure means we say you have neither
                                case MixtureSemantics.Uncertainty:
                                    value = MissingStatistics.GetInstance(); break;   //Uncertainty says we don't know which you have.
                                case MixtureSemantics.Distribution:
                                    double pTrue = 1.0 / myRows.Count;
                                    value = MultinomialStatistics.GetInstance(new double[] { 1 - pTrue, pTrue });
                                    break;
                                default:
                                    throw new NotImplementedException("Missing a case.");
                            }
                        }
                        m.SetValueOrMissing(row, seq.Name, value);
                    }
                }
            }

            if (!keepOneValueVariables)
            {
                m = m.SelectRowsView(m.RowKeys.Where(row => m.RowView(row).Values.Distinct().Count() > 1));
            }

            switch (dataType)
            {
                case BinaryOrMultistate.Binary:
                    return m;
                case BinaryOrMultistate.Multistate:
                    return new BinaryToMultistateView<string, string, SufficientStatistics>(m, Tabulate.BinaryToMultistateMapping(m), ValueConverter.SufficientStatisticsToMultinomial);
                default:
                    throw new NotImplementedException("Missing a case");
            }
        }
    }
}
