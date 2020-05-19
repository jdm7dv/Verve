// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Assembly.PaDeNA.Scaffold;

namespace MBF.Algorithms.Assembly
{
    /// <summary>
    /// Map Reads to Mate Pairs
    /// Interface can be implemented by classes which map reads to mate pairs
    /// in another input formats
    /// </summary>
    public interface IMatePairMapper
    {
        /// <summary>
        /// Map Reads to mate pairs.
        /// </summary>
        /// <param name="reads">List of reads.</param>
        /// <returns>List of mate pairs.</returns>
        IList<MatePair> Map(IList<ISequence> reads);

        /// <summary>
        /// Finds contig pairs having valid mate pairs connection between them.
        /// </summary>
        /// <param name="reads">Input list of reads.</param>
        /// <param name="alignment">Reads con alignment.</param>
        /// <returns>Contig Mate pair map.</returns>
        ContigMatePairs MapContigToMatePairs(IList<ISequence> reads, ReadContigMap alignment);
    }
}
