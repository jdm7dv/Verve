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

namespace MBF.IO.GenBank
{
    /// <summary>
    /// Accession is identifier assigned to each GenBank sequence record.
    /// It contains primary accession number and may contain secondary accession numbers.
    /// </summary>
    [Serializable]
    public class GenBankAccession : ICloneable
    {
        #region Constructors
        /// <summary>
        /// Default Constructor.
        /// Creates a new GenBankAccession instance.
        /// </summary>
        public GenBankAccession()
        {
            Secondary = new List<string>();
        }

        /// <summary>
        /// Private Constructor for clone method.
        /// </summary>
        /// <param name="other">GenBankAccession instance to clone.</param>
        private GenBankAccession(GenBankAccession other)
        {
            Primary = other.Primary;
            Secondary = new List<string>(other.Secondary);
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Primary accession number.
        /// </summary>
        public string Primary { get; set; }

        /// <summary>
        /// List of secondary accession numbers.
        /// </summary>
        public IList<string> Secondary { get; private set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Validates whether the specified accession number is present in 
        /// this Accession as primary or secondary accession number.
        /// </summary>
        /// <param name="accession">Accession number.</param>
        /// <returns>If found returns true else returns false.</returns>
        public bool Contains(string accession)
        {
            if (string.Compare(Primary, accession, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }

            foreach (string str in Secondary)
            {
                if (string.Compare(str, accession, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a new GenBankAccession that is a copy of the current GenBankAccession.
        /// </summary>
        /// <returns>A new GenBankAccession that is a copy of this GenBankAccession.</returns>
        public GenBankAccession Clone()
        {
            return new GenBankAccession(this);
        }
        #endregion Methods

        #region ICloneable Members
        /// <summary>
        /// Creates a new GenBankAccession that is a copy of the current GenBankAccession.
        /// </summary>
        /// <returns>A new object that is a copy of this GenBankAccession.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion  ICloneable Members
    }
}
