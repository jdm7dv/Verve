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
using System.Linq;

namespace Bio.Algorithms.Assembly
{
    /// <summary>
    /// OverlapDeNovoAssembly is a implementation of IOverlapDeNovoAssembly that stores the 
    /// assembly result.
    /// This class contains list of contigs and list of unmerged sequences.
    /// To maintain the information like history or context, use Documentation property of this class.
    /// </summary>
    public class OverlapDeNovoAssembly : IOverlapDeNovoAssembly
    {
        #region Fields
        /// <summary>
        /// Holds list of contigs created after Assembly.
        /// </summary>
        private List<Contig> contigs;

        /// <summary>
        /// Holds list of sequences that could not be merged into any contig.
        /// </summary>
        private List<ISequence> unmergedSequences;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the OverlapDeNovoAssembly class.
        /// Default constructor.
        /// </summary>
        public OverlapDeNovoAssembly()
        {
            contigs = new List<Contig>();
            unmergedSequences = new List<ISequence>();
        }
        #endregion Constructors

        #region IOverlapLayoutSequenceAssembly Members
        /// <summary>
        /// Gets list of contigs created after Assembly.
        /// </summary>
        public IList<Contig> Contigs
        {
            get
            {
                return this.contigs;
            }
        }

        /// <summary>
        /// Gets list of sequences that could not be merged into any contig.
        /// </summary>
        public IList<ISequence> UnmergedSequences
        {
            get
            {
                return this.unmergedSequences;
            }
        }

        /// <summary>
        /// Gets the list of assembled sequences.
        /// </summary>
        public IList<ISequence> AssembledSequences
        {
            get { return (this.contigs.Select(c => c.Consensus).ToList()); }
        }

        /// <summary>
        /// Gets or sets the Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a OverlapDeNovoAssembly. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public object Documentation { get; set; }
        #endregion
    }
}
