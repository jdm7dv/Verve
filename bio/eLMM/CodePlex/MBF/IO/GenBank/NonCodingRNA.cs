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

namespace Bio.IO.GenBank
{
    /// <summary>
    /// A non-protein-coding gene (ncRNA), other than ribosomal RNA and transfer RNA, the functional 
    /// molecule of which is the RNA transcript.
    /// </summary>
    [Serializable]
    public class NonCodingRNA : FeatureItem
    {
        #region Constructors
        /// <summary>
        /// Creates new NonCodingRNA feature item from the specified location.
        /// </summary>
        /// <param name="location">Location of the NonCodingRNA.</param>
        public NonCodingRNA(ILocation location)
            : base(StandardFeatureKeys.NonCodingRNA, location) { }

        /// <summary>
        /// Creates new NonCodingRNA feature item with the specified location.
        /// Note that this constructor uses LocationBuilder to construct location object from the specified 
        /// location string.
        /// </summary>
        /// <param name="location">Location of the NonCodingRNA.</param>
        public NonCodingRNA(string location)
            : base(StandardFeatureKeys.NonCodingRNA, location) { }

        /// <summary>
        /// Private constructor for clone method.
        /// </summary>
        /// <param name="other">Other NonCodingRNA instance.</param>
        private NonCodingRNA(NonCodingRNA other)
            : base(other) { }
        #endregion Constructors

        #region Properties

        /// <summary>
        /// ncRNA_class; A structured description of the classification of the non-coding RNA described by the ncRNA parent key.
        /// </summary>
        public string NonCodingRNAClass
        {
            get
            {
                return GetSingleTextQualifier(StandardQualifierNames.NonCodingRNAClass);
            }

            set
            {
                SetSingleTextQualifier(StandardQualifierNames.NonCodingRNAClass, value);
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new NonCodingRNA that is a copy of the current NonCodingRNA.
        /// </summary>
        /// <returns>A new NonCodingRNA that is a copy of this NonCodingRNA.</returns>
        public new NonCodingRNA Clone()
        {
            return new NonCodingRNA(this);
        }
        #endregion Methods
    }
}
