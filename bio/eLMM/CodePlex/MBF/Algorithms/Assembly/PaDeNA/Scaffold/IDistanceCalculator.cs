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



namespace Bio.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Classes implementing interface calculates distance between contigs using 
    /// mate pair information.
    /// </summary>
    public interface IDistanceCalculator
    {
        /// <summary>
        /// Calculates distances between contigs.
        /// </summary>
        /// <param name="contigPairedReads">Input Contigs and mate pair mappping.s</param>
        void CalculateDistance(ContigMatePairs contigPairedReads);
    }
}
