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
    /// CrossReferenceLink provides cross-references to resources that support the existence 
    /// a sequence record, such as the Project Database and the NCBI 
    /// Trace Assembly Archive.
    /// </summary>
    [Serializable]
    public class CrossReferenceLink : ICloneable
    {
        #region Constructors
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public CrossReferenceLink()
        {
            Numbers = new List<string>();
        }

        /// <summary>
        /// Private Constructor for clone method.
        /// </summary>
        /// <param name="other">CrossReferenceLink instance to clone.</param>
        private CrossReferenceLink(CrossReferenceLink other)
        {
            Type = other.Type;
            Numbers = new List<string>(other.Numbers);
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// A CrossReferenceType specifies whether the DBLink is 
        /// refering to project or a Trace Assembly Archive.
        /// </summary>
        public CrossReferenceType Type { get; set; }

        /// <summary>
        /// Project numbers.
        /// </summary>
        public IList<string> Numbers { get; private set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new CrossReferenceLink that is a copy of the current CrossReferenceLink.
        /// </summary>
        /// <returns>A new CrossReferenceLink that is a copy of this CrossReferenceLink.</returns>
        public CrossReferenceLink Clone()
        {
            return new CrossReferenceLink(this);
        }
        #endregion Methods

        #region ICloneable Members
        /// <summary>
        /// Creates a new CrossReferenceLink that is a copy of the current CrossReferenceLink.
        /// </summary>
        /// <returns>A new object that is a copy of this CrossReferenceLink.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion ICloneable Members
    }
}
