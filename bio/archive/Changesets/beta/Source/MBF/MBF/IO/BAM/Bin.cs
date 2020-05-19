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

namespace MBF.IO.BAM
{
    /// <summary>
    /// Class to hold Bin information.
    /// An instance of this class can contain Bin number and list of chunks related to the bin number.
    /// </summary>
    public class Bin
    {
        #region Properties
        /// <summary>
        /// Gets or sets bin number.
        /// </summary>
        public UInt32 BinNumber { get; set; }

        /// <summary>
        /// Gets list of chunks.
        /// </summary>
        public IList<Chunk> Chunks { get; private set; }
        #endregion

        #region Contructos
        /// <summary>
        /// Creates an instance of Bin class.
        /// </summary>
        public Bin()
        {
            Chunks = new List<Chunk>();
        }
        #endregion
    }
}
