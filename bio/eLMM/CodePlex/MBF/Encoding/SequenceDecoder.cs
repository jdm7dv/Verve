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
using System.Linq;
using System.Runtime.Serialization;

namespace Bio.Encoding
{
    /// <summary>
    /// This basic implementation of the ISequenceDecoder provides a one to one
    /// translation from byte values to ISequenceItem. It uses an IEncoding to determine
    /// what the mapping is for this translation. This encoding should be set
    /// in the constructor.
    /// 
    /// Note that the decoded item will come from the IEncoding and not from an IAlphabet.
    /// You may need to use an EncodingMap to get from the ISequenceItems returned here
    /// and those of an alphabet, such as DNA or RNA.
    /// 
    /// For example, using the following code:
    /// 
    /// SequenceDecoder decoder = new SequenceDecoder(Encodings.Ncbi4NA);
    /// ISequenceItem encItem = decoder.Decode(4);
    /// 
    /// In this case the resulting item will be:
    /// 
    /// Encodings.Ncbi4NA.G
    /// 
    /// You may want instead to have the result Alphabets.DNA.G. To do this add:
    /// 
    /// EncodingMap map = EncodingMap.GetMapToAlphabet(Encodings.Ncbi4NA, Alphabets.DNA);
    /// ISequenceItem alphaItem = map.Convert(encItem)
    ///
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class SequenceDecoder : ISequenceDecoder
    {
        private IEncoding encoding;

        /// <summary>
        /// The Encoding provides the map from symbol characters to byte values.
        /// Without a set encoding the decoder can not work.
        /// </summary>
        public IEncoding Encoding
        {
            get { return encoding; }
            internal set { encoding = value; }
        }

        /// <summary>
        /// Creates a sequence decoder by defining the encoding to use.
        /// </summary>
        public SequenceDecoder(IEncoding encoding)
        {
            this.encoding = encoding;
        }

        #region ISequenceDecoder Members

        /// <summary>
        /// Converts a byte value representation of a sequence item into an
        /// ISequenceItem representation from the IEncoding specified for this
        /// instance of the decoder.
        /// 
        /// See the comments for the class to see how to convert the resulting
        /// item into a particular alphabet.
        /// </summary>
        /// <param name="value">The internal byte representation of an ISequenceItem</param>
        /// <returns>The ISequenceItem found by looking up they byte value in the decoder's IEncoding</returns>
        public ISequenceItem Decode(byte value)
        {
            return encoding.LookupByValue(value);
        }

        #endregion

        #region ISerializable Members
        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected SequenceDecoder(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            encoding = Encodings.All.Single(E => E.Name.Equals(info.GetString("SequenceDecoder:EncodingName")));
        }

        /// <summary>
        /// Method for serializing the SequenceDecoder.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("SequenceDecoder:EncodingName", encoding.Name);
        }
        #endregion
    }
}
