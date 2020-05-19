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



using System.Collections.Generic;
using Bio.Algorithms.Assembly.Graph;

namespace Bio.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// Framework for building contig sequence from de bruijn graph.
    /// </summary>
    public interface IContigBuilder
    {
        /// <summary>
        /// Contructs the contigs by performing graph walking
        /// or graph modification.
        /// </summary>
        /// <param name="graph">Input graph</param>
        /// <returns>List of contigs</returns>
        IList<ISequence> Build(DeBruijnGraph graph);
    }
}
