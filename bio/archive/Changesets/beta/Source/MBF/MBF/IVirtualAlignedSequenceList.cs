// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Alignment;

namespace MBF
{
    /// <summary>
    /// Interface to a virtual aligned sequence list. This interface is used for 
    /// data virtualization on large datasets. 
    /// For example, if a SAM file has more than one sequence and data virtualization 
    /// is enabled, this interface will be returned. Then each sequence is loaded on 
    /// demand from the SAM file when it is first accessed.
    /// Classes which implement this interface should hold a virtual list of sequences.
    /// </summary>
    public interface IVirtualAlignedSequenceList<T> : IList<T> where T : IAlignedSequence
    {
        // IVirtualAlignedSequenceList is an extension of IList<IAlignedSequence> 
        // which allows extensibility.
        // Currently there are no specific methods or properties.
    }
}

