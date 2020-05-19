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
using System.Collections.Generic;

namespace Bio.Algorithms.Assembly.Padena.Scaffold
{
    /// <summary>
    /// Filter mate pairs based on support for contig orientation.
    /// The mate pairs support specific orientation of contigs, 
    /// based on mapping of reverse read or forward read to specify orientation.
    /// Orientation 1
    /// ----------) (------------- 
    /// contig 1      contig 2
    /// 
    /// Orientation 2
    /// ----------) (-------------
    /// 
    /// contig 2      contig 1
    /// </summary>
    public interface IOrientationBasedMatePairFilter
    {
        /// <summary>
        /// Filter mate pairs.
        /// </summary>
        /// <param name="matePairMap">Dictionary of Map between contigs using mate pair information.</param>
        /// <param name="redundancy">Number of mate pairs require to create a link 
        /// between two contigs.</param>
        /// <returns>List of contig mate pairs.</returns>
        ContigMatePairs FilterPairedReads(ContigMatePairs matePairMap, int redundancy = 2);
    }
}
