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
    /// SequenceAlignmentFormatters class is an abstraction class which provides instances
    /// and lists of all SequenceAlignmentFormatter currently supported by MBF. 	
    /// </summary>
    public static class SequenceAlignmentFormatters
    {
        /// <summary>
        /// An instance of SAMFormatter class which is capable of
        /// saving a ISequenceAlignment according to the SAM file format.
        /// </summary>
        private static SAMFormatter _sam = new SAMFormatter();

        /// <summary>
        /// An instance of BAMFormatter class which is capable of
        /// saving a ISequenceAlignment according to the BAM file format.
        /// </summary>
        private static BAMFormatter _bam = new BAMFormatter();

        /// <summary>
        /// List of all supported sequence formatters.
        /// </summary>
        private static List<ISequenceAlignmentFormatter> all = new List<ISequenceAlignmentFormatter>() { _sam, _bam };

        /// <summary>
        /// Gets an instance of SAMFormatter class which is capable of
        /// saving a ISequenceAlignment according to the SAM file format.
        /// </summary>
        public static SAMFormatter SAM
        {
            get
            {
                return _sam;
            }
        }

        /// <summary>
        /// Gets an instance of BAMFormatter class which is capable of
        /// saving a ISequenceAlignment according to the BAM file format.
        /// </summary>
        public static BAMFormatter BAM
        {
            get
            {
                return _bam;
            }
        }

        /// <summary>
        /// Gets the list of all sequence alignment formatters which is supported by the framework.
        /// </summary>
        public static IList<ISequenceAlignmentFormatter> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns sequence alignment formatter which supports the specified file.
        /// </summary>
        /// <param name="fileName">File name for which the formatter is required.</param>
        /// <returns>If found returns the formatter as ISequenceAlignmentFormatter else returns null.</returns>
        public static ISequenceAlignmentFormatter FindFormatterByFile(string fileName)
        {
            ISequenceAlignmentFormatter formatter = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (Helper.IsSAM(fileName))
                {
                    formatter = new SAMFormatter();
                }
                else if (Helper.IsBAM(fileName))
                {
                    formatter = new BAMFormatter();
                }
                else
                {
                    formatter = null;
                }
            }

            return formatter;
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static SequenceAlignmentFormatters()
        {
            //get the registered alignment formatter
            IList<ISequenceAlignmentFormatter> registeredFormatters = GetAlignmentFormatters(true);

            if (null != registeredFormatters && registeredFormatters.Count > 0)
            {
                foreach (ISequenceAlignmentFormatter formatter in registeredFormatters)
                {
                    if (formatter != null && all.FirstOrDefault(IA => string.Compare(IA.Name,
                        formatter.Name, StringComparison.InvariantCultureIgnoreCase) == 0) == null)
                    {
                        all.Add(formatter);
                    }
                }

                registeredFormatters.Clear();
            }
        }

        /// <summary>
        /// Gets all registered sequence alignment formatters in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered formatters</returns>
        private static IList<ISequenceAlignmentFormatter> GetAlignmentFormatters(bool includeAddinFolder)
        {
            IList<ISequenceAlignmentFormatter> registeredFormatters = new List<ISequenceAlignmentFormatter>();

            if (includeAddinFolder)
            {
                IList<ISequenceAlignmentFormatter> addInFormatters;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInFormatters = RegisteredAddIn.GetInstancesFromAssemblyPath<ISequenceAlignmentFormatter>(RegisteredAddIn.AddinFolderPath, RegisteredAddIn.DLLFilter);
                    if (null != addInFormatters && addInFormatters.Count > 0)
                    {
                        foreach (ISequenceAlignmentFormatter formatter in addInFormatters)
                        {
                            if (formatter != null && registeredFormatters.FirstOrDefault(IA => string.Compare(
                                IA.Name, formatter.Name, StringComparison.OrdinalIgnoreCase) == 0) == null)
                            {
                                registeredFormatters.Add(formatter);
                            }
                        }
                    }
                }
            }

            return registeredFormatters;
        }

    }
}
