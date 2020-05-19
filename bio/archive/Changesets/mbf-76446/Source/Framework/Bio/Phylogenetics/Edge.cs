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
using System;

namespace Bio.Phylogenetics
{
    /// <summary>
    /// Edge: a tree edge and its descendant subtree.
    /// Edge --> Distance/Length
    /// </summary>
    public class Edge : ICloneable
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Edge()
        {
            //default distance is set to 0
            Distance = 0;
        }
        #endregion Constructors

        #region -- Properties --
        /// <summary>
        /// Length of a tree edge.
        /// </summary>
        public double Distance { set; get; }
        #endregion -- Properties --

        #region -- Methods --
        /// <summary>
        /// Clone object
        /// </summary>
        /// <returns>Edge as object</returns>
        public object Clone()
        {
            Edge newEdge = (Edge)this.MemberwiseClone();
            return newEdge;
        }
        #endregion -- Methods --
    }

}
