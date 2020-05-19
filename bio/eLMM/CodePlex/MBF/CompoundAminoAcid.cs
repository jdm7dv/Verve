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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.Properties;
using System.Runtime.Serialization;

namespace Bio
{
    /// <summary>
    /// CompoundAminoAcid supports, storing more than one amino acids with their weight in one base entry.
    /// For example,
    ///  Consider the below sequence where {D|N} could have statistical values of 30% and 65% and
    ///  {C|G|T} could have 30% each (they dont necessarily have to sum upto 100)
    ///     GATTADAGCTA
    ///          N  G
    ///             T
    /// Above sequence items {D|N} and {C|G|T} can be represented in compound sequence items like, 
    /// GATTARAGBTA
    ///  Where “B” and “X” are CompoundAminoAcids.
    /// CompoundAminoAcid B will contain amino acids D and N with their weights.
    /// CompoundAminoAcid X will contain nucleotides C, G and T with their weights.
    /// 
    /// This class can be used in SparseSequence.
    /// </summary>
    [Serializable]
    public class CompoundAminoAcid : AminoAcid, ICompoundSequenceItem
    {
        #region Fields
        // This will hold the 1:1 mapping of ISequenceItem and weight.
        private Dictionary<ISequenceItem, double> _seqItemToWeightMap = new Dictionary<ISequenceItem, double>();
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        public CompoundAminoAcid(char symbol, string name)
            : base(symbol, name) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        public CompoundAminoAcid(char symbol, string name, bool isGap, bool isAmbiguous)
            : base(symbol, name, isGap, isAmbiguous, false) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        public CompoundAminoAcid(char symbol, string extSymbol, string name)
            : base(symbol, extSymbol, name) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the acid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        public CompoundAminoAcid(char symbol, string extSymbol, string name, bool isGap, bool isAmbiguous)
            : base(symbol, extSymbol, name, isGap, isAmbiguous, false) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        public CompoundAminoAcid(byte value, char symbol, string name)
            : base(value, symbol, name) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        public CompoundAminoAcid(byte value, char symbol, string name, bool isGap, bool isAmbiguous)
            : base(value, symbol, name, isGap, isAmbiguous, false) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        public CompoundAminoAcid(byte value, char symbol, string extSymbol, string name)
            : base(value, symbol, extSymbol, name) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        public CompoundAminoAcid(byte value, char symbol, string extSymbol, string name, bool isGap, bool isAmbiguous)
            : base(value, symbol, extSymbol, name, isGap, isAmbiguous, false) { }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param>
        public CompoundAminoAcid(char symbol, string name, List<ISequenceItem> aminoacids, List<double> weights)
            : this(symbol, name)
        {
            Add(aminoacids, weights);
        }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param> 
        public CompoundAminoAcid(char symbol, string name, bool isGap, bool isAmbiguous, List<ISequenceItem> aminoacids, List<double> weights)
            : this(symbol, name, isGap, isAmbiguous)
        {
            Add(aminoacids, weights);
        }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param>  
        public CompoundAminoAcid(char symbol, string extSymbol, string name, List<ISequenceItem> aminoacids, List<double> weights)
            : this(symbol, extSymbol, name)
        {
            Add(aminoacids, weights);
        }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the acid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param>   
        public CompoundAminoAcid(char symbol, string extSymbol, string name, bool isGap, bool isAmbiguous, List<ISequenceItem> aminoacids, List<double> weights)
            : this(symbol, extSymbol, name, isGap, isAmbiguous)
        {
            Add(aminoacids, weights);
        }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param>    
        public CompoundAminoAcid(byte value, char symbol, string name, List<ISequenceItem> aminoacids, List<double> weights)
            : this(value, symbol, name)
        {
            Add(aminoacids, weights);
        }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param>    
        public CompoundAminoAcid(byte value, char symbol, string name, bool isGap, bool isAmbiguous, List<ISequenceItem> aminoacids, List<double> weights)
            : this(value, symbol, name, isGap, isAmbiguous)
        {
            Add(aminoacids, weights);
        }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param>    
        public CompoundAminoAcid(byte value, char symbol, string extSymbol, string name, List<ISequenceItem> aminoacids, List<double> weights)
            : this(value, symbol, extSymbol, name)
        {
            Add(aminoacids, weights);
        }

        /// <summary>
        /// Creates a CompoundAminoAcid representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundAminoAcid</param>
        /// <param name="symbol">A symbol representing the CompoundAminoAcid</param>
        /// <param name="extSymbol">A multi-character symbol representing the CompoundAminoAcid</param>
        /// <param name="name">A readable name for the CompoundAminoAcid</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="aminoacids">List of aminoacids.</param>
        /// <param name="weights">List of weights.</param>     
        public CompoundAminoAcid(byte value, char symbol, string extSymbol, string name, bool isGap, bool isAmbiguous, List<ISequenceItem> aminoacids, List<double> weights)
            : this(value, symbol, extSymbol, name, isGap, isAmbiguous)
        {
            Add(aminoacids, weights);
        }

        #endregion Constructors

        #region ICompoundSequenceItem Members
        /// <summary>
        /// Returns the read only collection of sequece items present in this CompoundAminoAcid.
        /// </summary>
        public IList<ISequenceItem> SequenceItems
        {
            get
            {
                return _seqItemToWeightMap.Keys.ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the weight for the specified sequence item.
        /// </summary>
        /// <param name="item">Sequence Item.</param>
        /// <returns>If found returns the weight of the specified item, otherwise returns double.NaN.</returns>
        public double GetWeight(ISequenceItem item)
        {
            if (item == null)
            {
                return double.NaN;
            }

            if (_seqItemToWeightMap.ContainsKey(item))
            {
                return _seqItemToWeightMap[item];
            }
            else
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Sets the weight for the specified item.
        /// </summary>
        /// <param name="item">Sequence Item for which the weight has to be set.</param>
        /// <param name="weight">New weight for the item.</param>
        public void SetWeight(ISequenceItem item, double weight)
        {
            _seqItemToWeightMap[item] = weight;
        }

        /// <summary>
        /// Adds specified Sequenceitem and weight.
        /// </summary>
        /// <param name="item">Sequence Item.</param>
        /// <param name="weight">Weight of specified item.</param>
        public void Add(ISequenceItem item, double weight)
        {
            if (item == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameItem);
            }

            if (!(item is AminoAcid))
            {
                throw new ArgumentException(Resource.ParameterItemMustBeAminoAcid);
            }

            if (_seqItemToWeightMap.ContainsKey(item))
            {
                throw new ArgumentException(Resource.ItemAlreadyExists);
            }

            _seqItemToWeightMap.Add(item, weight);
        }

        /// <summary>
        /// Removes the specified sequence item.
        /// </summary>
        /// <param name="item">Sequence item.</param>
        /// <returns>Returns true if the specified item removed, otherwise false.</returns>
        public bool Remove(ISequenceItem item)
        {
            if (item == null)
                return false;

            return _seqItemToWeightMap.Remove(item);
        }

        /// <summary>
        /// Returns a new copy of the CompoundAminoAcid object.
        /// </summary>
        ICompoundSequenceItem ICompoundSequenceItem.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Returns a new copy of the CompoundAminoAcid object.
        /// </summary>
        ISequenceItem ISequenceItem.Clone()
        {
            return Clone();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a new copy of the CompoundAminoAcid object.
        /// </summary>
        new public CompoundAminoAcid Clone()
        {
            CompoundAminoAcid compound = new CompoundAminoAcid(this.Value,this.Symbol,this.ExtendedSymbol, this.Name,this.IsGap,this.IsAmbiguous);

            foreach (ISequenceItem item in this._seqItemToWeightMap.Keys)
            {
                ISequenceItem clonedItem = item.Clone();
                double doubleValue = _seqItemToWeightMap[item];
                compound._seqItemToWeightMap.Add(clonedItem, doubleValue);
            }

            return compound;
        }
        #endregion Public Methods

        #region Private Methods
        // private method used in constructors.
        private void Add(List<ISequenceItem> aminoacids, List<double> weights)
        {
            if (aminoacids == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameAminoAcids);
            }

            if (weights == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameWeights);
            }

            if (aminoacids.Contains(null))
            {
                throw new ArgumentException(Resource.ParameterContainsNullValue, Resource.ParameterNameAminoAcids);
            }

            if (aminoacids.Count != weights.Count)
            {
                throw new ArgumentException(Resource.AminoAcidsAndWeightsShouldMatch);
            }

            for (int i = 0; i < aminoacids.Count; i++)
            {
                Add(aminoacids[i], weights[i]);
            }
        }
        #endregion Private Methods
      
        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected CompoundAminoAcid(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _seqItemToWeightMap = (Dictionary<ISequenceItem, double>)info.GetValue("CompoundAminoAcid:Map", typeof(Dictionary<ISequenceItem, double>));
        }

        /// <summary>
        /// Method for serializing the SparseSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        new public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            base.GetObjectData(info, context);

            info.AddValue("CompoundAminoAcid:Map", _seqItemToWeightMap);
        }

        #endregion ISerializable Members

        #region ICloneable Members
        /// <summary>
        /// Returns a new copy of the CompoundNucleotide object.
        /// </summary>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members
    }
}
