// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Runtime.Serialization;

namespace MBF.Encoding
{
    /// <summary>
    /// Defines the interface for an implementation of a decoder that is able
    /// to convert byte values representing ISequenceItems into instances of
    /// ISequenceItem.
    /// </summary>
    public interface ISequenceDecoder : ISerializable
    {
        /// <summary>
        /// Converts a byte value representation of a sequence item into an
        /// ISequenceItem representation from the IEncoding specified for this
        /// instance of the decoder.
        /// </summary>
        /// <param name="value">The internal byte representation of an ISequenceItem</param>
        ISequenceItem Decode(byte value);
    }
}
