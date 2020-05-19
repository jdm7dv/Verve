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
    /// Interface to virtual sequence data.
    /// Classes which implement this interface should hold virtual data of sequence.
    /// </summary>
    public interface IVirtualQualitativeSequenceProvider : IVirtualSequenceProvider
    {
        /// <summary>
        /// Get the quality scores of a particular sequence.
        /// </summary>
        /// <returns>The quality scores.</returns>
        byte[] GetScores();
    }
}
