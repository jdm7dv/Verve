// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.IO.BAM
{
    /// <summary>
    /// Class to hold BAMIndex information.
    /// </summary>
    public class BAMIndex
    {
        #region Properties
        /// <summary>
        /// Gets list of reference indices.
        /// </summary>
        public IList<BAMReferenceIndexes> RefIndexes { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of BAMIndex class.
        /// </summary>
        public BAMIndex()
        {
            RefIndexes = new List<BAMReferenceIndexes>();
        }
        #endregion
    }
}
