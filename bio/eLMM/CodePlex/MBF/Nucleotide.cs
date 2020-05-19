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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Bio
{
    /// <summary>
    /// The nucleotide implementation of ISequenceItem allows for items that
    /// when placed in sequence make up a DNA or RNA strand.
    /// </summary>
    [Serializable]
    public class Nucleotide : ISequenceItem
    {
        #region Fields
        /// <summary>
        /// An encoding value, unique for the particular item within its alphabet.
        /// </summary>
        private byte val;

        /// <summary>
        /// A character symbol representing the item. For instance in DNA,
        /// symbols would include G, A, T, and C. Also possible are symbols
        /// representing gaps, termination characters, or ambiguities.
        /// </summary>
        private char symbol;

        /// <summary>
        /// A human readable and display appropriate name for the item. For
        /// example, 'Adenine' or 'Cytosine'. For ambigous items, 'Adenine or Cytosine'.
        /// Also acceptable are 'Gap' or 'Termination'.
        /// </summary>
        private string name;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Nucleotide class.
        /// Creates an nucleotide representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the nucleotide</param>
        /// <param name="symbol">A symbol representing the nucleotide</param>
        /// <param name="name">A readable name for the nucleotide</param>
        public Nucleotide(byte value, char symbol, string name)
        {
            this.val = value;
            this.symbol = symbol;
            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the Nucleotide class.
        /// Creates an nucleotide representation based on its data. Nucleotides
        /// not defined for encoding will not have a byte Value field set, so
        /// this constructor automatically sets that field to zero.
        /// </summary>
        /// <param name="symbol">A symbol representing the nucleotide</param>
        /// <param name="name">A readable name for the nucleotide</param>
        public Nucleotide(char symbol, string name) : this(0, symbol, name) { }

        /// <summary>
        /// Initializes a new instance of the Nucleotide class.
        /// Creates an nucleotide representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the nucleotide</param>
        /// <param name="symbol">A symbol representing the nucleotide</param>
        /// <param name="name">A readable name for the nucleotide</param>
        /// <param name="isGap">Indicates if this is a gap character</param>
        /// <param name="isAmbiguous">Indicates if this is an ambigous character</param>
        public Nucleotide(byte value, char symbol, string name, bool isGap, bool isAmbiguous)
        {
            this.val = value;
            this.symbol = symbol;
            this.name = name;
            IsGap = isGap;
            IsAmbiguous = isAmbiguous;
        }

        /// <summary>
        /// Initializes a new instance of the Nucleotide class.
        /// Creates an nucleotide representation based on its data. Nucleotides
        /// not defined for encoding will not have a byte Value field set, so
        /// this constructor automatically sets that field to zero.
        /// </summary>
        /// <param name="symbol">A symbol representing the nucleotide</param>
        /// <param name="name">A readable name for the nucleotide</param>
        /// <param name="isGap">Indicates if this is a gap character</param>
        /// <param name="isAmbiguous">Indicates if this is an ambigous character</param>
        public Nucleotide(char symbol, string name, bool isGap, bool isAmbiguous) :
            this(0, symbol, name, isGap, isAmbiguous) { }

        /// <summary>
        /// Initializes a new instance of the Nucleotide class.
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected Nucleotide(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            val = info.GetByte("Nucleotide:Value");
            symbol = info.GetChar("Nucleotide:Symbol");
            name = info.GetString("Nucleotide:Name");
            IsGap = info.GetBoolean("Nucleotide:IsGap");
            IsAmbiguous = info.GetBoolean("Nucleotide:IsAmbiguous");
        }

        /// <summary>
        /// Prevents a default instance of the Nucleotide class from being created.
        /// </summary>
        private Nucleotide() { }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets encoding value of this item.
        /// </summary>
        public byte Value
        {
            get { return val; }
            internal set { val = value; }
        }

        /// <summary>
        /// Gets the character symbol representing the item.
        /// </summary>
        public char Symbol
        {
            get { return symbol; }
            internal set { symbol = value; }
        }

        /// <summary>
        /// Gets the display name for the item.
        /// </summary>
        public string Name
        {
            get { return name; }
            internal set { name = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this Nucleotide is a Gap character.
        /// </summary>
        public bool IsGap { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this Nucleotide is Ambiguous.
        /// </summary>
        public bool IsAmbiguous { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this Nucleotide is a Termination item
        /// Always returns false, as Nuceleotides do not have a defined termination character.
        /// </summary>
        public bool IsTermination
        {
            get
            {
                return false;
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Returns a new copy of the Nucleotide object.
        /// </summary>
        /// <returns>Clone of this nucleotide</returns>
        public virtual Nucleotide Clone()
        {
            Nucleotide nucleotide = new Nucleotide();
            nucleotide.val = val;
            nucleotide.symbol = symbol;
            nucleotide.Name = name;
            nucleotide.IsGap = IsGap;
            nucleotide.IsAmbiguous = IsAmbiguous;
            return nucleotide;
        }

        /// <summary>
        /// Returns a new copy of the Nucleotide object.
        /// </summary>
        /// <returns>Clone of this nucleotide</returns>
        ISequenceItem ISequenceItem.Clone()
        {
            return Clone();
        }

        #endregion Methods

        #region ISerializable Members
        /// <summary>
        /// Method for serializing the SparseSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Nucleotide:Value", val);
            info.AddValue("Nucleotide:Symbol", symbol);
            info.AddValue("Nucleotide:Name", name);
            info.AddValue("Nucleotide:IsGap", IsGap);
            info.AddValue("Nucleotide:IsAmbiguous", IsAmbiguous);
        }
        #endregion ISerializable Members

        #region ICloneable Members
        /// <summary>
        /// Returns a new copy of the Nucleotide object.
        /// </summary>
        /// <returns>Clone of this nucleotide</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members

        #region Overridden Object Methods
        /// <summary>
        /// Overrides Object Equals.
        /// Two nucleotides are judged equal, if they have the same symbol
        /// </summary>
        /// <param name="obj">Object to be compared with</param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            Nucleotide other = obj as Nucleotide;
            if (other != null)
            {
                return this.Symbol == other.Symbol;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get hash code for nucleotide
        /// Uses symbol for calculation
        /// </summary>
        /// <returns>Hash value</returns>
        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode();
        }

        #endregion
    }
}
