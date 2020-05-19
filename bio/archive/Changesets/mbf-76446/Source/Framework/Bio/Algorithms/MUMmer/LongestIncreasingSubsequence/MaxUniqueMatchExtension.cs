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

namespace Bio.Algorithms.MUMmer.LIS
{
    /// <summary>
    /// This is an extension to MaxUniqueMatch class. And add its own set of properties
    /// to existing properties of MUM
    /// </summary>
    public class MaxUniqueMatchExtension : MaxUniqueMatch
    {
        /// <summary>
        /// Gets or sets cluster Identifier
        /// </summary>
        public long ID;

        /// <summary>
        /// Gets or sets a value indicating whether MUM is Good candidate
        /// </summary>
        public bool IsGood;

        /// <summary>
        /// Gets or sets a value indicating whether MUM is Tentative candidate
        /// </summary>
        public bool IsTentative;

        /// <summary>
        /// Gets or sets score of MUM
        /// </summary>
        public long Score;

        /// <summary>
        /// Gets or sets offset to adjacent MUM
        /// </summary>
        public long Adjacent;

        /// <summary>
        /// Gets or sets From (index representing the previous MUM to form LIS) of MUM
        /// </summary>
        public long From;

        /// <summary>
        /// Gets or sets wrap score
        /// </summary>
        public long WrapScore;

        /// <summary>
        /// Initializes a new instance of the MaxUniqueMatchExtension class
        /// </summary>
        /// <param name="mum">Maximum Unique Match</param>
        public MaxUniqueMatchExtension(MaxUniqueMatch mum)
        {
            if (mum == null)
            {
                throw new ArgumentNullException("mum");
            }

            mum.CopyTo(this);
            this.IsGood = false;
            this.IsTentative = false;
        }

        /// <summary>
        /// Copy the content to match
        /// </summary>
        /// <param name="match">Maximum unique match</param>
        public void CopyTo(MaxUniqueMatchExtension match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            match.ID = this.ID;
            match.IsGood = this.IsGood;
            match.IsTentative = this.IsTentative;
            match.Score = this.Score;
            match.Adjacent = this.Adjacent;
            match.From = this.From;
            match.WrapScore = this.WrapScore;

            this.CopyTo((MaxUniqueMatch)match);
        }
    }
}