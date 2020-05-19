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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Bio.Encoding;
using Bio.Properties;

namespace Bio
{
    /// <summary>
    /// QualitativeSequence stores sequence item information along with its quality score.
    /// 
    /// Following are three types of QualitativeSequence.
    ///  1. Sanger type: 
    ///         A QualitativeSequence of type Sanger stores Phred quality scores.
    ///         Phred quality score values range from 0 to 93 are encoded and stored using ASCII 33 to 126.
    ///         Although in raw read data the Phred quality score rarely exceeds 60, higher scores are possible 
    ///         in assemblies or read maps.
    ///  2. Solexa type: 
    ///         A QualitativeSequence of type Solexa stores Solexa / Illumina quality scores.
    ///         Score values ranges from -5 to 62 and these scores are encoded and stored using ASCII 59 to 126.
    ///  3. Illumina type:
    ///         A QualitativeSequence of type Illumina stores Phred quality scores.
    ///         Score values ranges from 0 to 62 and these scores are encoded and stored using ASCII 64 to 126.
    ///         This is similar to Sanger but in this case base value is ASCII 64.
    /// By default this class assumes FastQFormatType as Illumina.
    /// Type property of this class specifies the type of QualitativeSequence.
    /// Note that all quality scores are ASCII encoded.
    /// </summary>
    [Serializable]
    public class QualitativeSequence : IQualitativeSequence, IVirtualSequence
    {
        #region Fields
        /// <summary>
        /// Minimum quality score for Sanger type.
        /// </summary>
        public const byte SangerMinQualScore = 33;

        /// <summary>
        /// Minimum quality score for Solexa type.
        /// </summary>
        public const byte SolexaMinQualScore = 59;

        /// <summary>
        /// Minimum quality score for Illumina type.
        /// </summary>
        public const byte IlluminaMinQualScore = 64;

        /// <summary>
        /// Maximum quality score for Sanger type
        /// </summary>
        public const byte SangerMaxQualScore = 126;

        /// <summary>
        /// Maximum quality score for Solexa type.
        /// </summary>
        public const byte SolexaMaxQualScore = 126;

        /// <summary>
        /// Maximum quality score for Illumina type.
        /// </summary>
        public const byte IlluminaMaxQualScore = 126;

        /// <summary>
        /// Default quality score.
        /// </summary>
        private const int DefaultQualScore = 60;

        /// <summary>
        /// ASCII Base value for encoding quality scores in Sanger format.
        /// </summary>
        private const int SangerAsciiBaseValue = 33;

        /// <summary>
        /// ASCII Base value for encoding quality scores in Solexa/Illumina 1.0 format.
        /// </summary>
        private const int SolexaAsciiBaseValue = 64;

        /// <summary>
        /// ASCII Base value for encoding quality scores in Illumina 1.3 format.
        /// </summary>
        private const int IlluminaAsciiBaseValue = 64;

        /// <summary>
        /// Sequence instance to hold sequence items.
        /// </summary>
        private Sequence _sequence;

        /// <summary>
        /// Holds quality scores when this sequence is in readonly mode.
        /// </summary>
        private byte[] _scores;

        /// <summary>
        /// Holds quality scores when the sequence is not in readonly mode.
        /// </summary>
        private List<byte> _scoreList = new List<byte>();
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Private constructor for Clone method.
        /// </summary>
        /// <param name="qualSequence">QualitativeSequence instance.</param>
        private QualitativeSequence(QualitativeSequence qualSequence)
            : this(null as IAlphabet, null as IEncoding, qualSequence) { }

        /// <summary>
        /// Constructor to create a new QualitativeSequence from the specified QualitativeSequence,
        /// alphabet and encoding.
        /// </summary>
        /// <param name="alphabet">Alphabet for the new QualitativeSequence.</param>
        /// <param name="encoding">Encoding for the new QualitativeSequence.</param>
        /// <param name="qualSequence">QualitativeSequence instance.</param>
        internal QualitativeSequence(IAlphabet alphabet, IEncoding encoding, QualitativeSequence qualSequence)
        {
            if (qualSequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualSequence);
            }

            Type = qualSequence.Type;

            _sequence = new Sequence(alphabet, encoding, qualSequence._sequence);

            if (IsReadOnly)
            {
                _scores = new byte[qualSequence._scores.Length];
                qualSequence._scores.CopyTo(_scores, 0);
            }
            else
            {
                _scoreList = new List<byte>(qualSequence._scoreList);
            }
        }

        /// <summary>
        /// Creates a QualitativeSequence with no sequence data and sets IsReadOnly property 
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
        public QualitativeSequence(IAlphabet alphabet)
            : this(alphabet, FastQFormatType.Illumina) { }

        /// <summary>
        /// Creates a QualitativeSequence with no sequence data and sets IsReadOnly property 
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
        /// <param name="type">FastQ format type.</param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type)
        {
            Type = type;
            _sequence = new Sequence(alphabet);
        }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
        /// parameter and with default quality score.
        /// The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// string someSequenceData = "GATTCAAGGGCT";
        /// QualitativeSequence mySequence = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Sanger, someSequenceData);
        /// 
        /// The correlary for RNA:
        /// 
        /// string someSequenceData = "GAUUCAAGGGCU";
        /// QualitativeSequence mySequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Sanger, someSequenceData);
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="type">FastQ format type.</param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, string sequence)
            : this(alphabet, type, sequence, GetDefaultQualScore(type)) { }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
        /// parameter and with specified quality score.
        /// The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// string someSequenceData = "GATTCAAGGGCT";
        /// QualitativeSequence mySequence = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Sanger, someSequenceData, 40);
        /// All sequence item of mySequence will have quality score 40.
        /// 
        /// The correlary for RNA:
        /// 
        /// string someSequenceData = "GAUUCAAGGGCU";
        /// QualitativeSequence mySequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Sanger, someSequenceData, 40);
        /// All sequence item of mySequence will have quality score 40.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="type">FastQ format type.</param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="qualScore">Quality score.</param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, string sequence, byte qualScore)
        {
            Type = type;
            if (!ValidateQualScore(qualScore))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            _sequence = new Sequence(alphabet, sequence);

            if (string.IsNullOrEmpty(sequence))
            {
                _scores = new byte[0];
            }
            else
            {
                _scores = new byte[sequence.Length];

                for (int i = 0; i < sequence.Length; i++)
                {
                    _scores[i] = qualScore;
                }
            }
        }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
        /// parameter and quality score via qualScores parameter.
        /// The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// The number of characters in the sequence and number of bytes in the 
        /// qualScores must be equal, otherwise an Exception will occur.
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// string someSequenceData = "GATTCAAGGGCT";
        ///  byte[] someQualityScores = new byte[]{33,40,45,40,39,40,40,40,50,45,38,45};
        /// Sequence mySequence = new Sequence(Alphabets.DNA, FastQFormatType.Sanger, someSequenceData, someQualityScores);
        /// 
        /// The correlary for RNA:
        /// 
        /// string someSequenceData = "GAUUCAAGGGCU";
        /// byte[] someQualityScores = new byte[]{33,40,45,40,39,40,40,40,50,45,38,45};
        /// QualitativeSequence mySequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Sanger, someSequenceData, someQualityScores);
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="type">FastQ format type.</param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="qualScores">Quality scores.</param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, string sequence, IEnumerable<byte> qualScores)
        {
            Type = type;

            if (string.IsNullOrEmpty(sequence))
            {
                if (qualScores != null && qualScores.Count() > 0)
                {
                    throw new ArgumentException(Resource.InvalidLength_Sequence_QualScores);
                }

                _sequence = new Sequence(alphabet, sequence);
                _scores = new byte[0];
            }
            else
            {
                if (qualScores == null)
                {
                    throw new ArgumentNullException(Resource.ParameterNameQualScores);
                }

                if (sequence.Length != qualScores.Count())
                {
                    throw new ArgumentException(Resource.InvalidLength_Sequence_QualScores);
                }

                foreach (byte qualScore in qualScores)
                {
                    if (!ValidateQualScore(qualScore))
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                        throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                    }
                }

                _sequence = new Sequence(alphabet, sequence);
                _scores = new byte[sequence.Length];
                int index = 0;

                foreach (byte qualScore in qualScores)
                {
                    _scores[index++] = qualScore;
                }
            }
        }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the QualitativeSequence(IAlphabet,
        /// FastQFormatType, string) constructor instead.
        /// 
        /// The standard SequenceEncoder for the given encoding is used.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="type">FastQ format type.</param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        ///  </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, IEncoding encoding, string sequence)
            : this(alphabet, type, encoding, sequence, GetDefaultQualScore(type)) { }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the QualitativeSequence(IAlphabet,
        /// FastQFormatType, string) constructor instead.
        /// 
        /// The standard SequenceEncoder for the given encoding is used.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="type">FastQ format type.</param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="qualScore">Quality score, this score is used for all characters in sequence parameter.</param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, IEncoding encoding, string sequence, byte qualScore)
        {
            Type = type;
            if (!ValidateQualScore(qualScore))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            _sequence = new Sequence(alphabet, encoding, sequence);

            if (string.IsNullOrEmpty(sequence))
            {
                _scores = new byte[0];
            }
            else
            {
                _scores = new byte[sequence.Length];

                for (int i = 0; i < sequence.Length; i++)
                {
                    _scores[i] = qualScore;
                }
            }
        }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
        /// parameter. The characters in the sequence parameter must be contained
        /// in the alphabet or an Exception will occur.
        /// 
        /// This constructor also allows the setting of a particular encoding and
        /// is designed for programmers familiar with the encoding techniques used
        /// for storing sequence data in memory. Users not familiar with these
        /// encodings should consider using the QualitativeSequence(IAlphabet,
        /// FastQFormatType, string) constructor instead.
        /// 
        /// The standard SequenceEncoder for the given encoding is used.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA)
        /// </param>
        /// <param name="type">FastQ format type.</param>
        /// <param name="encoding">
        /// The encoding to use when converting the sequence data into in memory
        /// byte represenation. This encoding must have an EncodingMap registered
        /// for it and the alphabet via EncodingMaps.GetMapToEncoding().
        /// </param>
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="qualScores">Quality scores.</param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, IEncoding encoding, string sequence, IEnumerable<byte> qualScores)
        {
            Type = type;

            if (string.IsNullOrEmpty(sequence))
            {
                if (qualScores != null && qualScores.Count() > 0)
                {
                    throw new ArgumentException(Resource.InvalidLength_Sequence_QualScores);
                }

                _sequence = new Sequence(alphabet, encoding, sequence);
                _scores = new byte[0];
            }
            else
            {
                if (qualScores == null)
                {
                    throw new ArgumentNullException(Resource.ParameterNameQualScores);
                }

                if (sequence.Length != qualScores.Count())
                {
                    throw new ArgumentException(Resource.InvalidLength_Sequence_QualScores);
                }

                foreach (byte qualScore in qualScores)
                {
                    if (!ValidateQualScore(qualScore))
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                        throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                    }
                }

                _sequence = new Sequence(alphabet, encoding, sequence);
                _scores = new byte[sequence.Length];
                int index = 0;

                foreach (byte qualScore in qualScores)
                {
                    _scores[index++] = qualScore;
                }
            }
        }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
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
        /// <param name="type">FastQ format type.</param>
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
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, IEncoding encoding, ISequenceEncoder encoder, ISequenceDecoder decoder, string sequence)
            : this(alphabet, type, encoding, encoder, decoder, sequence, GetDefaultQualScore(type)) { }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
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
        /// <param name="type">FastQ format type.</param>
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
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="qualScore">Quality score, this score is used for all characters in sequence parameter.</param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, IEncoding encoding, ISequenceEncoder encoder, ISequenceDecoder decoder, string sequence, byte qualScore)
        {
            Type = type;
            if (!ValidateQualScore(qualScore))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            _sequence = new Sequence(alphabet, encoding, encoder, decoder, sequence);

            if (string.IsNullOrEmpty(sequence))
            {
                _scores = new byte[0];
            }
            else
            {
                _scores = new byte[sequence.Length];

                for (int i = 0; i < sequence.Length; i++)
                {
                    _scores[i] = qualScore;
                }
            }
        }

        /// <summary>
        /// Creates a QualitativeSequence based on sequence data passed in via a string
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
        /// <param name="type">FastQ format type.</param>
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
        /// <param name="sequence">
        /// A description of the sequence where each character in the string is
        /// known by the alphabet.
        /// </param>
        /// <param name="qualScores">Quality scores.</param>
        public QualitativeSequence(IAlphabet alphabet, FastQFormatType type, IEncoding encoding, ISequenceEncoder encoder, ISequenceDecoder decoder, string sequence, IEnumerable<byte> qualScores)
        {
            Type = type;

            if (string.IsNullOrEmpty(sequence))
            {
                if (qualScores != null && qualScores.Count() > 0)
                {
                    throw new ArgumentException(Resource.InvalidLength_Sequence_QualScores);
                }

                _sequence = new Sequence(alphabet, encoding, encoder, decoder, sequence);
                _scores = new byte[0];
            }
            else
            {
                if (qualScores == null)
                {
                    throw new ArgumentNullException(Resource.ParameterNameQualScores);
                }

                if (sequence.Length != qualScores.Count())
                {
                    throw new ArgumentException(Resource.InvalidLength_Sequence_QualScores);
                }

                foreach (byte qualScore in qualScores)
                {
                    if (!ValidateQualScore(qualScore))
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                        throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                    }
                }

                _sequence = new Sequence(alphabet, encoding, encoder, decoder, sequence);
                _scores = new byte[sequence.Length];
                int index = 0;

                foreach (byte qualScore in qualScores)
                {
                    _scores[index++] = qualScore;
                }
            }
        }
        #endregion Constructors

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
            get { return _sequence.Encoding; }
            private set { _sequence.Encoding = value; }
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
                return _sequence.EncodedValues;
            }
        }

        /// <summary>
        /// The encoder provides the means of converting a string representation
        /// of the sequence into the underlying memory structure of the sequence.
        /// </summary>
        public ISequenceEncoder Encoder
        {
            get { return _sequence.Encoder; }
            private set { _sequence.Encoder = value; }
        }

        /// <summary>
        /// The decoder provides the means of converting the memory represenation
        /// of the sequence into a string representation.
        /// </summary>
        public ISequenceDecoder Decoder
        {
            get { return _sequence.Decoder; }
            private set { _sequence.Decoder = value; }
        }

        /// <summary>
        /// Gets or sets the virtual sequence provider
        /// </summary>
        public IVirtualQualitativeSequenceProvider VirtualQualitativeSequenceProvider { get; set; }
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// Converts Sanger quality score to Illumina quality score.
        /// </summary>
        /// <param name="qualScore">Sanger quality score.</param>
        /// <returns>Returns Illumina quality score.</returns>
        public static byte ConvertFromSangerToIllumina(byte qualScore)
        {
            if (!ValidateQualScore(qualScore, FastQFormatType.Sanger))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            return (byte)Math.Min(IlluminaMaxQualScore,
                GetEncodedQualScore(GetQualScore(qualScore, FastQFormatType.Sanger), FastQFormatType.Illumina));
        }

        /// <summary>
        /// Converts Sanger quality score to Solexa quality score.
        /// </summary>
        /// <param name="qualScore">Sanger quality score.</param>
        /// <returns>Returns Solexa quality score.</returns>
        public static byte ConvertFromSangerToSolexa(byte qualScore)
        {
            if (!ValidateQualScore(qualScore, FastQFormatType.Sanger))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            return (byte)Math.Min(SolexaMaxQualScore,
              GetEncodedQualScore(ConvertFromPhredToSolexa(GetQualScore(qualScore, FastQFormatType.Sanger)), FastQFormatType.Solexa));
        }

        /// <summary>
        /// Converts Solexa quality score to Sanger quality score.
        /// </summary>
        /// <param name="qualScore">Solexa quality score.</param>
        /// <returns>Returns Sanger quality score.</returns>
        public static byte ConvertFromSolexaToSanger(byte qualScore)
        {
            if (!ValidateQualScore(qualScore, FastQFormatType.Solexa))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            return (byte)Math.Min(SangerMaxQualScore,
              GetEncodedQualScore(ConvertFromSolexaToPhared(GetQualScore(qualScore, FastQFormatType.Solexa)), FastQFormatType.Sanger));
        }

        /// <summary>
        /// Converts Solexa quality score to Illumina quality score.
        /// </summary>
        /// <param name="qualScore">Solexa quality score.</param>
        /// <returns>Returns Illumina quality score.</returns>
        public static byte ConvertFromSolexaToIllumina(byte qualScore)
        {
            if (!ValidateQualScore(qualScore, FastQFormatType.Solexa))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            return (byte)Math.Min(IlluminaMaxQualScore,
              GetEncodedQualScore(ConvertFromSolexaToPhared(GetQualScore(qualScore, FastQFormatType.Solexa)), FastQFormatType.Illumina));
        }

        /// <summary>
        /// Converts Illumina quality score to Sanger quality score.
        /// </summary>
        /// <param name="qualScore">Illumina quality score.</param>
        /// <returns>Returns Sanger quality score.</returns>
        public static byte ConvertFromIlluminaToSanger(byte qualScore)
        {
            if (!ValidateQualScore(qualScore, FastQFormatType.Illumina))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            return (byte)Math.Min(SangerMaxQualScore,
              GetEncodedQualScore(GetQualScore(qualScore, FastQFormatType.Illumina), FastQFormatType.Sanger));
        }

        /// <summary>
        /// Converts Illumina quality score to Solexa quality score.
        /// </summary>
        /// <param name="qualScore">Illumina quality score.</param>
        /// <returns>Returns Solexa quality score.</returns>
        public static byte ConvertFromIlluminaToSolexa(byte qualScore)
        {
            if (!ValidateQualScore(qualScore, FastQFormatType.Illumina))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            return (byte)Math.Min(SolexaMaxQualScore,
              GetEncodedQualScore(ConvertFromPhredToSolexa(GetQualScore(qualScore, FastQFormatType.Illumina)), FastQFormatType.Solexa));
        }

        /// <summary>
        /// Converts Sanger quality scores to Solexa quality scores.
        /// </summary>
        /// <param name="qualScores">Sanger quality scores.</param>
        /// <returns>Returns Solexa quality scores.</returns>
        public static byte[] ConvertFromSangerToSolexa(byte[] qualScores)
        {
            if (qualScores == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualScores);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore, FastQFormatType.Sanger))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            byte[] newQualScores = new byte[qualScores.Length];

            for (int i = 0; i < qualScores.Length; i++)
            {
                newQualScores[i] = ConvertFromSangerToSolexa(qualScores[i]);
            }

            return newQualScores;
        }

        /// <summary>
        /// Converts Sanger quality scores to Illumina quality scores.
        /// </summary>
        /// <param name="qualScores">Sanger quality scores.</param>
        /// <returns>Returns Illumina quality scores.</returns>
        public static byte[] ConvertFromSangerToIllumina(byte[] qualScores)
        {
            if (qualScores == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualScores);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore, FastQFormatType.Sanger))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            byte[] newQualScores = new byte[qualScores.Length];

            for (int i = 0; i < qualScores.Length; i++)
            {
                newQualScores[i] = ConvertFromSangerToIllumina(qualScores[i]);
            }

            return newQualScores;
        }

        /// <summary>
        /// Converts Solexa quality scores to Sanger quality scores.
        /// </summary>
        /// <param name="qualScores">Solexa quality scores.</param>
        /// <returns>Returns Sanger quality scores.</returns>
        public static byte[] ConvertFromSolexaToSanger(byte[] qualScores)
        {
            if (qualScores == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualScores);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore, FastQFormatType.Solexa))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            byte[] newQualScores = new byte[qualScores.Length];

            for (int i = 0; i < qualScores.Length; i++)
            {
                newQualScores[i] = ConvertFromSolexaToSanger(qualScores[i]);
            }

            return newQualScores;
        }

        /// <summary>
        /// Converts Solexa quality scores to Illumina quality scores.
        /// </summary>
        /// <param name="qualScores">Solexa quality scores.</param>
        /// <returns>Returns Illumina quality scores.</returns>
        public static byte[] ConvertFromSolexaToIllumina(byte[] qualScores)
        {
            if (qualScores == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualScores);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore, FastQFormatType.Solexa))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            byte[] newQualScores = new byte[qualScores.Length];

            for (int i = 0; i < qualScores.Length; i++)
            {
                newQualScores[i] = ConvertFromSolexaToIllumina(qualScores[i]);
            }

            return newQualScores;
        }

        /// <summary>
        /// Converts Illumina quality scores to Sanger quality scores.
        /// </summary>
        /// <param name="qualScores">Illumina quality scores.</param>
        /// <returns>Returns Sanger quality scores.</returns>
        public static byte[] ConvertFromIlluminaToSanger(byte[] qualScores)
        {
            if (qualScores == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualScores);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore, FastQFormatType.Illumina))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            byte[] newQualScores = new byte[qualScores.Length];

            for (int i = 0; i < qualScores.Length; i++)
            {
                newQualScores[i] = ConvertFromIlluminaToSanger(qualScores[i]);
            }

            return newQualScores;
        }

        /// <summary>
        /// Converts Illumina quality scores to Solexa quality scores.
        /// </summary>
        /// <param name="qualScores">Illumina quality scores.</param>
        /// <returns>Returns Solexa quality scores.</returns>
        public static byte[] ConvertFromIlluminaToSolexa(byte[] qualScores)
        {
            if (qualScores == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualScores);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore, FastQFormatType.Illumina))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            byte[] newQualScores = new byte[qualScores.Length];

            for (int i = 0; i < qualScores.Length; i++)
            {
                newQualScores[i] = ConvertFromIlluminaToSolexa(qualScores[i]);
            }

            return newQualScores;
        }

        /// <summary>
        /// Gets the default quality score for the specified FastQFormatType.
        /// </summary>
        ///  /// <param name="type">FastQ format type.</param>
        /// <returns>Quality score.</returns>
        public static byte GetDefaultQualScore(FastQFormatType type)
        {
            if (type == FastQFormatType.Sanger)
            {
                return (byte)(SangerAsciiBaseValue + DefaultQualScore);
            }
            else if (type == FastQFormatType.Solexa)
            {
                return (byte)(SolexaAsciiBaseValue + DefaultQualScore);
            }
            else
            {
                return (byte)(IlluminaAsciiBaseValue + DefaultQualScore);
            }
        }

        /// <summary>
        /// Gets the maximum quality score for the specified FastQFormatType.
        /// </summary>
        ///  /// <param name="type">FastQ format type.</param>
        /// <returns>Quality score.</returns>
        public static byte GetMaxQualScore(FastQFormatType type)
        {
            if (type == FastQFormatType.Solexa)
            {
                return SolexaMaxQualScore;
            }
            else if (type == FastQFormatType.Sanger)
            {
                return SangerMaxQualScore;
            }
            else
            {
                return IlluminaMaxQualScore;
            }
        }

        /// <summary>
        /// Gets the minimum quality score for the specified FastQFormatType.
        /// </summary>
        /// <param name="type">FastQ format type.</param>
        /// <returns>Quality score.</returns>
        public static byte GetMinQualScore(FastQFormatType type)
        {
            if (type == FastQFormatType.Solexa)
            {
                return SolexaMinQualScore;
            }
            else if (type == FastQFormatType.Sanger)
            {
                return SangerMinQualScore;
            }
            else
            {
                return IlluminaMinQualScore;
            }
        }

        /// <summary>
        /// Creates a new QualitativeSequence that is a copy of the current QualitativeSequence.
        /// </summary>
        /// <returns>A new QualitativeSequence that is a copy of this QualitativeSequence.</returns>
        public QualitativeSequence Clone()
        {
            return new QualitativeSequence(this);
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
            StringBuilder buff = new StringBuilder();

            for (int i = 0; i < Count; i++)
            {
                buff.Append(this[i].Symbol);
            }

            return buff.ToString();
        }

        /// <summary>
        /// Converts the current instance to the specified FastQ format type 
        /// and returns a new instance of QualitativeSequence.
        /// </summary>
        /// <param name="type">FastQ format type to convert</param>
        public QualitativeSequence ConvertTo(FastQFormatType type)
        {
            QualitativeSequence qualSeq = null;

            if (Type == type)
            {
                qualSeq = Clone();
            }

            if (Type == FastQFormatType.Sanger)
            {
                if (type == FastQFormatType.Illumina)
                {
                    // Sanger to Illumina.
                    qualSeq = new QualitativeSequence(this.Alphabet,
                        type,
                        this.Encoding,
                        this.ToString(),
                        ConvertFromSangerToIllumina(this.Scores));
                }
                else if (type == FastQFormatType.Solexa)
                {
                    // Sanger To Solexa.
                    qualSeq = new QualitativeSequence(this.Alphabet,
                      type,
                      this.Encoding,
                      this.ToString(),
                      ConvertFromSangerToSolexa(this.Scores));
                }
            }
            else if (Type == FastQFormatType.Solexa)
            {
                if (type == FastQFormatType.Sanger)
                {
                    // Solexa to Sanger.
                    qualSeq = new QualitativeSequence(this.Alphabet,
                     type,
                     this.Encoding,
                     this.ToString(),
                     ConvertFromSolexaToSanger(this.Scores));
                }
                else if (type == FastQFormatType.Illumina)
                {
                    // Solexa to Illumina.
                    qualSeq = new QualitativeSequence(this.Alphabet,
                    type,
                    this.Encoding,
                    this.ToString(),
                    ConvertFromSolexaToIllumina(this.Scores));
                }
            }
            else
            {
                if (type == FastQFormatType.Sanger)
                {
                    // Illumina to Sanger.
                    qualSeq = new QualitativeSequence(this.Alphabet,
                    type,
                    this.Encoding,
                    this.ToString(),
                    ConvertFromIlluminaToSanger(this.Scores));
                }
                else if (type == FastQFormatType.Solexa)
                {
                    // Illumina to Solexa.
                    qualSeq = new QualitativeSequence(this.Alphabet,
                    type,
                    this.Encoding,
                    this.ToString(),
                    ConvertFromIlluminaToSolexa(this.Scores));
                }
            }

            return qualSeq;
        }
        #endregion Public Methods

        #region IQualitativeSequence Members
        /// <summary>
        /// Gets the type of this QualitativeSequence.
        /// </summary>
        public FastQFormatType Type { get; private set; }

        /// <summary>
        /// Gets the quality scores.
        /// </summary>
        public byte[] Scores
        {
            get
            {
                if (IsReadOnly)
                {
                    if ((_scores == null || _scores.Length == 0) && VirtualQualitativeSequenceProvider != null)
                    {
                        _scores = VirtualQualitativeSequenceProvider.GetScores().ToArray();
                    }
                    return _scores;
                }
                else
                {
                    if (_scoreList.Count == 0 && VirtualQualitativeSequenceProvider != null)
                    {
                        _scoreList = VirtualQualitativeSequenceProvider.GetScores().ToList();
                    }
                    return _scoreList.ToArray();
                }
                
            }
        }

        /// <summary>
        /// Indicates if the specified quality value is contained in the sequence anywhere.
        /// </summary>
        /// <param name="qualScore">Quality score to be verified.</param>
        /// <returns>If found returns true else returns false.</returns>
        public bool Contains(byte qualScore)
        {
            if (IsReadOnly)
            {
                return _scores.Contains(qualScore);
            }

            return _scoreList.Contains(qualScore);
        }

        /// <summary>
        /// Adds a sequence item and its quality score to the end of the sequence. The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        /// <param name="item">The item to add to the end of the sequence</param>
        /// <param name="qualScore">Quality score.</param>
        public void Add(ISequenceItem item, byte qualScore)
        {
            Insert(Count, item, qualScore);
        }

        /// <summary>
        /// Inserts the specified sequence item to a specified positon in this sequence.
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be inserted.</param>
        /// <param name="item">Sequence item to be inserted.</param>
        /// <param name="qualScore">Quality score.</param>
        public void Insert(int position, ISequenceItem item, byte qualScore)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (item == null)
                throw new ArgumentNullException(Resource.ParameterNameItem);

            InsertRange(position, item.Symbol.ToString(), qualScore);
        }

        /// <summary>
        /// Inserts specified character at the specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="character">A character which indicates a sequence item.</param>
        /// <param name="qualScore">Quality score.</param>
        public void Insert(int position, char character, byte qualScore)
        {
            InsertRange(position, character.ToString(), qualScore);
        }

        /// <summary>
        /// Inserts specified sequence string at specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScore">Quality score.</param>
        public void InsertRange(int position, string sequence, byte qualScore)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (!ValidateQualScore(qualScore))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            // non-DV insert
            if (VirtualQualitativeSequenceProvider == null)
            {
                _sequence.InsertRange(position, sequence);
            }
            else // DV insert
            {
                VirtualQualitativeSequenceProvider.InsertRange(position, sequence);
            }

            for (int i = position; i < position + sequence.Length; i++)
            {
                _scoreList.Insert(i, qualScore);
            }
        }

        /// <summary>
        /// Inserts specified sequence string at specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScores">Quality scores.</param>
        public void InsertRange(int position, string sequence, IEnumerable<byte> qualScores)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position > Count)
                throw new ArgumentOutOfRangeException(
                    Resource.ParameterNamePosition,
                    Resource.ParameterMustLessThanOrEqualToCount);

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            if (qualScores == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualScores);
            }

            if (sequence.Length != qualScores.Count())
            {
                throw new ArgumentException(Resource.InvalidLength_Sequence_QualScores);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            // non-DV insert
            if (VirtualQualitativeSequenceProvider == null)
            {
                _sequence.InsertRange(position, sequence);
            }
            else // DV insert
            {
                VirtualQualitativeSequenceProvider.InsertRange(position, sequence);
            }

            _scoreList.InsertRange(position, qualScores);
        }

        /// <summary>
        /// Replaces the sequence item present in the specified postion in this sequence 
        /// with a sequence item which is represented by specified character. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="character">Character which represent a sequence item.</param>
        /// <param name="qualScore">Quality score.</param>
        public void Replace(int position, char character, byte qualScore)
        {
            ReplaceRange(position, character.ToString(), qualScore);
        }

        /// <summary>
        /// Replaces the sequence item present in the specified postion in this sequence with the specified sequence item. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="item">Sequence item to be placed at the specified position.</param>
        /// /// <param name="qualScore">Quality score.</param>
        public void Replace(int position, ISequenceItem item, byte qualScore)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (item == null)
                throw new ArgumentNullException(Resource.ParameterNameItem);

            ReplaceRange(position, item.Symbol.ToString(), qualScore);
        }

        /// <summary>
        /// Replaces the sequence items present in the specified position in this sequence with the specified sequence.
        /// </summary>
        /// <param name="position">Position from which the replace of sequence items has to be started.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScore">Quality score.</param>
        public void ReplaceRange(int position, string sequence, byte qualScore)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
                throw new ArgumentOutOfRangeException(
                    Resource.ParameterNamePosition,
                    Resource.ParameterMustLessThanCount);

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            if (!ValidateQualScore(qualScore))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            if ((Count - position) < sequence.Length)
            {
                throw new ArgumentException(
                    Resource.InvalidPositionAndLength,
                    Resource.ParameterNameSequence);
            }

            // non-DV replace
            if (VirtualQualitativeSequenceProvider == null)
            {
                _sequence.ReplaceRange(position, sequence);
            }
            else // DV replace
            {
                VirtualQualitativeSequenceProvider.RemoveRange(position, sequence.Length);
                VirtualQualitativeSequenceProvider.InsertRange(position, sequence);
            }

            for (int i = position; i < position + sequence.Length; i++)
            {
                _scoreList[i] = qualScore;
            }
        }

        /// <summary>
        /// Replaces the sequence items present in the specified position in this sequence with the specified sequence.
        /// </summary>
        /// <param name="position">Position from which the replace of sequence items has to be started.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        /// <param name="qualScores">Quality scores.</param>
        public void ReplaceRange(int position, string sequence, IEnumerable<byte> qualScores)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
                throw new ArgumentOutOfRangeException(
                    Resource.ParameterNamePosition,
                    Resource.ParameterMustLessThanCount);

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScores, message);
                }
            }

            if ((Count - position) < sequence.Length)
            {
                throw new ArgumentException(
                    Resource.InvalidPositionAndLength,
                    Resource.ParameterNameSequence);
            }

            // non-DV replace
            if (VirtualQualitativeSequenceProvider == null)
            {
                _sequence.ReplaceRange(position, sequence);
            }
            else // DV replace
            {
                VirtualQualitativeSequenceProvider.RemoveRange(position, sequence.Length);
                VirtualQualitativeSequenceProvider.InsertRange(position, sequence);
            }

            int index = position;
            foreach (byte qualScore in qualScores)
            {
                _scoreList[index++] = qualScore;
            }
        }

        /// <summary>
        /// Creates a new QualitativeSequence that is a copy of the current QualitativeSequence.
        /// </summary>
        /// <returns>A new IQualitativeSequence that is a copy of this QualitativeSequence.</returns>
        IQualitativeSequence IQualitativeSequence.Clone()
        {
            return new QualitativeSequence(this);
        }
        #endregion IQualitativeSequence Members

        #region IVirtualSequence Members
        /// <summary>
        /// Gets or sets maximum number of blocks per sequence
        /// </summary>
        public int MaxNumberOfBlocks
        {
            get
            {
                if (null == VirtualQualitativeSequenceProvider)
                {
                    return 0;
                }

                return VirtualQualitativeSequenceProvider.MaxNumberOfBlocks;
            }

            set
            {
                if (null == VirtualQualitativeSequenceProvider)
                {
                    throw new InvalidOperationException(Properties.Resource.DataVirtualizationPropertyCannotBeSet);
                }

                if (0 >= value)
                {
                    throw new InvalidOperationException(Properties.Resource.DataVirtualizationPropertyInvalidSet);
                }

                VirtualQualitativeSequenceProvider.MaxNumberOfBlocks = value;
            }
        }

        /// <summary>
        /// Gets or sets block size
        /// </summary>
        public int BlockSize
        {
            get
            {
                if (null == VirtualQualitativeSequenceProvider)
                {
                    return -1;
                }

                return VirtualQualitativeSequenceProvider.BlockSize;
            }

            set
            {
                if (null == VirtualQualitativeSequenceProvider)
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

                VirtualQualitativeSequenceProvider.BlockSize = value;
            }
        }
        #endregion

        #region ISequence Members
        /// <summary>
        /// An identification provided to distinguish the sequence to others
        /// being worked with.
        /// </summary>
        public string ID
        {
            get { return _sequence.ID; }
            set { _sequence.ID = value; }
        }

        /// <summary>
        /// An identification of the sequence that is meant to be understood
        /// by human users when displayed in an application or file format.
        /// </summary>
        public string DisplayID
        {
            get { return _sequence.DisplayID; }
            set { _sequence.DisplayID = value; }
        }

        /// <summary>
        /// The alphabet to which string representations of the sequence should
        /// conform.
        /// </summary>
        public IAlphabet Alphabet
        {
            get { return _sequence.Alphabet; }
            private set { _sequence.Alphabet = value; }
        }

        /// <summary>
        /// The molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        public MoleculeType MoleculeType
        {
            get { return _sequence.MoleculeType; }
            set { _sequence.MoleculeType = value; }
        }

        /// <summary>
        /// Keeps track of the number of occurrances of each symbol within a sequence.
        /// </summary>
        public SequenceStatistics Statistics
        {
            get { return _sequence.Statistics; }
        }

        /// <summary>
        /// Many sequence representations when saved to file also contain
        /// information about that sequence. Unfortunately there is no standard
        /// around what that data may be from format to format. This property
        /// allows a place to put structured metadata that can be accessed by
        /// a particular key.
        /// 
        /// For example, if species information is stored in a particular OrganismInfo
        /// class, you could add it to the dictionary by:
        /// 
        /// mySequence.Metadata["OrganismInfo"] = myOrganismInfo;
        /// 
        /// To fetch the data you would use:
        /// 
        /// OrganismInfo myOrganismInfo = mySequence.Metadata["OrganismInfo"];
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
            get { return _sequence.Metadata; }
        }

        /// <summary>
        /// The Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a sequence. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public object Documentation
        {
            get
            {
                return _sequence.Documentation;
            }

            set
            {
                _sequence.Documentation = value;
            }
        }

        /// <summary>
        /// Return a sequence representing this sequence with the orientation reversed.
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
        /// Return a sequence representing the complement of this sequence.
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
        /// Return a sequence representing the reverse complement of this sequence.
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
        /// Return a sequence representing a range (substring) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The sequence which is sub sequence of this sequence.</returns>
        public ISequence Range(int start, int length)
        {
            if (start < 0 || start >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Resource.ParameterNameStart,
                    Resource.ParameterMustLessThanCount);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    Resource.ParameterNameLength,
                    Resource.ParameterMustNonNegative);
            }

            if ((Count - start) < length)
            {
                throw new ArgumentException(Resource.InvalidStartAndLength);
            }

            // reversed = false, complemented = false, range is as passed.
            return new BasicDerivedSequence(this, false, false, start, length);
        }

        /// <summary>
        /// Inserts specified character at the specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="character">A character which indicates a sequence item.</param>
        public void Insert(int position, char character)
        {
            InsertRange(position, character.ToString(), GetDefaultQualScore());
        }

        /// <summary>
        /// Inserts specified sequence string at specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        public void InsertRange(int position, string sequence)
        {
            InsertRange(position, sequence, GetDefaultQualScore());
        }

        /// <summary>
        /// Removes specified length of sequence items present in this sequence from the specified postion.
        /// </summary>
        /// <param name="position">Position from which the sequence items to be removed.</param>
        /// <param name="length">Number of sequence items to be removed.</param>
        public void RemoveRange(int position, int length)
        {
            // non-DV edit
            if (VirtualQualitativeSequenceProvider == null)
            {
                _sequence.RemoveRange(position, length);
            }
            else // DV edit
            {
                VirtualQualitativeSequenceProvider.RemoveRange(position, length);
            }

            _scoreList.RemoveRange(position, length);
        }

        /// <summary>
        /// Replaces the quality score present in the specified postion in this sequence 
        /// with the specified quality score.
        /// </summary>
        /// <param name="position">Position at which the quality score has to be replaced.</param>
        /// <param name="qualScore">Quality score.</param>
        public void Replace(int position, byte qualScore)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
                throw new ArgumentOutOfRangeException(
                    Resource.ParameterNamePosition,
                    Resource.ParameterMustLessThanCount);

            if (!ValidateQualScore(qualScore))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
            }

            _scoreList[position] = qualScore;
        }

        /// <summary>
        /// Replaces the quality scores present in the specified position in this sequence with the specified quality scores.
        /// </summary>
        /// <param name="position">Position from which the replace of quality scores has to be started.</param>
        /// <param name="qualScores">List of quality scores.</param>
        public void ReplaceRange(int position, IEnumerable<byte> qualScores)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
                throw new ArgumentOutOfRangeException(
                    Resource.ParameterNamePosition,
                    Resource.ParameterMustLessThanCount);
            int length = qualScores.Count();

            if ((Count - position) < length)
            {
                throw new ArgumentException(
                    Resource.InvalidPositionAndLength,
                    Resource.ParameterNameSequence);
            }

            foreach (byte qualScore in qualScores)
            {
                if (!ValidateQualScore(qualScore))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    throw new ArgumentOutOfRangeException(Resource.ParameterNameQualScore, message);
                }
            }

            int index = position;
            foreach (byte qualScore in qualScores)
            {
                _scoreList[index++] = qualScore;
            }
        }

        /// <summary>
        /// Replaces the sequence item present in the specified postion in this sequence 
        /// with a sequence item which is represented by specified character. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="character">Character which represent a sequence item.</param>
        public void Replace(int position, char character)
        {
            ReplaceRange(position, character.ToString(), GetDefaultQualScore());
        }

        /// <summary>
        /// Replaces the sequence item present in the specified postion in this sequence with the specified sequence item. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="item">Sequence item to be placed at the specified position.</param>
        public void Replace(int position, ISequenceItem item)
        {
            Replace(position, item, GetDefaultQualScore());
        }

        /// <summary>
        /// Replaces the sequence data present in the specified position in this sequence with the specified sequence data.
        /// </summary>
        /// <param name="position">Position from which the replace of sequence data has to be started.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        public void ReplaceRange(int position, string sequence)
        {
            ReplaceRange(position, sequence, GetDefaultQualScore());
        }

        /// <summary>
        /// Gets the index of first non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public int IndexOfNonGap()
        {
            return _sequence.IndexOfNonGap();
        }

        /// <summary>
        /// Returns the position of the first item from startPos that does not 
        /// have a Gap character.
        /// </summary>
        /// <param name="startPos">Index value above which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public int IndexOfNonGap(int startPos)
        {
            return _sequence.IndexOfNonGap(startPos);
        }

        /// <summary>
        /// Gets the index of last non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public int LastIndexOfNonGap()
        {
            return _sequence.LastIndexOfNonGap();
        }

        /// <summary>
        /// Gets the index of last non gap character within the specified end position.
        /// </summary>
        /// <param name="endPos">Index value below which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public int LastIndexOfNonGap(int endPos)
        {
            return _sequence.LastIndexOfNonGap(endPos);
        }

        /// <summary>
        /// Creates a new QualitativeSequence that is a copy of the current QualitativeSequence.
        /// </summary>
        /// <returns>A new ISequence that is a copy of this QualitativeSequence.</returns>
        ISequence ISequence.Clone()
        {
            return Clone();
        }

        #endregion ISequence Members

        #region IList<ISequenceItem> Members

        /// <summary>
        /// Returns index of the first item matching the specified item.
        /// </summary>
        /// <param name="item">Sequence item.</param>
        /// <returns>If found returns index of first occurrence; otherwise returns -1.</returns>
        public int IndexOf(ISequenceItem item)
        {
            // Non-DV index
            if (VirtualQualitativeSequenceProvider == null)
            {
                return _sequence.IndexOf(item);
            }
            else // DV index
            {
                return VirtualQualitativeSequenceProvider.IndexOf(item);
            }
        }

        /// <summary>
        /// Inserts the specified sequence item to a specified positon in this sequence.
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be inserted.</param>
        /// <param name="item">Sequence item to be inserted.</param>
        public void Insert(int position, ISequenceItem item)
        {
            Insert(position, item, GetDefaultQualScore());
        }

        /// <summary>
        /// Removes the sequence data present in the specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence data has to be removed.</param>
        public void RemoveAt(int position)
        {
            RemoveRange(position, 1);
        }

        /// <summary>
        /// Allows the sequence to function like an array, gets or sets
        /// the sequence item at the specified index. Note that the
        /// index value starts its count at 0.
        /// </summary>
        public ISequenceItem this[int index]
        {
            get
            {
                if (index >= 0 && index < Count)
                {
                    if (VirtualQualitativeSequenceProvider != null)
                    {
                        return VirtualQualitativeSequenceProvider[index];
                    }

                    return _sequence[index];
                }

                throw new ArgumentOutOfRangeException("index");

            }

            set
            {
                _sequence[index] = value;
            }
        }

        #endregion  IList<ISequenceItem> Members

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// Adds the specified sequence item to the end of this sequence.
        /// </summary>
        /// <param name="item">Sequence item to be added.</param>
        public void Add(ISequenceItem item)
        {
            Insert(Count, item);
        }

        /// <summary>
        /// Clears the underlying sequence data in this sequence.
        /// </summary>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            // non-DV clear
            if (VirtualQualitativeSequenceProvider == null)
            {
                _sequence.Clear();
            }
            else // DV clear
            {
                VirtualQualitativeSequenceProvider.Clear();
            }
            _scoreList.Clear();
        }

        /// <summary>
        /// Indicates if a sequence item is contained in the sequence anywhere.
        /// </summary>
        /// <param name="item">Sequence item to be verified.</param>
        /// <returns>If found returns true else returns false.</returns>
        public bool Contains(ISequenceItem item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the sequence items in this instace into a preallocated array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">A preallocated array of ISequenceItem to which the 
        /// ISequenceItems in this instance has to be copied.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            // non-DV copy
            if (VirtualQualitativeSequenceProvider == null)
            {
                _sequence.CopyTo(array, arrayIndex);
            }
            else // DV copy
            {
                VirtualQualitativeSequenceProvider.CopyTo(array, arrayIndex);
            }

        }

        /// <summary>
        /// The number of sequence items contained in the QualitativeSequence.
        /// </summary>
        public int Count
        {
            get
            {
                if (VirtualQualitativeSequenceProvider != null)
                {
                    return VirtualQualitativeSequenceProvider.Count;
                }
                return _sequence.Count;
            }
        }

        /// <summary>
        /// A flag indicating whether or not edits can be made to this QualitativeSequence.
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
            get
            {
                return _sequence.IsReadOnly;
            }

            set
            {
                if (VirtualQualitativeSequenceProvider != null && (_scores == null || _scores.Length == 0) && value == false)
                {
                    _scores = VirtualQualitativeSequenceProvider.GetScores().ToArray();
                }

                if (value == false && _sequence.IsReadOnly == true)
                {
                    // Switch from array to list
                    _scoreList = _scores.ToList();
                    _scores = null;
                }
                else if (value == true && _sequence.IsReadOnly == false)
                {
                    // Switch from list to array
                    _scores = _scoreList.ToArray();
                    _scoreList.Clear();
                }

                _sequence.IsReadOnly = value;
            }
        }

        /// <summary>
        /// Removes first occurance of the specified sequence item in this QualitativeSequence.
        /// </summary>
        /// <param name="item">Sequence item to be removed.</param>
        /// <returns>True if the item was found and removed, false if the item was not found.</returns>
        public bool Remove(ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Resource.CanNotModifyReadonlySequence);

            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            RemoveRange(index, 1);
            return true;
        }

        #endregion ICollection<ISequenceItem> Members

        #region IEnumerable<ISequenceItem> Members
        /// <summary>
        /// Retrieves an enumerator for this QualitativeSequence.
        /// </summary>
        /// <returns>IEnumerator of ISequenceItem.</returns>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion IEnumerable<ISequenceItem> Members

        #region IEnumerable Members
        /// <summary>
        /// Retrieves an enumerator for this QualitativeSequence.
        /// </summary>
        /// <returns>IEnumerator of ISequenceItem.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion IEnumerable Members

        #region ICloneable Members
        /// <summary>
        /// Creates a new QualitativeSequence that is a copy of the current QualitativeSequence.
        /// </summary>
        /// <returns>A new object that is a copy of this QualitativeSequence.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected QualitativeSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Type = (FastQFormatType)info.GetValue("QualitativeSequence:Type", typeof(FastQFormatType));
            _sequence = (Sequence)info.GetValue("QualitativeSequence:Sequence", typeof(Sequence));
            if (_sequence.IsReadOnly)
            {
                _scores = (byte[])info.GetValue("QualitativeSequence:Values", typeof(byte[]));
            }
            else
            {
                _scoreList = (List<byte>)info.GetValue("QualitativeSequence:Values", typeof(List<byte>));
            }
        }

        /// <summary>
        /// Method for serializing the QualitativeSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("QualitativeSequence:Sequence", _sequence);
            info.AddValue("QualitativeSequence:Type", Type);
            if (_sequence.IsReadOnly)
            {
                info.AddValue("QualitativeSequence:Values", _scores);
            }
            else
            {
                info.AddValue("QualitativeSequence:Values", _scoreList);
            }
        }

        #endregion ISerializable Members

        #region Private Methods
        /// <summary>
        /// Gets the quality score from the ASCII encoded quality score.
        /// </summary>
        /// <param name="qualScore">ASCII Encoded quality score.</param>
        /// <param name="type">FastQ format type.</param>
        /// <returns>Returns quality score.</returns>
        private static int GetQualScore(byte qualScore, FastQFormatType type)
        {
            if (type == FastQFormatType.Sanger)
            {
                return qualScore - SangerAsciiBaseValue;
            }
            else if (type == FastQFormatType.Solexa)
            {
                return qualScore - SolexaAsciiBaseValue;
            }
            else
            {
                return qualScore - IlluminaAsciiBaseValue;
            }
        }

        /// <summary>
        /// Gets the ASCII encoded quality score for the given quality score.
        /// </summary>
        /// <param name="qualScore">Quality Score.</param>
        /// <param name="type">FastQ format type</param>
        /// <returns>ASCII encoded quality score.</returns>
        private static byte GetEncodedQualScore(int qualScore, FastQFormatType type)
        {
            if (type == FastQFormatType.Sanger)
            {
                return (byte)(qualScore + SangerAsciiBaseValue);
            }
            else if (type == FastQFormatType.Solexa)
            {
                return (byte)(qualScore + SolexaAsciiBaseValue);
            }
            else
            {
                return (byte)(qualScore + IlluminaAsciiBaseValue);
            }
        }

        /// <summary>
        /// Validates whether the specified quality score is within the FastQFormatType limit or not.
        /// </summary>
        /// <param name="qualScore">Quality score.</param>
        /// <param name="type">Fastq format type.</param>
        /// <returns>Returns true if the specified quality score is with in the limit, otherwise false.</returns>
        private static bool ValidateQualScore(byte qualScore, FastQFormatType type)
        {
            if (type == FastQFormatType.Sanger)
            {
                return qualScore >= SangerMinQualScore && qualScore <= SangerMaxQualScore;
            }
            else if (type == FastQFormatType.Solexa)
            {
                return qualScore >= SolexaMinQualScore && qualScore <= SolexaMaxQualScore;
            }
            else
            {
                return qualScore >= IlluminaMinQualScore && qualScore <= IlluminaMaxQualScore;
            }
        }

        /// <summary>
        /// Converts Phred quality score to Solexa quality score.
        /// </summary>
        /// <param name="qualScore">Quality score to be converted.</param>
        /// <returns>Solexa quality score.</returns>
        private static int ConvertFromPhredToSolexa(int qualScore)
        {
            if (qualScore == 0)
            {
                return -5;
            }

            int minQualvalue = GetQualScore(SolexaMinQualScore, FastQFormatType.Solexa);
            int maxQualValue = GetQualScore(SolexaMaxQualScore, FastQFormatType.Solexa);

            return Math.Min(maxQualValue, Math.Max(minQualvalue, (int)Math.Round(10 * Math.Log10(Math.Pow(10, (qualScore / 10.0)) - 1), 0)));
        }

        /// <summary>
        /// Converts Solexa quality score to Phred quality score.
        /// </summary>
        /// <param name="qualScore">Quality score to be converted.</param>
        /// <returns>Phred quality score.</returns>
        private static int ConvertFromSolexaToPhared(int qualScore)
        {
            if (qualScore == -5)
            {
                return 0;
            }

            int minQualvalue = GetQualScore(SangerMinQualScore, FastQFormatType.Sanger);
            int maxQualValue = GetQualScore(SangerMaxQualScore, FastQFormatType.Sanger);

            return Math.Min(maxQualValue, Math.Max(minQualvalue, (int)Math.Round(10 * Math.Log10(Math.Pow(10, (qualScore / 10.0)) + 1), 0)));
        }

        /// <summary>
        /// Gets the default quality score depending on the QualitativeSequenceItem.
        /// </summary>
        /// <returns></returns>
        private byte GetDefaultQualScore()
        {
            return GetDefaultQualScore(Type);
        }

        /// <summary>
        /// Validates whether the specified quality score is within the FastQFormatType limit or not.
        /// </summary>
        /// <param name="qualScore">Quality score.</param>
        /// <returns>Returns true if the specified quality score is with in the limit, otherwise false.</returns>
        private bool ValidateQualScore(byte qualScore)
        {
            return ValidateQualScore(qualScore, Type);
        }

        #endregion Private Methods

    }
}
