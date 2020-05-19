// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Interface is used by classes that maps mate pairs to contigs. 
    /// </summary>
    public interface IReadContigMapper
    {
        /// <summary>
        /// Map reads to contigs.
        /// Reads are aligned to contigs for distance calculation between
        /// contigs using mate pair library information, which will aid in scaffold building. 
        /// </summary>
        /// <param name="contigs">List of contig sequences</param>
        /// <param name="reads">List of paired reads to be mapped</param>
        /// <param name="kmerLength">Length of kmer</param>
        /// <returns>Read contig Map</returns>
        ReadContigMap Map(IList<ISequence> contigs, IList<ISequence> reads, int kmerLength);
    }
}
