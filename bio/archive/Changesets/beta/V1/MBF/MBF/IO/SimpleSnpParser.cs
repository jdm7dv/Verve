// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.IO;

using MBF.Encoding;
using MBF.Properties;
using System;

namespace MBF.IO
{

    /// <summary>
    /// Simple SNP Parser that uses an XsvSnpReader for parsing files with 
    /// chromosome number, position, allele 1 and allele 2 in tab separated 
    /// columns into sequences with the first allele.
    /// </summary>
    public class SimpleSnpParser : SnpParser
    {

        #region Constructor
        /// <summary>
        /// Creates a SimpleSnpParser which generates parsed sequences that use the the 
        /// given alphabet and encoding.
        /// NOTE: Given that this parses Snps, should we always use the DnaAlphabet?
        /// </summary>
        /// <param name="encoding">Encoding to use in the sequence parsed.</param>
        /// <param name="alphabet">Alphabet to use in the sequence parsed.</param>
        public SimpleSnpParser (IEncoding encoding, IAlphabet alphabet)
            : base(encoding, alphabet)
        {

        }

        /// <summary>
        /// Creates a SimpleSnpParser which generates parsed sequences that use the the 
        /// given encoding and the DnaAlphabet.
        /// </summary>
        /// <param name="encoding">Encoding to use in the sequence parsed.</param>
        public SimpleSnpParser (IEncoding encoding) : this(encoding, DnaAlphabet.Instance)
        {
        }

        /// <summary>
        /// Creates a SimpleSnpParser which generates parsed sequences that use the the 
        /// Ncbi4NAEncoding and DnaAlphabet.
        /// </summary>
        public SimpleSnpParser () : this(Ncbi4NAEncoding.Instance, DnaAlphabet.Instance)
        {
        }

        #endregion Constructor

        #region Overrides of SnpParser

        /// <summary>
        /// Gets the name of the parser. 
        /// </summary>
        public override string Name
        {
            get
            {
                return Resource.SIMPLE_SNP_NAME;
            }
        }

        /// <summary>
        /// Gets the description of the parser.
        /// </summary>
        public override string Description
        {
            get
            {
                return Resource.SIMPLE_SNP_DESCRIPTION;
            }
        }

        /// <summary>
        /// Gets the filetypes supported by the parser.
        /// </summary>
        public override string FileTypes
        {
            get
            {
                return Resource.SIMPLE_SNP_FILEEXTENSION;
            }
        }

        /// <summary>
        /// Returns a XsvSnpReader that wraps the given TextReader. 
        /// Reads SNP Files with the following tab separated value format:
        /// chrom   pos     allele1 allele2
        /// 1       45162   C       T
        /// 1       72434   G       A
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Returns the ISnpReader created for the TextReader</returns>
        protected override ISnpReader GetSnpReader (TextReader reader)
        {
            // Check input arguments
            if (reader == null) 
            {
                throw new ArgumentNullException("reader", "Text reader to read SNP sequences from cannot be null");
            }

            return new XsvSnpReader(reader, new[] { '\t' }, true, true, 0, 1, 2, 3);
        }

        #endregion
    }
}