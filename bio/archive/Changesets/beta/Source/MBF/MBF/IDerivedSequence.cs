// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF
{
    /// <summary>
    /// Classes which implements this interface will provide additional functionality 
    /// for the source sequence.
    /// </summary>
    public interface IDerivedSequence : ISequence
    {
        /// <summary>
        /// The sequence (if any) from which this sequence was derived.
        /// </summary>
        ISequence Source { get; }
    }
}
