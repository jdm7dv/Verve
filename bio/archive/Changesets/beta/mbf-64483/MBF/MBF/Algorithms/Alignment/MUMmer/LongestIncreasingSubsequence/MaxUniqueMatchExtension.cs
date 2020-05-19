// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// This is an extension to MaxUniqueMatch class. And add its own set of properties
    /// to existing properties of MUM
    /// </summary>
    public class MaxUniqueMatchExtension : MaxUniqueMatch
    {
        /// <summary>
        /// Initializes a new instance of the MaxUniqueMatchExtension class
        /// </summary>
        /// <param name="mum">Maximum Unique Match</param>
        public MaxUniqueMatchExtension(MaxUniqueMatch mum)
        {
            mum.CopyTo(this);
            IsGood = false;
            IsTentative = false;
        }

        /// <summary>
        /// Gets or sets cluster Identifier
        /// </summary>
        public int ID;

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
        public int Score;

        /// <summary>
        /// Gets or sets offset to adjacent MUM
        /// </summary>
        public int Adjacent;

        /// <summary>
        /// Gets or sets From (index representing the previous MUM to form LIS) of MUM
        /// </summary>
        public int From;

        /// <summary>
        /// Gets or sets wrap score
        /// </summary>
        public int WrapScore;

        /// <summary>
        /// Copy the content to match
        /// </summary>
        /// <param name="match">Maximum unique match</param>
        public void CopyTo(MaxUniqueMatchExtension match)
        {
            match.ID = ID;
            match.IsGood = IsGood;
            match.IsTentative = IsTentative;
            match.Score = Score;
            match.Adjacent = Adjacent;
            match.From = From;
            match.WrapScore = WrapScore;

            this.CopyTo((MaxUniqueMatch)match);
        }
    }
}