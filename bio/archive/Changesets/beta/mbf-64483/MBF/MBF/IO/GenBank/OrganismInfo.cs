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
    /// Provides Genus, Species and taxonomic classification levels of the sequence.
    /// </summary>
    [Serializable]
    public class OrganismInfo : ICloneable
    {
        #region Properties
        /// <summary>
        /// Genus name of the Organism.
        /// </summary>
        public string Genus { get; set; }

        /// <summary>
        /// Species of the Oraganism.
        /// </summary>
        public string Species { get; set; }

        /// <summary>
        /// Taxonomic classification levels of the Organism.
        /// </summary>
        public string ClassLevels { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new OrganismInfo that is a copy of the current OrganismInfo.
        /// </summary>
        /// <returns>A new OrganismInfo that is a copy of this OrganismInfo.</returns>
        public OrganismInfo Clone()
        {
            return (OrganismInfo)this.MemberwiseClone();
        }
        #endregion Methods

        #region ICloneable Members
        /// <summary>
        /// Creates a new OrganismInfo that is a copy of the current OrganismInfo.
        /// </summary>
        /// <returns>A new object that is a copy of this OrganismInfo.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion ICloneable Members
    }
}
