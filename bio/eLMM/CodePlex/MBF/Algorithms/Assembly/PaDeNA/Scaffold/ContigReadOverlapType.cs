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
    /// Overlap between Read and Contig 
    /// </summary>
    public enum ContigReadOverlapType
    {
        /// <summary>
        /// FullOverlap 
        /// ------------- Contig
        ///    ------     Read
        /// </summary>
        FullOverlap,

        /// <summary>
        /// PartialOverlap
        /// -------------       Contig
        ///            ------   Read
        /// </summary>
        PartialOverlap
    }
}
