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
using System.Runtime.Serialization;

namespace MBF.Algorithms.Assembly
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
