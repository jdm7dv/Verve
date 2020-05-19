// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Algorithms.Kmer
{
    /// <summary>
    /// This interface defines contract for classes that implement construction 
    /// of k-mer sequences from input sequences. It provides methods that take 
    /// input sequence(s) and construct corresponding k-mers.
    /// </summary>
    public interface IKmerBuilder
    {
        /// <summary>
        /// Builds k-mers for input sequence and constructs KmersOfSequence 
        /// corresponding to the sequence and associated k-mers.
        /// </summary>
        /// <param name="sequence">Input Sequence</param>
        /// <param name="kmerLength">k-mer length</param>
        /// <returns>KmersOfSequence constructed from sequence and associated k-mers</returns>
        KmersOfSequence Build(ISequence sequence, int kmerLength);

        /// <summary>
        /// Builds k-mers for a list of input sequences.
        /// For each sequence in input list, constructs a KmersOfSequence 
        /// corresponding to the sequence and associated k-mers.
        /// </summary>
        /// <param name="sequences">List of input sequences</param>
        /// <param name="kmerLength">k-mer length</param>
        /// <returns>List of KmersOfSequence instances</returns>
        IEnumerable<KmersOfSequence> Build(IList<ISequence> sequences, int kmerLength);
    }
}
