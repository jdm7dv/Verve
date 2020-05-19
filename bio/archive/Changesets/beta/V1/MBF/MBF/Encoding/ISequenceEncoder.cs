// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBF.Encoding
{
    /// <summary>
    /// Defines the interface for an implementation of an encoder that is able
    /// to convert ISequenceItems into byte values for internal storage of those
    /// items.
    /// </summary>
    public interface ISequenceEncoder : ISerializable
    {
        /// <summary>
        /// Encodes the source sequence onto a byte array that has already been
        /// allocated its size. The encoding will begin at the position held in
        /// the offset parameter (indexed counting from 0). If the target array
        /// is not sufficiently large enough to handle the encoding, the encoding
        /// will stop once the end of the array is reached.
        /// </summary>
        /// <param name="source">The data to be encoded</param>
        /// <param name="target">The array into which the encoded values will be placed</param>
        /// <param name="offset">
        /// The start index of where the encoding will take place. Counting for this
        /// offset starts at position zero. For instance,
        /// if the target array has a length of 10 and the source has a length of 5
        /// and the offset 7, the last 3 entries of the target array will get the
        /// encoded values of the first 3 items from the source.
        /// </param>
        void Encode(List<ISequenceItem> source, byte[] target, int offset);

        /// <summary>
        /// Encodes the source sequence onto a byte array that has already been
        /// allocated its size. The encoding will start at the beginning of the
        /// target array and will end when reaching either the end of the source
        /// or the target array.
        /// </summary>
        /// <param name="source">The data to be encoded</param>
        /// <param name="target">The array into which the encoded values will be placed</param>
        void Encode(List<ISequenceItem> source, byte[] target);

        /// <summary>
        /// Encodes the source sequence onto a byte array. The array will be the
        /// size of the source when returned.
        /// </summary>
        /// <param name="source">The data to be encoded</param>
        /// <returns>The array into which the encoded values will be placed</returns>
        byte[] Encode(List<ISequenceItem> source);

        /// <summary>
        /// Encodes a single sequence item into its byte value
        /// </summary>
        byte Encode(ISequenceItem item);

        /// <summary>
        /// Encodes the source sequence onto a byte array that has already been
        /// allocated its size. The encoding will begin at the position held in
        /// the offset parameter (indexed counting from 0). If the target array
        /// is not sufficiently large enough to handle the encoding, the encoding
        /// will stop once the end of the array is reached.
        /// </summary>
        /// <param name="source">The data to be encoded (eg. "GATTC")</param>
        /// <param name="target">The array into which the encoded values will be placed</param>
        /// <param name="offset">
        /// The start index of where the encoding will take place. Counting for this
        /// offset starts at position zero. For instance,
        /// if the target array has a length of 10 and the source has a length of 5
        /// and the offset 7, the last 3 entries of the target array will get the
        /// encoded values of the first 3 items from the source.
        /// </param>
        void Encode(string source, byte[] target, int offset);

        /// <summary>
        /// Encodes the source sequence onto a byte array that has already been
        /// allocated its size. The encoding will start at the beginning of the
        /// target array and will end when reaching either the end of the source
        /// or the target array.
        /// </summary>
        /// <param name="source">The data to be encoded (eg. "GATTC")</param>
        /// <param name="target">The array into which the encoded values will be placed</param>
        void Encode(string source, byte[] target);

        /// <summary>
        /// Encodes the source sequence onto a byte array. The array will be the
        /// size of the source when returned.
        /// </summary>
        /// <param name="source">The data to be encoded (eg. "TAGGC")</param>
        /// <returns>The array into which the encoded values will be placed</returns>
        byte[] Encode(string source);

        /// <summary>
        /// Encodes a single sequence item symbol into its byte value
        /// </summary>
        byte Encode(char symbol);
    }
}
