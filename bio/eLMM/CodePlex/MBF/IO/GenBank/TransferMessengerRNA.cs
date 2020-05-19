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
    /// Transfer messenger RNA; tmRNA acts as a tRNA first, and then as an mRNA that encodes a peptide tag; 
    /// the ribosome translates this mRNA region of tmRNA and attaches the encoded peptide tag to the 
    /// C-terminus of the unfinished protein; this attached tag targets the protein for destruction or proteolysis.
    /// </summary>
    [Serializable]
    public class TransferMessengerRNA : FeatureItem
    {
        #region Constructors
        /// <summary>
        /// Creates new TransferMessengerRNA feature item from the specified location.
        /// </summary>
        /// <param name="location">Location of the TransferMessengerRNA.</param>
        public TransferMessengerRNA(ILocation location)
            : base(StandardFeatureKeys.TransferMessengerRNA, location) { }

        /// <summary>
        /// Creates new TransferMessengerRNA feature item with the specified location.
        /// Note that this constructor uses LocationBuilder to construct location object from the specified 
        /// location string.
        /// </summary>
        /// <param name="location">Location of the TransferMessengerRNA.</param>
        public TransferMessengerRNA(string location)
            : base(StandardFeatureKeys.TransferMessengerRNA, location) { }

        /// <summary>
        /// Private constructor for clone method.
        /// </summary>
        /// <param name="other">Other TransferMessengerRNA instance.</param>
        private TransferMessengerRNA(TransferMessengerRNA other)
            : base(other) { }
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Base location encoding the polypeptide for proteolysis tag of tmRNA and its termination codon.
        /// </summary>
        public string TagPeptide
        {
            get
            {
                return GetSingleTextQualifier(StandardQualifierNames.TagPeptide);
            }

            set
            {
                SetSingleTextQualifier(StandardQualifierNames.TagPeptide, value);
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new TransferMessengerRNA that is a copy of the current TransferMessengerRNA.
        /// </summary>
        /// <returns>A new TransferMessengerRNA that is a copy of this TransferMessengerRNA.</returns>
        public new TransferMessengerRNA Clone()
        {
            return new TransferMessengerRNA(this);
        }
        #endregion Methods
    }
}
