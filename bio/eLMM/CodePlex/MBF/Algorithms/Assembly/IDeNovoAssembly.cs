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
using System.Runtime.Serialization;

namespace Bio.Algorithms.Assembly
{
    /// <summary>
    /// An IDeNovoAssembly is the result of running De Novo Assembly on a set of two or more sequences. 
    /// </summary>
    public interface IDeNovoAssembly : ISerializable
    {
        /// <summary>
        /// Gets list of sequences created after Assembly.
        /// </summary>
        IList<ISequence> AssembledSequences { get; }

        /// <summary>
        /// Gets or sets the Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a IDeNovoAssembly. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        Object Documentation { set; get; }
    }
}
