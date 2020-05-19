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
    /// Source provides the common name of the organism or the name most frequently used
    /// in the literature along with the taxonomic classification levels 
    /// </summary>
    [Serializable]
    public class SequenceSource : ICloneable
    {
        #region Constructors
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public SequenceSource()
        {
            Organism = new OrganismInfo();
        }

        /// <summary>
        /// Private Constructor for clone method.
        /// </summary>
        /// <param name="other">SequenceSource instance to clone.</param>
        private SequenceSource(SequenceSource other)
        {
            CommonName = other.CommonName;
            if (other.Organism != null)
            {
                Organism = other.Organism.Clone();
            }
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Common name of the organism.
        /// </summary>
        public string CommonName { get; set; }

        /// <summary>
        /// Provides Genus, Species and taxonomic classification levels 
        /// </summary>
        public OrganismInfo Organism { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new SequenceSource that is a copy of the current SequenceSource.
        /// </summary>
        /// <returns>A new SequenceSource that is a copy of this SequenceSource.</returns>
        public SequenceSource Clone()
        {
            return new SequenceSource(this);
        }
        #endregion Methods

        #region ICloneable Members

        /// <summary>
        /// Creates a new SequenceSource that is a copy of the current SequenceSource.
        /// </summary>
        /// <returns>A new object that is a copy of this SequenceSource.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion ICloneable Members
    }
}
