// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.IO.GenBank
{
    /// <summary>
    /// A compound identifier consisting of the primary accession number and 
    /// a numeric version number associated with the current version of the 
    /// sequence data in the record. This is followed by an integer key 
    /// (a "GI") assigned to the sequence by NCBI.
    /// </summary>
    [Serializable]
    public class GenBankVersion : ICloneable
    {
        #region Properties
        /// <summary>
        /// Primary accession number.
        /// </summary>
        public string Accession { get; set; }

        /// <summary>
        /// Version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets the CompoundAccession that is Accession.Version.
        /// </summary>
        public string CompoundAccession
        {
            get
            {
                return Accession + "." + Version;
            }
        }

        /// <summary>
        /// GI number.
        /// </summary>
        public string GINumber { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new GenBankVersion that is a copy of the current GenBankVersion.
        /// </summary>
        /// <returns>A new GenBankVersion that is a copy of this GenBankVersion.</returns>
        public GenBankVersion Clone()
        {
            return (GenBankVersion)this.MemberwiseClone();
        }
        #endregion Methods

        #region ICloneable Members
        /// <summary>
        /// Creates a new GenBankVersion that is a copy of the current GenBankVersion.
        /// </summary>
        /// <returns>A new object that is a copy of this GenBankVersion.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion ICloneable Members
    }
}
