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
using System.Linq;
using MBF.IO.BAM;
using MBF.IO.SAM;
using MBF.Registration;
using MBF.Util;

namespace MBF.IO
{
    /// <summary>
    /// SequenceAlignmentParsers class is an abstraction class which provides instances
    /// and lists of all Sequence Alignment Parsers currently supported by MBF. 	
    /// </summary>
    public static class SequenceAlignmentParsers
    {
        /// <summary>
        /// An instance of SAMParser class which is capable of
        /// parsing SAM format files.
        /// </summary>
        private static SAMParser _sam = new SAMParser();

        /// <summary>
        /// An instance of BAMParser class which is capable of
        /// parsing BAM format files.
        /// </summary>
        private static BAMParser _bam = new BAMParser();

        /// <summary>
        /// List of all supported sequence alignment parsers.
        /// </summary>
        private static List<ISequenceAlignmentParser> all = new List<ISequenceAlignmentParser>() { _sam, _bam };

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
        /// Gets an instance of BAMParser class which is capable of
        /// parsing BAM format files.
        /// </summary>
        public static BAMParser BAM
        {
            get
            {
                return _bam;
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
                else if (Helper.IsBAM(fileName))
                {
                    parser = new BAMParser();
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
