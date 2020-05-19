// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// This interface defines the contract that has to be implemented by and class implementing suffix tree.
    /// </summary>
    public interface ISuffixTree
    {
        /// <summary>
        /// Gets sequence of Suffix Tree.
        /// </summary>
        ISequence Sequence { get; }
    }
}
