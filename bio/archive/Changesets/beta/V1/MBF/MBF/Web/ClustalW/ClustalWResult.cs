// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using MBF.Algorithms.Alignment;
using MBF.Phylogenetics;

namespace MBF.Web.ClustalW
{
    /// <summary>
    /// Represent a result of ClustalW Web service.
    /// ClustalW will return either of the following
    ///  SequenceAlignment: If the operation performed is Alignment.
    ///  Tree: If the operation performed is tree for multiple sequences.
    /// </summary>
    public class ClustalWResult
    {
        /// <summary>
        /// Sequence Alignment result of ClustalW
        /// </summary>
        private ISequenceAlignment _sequenceAlignment;

        /// <summary>
        /// Tree result of ClustalW
        /// </summary>
        private Tree _tree;

        /// <summary>
        /// Constructor to set the SequenceAlignment result
        /// </summary>
        /// <param name="sequenceAlignment">Sequence alignment object</param>
        public ClustalWResult(ISequenceAlignment sequenceAlignment)
        {
            _sequenceAlignment = sequenceAlignment;
        }

        /// <summary>
        /// Constructor to set the Tree result
        /// </summary>
        /// <param name="tree">Phylogentic Tree object</param>
        public ClustalWResult(Tree tree)
        {
            _tree = tree;
        }

        /// <summary>
        /// Gets the SequenceAlignment result of ClustalW
        /// </summary>
        public ISequenceAlignment SequenceAlignment
        {
            get { return _sequenceAlignment; }
        }

        /// <summary>
        /// Gets the Tree result of ClustalW
        /// </summary>
        public Tree Tree
        {
            get { return _tree; }
        }
    }
}