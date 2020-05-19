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



using System.Runtime.Serialization;
using System;
namespace Bio
{
    /// <summary>
    /// The data point values stored within an ISequence implementation.
    /// This data model allows the association of a particular value with
    /// a character symbol, and a byte value used for encoding or serializing
    /// the item. For examples of implementations of this interface see the
    /// Nucleotide and AminoAcid classes.
    /// </summary>
    public interface ISequenceItem : ISerializable, ICloneable
    {
        /// <summary>
        /// An encoding value, unique for the particular item within its
        /// alphabet.
        /// </summary>
        byte Value { get; }

        /// <summary>
        /// A character symbol representing the item. For instance in DNA,
        /// symbols would include G, A, T, and C. Also possible are symbols
        /// representing gaps, termination characters, or ambiguities.
        /// </summary>
        char Symbol { get; }

        /// <summary>
        /// A human readable and display appropriate name for the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns true if this ISequenceItem is a Gap character, otherwise returns false.
        /// </summary>
        bool IsGap { get; }

        /// <summary>
        /// Returns true if this ISequenceItem is Ambiguous, otherwise returns false.
        /// </summary>
        bool IsAmbiguous { get; }

        /// <summary>
        /// Returns true if this ISequenceItem is a Termination character, otherwise returns false.
        /// </summary>
        bool IsTermination { get; }

        /// <summary>
        /// Gets the clone copy of this ISequence Item.
        /// </summary>
        new ISequenceItem Clone();
    }
}
