// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF
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
