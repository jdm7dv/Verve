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
    /// Removes containing paths and merge Overlapping scaffold paths.
    /// Containing Paths
    /// -------------- Contig 1
    ///     --------   Contig 2
    /// Overlapping Paths
    /// --------        Contig 1 
    ///     ---------   Contig 2
    /// </summary>
    public interface IPathPurger
    {
        /// <summary>
        /// Removes containing paths and merge overlapping paths.
        /// </summary>
        /// <param name="scaffoldPaths">Input paths/scaffold.</param>
        void PurgePath(IList<ScaffoldPath> scaffoldPaths);
    }
}
