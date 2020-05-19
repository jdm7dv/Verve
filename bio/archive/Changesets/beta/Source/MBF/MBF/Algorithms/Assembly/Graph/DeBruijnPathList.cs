// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;

namespace MBF.Algorithms.Assembly.Graph
{
    /// <summary>
    /// Class representing the list of paths in de bruijn graph
    /// </summary>
    public class DeBruijnPathList
    {
        /// <summary>
        /// List of paths
        /// </summary>
        List<DeBruijnPath> _paths;

        /// <summary>
        /// Initializes a new instance of the DeBruijnPathList class.
        /// </summary>
        public DeBruijnPathList()
        {
            _paths = new List<DeBruijnPath>();
        }

        /// <summary>
        /// Initializes a new instance of the DeBruijnPathList class.
        /// Adds elements in input enumerable type to list.
        /// </summary>
        /// <param name="paths">List of paths</param>
        public DeBruijnPathList(IEnumerable<DeBruijnPath> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            _paths = new List<DeBruijnPath>(paths);
        }

        /// <summary>
        /// Gets list of paths
        /// </summary>
        public IList<DeBruijnPath> Paths
        {
            get { return _paths; }
        }

        /// <summary>
        /// Add the given list of paths to local variable
        /// </summary>
        /// <param name="paths">List of paths to add</param>
        public void AddPaths(IList<DeBruijnPath> paths)
        {
            _paths.AddRange(paths);
        }
    }
}
