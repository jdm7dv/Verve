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
namespace Bio.Algorithms.Assembly.Padena.Scaffold
{
    /// <summary>
    /// Overlap between Read and Contig.
    /// </summary>
    public enum ContigReadOverlapType
    {
        /// <summary>
        /// FullOverlap.
        /// ------------- Contig
        ///    ------     Read
        /// </summary>
        FullOverlap,

        /// <summary>
        /// PartialOverlap.
        /// -------------       Contig
        ///            ------   Read
        /// </summary>
        PartialOverlap
    }
}
