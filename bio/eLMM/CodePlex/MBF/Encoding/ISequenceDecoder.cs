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



using System.Runtime.Serialization;

namespace Bio.Encoding
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
