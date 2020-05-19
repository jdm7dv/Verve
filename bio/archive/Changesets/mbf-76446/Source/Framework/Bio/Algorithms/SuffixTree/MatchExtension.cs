// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bio.Algorithms.SuffixTree
{
    /// <summary>
    /// Maximum Unique Match Class.
    /// </summary>
    public class MatchExtension
    {
        /// <summary>
        /// Gets or sets the length of match.
        /// </summary>
        public long Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start index of this match in reference sequence.
        /// </summary>
        public long ReferenceSequenceOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets start index of this match in query sequence.
        /// </summary>
        public long QuerySequenceOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Sequence one's MaxUniqueMatch order. 
        /// </summary>
        public long ReferenceSequenceMumOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Sequence Two's MaxUniqueMatch order.
        /// </summary>
        public long QuerySequenceMumOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Query sequence.
        /// </summary>
        public ISequence Query
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets cluster Identifier.
        /// </summary>
        public long ID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether MUM is Good candidate.
        /// </summary>
        public bool IsGood
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether MUM is Tentative candidate.
        /// </summary>
        public bool IsTentative
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets score of MUM.
        /// </summary>
        public long Score
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets offset to adjacent MUM.
        /// </summary>
        public long Adjacent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets From (index representing the previous MUM to form LIS) of MUM.
        /// </summary>
        public long From
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets wrap score.
        /// </summary>
        public long WrapScore
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the MaxUniqueMatchExtension class
        /// </summary>
        public MatchExtension()
        {

        }

        /// <summary>
        /// Initializes a new instance of the MaxUniqueMatchExtension class
        /// </summary>
        /// <param name="mum">Maximum Unique Match</param>
        public MatchExtension(Match mum)
        {
            //mum.CopyTo(this);
            this.ReferenceSequenceOffset = mum.ReferenceSequenceOffset;
            this.QuerySequenceOffset = mum.QuerySequenceOffset;
            this.Length = mum.Length;
            this.IsGood = false;
            this.IsTentative = false;
        }

        /// <summary>
        /// Copy the content to MUM.
        /// </summary>
        /// <param name="match">Maximum unique match.</param>
        public void CopyTo(MatchExtension match)
        {
            if (match != null)
            {
                match.ReferenceSequenceMumOrder = this.ReferenceSequenceMumOrder;
                match.ReferenceSequenceOffset = this.ReferenceSequenceOffset;
                match.QuerySequenceMumOrder = this.QuerySequenceMumOrder;
                match.QuerySequenceOffset = this.QuerySequenceOffset;
                match.Length = this.Length;
                match.Query = this.Query;

                match.ID = this.ID;
                match.IsGood = this.IsGood;
                match.IsTentative = this.IsTentative;
                match.Score = this.Score;
                match.Adjacent = this.Adjacent;
                match.From = this.From;
                match.WrapScore = this.WrapScore;
            }
        }
    }
}