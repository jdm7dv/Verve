// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF
{
    /// <summary>
    /// Interface to virtual sequence list. This interface is used for 
    /// Large Dataset on Data Virtualization. Example, FastA file has 
    /// more than one sequence and Data Virtualization returns this 
    /// interface as a starting. Then, on demand each sequences
    /// are loaded from the FastA file.
    /// Classes which implements this interface should hold virtual list of sequence.
    /// </summary>
    public interface IVirtualSequenceList : IList<ISequence>
    {
        //IVirtualSequenceList is extension of IList<ISequence>, which makes the 
        //existing modules to work without any issue and currently there is no specific 
        //methods or properties.
    }
}

