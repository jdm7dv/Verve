//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;
using System.Collections.Generic;
using System.Linq;
using Bio.IO.Fasta;
using Bio.IO.FastQ;
using Bio.IO.GenBank;
using Bio.IO.Gff;
using Bio.Properties;
using Bio.Registration;
using Bio.Util;

namespace Bio.IO
{
    /// <summary>
    /// SequenceParsers class is an abstraction class which provides instances
    /// and lists of all Parsers currently supported by MBF. 	
    /// </summary>
    public static class SequenceParsers
    {
        /// <summary>
        /// A singleton instance of GenBankParser class which is capable of
        /// parsing GenBank format files.
        /// </summary>
        private static GenBankParser genBank = new GenBankParser();

        /// <summary>
        /// A singleton instance of FastaParser class which is capable of
        /// parsing FASTA format files.
        /// </summary>
        private static FastaParser fasta = new FastaParser();

        /// <summary>
        /// A singleton instance of FastQParser class which is capable of
        /// parsing FastQ format files.
        /// </summary>
        private static FastQParser fastq = new FastQParser();

        /// <summary>
        /// A singleton instance of GffParser class which is capable of
        /// parsing GFF format files.
        /// </summary>
        private static GffParser gff = new GffParser();

        /// <summary>
        /// List of all supported sequence parsers.
        /// </summary>
        private static List<ISequenceParser> all = new List<ISequenceParser>() { 
            fasta, 
            fastq,
            genBank,
            gff };

        /// <summary>
        /// Gets an instance of GenBankParser class which is capable of
        /// parsing GenBank format files.
        /// </summary>
        public static GenBankParser GenBank
        {
            get
            {
                return genBank;
            }
        }

        /// <summary>
        /// Gets an instance of FastaParser class which is capable of
        /// parsing FASTA format files.
        /// </summary>
        public static FastaParser Fasta
        {
            get
            {
                return fasta;
            }
        }

        /// <summary>
        /// Gets an instance of FastQParser class which is capable of
        /// parsing FASTQ format files.
        /// </summary>
        public static FastQParser FastQ
        {
            get
            {
                return fastq;
            }
        }

        /// <summary>
        /// Gets an instance of GffParser class which is capable of
        /// parsing GFF format files.
        /// </summary>
        public static GffParser Gff
        {
            get
            {
                return gff;
            }
        }

        /// <summary>
        /// Gets the list of all parsers which is supported by the framework.
        /// </summary>
        public static IList<ISequenceParser> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns parser which supports the specified file.
        /// </summary>
        /// <param name="fileName">File name for which the parser is required.</param>
        /// <returns>If found returns the parser as ISequenceParser else returns null.</returns>
        public static ISequenceParser FindParserByFile(string fileName)
        {
            ISequenceParser parser = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (Helper.IsGenBank(fileName))
                {
                    parser = new GenBankParser();
                }
                else if (fileName.EndsWith(Resource.GFF_FILEEXTENSION, StringComparison.OrdinalIgnoreCase))
                {
                    parser = new GffParser();
                }
                else if (Helper.IsFasta(fileName))
                {
                    parser = new FastaParser();
                }
                else if (Helper.IsFastQ(fileName))
                {
                    parser = new FastQParser();
                }
                else
                {
                    parser = null;
                }
            }

            return parser;
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static SequenceParsers()
        {
            //get the registered parsers
            IList<ISequenceParser> registeredParsers = GetSequenceParsers(true);

            if (null != registeredParsers && registeredParsers.Count > 0)
            {
                foreach (ISequenceParser parser in registeredParsers)
                {
                    if (parser != null && all.FirstOrDefault(IA => string.Compare(IA.Name,
                        parser.Name, StringComparison.OrdinalIgnoreCase) == 0) == null)
                    {
                        all.Add(parser);
                    }
                }
                registeredParsers.Clear();
            }
        }

        /// <summary>
        /// Gets all registered parsers in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered parsers</returns>
        private static IList<ISequenceParser> GetSequenceParsers(bool includeAddinFolder)
        {
            IList<ISequenceParser> registeredParsers = new List<ISequenceParser>();

            if (includeAddinFolder)
            {
                IList<ISequenceParser> addInParsers;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInParsers = RegisteredAddIn.GetInstancesFromAssemblyPath<ISequenceParser>(RegisteredAddIn.AddinFolderPath, RegisteredAddIn.DLLFilter);
                    if (null != addInParsers && addInParsers.Count > 0)
                    {
                        foreach (ISequenceParser parser in addInParsers)
                        {
                            if (parser != null && registeredParsers.FirstOrDefault(IA => string.Compare(
                                IA.Name, parser.Name, StringComparison.OrdinalIgnoreCase) == 0) == null)
                            {
                                registeredParsers.Add(parser);
                            }
                        }
                    }
                }
            }
            return registeredParsers;
        }
    }
}
