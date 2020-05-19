//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;
using System.Collections.Generic;

namespace Bio.Algorithms.Assembly.Graph
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
    }

    
}
