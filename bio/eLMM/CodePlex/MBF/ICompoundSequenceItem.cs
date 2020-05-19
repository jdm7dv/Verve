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

namespace Bio
{
    /// <summary>
    /// ICompoundSequenceItem used to represent posibility of various sequence item in a base entry.
    /// Classes which implements this interface can store more than one sequence items with their 
    /// weights in one base entry.
    /// 
    /// For examples of implementations of this interface see the CompoundNucleotide 
    /// and CompoundAminoAcid classes.
    /// </summary>
    public interface ICompoundSequenceItem : ISequenceItem
    {
        /// <summary>
        /// Returns the read only collection of sequece items present in this CompoundNucleotide.
        /// </summary>
        IList<ISequenceItem> SequenceItems { get; }

        /// <summary>
        /// Gets the weight for the specified sequence item.
        /// </summary>
        /// <param name="item">Sequence Item.</param>
        /// <returns>If found returns the weight of the specified item, otherwise returns double.NaN.</returns>
        double GetWeight(ISequenceItem item);

        /// <summary>
        /// Sets the weight for the specified item.
        /// </summary>
        /// <param name="item">Sequence Item for which the weight has to be set.</param>
        /// <param name="weight">Weight of the item.</param>
        void SetWeight(ISequenceItem item, double weight);

        /// <summary>
        /// Adds specified Sequenceitem and weight.
        /// </summary>
        /// <param name="item">Sequence Item.</param>
        /// <param name="weight">Weight of specified item.</param>
        void Add(ISequenceItem item, double weight);

        /// <summary>
        /// Removes the specified sequence item.
        /// </summary>
        /// <param name="item">Sequence item.</param>
        /// <returns>Returns true if the specified item removed, otherwise false.</returns>
        bool Remove(ISequenceItem item);

        /// <summary>
        /// Creates a new ICompoundSequenceItem that is a copy of the current ICompoundSequenceItem.
        /// </summary>
        /// <returns>A new ICompoundSequenceItem that is a copy of this ICompoundSequenceItem.</returns>     
        new ICompoundSequenceItem Clone();
    }
}
