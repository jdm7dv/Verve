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
using System.Text;
using MBF.Properties;
using System.Runtime.Serialization;

namespace MBF
{
    /// <summary>
    /// CompoundNucleotide supports, storing more than one nucletides with their weight in one base entry.
    /// For example,
    ///  Consider the below sequence where {G|A} could have statistical values of 30% and 65% and
    ///  {C|G|T} could have 30% each (they dont necessarily have to sum upto 100)
    ///     GATTAGAGCTA
    ///          A  G
    ///             T
    /// Above sequence items {G|A} and {C|G|T} can be represented in compound sequence items like, 
    /// GATTARAGBTA
    ///  Where “R” and “B” are CompoundNucleotides.
    /// CompoundNucleotide R will contain nucleotides G and A with their weights.
    /// CompoundNucleotide B will contain nucleotides C, G and T with their weights.
    /// 
    /// This class can be used in SparseSequence.
    /// </summary>
    [Serializable]
    public class CompoundNucleotide : Nucleotide, ICompoundSequenceItem
    {
        #region Fields
        // This will hold the 1:1 mapping of ISequenceItem and weight.
        private Dictionary<ISequenceItem, double> _seqItemToWeightMap = new Dictionary<ISequenceItem, double>();

        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundNucleotide</param>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        public CompoundNucleotide(byte value, char symbol, string name)
            : base(value, symbol, name) { }

        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data. CompoundNucleotide
        /// not defined for encoding will not have a byte Value field set, so
        /// this constructor automatically sets that field to zero.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        public CompoundNucleotide(char symbol, string name)
            : base(symbol, name) { }

        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundNucleotide</param>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        /// <param name="isGap">Indicates if this is a gap CompoundNucleotide</param>
        /// <param name="isAmbiguous">Indicates if this is an ambigous CompoundNucleotide</param>
        public CompoundNucleotide(byte value, char symbol, string name, bool isGap, bool isAmbiguous)
            : base(value, symbol, name, isGap, isAmbiguous) { }

        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data. CompoundNucleotide
        /// not defined for encoding will not have a byte Value field set, so
        /// this constructor automatically sets that field to zero.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        /// <param name="isGap">Indicates if this is a gap CompoundNucleotide</param>
        /// <param name="isAmbiguous">Indicates if this is an ambigous CompoundNucleotide</param>
        public CompoundNucleotide(char symbol, string name, bool isGap, bool isAmbiguous)
            : base(symbol, name, isGap, isAmbiguous) { }

        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundNucleotide</param>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        /// <param name="nucleotides">List of nucleoties.</param>
        /// <param name="weights">List of weights.</param>
        public CompoundNucleotide(byte value, char symbol, string name, List<ISequenceItem> nucleotides, List<double> weights)
            : this(value, symbol, name)
        {
            Add(nucleotides, weights);
        }

        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data. CompoundNucleotide
        /// not defined for encoding will not have a byte Value field set, so
        /// this constructor automatically sets that field to zero.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        /// <param name="nucleotides">List of nucleoties.</param>
        /// <param name="weights">List of weights.</param>
        public CompoundNucleotide(char symbol, string name, List<ISequenceItem> nucleotides, List<double> weights)
            : this(symbol, name)
        {
            Add(nucleotides, weights);
        }

        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data.
        /// </summary>
        /// <param name="value">A byte encoding for the CompoundNucleotide</param>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        /// <param name="isGap">Indicates if this is a gap CompoundNucleotide</param>
        /// <param name="isAmbiguous">Indicates if this is an ambigous CompoundNucleotide</param>
        /// <param name="nucleotides">List of nucleoties.</param>
        /// <param name="weights">List of weights.</param>
        public CompoundNucleotide(byte value, char symbol, string name, bool isGap, bool isAmbiguous, List<ISequenceItem> nucleotides, List<double> weights)
            : this(value, symbol, name, isGap, isAmbiguous)
        {
            Add(nucleotides, weights);
        }

        /// <summary>
        /// Creates a CompoundNucleotide representation based on its data. CompoundNucleotide
        /// not defined for encoding will not have a byte Value field set, so
        /// this constructor automatically sets that field to zero.
        /// </summary>
        /// <param name="symbol">A symbol representing the CompoundNucleotide</param>
        /// <param name="name">A readable name for the CompoundNucleotide</param>
        /// <param name="isGap">Indicates if this is a gap CompoundNucleotide</param>
        /// <param name="isAmbiguous">Indicates if this is an ambigous CompoundNucleotide</param>
        /// <param name="nucleotides">List of nucleoties.</param>
        /// <param name="weights">List of weights.</param>
        public CompoundNucleotide(char symbol, string name, bool isGap, bool isAmbiguous, List<ISequenceItem> nucleotides, List<double> weights)
            : this(symbol, name, isGap, isAmbiguous)
        {
            Add(nucleotides, weights);
        }
        #endregion Constructors

        #region ICompoundSequenceItem Members
        /// <summary>
        /// Returns the read only collection of sequece items present in this CompoundNucleotide.
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

            if (!(item is Nucleotide))
            {
                throw new ArgumentException(Resource.ParameterItemMustBeNucleotide);
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
        /// Returns a new copy of the CompoundNucleotide object.
        /// </summary>
        ICompoundSequenceItem ICompoundSequenceItem.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Returns a new copy of the CompoundNucleotide object.
        /// </summary>
        ISequenceItem ISequenceItem.Clone()
        {
            return Clone();
        }
        #endregion ICompoundSequenceItem Members

        #region Public Methods
        /// <summary>
        /// Returns a new copy of the CompoundNucleotide object.
        /// </summary>
        new public CompoundNucleotide Clone()
        {
            CompoundNucleotide compound = new CompoundNucleotide(this.Value, this.Symbol, this.Name, this.IsGap, this.IsAmbiguous);

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
        // private method used in constructor.
        private void Add(List<ISequenceItem> nucleotides, List<double> weights)
        {
            if (nucleotides == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameNucleotides);
            }

            if (weights == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameWeights);
            }

            if (nucleotides.Contains(null))
            {
                throw new ArgumentException(Resource.ParameterContainsNullValue, Resource.ParameterNameNucleotides);
            }

            if (nucleotides.Count != weights.Count)
            {
                throw new ArgumentException(Resource.NucleotidesAndWeightsShouldMatch);
            }

            for (int i = 0; i < nucleotides.Count; i++)
            {
                Add(nucleotides[i], weights[i]);
            }
        }
        #endregion Private Methods

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected CompoundNucleotide(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _seqItemToWeightMap = (Dictionary<ISequenceItem, double>)info.GetValue("CompoundNucleotide:Map", typeof(Dictionary<ISequenceItem, double>));
        }

        ///<summary>
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

            info.AddValue("CompoundNucleotide:Map", _seqItemToWeightMap);
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
