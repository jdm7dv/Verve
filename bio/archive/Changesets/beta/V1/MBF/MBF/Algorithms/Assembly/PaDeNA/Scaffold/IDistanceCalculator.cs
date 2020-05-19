// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Algorithms.Assembly.PaDeNA.Scaffold
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
