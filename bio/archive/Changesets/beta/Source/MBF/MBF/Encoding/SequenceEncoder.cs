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
using System.Linq;
using System.Runtime.Serialization;

namespace MBF.Encoding
{
    /// <summary>
    /// This basic implementation of the ISequenceEncoder provides a one to one
    /// translation from characters to byte values. It uses an IEncoding to determine
    /// what the mapping is from character to byte value. This encoding should be set
    /// in the constructor.
    /// 
    /// If one of the symbols passed in via string (charcters) or via ISequenceItem
    /// (ISequenceItem.Symbol) has a symbol that is not recognized by the IEncoding for
    /// your particular instance of this class, an Exception will be thrown when trying
    /// to encode that item.
    /// 
    /// For example, using the following code:
    /// 
    /// SequenceEncoder encoder = new SequenceEncoder(Encodings.Ncbi4Na);
    /// byte[] encoded = encoder.Encode("GATTC");
    /// 
    /// will result in a byte array with the values:
    /// 
    /// { 4, 1, 8, 8, 2 }
    ///
    /// 
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and from the stored files 
    /// instances can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class SequenceEncoder : ISequenceEncoder
    {
        private IEncoding encoding;

        /// <summary>
        /// The Encoding provides the map from symbol characters to byte values.
        /// Without a set encoding the encoder can not work.
        /// </summary>
        public IEncoding Encoding
        {
            get { return encoding; }
            set { this.encoding = value; }
        }

        /// <summary>
        /// Creates a sequence encoder by defining the encoding to use.
        /// </summary>
        public SequenceEncoder(IEncoding encoding)
        {
            if (encoding == null)
                throw new ArgumentException("The encoding paramater can not be null");

            this.encoding = encoding;
        }

        // Hide the default constructor
        private SequenceEncoder() { }

        #region ISequenceEncoder Members

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
        public void Encode(List<ISequenceItem> source, byte[] target, int offset)
        {
            if (encoding == null)
                throw new Exception("SequenceEncoders Encoder has not been set");
            if (offset < 0)
                throw new ArgumentException("Array offsets can not be negative numbers");

            int sourceIndex = 0;
            int targetIndex = offset;
            int sourceLength = source.Count;
            int targetLength = target.Length;

            while (offset < targetLength && sourceIndex < sourceLength)
            {
                try
                {
                    target[targetIndex] = encoding.LookupBySymbol(source[sourceIndex].Symbol).Value;
                    targetIndex++;
                }
                catch (Exception ex)
                {
                    throw new Exception("The encoder did not recognize the value: " +
                        source[sourceIndex].Symbol, ex);
                }
                sourceIndex++;
            }
        }

        /// <summary>
        /// Encodes the source sequence onto a byte array that has already been
        /// allocated its size. The encoding will start at the beginning of the
        /// target array and will end when reaching either the end of the source
        /// or the target array.
        /// </summary>
        /// <param name="source">The data to be encoded</param>
        /// <param name="target">The array into which the encoded values will be placed</param>
        public void Encode(List<ISequenceItem> source, byte[] target)
        {
            Encode(source, target, 0);
        }

        /// <summary>
        /// Encodes the source sequence onto a byte array. The array will be the
        /// size of the source when returned.
        /// </summary>
        /// <param name="source">The data to be encoded</param>
        /// <returns>The array into which the encoded values will be placed</returns>
        public byte[] Encode(List<ISequenceItem> source)
        {
            if (encoding == null)
                throw new Exception("SequenceEncoders Encoder has not been set");

            byte[] result = new byte[source.Count];
            int index = 0;

            foreach (ISequenceItem item in source)
            {
                try
                {
                    result[index] = encoding.LookupBySymbol(item.Symbol).Value;
                    index++;
                }
                catch (Exception ex)
                {
                    throw new Exception("The encoder did not recognize the value: " +
                        source[index].Symbol, ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Encodes a single sequence item into its byte value
        /// </summary>
        public byte Encode(ISequenceItem item)
        {
            return encoding.LookupBySymbol(item.Symbol).Value;
        }

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
        public void Encode(string source, byte[] target, int offset)
        {
            if (encoding == null)
                throw new Exception("SequenceEncoders Encoder has not been set");
            if (offset < 0)
                throw new ArgumentException("Array offsets can not be negative numbers");

            char[] sourceArray = source.ToCharArray();
            int sourceIndex = 0;
            int targetIndex = offset;
            int sourceLength = sourceArray.Length;
            int targetLength = target.Length;

            while (offset < targetLength && sourceIndex < sourceLength)
            {
                try
                {
                    target[targetIndex] = encoding.LookupBySymbol(sourceArray[sourceIndex]).Value;
                    targetIndex++;
                }
                catch (Exception ex)
                {
                    throw new Exception("The encoder did not recognize the value: " +
                        source[sourceIndex], ex);
                }
                sourceIndex++;
            }
        }

        /// <summary>
        /// Encodes the source sequence onto a byte array that has already been
        /// allocated its size. The encoding will start at the beginning of the
        /// target array and will end when reaching either the end of the source
        /// or the target array.
        /// </summary>
        /// <param name="source">The data to be encoded (eg. "GATTC")</param>
        /// <param name="target">The array into which the encoded values will be placed</param>
        public void Encode(string source, byte[] target)
        {
            Encode(source, target, 0);
        }

        /// <summary>
        /// Encodes the source sequence onto a byte array. The array will be the
        /// size of the source when returned.
        /// </summary>
        /// <param name="source">The data to be encoded (eg. "TAGGC")</param>
        /// <returns>The array into which the encoded values will be placed</returns>
        public byte[] Encode(string source)
        {
            if (encoding == null)
                throw new Exception("SequenceEncoders Encoder has not been set");

            char[] sourceArray = source.ToCharArray();
            int sourceLength = sourceArray.Length;
            byte[] result = new byte[sourceLength];

            for (int i = 0; i < sourceLength; i++)
            {
                try
                {
                    result[i] = encoding.LookupBySymbol(sourceArray[i]).Value;
                }
                catch (Exception ex)
                {
                    throw new Exception("The encoder did not recognize the value: " +
                        sourceArray[i], ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Encodes a single sequence item symbol into its byte value
        /// </summary>
        public byte Encode(char symbol)
        {
            return encoding.LookupBySymbol(symbol).Value;
        }

        #endregion

        #region ISerializable Members
        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected SequenceEncoder(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            encoding = Encodings.All.Single(E => E.Name.Equals(info.GetString("SequenceEncoder:EncodingName")));
        }

        /// <summary>
        /// Method for serializing the SequenceEncoder.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("SequenceEncoder:EncodingName", encoding.Name);
        }

        #endregion
    }
}
