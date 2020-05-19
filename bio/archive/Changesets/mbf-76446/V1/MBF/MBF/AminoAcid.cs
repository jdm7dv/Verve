// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MBF
{
    /// <summary>
    /// The amino acid implementations of ISequenceItem allows for the
    /// definition of the amino acid peptide sequences.
    /// <para>
    /// AminoAcid adds the ExtendedValue field to those found in ISequenceItem.
    /// This supports a second abbreviation beyond that of the one character
    /// representation.
    /// </para>
    /// <para>
    /// For example IUPAC and NCBI standards define Amino Acids with both one
    /// character and three character symbols. Thus for example Serine would have:
    /// </para>
    /// Value: 17
    /// Symbol: S
    /// ExtendedSymbol: Ser
    /// Name: Serine
    /// </summary>
    [Serializable]
    public class AminoAcid : ISequenceItem
    {
        #region Fields
        /// <summary>
        /// Stores encoding value
        /// </summary>
        private byte val;

        /// <summary>
        /// A character symbol representing the item. For instance,
        /// symbols could include A for Alanine and G for Glycine.
        /// Also possible are symbols representing gaps, termination
        /// characters, or ambiguities.
        /// </summary>
        private char symbol;

        /// <summary>
        /// A symbol representing the item. For instance,
        /// symbols could include Ala for Alanine and Gly for Glycine.
        /// Also possible are symbols representing gaps, termination
        /// characters, or ambiguities.
        /// </summary>
        private string extSymbol;

        /// <summary>
        /// A human readable and display appropriate name for the item.
        /// For example, 'Alanine' or 'Glycine'.
        /// </summary>
        private string name;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        public AminoAcid(char symbol, string name) :
            this(0, symbol, symbol.ToString(), name)
        {
            // Does not require any additional implementation
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="isTermination">Indicates if this is a termination character.</param>
        public AminoAcid(char symbol, string name, bool isGap, bool isAmbiguous, bool isTermination) :
            this(0, symbol, symbol.ToString(), name, isGap, isAmbiguous, isTermination)
        {
            // Does not require any additional implementation
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="extSymbol">A multi-character symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        public AminoAcid(char symbol, string extSymbol, string name) :
            this(0, symbol, extSymbol, name)
        {
            // Does not require any additional implementation
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="extSymbol">A multi-character symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="isTermination">Indicates if this is a termination character.</param>
        public AminoAcid(char symbol, string extSymbol, string name, bool isGap, bool isAmbiguous, bool isTermination) :
            this(0, symbol, extSymbol, name, isGap, isAmbiguous, isTermination)
        {
            // Does not require any additional implementation
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the acid</param>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        public AminoAcid(byte value, char symbol, string name) :
            this(value, symbol, symbol.ToString(), name)
        {
            // Does not require any additional implementation
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the acid</param>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="isTermination">Indicates if this is a termination character.</param>
        public AminoAcid(byte value, char symbol, string name, bool isGap, bool isAmbiguous, bool isTermination) :
            this(value, symbol, symbol.ToString(), name, isGap, isAmbiguous, isTermination)
        {
            // Does not require any additional implementation
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the acid</param>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="extSymbol">A multi-character symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        public AminoAcid(byte value, char symbol, string extSymbol, string name)
            : this(value, symbol, extSymbol, name, false, false, false)
        {
            // Does not require any additional implementation
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Creates an amino acid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the acid</param>
        /// <param name="symbol">A symbol representing the acid</param>
        /// <param name="extSymbol">A multi-character symbol representing the acid</param>
        /// <param name="name">A readable name for the acid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="isTermination">Indicates if this is a termination character.</param>
        public AminoAcid(byte value, char symbol, string extSymbol, string name, bool isGap, bool isAmbiguous, bool isTermination)
        {
            this.val = value;
            this.symbol = symbol;
            this.name = name;
            this.extSymbol = extSymbol;
            IsGap = isGap;
            IsAmbiguous = isAmbiguous;
            IsTermination = isTermination;
        }

        /// <summary>
        /// Initializes a new instance of the AminoAcid class.
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected AminoAcid(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            val = info.GetByte("AminoAcid:Value");
            symbol = info.GetChar("AminoAcid:Symbol");
            extSymbol = info.GetString("AmminoAcid:ExternalSymbol");
            name = info.GetString("AminoAcid:Name");
            IsGap = info.GetBoolean("AminoAcid:IsGap");
            IsAmbiguous = info.GetBoolean("AminoAcid:IsAmbiguous");
            IsTermination = info.GetBoolean("AminoAcid:IsTermination");
        }

        /// <summary>
        /// Prevents a default instance of the AminoAcid class from being created.
        /// </summary>
        private AminoAcid() { }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets encoding value, unique for the particular item within its
        /// alphabet.
        /// </summary>
        public byte Value
        {
            get { return val; }
            internal set { val = value; }
        }

        /// <summary>
        /// Gets character symbol for amino acid
        /// </summary>
        public char Symbol
        {
            get { return symbol; }
            internal set { symbol = value; }
        }

        /// <summary>
        /// Gets extended symbol for amino acid
        /// </summary>
        public string ExtendedSymbol
        {
            get { return extSymbol; }
            internal set { extSymbol = value; }
        }

        /// <summary>
        /// Gets the display name for the amino acid.
        /// </summary>
        public string Name
        {
            get { return name; }
            internal set { name = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this amino acid is a Gap character
        /// </summary>
        public bool IsGap { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this amino acid is an Ambiguous character
        /// </summary>
        public bool IsAmbiguous { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this amino acid is a Termination character
        /// </summary>
        public bool IsTermination { get; internal set; }
        #endregion Properties

        #region Methods
        /// <summary>
        ///  Returns a new copy of the AminoAcid object.
        /// </summary>
        /// <returns>Clone of this amino acid</returns>
        public AminoAcid Clone()
        {
            AminoAcid amminoAcid = new AminoAcid();
            amminoAcid.val = val;
            amminoAcid.symbol = symbol;
            amminoAcid.extSymbol = extSymbol;
            amminoAcid.Name = name;
            amminoAcid.IsGap = IsGap;
            amminoAcid.IsAmbiguous = IsAmbiguous;
            amminoAcid.IsTermination = IsTermination;
            return amminoAcid;
        }

        /// <summary>
        ///  Returns a new copy of the AminoAcid object.
        /// </summary>
        /// <returns>Clone of this amino acid</returns>
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

            info.AddValue("AminoAcid:Value", val);
            info.AddValue("AminoAcid:Symbol", symbol);
            info.AddValue("AmminoAcid:ExternalSymbol", extSymbol);
            info.AddValue("AminoAcid:Name", name);
            info.AddValue("AminoAcid:IsGap", IsGap);
            info.AddValue("AminoAcid:IsAmbiguous", IsAmbiguous);
            info.AddValue("AminoAcid:IsTermination", IsTermination);
        }
        #endregion ISerializable Members

        #region ICloneable Members

        /// <summary>
        ///  Returns a new copy of the AminoAcid object.
        /// </summary>
        /// <returns>Clone of this amino acid</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion ICloneable Members

        #region Overridden Object Methods
        /// <summary>
        /// Overrides Object Equals.
        /// Two amino acids are judged equal, if they have the same symbol
        /// </summary>
        /// <param name="obj">Object to be compared with</param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            AminoAcid other = obj as AminoAcid;
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
        /// Get hash code for amino acid
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
