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

// Copyright (c) Microsoft Corporation 2009, 2010.  All rights reserved.

using System.Collections.Generic;

namespace Bio.IO.BAM
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
