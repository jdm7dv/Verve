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
using MBF.IO.Fasta;
using MBF.IO.FastQ;
using MBF.IO.GenBank;
using MBF.IO.Gff;
using MBF.Properties;
using MBF.Registration;
using MBF.Util;

namespace MBF.IO
{
    /// <summary>
    /// SequenceFormatter class is an abstraction class which provides instances
    /// and lists of all Formatter currently supported by MBF. 	
    /// </summary>
    public static class SequenceFormatters
    {
        /// <summary>
        /// A singleton instance of GenBankFormatter class which is capable of
        /// saving a ISequence according to the GenBank file format.
        /// </summary>
        private static GenBankFormatter genBank = new GenBankFormatter();

        /// <summary>
        /// A singleton instance of FastaFormatter class which is capable of
        /// saving a ISequence according to the FASTA file format.
        /// </summary>
        private static FastaFormatter fasta = new FastaFormatter();

        /// <summary>
        /// A singleton instance of FastQFormatter class which is capable of
        /// saving a IQualitativeSequence according to the FASTQ file format.
        /// </summary>
        private static FastQFormatter fastq = new FastQFormatter();

        /// <summary>
        /// A singleton instance of GffFormatter class which is capable of
        /// saving a ISequence according to the GFF file format.
        /// </summary>
        private static GffFormatter gff = new GffFormatter();

        /// <summary>
        /// List of all supported sequence formatters.
        /// </summary>
        private static List<ISequenceFormatter> all = new List<ISequenceFormatter>() { 
            fasta,
            fastq,
            genBank,
            gff };

        /// <summary>
        /// Gets an instance of GenBankFormatter class which is capable of
        /// saving a ISequence according to the GenBank file format.
        /// </summary>
        public static GenBankFormatter GenBank
        {
            get
            {
                return genBank;
            }
        }

        /// <summary>
        /// Gets an instance of FastaFormatter class which is capable of
        /// saving a ISequence according to the FASTA file format.
        /// </summary>
        public static FastaFormatter Fasta
        {
            get
            {
                return fasta;
            }
        }

        /// <summary>
        /// Gets an instance of FastQFormatter class which is capable of
        /// saving a IQualitativeSequence according to the FASTQ file format.
        /// </summary>
        public static FastQFormatter FastQ
        {
            get
            {
                return fastq;
            }
        }

        /// <summary>
        /// Gets an instance of GffFormatter class which is capable of
        /// saving a ISequence according to the GFF file format.
        /// </summary>
        public static GffFormatter Gff
        {
            get
            {
                return gff;
            }
        }

        /// <summary>
        /// Gets the list of all formatters which is supported by the framework.
        /// </summary>
        public static IList<ISequenceFormatter> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns formatter which supports the specified file.
        /// </summary>
        /// <param name="fileName">File name for which the formatter is required.</param>
        /// <returns>If found returns the formatter as ISequenceFormatter else returns null.</returns>
        public static ISequenceFormatter FindFormatterByFile(string fileName)
        {
            ISequenceFormatter formatter = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (Helper.IsGenBank(fileName))
                {
                    formatter = new GenBankFormatter();
                }
                else if (fileName.EndsWith(Resource.GFF_FILEEXTENSION, StringComparison.InvariantCultureIgnoreCase))
                {
                    formatter = new GffFormatter();
                }
                else if (Helper.IsFasta(fileName))
                {
                    formatter = new FastaFormatter();
                }
                else if (Helper.IsFastQ(fileName))
                {
                    formatter = new FastQFormatter();
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
        static SequenceFormatters()
        {
            //get the registered formatter
            IList<ISequenceFormatter> registeredFormatters = GetSequenceFormatters(true);

            if (null != registeredFormatters && registeredFormatters.Count > 0)
            {
                foreach (ISequenceFormatter formatter in registeredFormatters)
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
        /// Gets all registered formatters in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered formatters</returns>
        private static IList<ISequenceFormatter> GetSequenceFormatters(bool includeAddinFolder)
        {
            IList<ISequenceFormatter> registeredFormatters = new List<ISequenceFormatter>();

            if (includeAddinFolder)
            {
                IList<ISequenceFormatter> addInFormatters;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInFormatters = RegisteredAddIn.GetInstancesFromAssemblyPath<ISequenceFormatter>(RegisteredAddIn.AddinFolderPath, RegisteredAddIn.DLLFilter);
                    if (null != addInFormatters && addInFormatters.Count > 0)
                    {
                        foreach (ISequenceFormatter formatter in addInFormatters)
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
