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
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MBF.Algorithms.StringSearch;
using MBF.Encoding;
using MBF.Util;

namespace MBF
{
    /// <summary>
    /// This is the standard implementation of the ISequence interface. It contains
    /// the raw data that defines the contents of a sequence. You can access that
    /// data in two ways. The first is by treating the Sequence as a list of
    /// SequenceItems. For example:
    /// 
    /// Sequence mySequence = new Sequence(Alphabets.DNA, "GATTC");
    /// foreach (Nucleotide nucleotide in mySequence) { ... }
    /// 
    /// You can also retrieve the data and work with it as a string. For example:
    /// 
    /// String nucleotides = mySequence.ToString();
    /// 
    /// In both cases the results will be based on the Alphabet associated with the
    /// sequence. Common alphabets include those for DNA, RNA, and Amino Acids.
    /// 
    /// For users who wish to get at the underlying data directly, Sequence provides
    /// a means to do this as well. This may be useful for those writing algorithms
    /// against the sequence where performance is especially important. For these
    /// advanced users access is provided to the encoding classes associated with the
    /// sequence.
    ///
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// 
    /// This class also used by Data Virtualization on editable and non-editable scenario 
    /// </summary>
    [Serializable]
    public class Sequence : IVirtualSequence
    {
        #region Fields
        /// <summary>
        /// Byte array of encode values
        /// </summary>
        private ByteArray _valueArray;

        /// <summary>
        /// List of bytes for editing the sequence
        /// </summary>
        private List<byte> _valueList = new List<byte>();

        /// <summary>
        /// Is sequence read only or not
        /// </summary>
        private bool _isReadOnly;

        /// <summary>
        /// Encoding
        /// </summary>
        private IEncoding _encoding;

        /// <summary>
        /// Encoding map
        /// </summary>
        private readonly EncodingMap _mapToEncoding;

        /// <summary>
        /// Map to alphabet
        /// </summary>
        private readonly EncodingMap _mapToAlphabet;

        /// <summary>
        /// Sequence encoder
        /// </summary>
        private ISequenceEncoder _encoder;

        /// <summary>
        /// Sequence decoder
        /// </summary>
        private ISequenceDecoder _decoder;

        /// <summary>
        /// Basic sequence info
        /// </summary>
        private readonly BasicSequenceInfo _seqInfo = new BasicSequenceInfo();

        /// <summary>
        /// Statistics valid or not
        /// </summary>
        private bool _areStatisticsValid;

        /// <summary>
        /// Use encoding (compressed sequence representation)
        /// </summary>
        private bool _useEncoding = false;

        /// <summary>
        /// Sequence statistics
        /// </summary>
        private SequenceStatistics _statistics;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The encoding being used to translate the sequence items into string
        /// representations when needed, such as when calling ToString(). This
        /// value may also be used by individual implementations of ISequence to
        /// convert strings or characters passed into constructors or methods to
        /// convert to sequence items.
        /// </summary>
        public IEncoding Encoding
        {
            get { return _encoding; }
            internal set { _encoding = value; }
        }

        /// <summary>
        /// Provides a means of accessing the underlying data where the sequence
        /// is stored in memory. To understand the byte values encoded here you
        /// will likely also need to make use of the Encoding or Decoder associated
        /// with this sequence.
        /// </summary>
        public byte[] EncodedValues
        {
            get
            {
                if (VirtualSequenceProvider != null)
                    return ConvertToByteArray();

                if (IsReadOnly && UseEncoding)
                    return _valueArray.ToArray();

                byte[] resultArray = new byte[_valueList.Count];
                if (UseEncoding)
                {
                    _valueList.CopyTo(resultArray);
                }
                else
                {
                    for (int index = 0; index < _valueList.Count; index++)
                    {
                        resultArray[index] = _encoder.Encode((char)_valueList[index]);
                    }
                }
                return resultArray;
            }
        }

        /// <summary>
        /// The encoder provides the means of converting a string representation
        /// of the sequence into the underlying memory structure of the sequence.
        /// </summary>
        public ISequenceEncoder Encoder
        {
            get { return _encoder; }
            internal set { _encoder = value; }
        }

        /// <summary>
        /// The decoder provides the means of converting the memory represenation
        /// of the sequence into a string representation.
        /// </summary>
        public ISequenceDecoder Decoder
        {
            get { return _decoder; }
            internal set { _decoder = value; }
        }

        /// <summary>
        /// Gets a value indicating whether encoding is used while storing
        /// sequence in memory
        /// </summary>
        public bool UseEncoding
        {
            get { return _useEncoding; }
            set
            {

                // if there is no change in the value.
                if (_useEncoding == value) return;

                if (VirtualSequenceProvider == null)
                {
                    // if _useEncoding is false and value is true.
                    if (!_useEncoding)
                    {
                        if (!_isReadOnly)
                        {
                            // Switch from array to list
                            _valueList = _encoder.Encode(this.ToString()).ToList();
                            _valueArray = null;
                        }
                        else
                        {
                            // Switch from list to array
                            _valueArray = new ByteArray(_encoding, _encoder.Encode(this.ToString()));
                            _valueList.Clear();
                        }

                    }
                    else // if _useEncoding is true and value is false.
                    {
                        // Switch from array to list
                        _valueList = System.Text.ASCIIEncoding.ASCII.GetBytes(this.ToString()).ToList();
                        _valueArray = null;
                    }
                }

                _useEncoding = value;
            }
        }

        /// <summary>
        /// Gets or sets the virtual sequence provider
        /// </summary>
        public IVirtualSequenceProvider VirtualSequenceProvider { get; set; }

        /// <summary>
        /// Gets or sets the Pattern Finder used to short string in sequence
        /// </summary>
        public IPatternFinder PatternFinder { get; set; }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alphabet"></param>
        /// <param name="virtualData"></param>
        public Sequence(IAlphabet alphabet, IVirtualSequenceProvider virtualData)
            : this(alphabet)
        {
            VirtualSequenceProvider = virtualData;
            BlockSize = virtualData.BlockSize;
            MaxNumberOfBlocks = virtualData.MaxNumberOfBlocks;
        }

        /// <summary>
        /// Constructor for clone method.
        /// </summary>
        internal Sequence(Sequence sequence) :
            this(null as IAlphabet, null as IEncoding, sequence) { }

        /// <summary>
        /// Constructor to create a new sequence from the specified sequence 
        /// alphabet and encoding.
        /// </summary>
        /// <param name="alphabet">Alphabet for the new sequence.</param>
        /// <param name="encoding">Encoding for the new sequence.</param>
        /// <param name="sequence">Sequence instance.</param>
        internal Sequence(IAlphabet alphabet, IEncoding encoding, Sequence sequence)
        {
            bool isSequenceClone = false;

            if (alphabet == null)
            {
                alphabet = sequence.Alphabet;
                if (encoding == null)
                {
                    encoding = sequence._encoding;
                    isSequenceClone = true;
                }
            }
            else
            {
                if (encoding == null)
                {
                    encoding = EncodingMap.GetDefaultMap(alphabet).Encoding;
                }
            }

            _seqInfo = sequence._seqInfo.Clone();
            Alphabet = alphabet;
            _encoding = encoding;

            _areStatisticsValid = sequence._areStatisticsValid;

            _isReadOnly = true;

            if (isSequenceClone)
            {
                _encoder = sequence._encoder;
                _decoder = sequence._decoder;
                _mapToAlphabet = sequence._mapToAlphabet;
                _mapToEncoding = sequence._mapToEncoding;
                _useEncoding = sequence._useEncoding;
                _isReadOnly = sequence._isReadOnly;

                object documentation = sequence.Documentation;

                // If documentation is ICloneable then get the copy of it.
                ICloneable cloneableDocument = documentation as ICloneable;
                if (cloneableDocument != null)
                {
                    documentation = cloneableDocument.Clone();
                }

                Documentation = documentation;
                MoleculeType = sequence.MoleculeType;

                // If the sequence is readonly get the values from valueArray else get them from valueList.
                if (_isReadOnly && _useEncoding)
                {
                    _valueArray = sequence._valueArray.Clone();
                }
                else
                {
                    foreach (byte b in sequence._valueList)
                    {
                        _valueList.Add(b);
                    }
                }

                if (sequence._areStatisticsValid)
                {
                    _statistics = sequence._statistics.Clone();
                }
            }
            else
            {
                _statistics = sequence._statistics;
                _encoder = new SequenceEncoder(encoding);
                _decoder = new SequenceDecoder(encoding);
                _mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encoding);
                _mapToAlphabet = EncodingMap.GetMapToAlphabet(encoding, alphabet);

                if (_mapToEncoding == null || _mapToAlphabet == null)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Properties.Resource.InvalidEncodingForAlphabet, encoding.Name, alphabet.Name);
                    throw new ArgumentException(message);
                }

                if (_useEncoding)
                {
                    _valueArray = new ByteArray(_encoding, sequence.Count);

                    if (sequence.Count > 0)
                    {
                        for (int i = 0; i < sequence.Count; i++)
                        {
                            _valueArray[i] = _mapToEncoding.Convert(alphabet.LookupBySymbol(sequence[i].Symbol)).Value;
                        }
                    }
                }
                else
                {
                    _valueList = new List<byte>();

                    for (int i = 0; i < sequence.Count; i++)
                    {
                        _valueList.Add((byte)sequence[i].Symbol);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a sequence with no sequence data and sets IsReadOnly property 
        /// of the created sequence to false.
        /// 
        /// For working with sequences that never have sequence data, but are
        /// only used for metadata storage (like keeping an ID or various features
        /// but no direct sequence data) consider using the VirtualSequence
        /// class instead.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        public Sequence(IAlphabet alphabet)
            : this(alphabet, String.Empty)
        {
            _isReadOnly = false;
            _areStatisticsValid = false;
        }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// string someSequenceData = "GATTCAAGGGCT";
        /// Sequence mySequence = new Sequence(Alphabets.DNA, someSequenceData);
        /// 
        /// The Corollary for RNA:
        /// 
        /// string someSequenceData = "GAUUCAAGGGCU";
        /// Sequence mySequence = new Sequence(Alphabets.RNA, someSequenceData);
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        public Sequence(IAlphabet alphabet, string sequence) :
            this(alphabet, sequence, null, false) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// string someSequenceData = "GATTCAAGGGCT";
        /// Sequence mySequence = new Sequence(Alphabets.DNA, someSequenceData);
        /// 
        /// The correlary for RNA:
        /// 
        /// string someSequenceData = "GAUUCAAGGGCU";
        /// Sequence mySequence = new Sequence(Alphabets.RNA, someSequenceData);
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="useEncoding">
        /// Indicates whether encoding is used to store the sequence in memory
        /// </param>
        public Sequence(IAlphabet alphabet, string sequence, bool useEncoding) :
            this(alphabet, sequence, null, useEncoding) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the Sequence(IAlphabet, string) constructor
        /// instead.
        /// 
        /// The standard SequenceEncoder for the given encoding is used.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        public Sequence(IAlphabet alphabet, IEncoding encoding, string sequence) :
            this(alphabet, encoding, sequence, null, false) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the Sequence(IAlphabet, string) constructor
        /// instead.
        /// 
        /// The standard SequenceEncoder for the given encoding is used.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        /// <param name="useEncoding">
        /// Indicates whether encoding is used to store the sequence in memory
        /// </param>
        public Sequence(IAlphabet alphabet, IEncoding encoding, string sequence, bool useEncoding) :
            this(alphabet, encoding, sequence, null, useEncoding) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the Sequence(IAlphabet, string) constructor
        /// instead.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        /// <param name="encoder">
        /// The class that performs the encoding work from an ISequenceItem to
        /// the underlying byte array.
        /// </param>
        /// <param name="decoder">
        /// The class that performs the decoding work from a byte value to an=
        /// </param>
        public Sequence(IAlphabet alphabet, IEncoding encoding, ISequenceEncoder encoder, ISequenceDecoder decoder, string sequence) :
            this(alphabet, encoding, encoder, decoder, sequence, null, false) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the Sequence(IAlphabet, string) constructor
        /// instead.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        /// <param name="encoder">
        /// The class that performs the encoding work from an ISequenceItem to
        /// the underlying byte array.
        /// </param>
        /// <param name="decoder">
        /// The class that performs the decoding work from a byte value to an=
        /// </param>
        /// <param name="useEncoding">
        /// Indicates whether encoding is used to store the sequence in memory
        /// </param>
        public Sequence(IAlphabet alphabet, IEncoding encoding, ISequenceEncoder encoder, ISequenceDecoder decoder, string sequence, bool useEncoding) :
            this(alphabet, encoding, encoder, decoder, sequence, null, useEncoding) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter, when sequence statistics have already been calculated for
        /// the sequence data. The characters in the sequence parameter must be
        /// contained in the alphabet or an Exception will occur.
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// string someSequenceData = "GATTCAAGGGCT";
        /// SequenceStatistics someStatistics = ...
        /// Sequence mySequence = new Sequence(Alphabets.DNA, someSequenceData, someStatistics);
        /// 
        /// The Corollary for RNA:
        /// 
        /// string someSequenceData = "GAUUCAAGGGCU";
        /// SequenceStatistics someStatistics = ...
        /// Sequence mySequence = new Sequence(Alphabets.RNA, someSequenceData, someStatistics);
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="statistics">
        /// The statistics for the given sequence data, or null if no statistics have been
        /// calculated.
        /// </param>
        /// <param name="useEncoding">
        /// Indicates whether encoding is used to store the sequence in memory
        /// </param>
        internal Sequence(IAlphabet alphabet, string sequence, SequenceStatistics statistics, bool useEncoding) :
            this(alphabet, EncodingMap.GetDefaultMap(alphabet).Encoding, sequence, statistics, useEncoding) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter, when sequence statistics have already been calculated for
        /// the sequence data. The characters in the sequence parameter must be
        /// contained in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the Sequence(IAlphabet, string) constructor
        /// instead.
        /// 
        /// The standard SequenceEncoder for the given encoding is used.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        /// <param name="statistics">
        /// The statistics for the given sequence data, or null if no statistics have been
        /// calculated.
        /// </param>
        /// <param name="useEncoding">
        /// Indicates whether encoding is used to store the sequence in memory
        /// </param>
        internal Sequence(IAlphabet alphabet, IEncoding encoding, string sequence, SequenceStatistics statistics, bool useEncoding) :
            this(alphabet, encoding, new SequenceEncoder(encoding), new SequenceDecoder(encoding), sequence, statistics, useEncoding) { }

        /// <summary>
        /// Creates a sequence based on sequence data passed in via a string
        /// parameter, when sequence statistics have already been calculated for
        /// the sequence data. The characters in the sequence parameter must be
        /// contained in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the Sequence(IAlphabet, string) constructor
        /// instead.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        /// <param name="encoder">
        /// The class that performs the encoding work from an ISequenceItem to
        /// the underlying byte array.
        /// </param>
        /// <param name="decoder">
        /// The class that performs the decoding work from a byte value to an=
        /// </param>
        /// <param name="statistics">
        /// The statistics for the given sequence data, or null if no statistics have been
        /// calculated.
        /// </param>
        /// <param name="useEncoding">
        /// Indicates whether encoding is used to store the sequence in memory
        /// </param>
        internal Sequence(IAlphabet alphabet, IEncoding encoding, ISequenceEncoder encoder, ISequenceDecoder decoder, string sequence, SequenceStatistics statistics, bool useEncoding)
        {
            Alphabet = alphabet;
            _encoding = encoding;
            _encoder = encoder;
            _decoder = decoder;
            _mapToEncoding = EncodingMap.GetMapToEncoding(alphabet, encoding);
            _mapToAlphabet = EncodingMap.GetMapToAlphabet(encoding, alphabet);
            _useEncoding = useEncoding;

            if (_mapToEncoding == null || _mapToAlphabet == null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Properties.Resource.InvalidEncodingForAlphabet, encoding.Name, alphabet.Name);
                throw new ArgumentException(message);
            }

            _isReadOnly = true;

            if (string.IsNullOrEmpty(sequence))
            {
                if (_useEncoding)
                {
                    _valueArray = new ByteArray(encoding, 0);
                }
                else
                {
                    _valueList = new List<byte>();
                }
            }
            else
            {
                if (_useEncoding)
                {
                    _valueArray = new ByteArray(encoding, sequence.Length);
                    for (int i = 0; i < sequence.Length; i++)
                    {
                        _valueArray[i] = _mapToEncoding.Convert(alphabet.LookupBySymbol(sequence[i])).Value;
                    }
                }
                else
                {
                    _valueList = new List<byte>();
                    for (int i = 0; i < sequence.Length; i++)
                    {
                        char symbol = Char.ToUpperInvariant(sequence[i]);
                        if (alphabet.LookupBySymbol(symbol) == null)
                        {
                            throw new ArgumentException("The alphabet does not contain the item being mapped");
                        }
                        _valueList.Add((byte)symbol);
                    }
                }
            }

            // Set the statistics
            _statistics = statistics;
            _areStatisticsValid = (statistics != null);
        }

        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates a new Sequence that is a copy of the current Sequence.
        /// </summary>
        /// <returns>A new Sequence that is a copy of this Sequence.</returns>
        public Sequence Clone()
        {
            // Create a new sequence by passing the sequence.
            return new Sequence(this);
        }
        #endregion

        #region ISequence Members

        /// <summary>
        /// Encodes the sequence item and places it at the indicated position
        /// within the current sequence data.
        /// </summary>
        /// <param name="index">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to start at the begging
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="item">The item to be encoded placed into the sequence</param>
        public void Insert(int index, ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (item == null)
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);

            InsertRange(index, item.Symbol.ToString());
        }

        /// <summary>
        /// Encodes sequence parameter and places the values obtained at the
        /// indicated position within the current sequence data. Insert also works on
        /// Data Virtualization enabled and non-Data Virtualization scenario 
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to start at the begging
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="sequence">The sequence to be encoded placed into the sequence</param>
        public void InsertRange(int position, string sequence)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position > Count)
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            // Check whether the all charater in the sequence are known by alphabet of this sequence or not.
            char invalidChar;
            if (!Helper.IsValidSequence(Alphabet, sequence, out invalidChar))
            {
                throw new ArgumentException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InvalidSymbol,
                        invalidChar));
            }

            // non-DV insert
            if (VirtualSequenceProvider == null)
            {
                byte[] encodedValues = null;
                if (UseEncoding)
                {
                    encodedValues = _encoder.Encode(sequence);
                }
                else
                {
                    encodedValues = ASCIIEncoding.ASCII.GetBytes(sequence.ToUpper(CultureInfo.InvariantCulture));
                }

                _valueList.InsertRange(position, encodedValues);
            }
            else // DV insert
            {
                VirtualSequenceProvider.InsertRange(position, sequence);
            }

            if (_areStatisticsValid)
            {
                _statistics.Add(sequence);
            }
        }

        /// <summary>
        /// Encodes the sequence item and places it at the indicated position
        /// within the current sequence data.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to start at the begging
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="character">The item to be encoded placed into the sequence</param>
        public void Insert(int position, char character)
        {
            InsertRange(position, character.ToString());
        }

        /// <summary>
        /// Removes the sequence data item at the indicated position. Remove also works on
        /// Data Virtualization enabled and non-Data Virtualization scenario 
        /// </summary>
        /// <param name="index">
        /// The position within the data to remove the data item. Note that this
        /// position starts its counting from 0. Thus to remove the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        public void RemoveAt(int index)
        {
            RemoveRange(index, 1);
        }

        /// <summary>
        /// Removes the sequence data at the indicated position for an indicated
        /// number of characters.
        /// </summary>
        /// <param name="position">
        /// The position within the data to remove the data item. Note that this
        /// position starts its counting from 0. Thus to remove the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="length">The number of characters to remove.</param>
        public void RemoveRange(int position, int length)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(Properties.Resource.ParameterNameLength);
            }

            if ((Count - position) < length)
            {
                throw new ArgumentException(Properties.Resource.InvalidPositionAndLength);
            }

            // non-DV edit
            if (VirtualSequenceProvider == null)
            {
                if (_areStatisticsValid)
                {
                    for (int i = position; i < position + length; i++)
                    {
                        if (UseEncoding)
                        {
                            _statistics.Remove(_decoder.Decode(_valueList[i]));
                        }
                        else
                        {
                            _statistics.Remove(Alphabet.LookupBySymbol((char)_valueList[i]));
                        }
                    }
                }
                _valueList.RemoveRange(position, length);
            }
            else
            {
                VirtualSequenceProvider.RemoveRange(position, length);
            }
        }

        /// <summary>
        /// Encodes the sequence item and places it at the indicated position
        /// within the current sequence data, replacing the item currently
        /// located at that position.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to replace the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="item">The item to be encoded and placed into the sequence</param>
        public void Replace(int position, ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            ISequenceItem seqItem = Alphabet.LookupBySymbol(item.Symbol);

            if (seqItem == null)
            {
                throw new ArgumentException(
                         string.Format(
                         CultureInfo.CurrentCulture,
                         Properties.Resource.InvalidSymbol,
                         item.Symbol));
            }

            // non-DV replace
            if (VirtualSequenceProvider == null)
            {
                byte encodedValue = (byte)item.Symbol;
                if (UseEncoding)
                {
                    encodedValue = _encoder.Encode(item);
                }

                if (_areStatisticsValid)
                {
                    if (UseEncoding)
                    {
                        _statistics.Remove(_decoder.Decode(_valueList[position]));
                    }
                    else
                    {
                        _statistics.Remove(Alphabet.LookupBySymbol((char)_valueList[position]));
                    }

                    _statistics.Add(item);
                }

                _valueList.RemoveAt(position);
                _valueList.Insert(position, encodedValue);
            }
            else // DV replace
            {
                VirtualSequenceProvider.RemoveAt(position);
                VirtualSequenceProvider.Insert(position, item);
            }

        }

        /// <summary>
        /// Encodes the sequence item and places it at the indicated position
        /// within the current sequence data, replacing the item currently
        /// located at that position.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to replace the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="character">The symbol of the item to be encoded and placed into the sequence</param>
        public void Replace(int position, char character)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            ISequenceItem seqItem = Alphabet.LookupBySymbol(character);

            if (seqItem == null)
            {
                throw new ArgumentException(
                         string.Format(
                         CultureInfo.CurrentCulture,
                         Properties.Resource.InvalidSymbol,
                         character));
            }

            // non-DV replace
            if (VirtualSequenceProvider == null)
            {
                byte encodedValue = (byte)Char.ToUpperInvariant(character);
                if (UseEncoding)
                {
                    encodedValue = _encoder.Encode(character);
                }

                if (_areStatisticsValid)
                {
                    if (UseEncoding)
                    {
                        _statistics.Remove(_decoder.Decode(_valueList[position]));
                    }
                    else
                    {
                        _statistics.Remove(Alphabet.LookupBySymbol((char)_valueList[position]));
                    }

                    _statistics.Add(character);
                }

                _valueList.RemoveAt(position);
                _valueList.Insert(position, encodedValue);
            }
            else // DV replace
            {
                VirtualSequenceProvider.RemoveRange(position, 1);
                VirtualSequenceProvider.InsertRange(position, character.ToString());
            }

        }

        /// <summary>
        /// Encodes the sequence and places it at the indicated position
        /// within the current sequence data, replacing the items currently
        /// located within that range. The number of items replaced will
        /// match the length of the sequence passed in. Replace also works on
        /// Data Virtualization enabled and non-Data Virtualization scenario 
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to replace the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="sequence">The item to be encoded placed into the sequence</param>
        public void ReplaceRange(int position, string sequence)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            int length = sequence.Length;

            if ((Count - position) < length)
            {
                throw new ArgumentException(
                    Properties.Resource.InvalidPositionAndLength,
                    Properties.Resource.ParameterNameSequence);
            }

            // Check whether the all charater in the sequence are known by alphabet of this sequence or not.
            char invalidChar;
            if (!Helper.IsValidSequence(Alphabet, sequence, out invalidChar))
            {
                throw new ArgumentException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InvalidSymbol,
                        invalidChar));
            }

            // non-DV replace
            if (VirtualSequenceProvider == null)
            {
                byte[] encodedValues = null;
                if (UseEncoding)
                {
                    encodedValues = _encoder.Encode(sequence);
                }
                else
                {
                    encodedValues = ASCIIEncoding.ASCII.GetBytes(sequence.ToUpper(CultureInfo.InvariantCulture));
                }

                if (_areStatisticsValid)
                {
                    for (int i = position; i < position + length; i++)
                    {
                        if (UseEncoding)
                        {
                            _statistics.Remove(_decoder.Decode(_valueList[i]));
                        }
                        else
                        {
                            _statistics.Remove(Alphabet.LookupBySymbol((char)_valueList[i]));
                        }
                    }

                    _statistics.Add(sequence);
                }

                _valueList.RemoveRange(position, length);
                _valueList.InsertRange(position, encodedValues);
            }
            else // DV replace
            {
                VirtualSequenceProvider.RemoveRange(position, length);
                VirtualSequenceProvider.InsertRange(position, sequence);
            }
        }

        /// <summary>
        /// An identification provided to distinguish the sequence to others
        /// being worked with.
        /// </summary>
        public string ID
        {
            get { return _seqInfo.ID; }
            set { _seqInfo.ID = value; }
        }

        /// <summary>
        /// An identification of the sequence that is meant to be understood
        /// by human users when displayed in an application or file format.
        /// </summary>
        public string DisplayID
        {
            get { return _seqInfo.DisplayID; }
            set { _seqInfo.DisplayID = value; }
        }

        /// <summary>
        /// Many sequence representations when saved to file also contain
        /// information about that sequence. Unfortunately there is no standard
        /// around what that data may be from format to format. This property
        /// allows a place to put structured metadata that can be accessed by
        /// a particular key.
        /// 
        /// For example, if species information is stored in a particular Species
        /// class, you could add it to the dictionary by:
        /// 
        /// mySequence.Metadata["SpeciesInfo"] = mySpeciesInfo;
        /// 
        /// To fetch the data you would use:
        /// 
        /// Species mySpeciesInfo = mySequence.Metadata["SpeciesInfo"];
        /// 
        /// Particular formats may create their own data model class for information
        /// unique to their format as well. Such as:
        /// 
        /// GenBankMetadata genBankData = new GenBankMetadata();
        /// // ... add population code
        /// mySequence.MetaData["GenBank"] = genBankData;
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get { return _seqInfo.Metadata; }
        }

        /// <summary>
        /// The Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a sequence. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public object Documentation { set; get; }

        /// <summary>
        /// The alphabet to which string representations of the sequence should
        /// conform.
        /// </summary>
        public IAlphabet Alphabet
        {
            get { return _seqInfo.Alphabet; }
            internal set { _seqInfo.Alphabet = value; }
        }

        /// <summary>
        /// The molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        public MoleculeType MoleculeType
        {
            get { return _seqInfo.MoleculeType; }
            set { _seqInfo.MoleculeType = value; }
        }

        /// <summary>
        /// Keeps track of the number of occurrances of each symbol within a sequence.
        /// </summary>
        public SequenceStatistics Statistics
        {
            get
            {
                if (!_areStatisticsValid)
                {
                    _statistics = new SequenceStatistics(this);
                    _areStatisticsValid = true;
                }

                return _statistics;
            }
        }

        /// <summary>
        /// Returns a string representation of the sequence data. This representation
        /// will come from the symbols in the alphabet defined for the sequence.
        /// 
        /// Thus a Sequence whose Alphabet is Alphabets.DNA may return a value like
        /// 'GATTCCA'
        /// </summary>
        public override string ToString()
        {
            // PERF ISSUE:  Do not use StringBuilder.Append when we know the size.
            //              The string constructor from a char array is much faster
            var buffer = new char[Count];

            if (UseEncoding || (VirtualSequenceProvider != null))
            {
                for (int i = 0; i < Count; i++)
                {
                    buffer[i] = this[i].Symbol;
                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    buffer[i] = (char)_valueList[i];
                }
            }

            return new string(buffer);
        }

        /// <summary>
        /// Return a virtual sequence representing this sequence with the orientation reversed.
        /// </summary>
        public ISequence Reverse
        {
            get
            {
                // reversed = true, complemented = false, range is a no-op.
                return new BasicDerivedSequence(this, true, false, -1, -1);
            }
        }

        /// <summary>
        /// Return a virtual sequence representing the complement of this sequence.
        /// </summary>
        public ISequence Complement
        {
            get
            {
                // reversed = false, complemented = true, range is a no-op.
                return new BasicDerivedSequence(this, false, true, -1, -1);
            }
        }

        /// <summary>
        /// Return a virtual sequence representing the reverse complement of this sequence.
        /// </summary>
        public ISequence ReverseComplement
        {
            get
            {
                // reversed = true, complemented = true, range is a no-op.
                return new BasicDerivedSequence(this, true, true, -1, -1);
            }
        }

        /// <summary>
        /// Return a virtual sequence representing a range (substring) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The virtual sequence.</returns>
        public ISequence Range(int start, int length)
        {
            if (start < 0 || start >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameStart,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameLength,
                    Properties.Resource.ParameterMustNonNegative);
            }

            if ((Count - start) < length)
            {
                throw new ArgumentException(Properties.Resource.InvalidStartAndLength);
            }

            // reversed = false, complemented = false, range is as passed.
            return new BasicDerivedSequence(this, false, false, start, length);
        }

        /// <summary>
        /// Gets the index of first non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public int IndexOfNonGap()
        {
            if (Count > 0)
            {
                if (Alphabet.HasGaps)
                {
                    return IndexOfNonGap(0);
                }

                return 0;
            }

            return -1;
        }

        /// <summary>
        /// Returns the position of the first item from startPos that does not 
        /// have a Gap character.
        /// </summary>
        /// <param name="startPos">Index value above which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public int IndexOfNonGap(int startPos)
        {
            if (startPos >= 0 && startPos < Count && !Alphabet.HasGaps)
            {
                return startPos;
            }

            return BasicSequenceInfo.IndexOfNonGap(this, startPos);
        }

        /// <summary>
        /// Gets the index of last non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public int LastIndexOfNonGap()
        {
            if (Count > 0)
            {
                if (Alphabet.HasGaps)
                {
                    return LastIndexOfNonGap(Count - 1);
                }
                else
                {
                    return Count - 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of last non gap character within the specified end position.
        /// </summary>
        /// <param name="endPos">Index value below which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public int LastIndexOfNonGap(int endPos)
        {
            if (endPos >= 0 && endPos < Count && !Alphabet.HasGaps)
            {
                return endPos;
            }

            return BasicSequenceInfo.LastIndexOfNonGap(this, endPos);
        }

        /// <summary>
        /// Creates a new Sequence that is a copy of the current Sequence.
        /// </summary>
        /// <returns>A new ISequence that is a copy of this Sequence.</returns>
        ISequence ISequence.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Finds the list of string that matches any of the patterns with the indices of each occurrence in sequence.
        /// </summary>
        /// <param name="patterns">List of patterns that needs to be searched in Sequence.</param>
        /// <param name="startIndex">Minimum index in Sequence at which match has to start.</param>
        /// <param name="ignoreCase">
        /// if true ignore character casing while match.
        /// <remarks>
        /// Note that symbols in Sequence are always Upper case.
        /// </remarks>
        /// </param>
        /// <returns></returns>
        public IDictionary<string, IList<int>> FindMatches(IList<string> patterns, int startIndex = 0, bool ignoreCase = true)
        {
            if (PatternFinder == null)
            {
                PatternFinder = new BoyerMoore();
            }

            PatternFinder.StartIndex = startIndex;
            PatternFinder.IgnoreCase = ignoreCase;
            return PatternFinder.FindMatch(
                this,
                PatternConverter.GetInstanace(this.Alphabet).Convert(patterns).Values.SelectMany(pattern => pattern).ToList());
        }
        #endregion

        #region IList<ISequenceItem> Members
        /// <summary>
        /// Returns the index of the first item matching the item
        /// passed in to the parameter. This does not do a symbol
        /// comparison. The match must be the exact same ISequenceItem.
        /// </summary>
        /// <returns>The index of the first matched item. Counting starts at 0.</returns>
        public int IndexOf(ISequenceItem item)
        {
            if (item != null)
            {
                // non-DV index
                if (VirtualSequenceProvider == null)
                {
                    if (Alphabet.LookupBySymbol(item.Symbol) != null)
                    {
                        byte value = (byte)item.Symbol;

                        if (_useEncoding)
                            value = _encoder.Encode(item);

                        if (!_isReadOnly || !_useEncoding)
                            return _valueList.IndexOf(value);

                        for (int i = 0; i < _valueArray.Count; i++)
                        {
                            if (_valueArray[i] == value)
                                return i;
                        }
                    }
                }
                else // DV index
                {
                    return VirtualSequenceProvider.IndexOf(item);
                }

            }

            return -1;
        }

        /// <summary>
        /// Allows the sequence to function like an array, getting and setting
        /// the sequence item at the particular index specified. Note that the
        /// index value starts its count at 0.
        /// </summary>
        public ISequenceItem this[int index]
        {
            get
            {
                if (index >= 0 && index < Count)
                {
                    // Calls DV provider
                    if (VirtualSequenceProvider != null)
                    {
                        return VirtualSequenceProvider[index];
                    }

                    if (!UseEncoding)
                        return Alphabet.LookupBySymbol((char)_valueList[index]);

                    if (IsReadOnly)
                        return _mapToAlphabet.Convert(_decoder.Decode(_valueArray[index]));

                    return _mapToAlphabet.Convert(_decoder.Decode(_valueList[index]));
                }

                throw new ArgumentOutOfRangeException("index");
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (IsReadOnly)
                    throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

                byte encodedValue = (byte)value.Symbol;
                if (_useEncoding)
                {
                    encodedValue = _encoder.Encode(value);
                }

                if (_areStatisticsValid)
                {
                    if (_useEncoding)
                    {
                        _statistics.Remove(_decoder.Decode(_valueList[index]));
                    }
                    else
                    {
                        _statistics.Remove(Alphabet.LookupBySymbol((char)_valueList[index]));
                    }

                    _statistics.Add(value);
                }

                _valueList[index] = encodedValue;
            }
        }

        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// Adds a sequence item to the end of the sequence. The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        /// <param name="item">The item to add to the end of the sequence</param>
        public void Add(ISequenceItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            ISequenceItem seqItem = Alphabet.LookupBySymbol(item.Symbol);

            if (seqItem == null)
            {
                throw new ArgumentException(
                         string.Format(
                         CultureInfo.CurrentCulture,
                         Properties.Resource.InvalidSymbol,
                         item.Symbol));
            }

            // non-DV add
            if (VirtualSequenceProvider == null)
            {
                if (_useEncoding)
                {
                    _valueList.Add(_encoder.Encode(item));
                }
                else
                {
                    // Validate the input character is valid
                    _encoding.LookupBySymbol(item.Symbol);
                    _valueList.Add((byte)item.Symbol);
                }

                if (_areStatisticsValid)
                {
                    _statistics.Add(item);
                }
            }
            else // DV add
            {
                VirtualSequenceProvider.Add(item);
            }
        }

        /// <summary>
        /// Removes all sequence data from the Sequence.  The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            // non-DV clear
            if (VirtualSequenceProvider == null)
            {
                if (_statistics == null)
                {
                    _statistics = new SequenceStatistics(Alphabet);
                }
                else
                {
                    _statistics.Clear();
                }

                _areStatisticsValid = true;

                _valueList.Clear();
            }
            else // DV clear
            {
                VirtualSequenceProvider.Clear();
            }

        }

        /// <summary>
        /// Indicates if a sequence item is contained in the sequence anywhere.
        /// Note that the SequenceItem must be taken from the alphabet defined
        /// for this sequence in order for this method to return true.
        /// </summary>
        public bool Contains(ISequenceItem item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the sequence items into a preallocated array.
        /// </summary>
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            // non-DV copy
            if (VirtualSequenceProvider == null)
            {
                int index = arrayIndex;
                foreach (ISequenceItem seqItem in this)
                {
                    array[index++] = seqItem;
                }

            }
            else // DV copy
            {
                VirtualSequenceProvider.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// The number of sequence items contained in the Sequence.
        /// </summary>
        public int Count
        {
            get
            {
                if (VirtualSequenceProvider != null)
                {
                    return VirtualSequenceProvider.Count;
                }

                if (IsReadOnly && _useEncoding)
                    return _valueArray.Count;

                return _valueList.Count;
            }
        }

        /// <summary>
        /// A flag indicating whether or not edits can be made to this Sequence.
        /// When IsReadOnly is true, the sequence data is stored in a compact
        /// array and any method for editing that data will throw an exception.
        /// When IsReadOnly is false, the sequence data is stored in a less
        /// compact structure that allows for quick edits.
        /// 
        /// You can set the IsReadOnly flag. The result of doing so will transfer
        /// the internal storage of the data from one mechanism to the other.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                // if there is no change in the value.
                if (_isReadOnly == value) return;

                if (VirtualSequenceProvider == null)
                {
                    if (_useEncoding)
                    {
                        // if _isReadOnly is true and value is false.
                        if (_isReadOnly)
                        {
                            // Switch from array to list
                            _valueList = _valueArray.ToList();
                            _valueArray = null;

                        }
                        else // if _isReadOnly is false and value is true.
                        {
                            // Switch from list to array
                            _valueArray = new ByteArray(_encoding, _valueList);
                            _valueList.Clear();
                        }
                    }
                }

                _isReadOnly = value;
            }
        }

        /// <summary>
        /// Removes the first instance found of a particular sequence item.
        /// This item must be from the alphabet defined for the Sequence.
        /// </summary>
        /// <param name="item">The items to search for and remove.</param>
        /// <returns>True if the item was found and removed, false if the item was not found.</returns>
        public bool Remove(ISequenceItem item)
        {
            int position = IndexOf(item);
            if (position < 0)
                return false;

            RemoveAt(position);
            return true;
        }

        #endregion

        #region IEnumerable<ISequenceItem> Members
        /// <summary>
        /// Retrieves an enumerator for this sequence
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion

        #region ICloneable Members
        /// <summary>
        /// Creates a new Sequence that is a copy of the current Sequence.
        /// </summary>
        /// <returns>A new object that is a copy of this Sequence.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected Sequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _seqInfo = (BasicSequenceInfo)info.GetValue("Sequence:seqInfo", typeof(BasicSequenceInfo));

            // Get the encoding from the encoding name.
            _encoding = Encodings.All.Single(E => E.Name.Equals(info.GetString("Sequence:EncodingName")));
            _encoder = (ISequenceEncoder)info.GetValue("Sequence:Encoder", typeof(ISequenceEncoder));
            _decoder = (ISequenceDecoder)info.GetValue("Sequence:Decoder", typeof(ISequenceDecoder));

            _mapToEncoding = EncodingMap.GetMapToEncoding(Alphabet, _encoding);
            _mapToAlphabet = EncodingMap.GetMapToAlphabet(_encoding, Alphabet);

            _useEncoding = info.GetBoolean("Sequence:UseEncoding");
            _isReadOnly = info.GetBoolean("Sequence:IsReadOnly");

            if (_isReadOnly && _useEncoding)
            {
                _valueArray = (ByteArray)info.GetValue("Sequence:Values", typeof(ByteArray));
            }
            else
            {
                _valueList = (List<byte>)info.GetValue("Sequence:Values", typeof(List<byte>));
            }

            Documentation = info.GetValue("Sequence:Documentation", typeof(object));
            MoleculeType = (MoleculeType)info.GetValue("Sequence:MoleculeType", typeof(MoleculeType));

            _areStatisticsValid = info.GetBoolean("Sequence:AreStatisticsValid");

            if (_areStatisticsValid)
            {
                _statistics = (SequenceStatistics)info.GetValue("Sequence:Statistics", typeof(SequenceStatistics));
            }
        }

        /// <summary>
        /// Method for serializing the sequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Sequence:seqInfo", _seqInfo);
            info.AddValue("Sequence:EncodingName", _encoding.Name);
            info.AddValue("Sequence:Encoder", _encoder);
            info.AddValue("Sequence:Decoder", _decoder);
            info.AddValue("Sequence:UseEncoding", _useEncoding);
            info.AddValue("Sequence:IsReadOnly", _isReadOnly);

            if (_isReadOnly && _useEncoding)
            {
                info.AddValue("Sequence:Values", _valueArray);
            }
            else
            {
                info.AddValue("Sequence:Values", _valueList);
            }

            if (Documentation != null && ((Documentation.GetType().Attributes &
                System.Reflection.TypeAttributes.Serializable) == System.Reflection.TypeAttributes.Serializable))
            {
                info.AddValue("Sequence:Documentation", Documentation);
            }
            else
            {
                info.AddValue("Sequence:Documentation", null);
            }

            info.AddValue("Sequence:MoleculeType", MoleculeType);
            info.AddValue("Sequence:AreStatisticsValid", _areStatisticsValid);

            if (_areStatisticsValid)
            {
                info.AddValue("Sequence:Statistics", _statistics);
            }
        }

        #endregion

        #region IVirtualSequence Members
        /// <summary>
        /// Gets or sets maximum number of blocks per sequence
        /// </summary>
        public int MaxNumberOfBlocks
        {
            get
            {
                if (null == VirtualSequenceProvider)
                {
                    return 0;
                }

                return VirtualSequenceProvider.MaxNumberOfBlocks;
            }

            set
            {
                if (null == VirtualSequenceProvider)
                {
                    throw new InvalidOperationException(Properties.Resource.DataVirtualizationPropertyCannotBeSet);
                }

                if (0 >= value)
                {
                    throw new InvalidOperationException(Properties.Resource.DataVirtualizationPropertyInvalidSet);
                }

                VirtualSequenceProvider.MaxNumberOfBlocks = value;
            }
        }

        /// <summary>
        /// Gets or sets block size
        /// </summary>
        public int BlockSize
        {
            get
            {
                if (null == VirtualSequenceProvider)
                {
                    return -1;
                }

                return VirtualSequenceProvider.BlockSize;
            }

            set
            {
                if (null == VirtualSequenceProvider)
                {
                    throw new InvalidOperationException(Properties.Resource.DataVirtualizationPropertyCannotBeSet);
                }

                if (0 >= value)
                {
                    throw new InvalidOperationException(Properties.Resource.DataVirtualizationPropertyInvalidSet);
                }

                if (value > Count)
                {
                    throw new InvalidOperationException(Properties.Resource.DataVirtualizationPropertyInvalidSet + " --" + Count);
                }

                VirtualSequenceProvider.BlockSize = value;
            }
        }
        #endregion

        /// <summary>
        /// Partial sequence - convert to byte array
        /// </summary>
        /// <returns></returns>
        private byte[] ConvertToByteArray()
        {
            byte[] resultArray = new byte[VirtualSequenceProvider.Count];
            for (int i = 0; i < VirtualSequenceProvider.Count; i++)
            {
                byte encodedValue = (byte)VirtualSequenceProvider[i].Symbol;
                if (UseEncoding)
                {
                    encodedValue = _encoder.Encode(VirtualSequenceProvider[i]);
                }

                resultArray[i] = encodedValue;
            }

            return resultArray;
        }
    }

    /// <summary>
    /// Enumerator implementation for the Sequence class
    /// </summary>
    internal class SequenceEnumerator : IEnumerator<ISequenceItem>
    {
        private IList<ISequenceItem> seq;
        private int index;

        /// <summary>
        /// Constructs an enumerator for a Sequence object.
        /// </summary>
        public SequenceEnumerator(IList<ISequenceItem> sequence)
        {
            seq = sequence;
            Reset();
        }

        #region IEnumerator<ISequenceItem> Members
        /// <summary>
        /// The current item reference for the enumerator.
        /// </summary>
        public ISequenceItem Current
        {
            get
            {
                if (index < 0)
                    return null;

                return seq[index];
            }
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Takes care of any allocated memory
        /// </summary>
        public void Dispose()
        {
            // No op
        }

        #endregion

        #region IEnumerator Members
        /// <summary>
        /// The current item reference for the enumerator
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return seq[index]; }
        }

        /// <summary>
        /// Advances the enumerator to the next item
        /// </summary>
        /// <returns>True if the enumerator can advance. False if the end of the sequence is reached.</returns>
        public bool MoveNext()
        {
            if (index < (seq.Count - 1))
            {
                index++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the enumerator to the start of the sequence
        /// </summary>
        public void Reset()
        {
            index = -1;
        }

        #endregion
    }
}
