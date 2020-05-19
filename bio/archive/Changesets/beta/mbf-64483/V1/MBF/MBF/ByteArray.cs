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

using MBF.Encoding;
using MBF.Properties;

namespace MBF
{
    /// <summary>
    /// ByteArray holds the encoded values and provides array like access to the data stored in it.
    /// This class internally stores compressed indices of encoded values into a byte array to reduce the 
    /// memory required to hold sequence data.
    /// For example:
    /// Ncbi2NAEncoding contains four encoded values to store an index of an encoded value; two bits are sufficient.
    /// Thus when creating this class, if Ncbi2NAEncoding is specified, then the index of specified encoded values 
    /// are stored in a byte array so that each byte will contain a maximum of four encoded values.
    /// 
    /// Note that data will be compressed when the total number of encoded values present in the 
    /// specified IEncoding is less than or equal to 16, otherwise it stores specified encoded values 
    /// in the byte array.
    /// </summary>
    [Serializable]
    internal class ByteArray : ICloneable, IEnumerable<byte>
    {
        #region Fields

        #region Mask Values

        /// <summary>
        /// Default mask value.
        /// Binary: 00000000
        /// </summary>
        private const byte _defaultMask = 0;

        /// <summary>
        /// Mask for four bit value stored in first position from leftside in a byte.
        /// Binary: 11110000
        /// </summary>
        private const byte _maskForFirstFourBitValue = 240;

        /// <summary>
        /// Mask for four bit value stored in second position from leftside in a byte.
        /// Binary: 00001111
        /// </summary>
        private const byte _maskForSecondFourBitValue = 15;

        /// <summary>
        /// Mask for two bit value stored in first position from leftside in a byte.
        /// Binary: 11000000
        /// </summary>
        private const byte _maskForFirstTwoBitValue = 192;

        /// <summary>
        /// Mask for two bit value stored in second position from leftside in a byte.
        /// Binary: 00110000
        /// </summary>
        private const byte _maskForSecondTwoBitValue = 48;

        /// <summary>
        /// Mask for two bit value stored in third position from leftside in a byte.
        /// Binary: 00001100
        /// </summary>
        private const byte _maskForThirdTwoBitValue = 12;

        /// <summary>
        /// Mask for two bit value stored in fourth position from leftside in a byte.
        /// Binary: 00000011
        /// </summary>
        private const byte _maskForFourthTwoBitValue = 3;

        /// <summary>
        /// Mask for one bit value stored in first position from leftside in a byte.
        /// Binary: 10000000
        /// </summary>
        private const byte _maskForFirstOneBitValue = 128;

        /// <summary>
        /// Mask for one bit value stored in second position from leftside in a byte.
        /// Binary: 01000000
        /// </summary>
        private const byte _maskForSecondOneBitValue = 64;

        /// <summary>
        /// Mask for one bit value stored in third position from leftside in a byte.
        /// Binary: 00100000
        /// </summary>
        private const byte _maskForThirdOneBitValue = 32;

        /// <summary>
        /// Mask for one bit value stored in fourth position from leftside in a byte.
        /// Binary: 00010000
        /// </summary>
        private const byte _maskForFourthOneBitValue = 16;

        /// <summary>
        /// Mask for one bit value stored in fifth position from leftside in a byte.
        /// Binary: 00001000
        /// </summary>
        private const byte _maskForFifthOneBitValue = 8;

        /// <summary>
        /// Mask for one bit value stored in sixth position from leftside in a byte.
        /// Binary: 00000100
        /// </summary>
        private const byte _maskForSixthOneBitValue = 4;

        /// <summary>
        /// Mask for one bit value stored in seventh position from leftside in a byte.
        /// Binary: 00000010
        /// </summary>
        private const byte _maskForSeventhOneBitValue = 2;

        /// <summary>
        /// Mask for one bit value stored in eighth position from leftside in a byte.
        /// Binary: 00000001
        /// </summary>
        private const byte _maskForEighthOneBitValue = 1;

        #endregion Mask Values

        /// <summary>
        /// Holds total number of bits required to store an ISequence item.
        /// </summary>
        private int _bitsRequired;

        /// <summary>
        /// Specifies number of values can be stored in a byte.
        /// </summary>
        private int _valuesPerByte;

        /// <summary>
        /// Holds all encoded values in the specified Encoding.
        /// </summary>
        private byte[] _encodedValues;

        /// <summary>
        /// Array to hold bytes.
        /// </summary>
        private byte[] _compressedBytes;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates new ByteArray instance from the specified encoding and list of byte values.
        /// ByteArray compacts and stores the byte values depending on the specified encoding.
        /// 
        /// For example: Ncbi2NAEncoding contains four values, In this case two bits are sufficient 
        /// to store index of an encoded value thus four encoded values can be stored in to a byte.
        /// 
        /// Note that data will be compressed when the total number of encoded values present in the 
        /// specified IEncoding is less than or equal to 16, otherwise it stores specified encoded values 
        /// in the byte array.
        /// </summary>
        /// <param name="encoding">Encoding to which the specified values are belongs to.</param>
        /// <param name="values">Encoded values.</param>
        public ByteArray(IEncoding encoding, IList<byte> values)
        {
            Count = (values == null ? 0 : values.Count);

            // initialize fields from specified encoding.
            SetEncoding(encoding);

            for (int i = 0; i < Count; i++)
            {
                SetByteValue(i, values[i]);
            }
        }

        /// <summary>
        /// Creates new ByteArray instance from the specified encoding which can store 
        /// specified size of encoded values.
        /// ByteArray compacts and stores the byte values depending on the specified encoding.
        /// 
        /// For example: Ncbi2NAEncoding contains four values, In this case two bits are sufficient 
        /// to store index of an encoded value thus four encoded values can be stored in to a byte.
        /// 
        /// Note that data will be compressed when the total number of encoded values present in the 
        /// specified IEncoding is less than or equal to 16, otherwise it stores specified encoded values 
        /// in the byte array.
        /// </summary>
        /// <param name="encoding">Encoding to which the specified values are belongs to.</param>
        /// <param name="size">Required size of ByteArray.</param>
        public ByteArray(IEncoding encoding, int size)
        {
            if (size < 0)
            {
                throw new ArgumentException(Resource.ParameterMustNonNegative, Resource.ParameterNameSize);
            }

            Count = size;

            // initialize fields from specified encoding.
            SetEncoding(encoding);
        }

        /// <summary>
        /// Constructor used for clone method.
        /// </summary>
        /// <param name="byteArray">ByteArray instance.</param>
        private ByteArray(ByteArray byteArray)
        {
            Count = byteArray.Count;
            _bitsRequired = byteArray._bitsRequired;
            _valuesPerByte = byteArray._valuesPerByte;
            _encodedValues = byteArray._encodedValues;
            _compressedBytes = new byte[byteArray._compressedBytes.Length];
            byteArray._compressedBytes.CopyTo(_compressedBytes, 0);
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Total number of values stored in this instance.
        /// </summary>
        public int Count
        {
            get;
            private set;
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets or sets the encoded value at specified index.
        /// Note that the encoded value should be from the encoding specified 
        /// while creating this instance, otherwise exception will occur.
        /// </summary>
        /// <param name="index">Zero based index.</param>
        /// <returns>Encoded value.</returns>
        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameIndex);
                }

                return GetByteValue(index);
            }

            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameIndex);
                }

                SetByteValue(index, value);
            }
        }

        /// <summary>
        /// Creates a clone copy of this instance.
        /// </summary>
        /// <returns>New instance of ByteArray.</returns>
        public ByteArray Clone()
        {
            return new ByteArray(this);
        }

        /// <summary>
        /// Sets required fields from the specified encoding.
        /// </summary>
        /// <param name="encoding">IEncoding instance.</param>
        private void SetEncoding(IEncoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameEncoding);
            }

            _valuesPerByte = 1;

            // Get number of bits required to store index of encoded values.
            _bitsRequired = GetBitsRequired(encoding.Count - 1);

            // If bits required is 1,2,4 then only compress the data.
            if ((8 % _bitsRequired) == 0 && _bitsRequired != 8)
            {
                _valuesPerByte = 8 / _bitsRequired;
            }
            else
            {
                _bitsRequired = 8;
            }

            // Calculate number of bytes required to store specified values.
            int totalBytesRequired = Count;
            if (_valuesPerByte > 1)
            {
                // Get all encoded values to a list so that the index can be maintained.
                _encodedValues = new byte[encoding.Count];
                int index = 0;
                foreach (ISequenceItem seqItem in encoding)
                {
                    _encodedValues[index++] = seqItem.Value;
                }

                totalBytesRequired = (Count / _valuesPerByte) + (Count % _valuesPerByte == 0 ? 0 : 1);
            }

            _compressedBytes = new byte[totalBytesRequired];
        }

        /// <summary>
        /// Stores specified encoded value.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <param name="value">Encoded value.</param>
        private void SetByteValue(int index, byte value)
        {
            // If values per byte is more than one then compress the index of specified 
            // encoded value and store in the byte array.
            if (_valuesPerByte > 1)
            {
                // Get the index of the value inside the byte.
                int internalValueIndex = index % _valuesPerByte;

                // Get the index of the byte.
                int byteIndex = index / _valuesPerByte;

                // Get index of the encoding value in IEncoding.
                int encodingIndex = -1;
                for (int i = 0; i < _encodedValues.Length; i++)
                {
                    if (_encodedValues[i] == value)
                    {
                        encodingIndex = i;
                        break;
                    }
                }

                if (encodingIndex == -1)
                {
                    throw new ArgumentException(Resource.InvalidParameterValue);
                }

                // Clear the value by setting it to zero.
                _compressedBytes[byteIndex] = (byte)(_compressedBytes[byteIndex] &
                                                (~GetByteMask(internalValueIndex + 1, _bitsRequired)));

                // No need to store zero value as by default byte is initialized to zero.
                if (encodingIndex != 0)
                {
                    // If the internal index is not the last index then move the bits.
                    if (internalValueIndex < _valuesPerByte - 1)
                    {
                        _compressedBytes[byteIndex] = (byte)(_compressedBytes[byteIndex] |
                                    (encodingIndex << ((_valuesPerByte - internalValueIndex - 1) * _bitsRequired)));
                    }
                    else
                    {
                        _compressedBytes[byteIndex] = (byte)(_compressedBytes[byteIndex] | encodingIndex);
                    }
                }
            }
            else
            {
                _compressedBytes[index] = value;
            }
        }

        /// <summary>
        /// Returns the encoded value for the specified index.
        /// </summary>
        /// <param name="index">Index of the value to be required.</param>
        /// <returns>Returns the encoded value.</returns>
        private byte GetByteValue(int index)
        {
            // If data is compressed then decompress it.
            if (_valuesPerByte > 1)
            {
                int internalValueIndex = index % _valuesPerByte;
                int byteIndex = index / _valuesPerByte;

                if (_compressedBytes[byteIndex] == 0)
                {
                    return _encodedValues[0];
                }

                int encodingIndex = (
                    (_compressedBytes[byteIndex] & GetByteMask(internalValueIndex + 1, _bitsRequired)) >>
                    ((_valuesPerByte - internalValueIndex - 1) * _bitsRequired));

                return _encodedValues[encodingIndex];
            }
            else
            {
                return _compressedBytes[index];
            }
        }

        /// <summary>
        /// Returns the number of bits required to store the specified value.
        ///
        /// For example if the input value is 3, the number of bits needed to store
        /// 3 is 2 bits (Binary - 11). Hence this method would return 2 bits as the
        /// output.
        /// </summary>
        /// <param name="value">Value for which the number of required bits has to be determined.</param>
        /// <returns>Number of bits required to store the value.</returns>
        private int GetBitsRequired(int value)
        {
            if (value < 2)
            {
                return 1;
            }

            if (value < 4)
            {
                return 2;
            }

            if (value < 16)
            {
                return 4;
            }

            return 8;
        }

        /// <summary>
        /// Returns mask value that can be used while reading each value present in the compressed byte.
        /// Note that position is one based and starts from lefthand side of the byte.
        /// 
        /// For example: 
        /// If 2 four bit values, 1011 and 1101 are placed in a byte then the value of byte will be
        /// 10111101. 
        /// Following are the steps to read first value i.e 1011.
        /// 
        /// Step1: "bitwise AND" with _maskForFirstFourBitValue.
        ///         10111101 &amp; 11110000 = 10110000
        /// Step2: Right shift the result from Step1 by 4 position.
        ///         10110000 >> 4 = 00001011
        /// </summary>
        /// <param name="position">Position of the value in the byte.</param>
        /// <param name="sizeInBits">Size of each value in bits.</param>
        /// <returns>Returns the mask value.</returns>
        private byte GetByteMask(int position, int sizeInBits)
        {
            switch (sizeInBits)
            {
                case 4:
                    switch (position)
                    {
                        case 1:
                            return _maskForFirstFourBitValue;
                        case 2:
                            return _maskForSecondFourBitValue;
                    }

                    break;
                case 2:
                    switch (position)
                    {
                        case 1:
                            return _maskForFirstTwoBitValue;
                        case 2:
                            return _maskForSecondTwoBitValue;
                        case 3:
                            return _maskForThirdTwoBitValue;
                        case 4:
                            return _maskForFourthTwoBitValue;
                    }

                    break;
                case 1:
                    switch (position)
                    {
                        case 1:
                            return _maskForFirstOneBitValue;
                        case 2:
                            return _maskForSecondOneBitValue;
                        case 3:
                            return _maskForThirdOneBitValue;
                        case 4:
                            return _maskForFourthOneBitValue;
                        case 5:
                            return _maskForFifthOneBitValue;
                        case 6:
                            return _maskForSixthOneBitValue;
                        case 7:
                            return _maskForSeventhOneBitValue;
                        case 8:
                            return _maskForEighthOneBitValue;
                    }

                    break;
            }

            return _defaultMask;
        }
        #endregion Methods

        #region IEnumerable<byte> Members

        /// <summary>
        /// Returns enumerator for this instance.
        /// </summary>
        /// <returns>Returns IEnumerator for ByteArray.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            return new ByteArrayEnumerator(this);
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns enumerator for this instance.
        /// </summary>
        /// <returns>Returns IEnumerator for ByteArray.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ByteArrayEnumerator(this);
        }

        #endregion

        #region ICloneable Members
        /// <summary>
        /// Creates a clone copy of this instance.
        /// </summary>
        /// <returns>New instance of ByteArray.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }

    /// <summary>
    /// Enumerator implementation for the ByteArray class
    /// </summary>
    internal class ByteArrayEnumerator : IEnumerator<byte>
    {
        /// <summary>
        /// Holds a ByteArray.
        /// </summary>
        private ByteArray _byteArray;

        /// <summary>
        /// Current index of ByteArray.
        /// </summary>
        private int _index;

        /// <summary>
        /// Constructs an enumerator for a ByteArray object.
        /// </summary>
        public ByteArrayEnumerator(ByteArray byteArray)
        {
            _byteArray = byteArray;
            Reset();
        }

        #region IEnumerator<byte> Members
        /// <summary>
        /// The current item reference for the enumerator.
        /// </summary>
        public byte Current
        {
            get
            {
                return _byteArray[_index];
            }
        }

        #endregion IEnumerator<byte> Members

        #region IDisposable Members
        /// <summary>
        /// Takes care of any allocated memory
        /// </summary>
        public void Dispose()
        {
            // No op
        }

        #endregion IDisposable Members

        #region IEnumerator Members
        /// <summary>
        /// The current item reference for the enumerator
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        /// <summary>
        /// Advances the enumerator to the next item.
        /// </summary>
        /// <returns>True if the enumerator can advance. False if the end of the ByteArray is reached.</returns>
        public bool MoveNext()
        {
            if (_index < (_byteArray.Count - 1))
            {
                _index++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the enumerator to the start of the ByteArray.
        /// </summary>
        public void Reset()
        {
            _index = -1;
        }

        #endregion IEnumerator Members
    }
}
