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



namespace Bio.Algorithms.Alignment
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
        public int Length;

        /// <summary>
        /// Gets or sets Sequence one's MaxUniqueMatch order 
        /// </summary>
        public int FirstSequenceMumOrder;

        /// <summary>
        /// Gets or sets Sequence one - MaxUniqueMatch's starting poistion 
        /// </summary>
        public int FirstSequenceStart;

        /// <summary>
        /// Gets or sets Sequence Two's MaxUniqueMatch order 
        /// </summary>
        public int SecondSequenceMumOrder;

        /// <summary>
        /// Gets or sets  Sequence Two - MaxUniqueMatch's starting poistion 
        /// </summary>
        public int SecondSequenceStart; 

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
            match.FirstSequenceMumOrder = FirstSequenceMumOrder;
            match.FirstSequenceStart = FirstSequenceStart;
            match.SecondSequenceMumOrder = SecondSequenceMumOrder;
            match.SecondSequenceStart = SecondSequenceStart;
            match.Length = Length;
            match.Query = Query;
        }
    }
}
