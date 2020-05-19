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
    /// Mum class will hold order and sequence start position of
    /// two input sequences. 
    /// </summary>
    public class MaxUniqueMatch
    {
        /// <summary>
        /// Gets or sets Mum length 
        /// </summary>
        public long Length;

        /// <summary>
        /// Gets or sets Sequence one's MaxUniqueMatch order 
        /// </summary>
        public long FirstSequenceMumOrder;

        /// <summary>
        /// Gets or sets Sequence one - MaxUniqueMatch's starting poistion 
        /// </summary>
        public long ReferenceSequenceOffset;

        /// <summary>
        /// Gets or sets Sequence Two's MaxUniqueMatch order 
        /// </summary>
        public long SecondSequenceMumOrder;

        /// <summary>
        /// Gets or sets  Sequence Two - MaxUniqueMatch's starting poistion 
        /// </summary>
        public long QuerySequenceOffset; 

        /// <summary>
        /// Gets or sets the Query sequence
        /// </summary>
        public ISequence Query;

        /// <summary>
        /// Copy the content to MUM
        /// </summary>
        /// <param name="match">Maximun unique match</param>
        public void CopyTo(MaxUniqueMatch match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            match.FirstSequenceMumOrder = this.FirstSequenceMumOrder;
            match.ReferenceSequenceOffset = this.ReferenceSequenceOffset;
            match.SecondSequenceMumOrder = this.SecondSequenceMumOrder;
            match.QuerySequenceOffset = this.QuerySequenceOffset;
            match.Length = this.Length;
            match.Query = this.Query;
        }
    }
}