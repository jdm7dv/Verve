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
using Bio.IO.SAM;
using Bio.Registration;
using Bio.Util;

namespace Bio.IO
{
    /// <summary>
    /// SequenceAlignmentParsers class is an abstraction class which provides instances
    /// and lists of all Sequence Alignment Parsers currently supported by MBF. 	
    /// </summary>
    public static class SequenceAlignmentParsers
    {
        /// <summary>
        /// A singleton instance of SAMParser class which is capable of
        /// parsing SAM format files.
        /// </summary>
        private static SAMParser _sam = new SAMParser();

        /// <summary>
        /// List of all supported sequence alignment parsers.
        /// </summary>
        private static List<ISequenceAlignmentParser> all = new List<ISequenceAlignmentParser>() { _sam };

        /// <summary>
        /// Gets an instance of SAMParser class which is capable of
        /// parsing SAM format files.
        /// </summary>
        public static SAMParser SAM
        {
            get
            {
                return _sam;
            }
        }

        /// <summary>
        /// Gets the list of all sequence alignment parsers which is supported by the framework.
        /// </summary>
        public static IList<ISequenceAlignmentParser> All
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
        /// <returns>If found returns the parser as ISequenceAlignmentParser else returns null.</returns>
        public static ISequenceAlignmentParser FindParserByFile(string fileName)
        {
            ISequenceAlignmentParser parser = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (Helper.IsSAM(fileName))
                {
                    parser = new SAMParser();
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
        static SequenceAlignmentParsers()
        {
            //get the registered parsers
            IList<ISequenceAlignmentParser> registeredParsers = GetSequenceAlignmentParsers(true);

            if (null != registeredParsers && registeredParsers.Count > 0)
            {
                foreach (ISequenceAlignmentParser parser in registeredParsers)
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
        /// Gets all registered sequence aplignment parsers in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered parsers</returns>
        private static IList<ISequenceAlignmentParser> GetSequenceAlignmentParsers(bool includeAddinFolder)
        {
            IList<ISequenceAlignmentParser> registeredParsers = new List<ISequenceAlignmentParser>();

            if (includeAddinFolder)
            {
                IList<ISequenceAlignmentParser> addInParsers;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInParsers = RegisteredAddIn.GetInstancesFromAssemblyPath<ISequenceAlignmentParser>(RegisteredAddIn.AddinFolderPath, RegisteredAddIn.DLLFilter);
                    if (null != addInParsers && addInParsers.Count > 0)
                    {
                        foreach (ISequenceAlignmentParser parser in addInParsers)
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
