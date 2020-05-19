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



namespace Bio
{
    /// <summary>
    /// Classes which implements this interface will provide additional functionality 
    /// for the source sequence.
    /// </summary>
    public interface IDerivedSequence : ISequence
    {
        /// <summary>
        /// The sequence (if any) from which this sequence was derived.
        /// </summary>
        ISequence Source { get; }
    }
}
