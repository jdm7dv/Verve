// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Encoding
{
    /// <summary>
    /// An encoding represents the mapping of an alphabet of sequence
    /// characters to an internal memory representation in bytes. Thus a
    /// small encoding implementation for DNA might show that:
    /// 
    /// G goes to 0
    /// A goes to 1
    /// T goes to 2
    /// C goes to 3
    /// 
    /// The implementations may be based on a community standard, such as
    /// those accepted by NCBI or IUPAC. They can also be individually
    /// tailored to an encoding familiar to the user.
    /// </summary>
    public interface IEncoding : ICollection<ISequenceItem>
    {
        /// <summary>
        /// The name of the entire encoding. Examples from the standards
        /// may include 'NCBI4na' or 'IUPACaa'.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates if the encoding has one or more characters that represent
        /// a gap.
        /// </summary>
        bool HasGaps { get; }

        /// <summary>
        /// Indicates if the encoding has one or more characters that represent
        /// an ambigous item (i.e. and item for which it is not precisely known
        /// what it represents)
        /// </summary>
        bool HasAmbiguity { get; }

        /// <summary>
        /// Indicates if the encoding has one or more characters that represent
        /// terminal items.
        /// </summary>
        bool HasTerminations { get; }

        /// <summary>
        /// Returns one of the items in the encoding based on the byte value
        /// of that item.
        /// </summary>
        /// <param name="value">The byte value to look up.</param>
        ISequenceItem LookupByValue(byte value);

        /// <summary>
        /// Returns one of the items in the encoding based on the character
        /// symbol of that item.
        /// </summary>
        /// <param name="symbol">The character symbol to look up, such as 'G' for Guanine.</param>
        ISequenceItem LookupBySymbol(char symbol);

        /// <summary>
        /// Returns one of the items in the encoding based on the symbol of
        /// that item. If the string is only composed of one character then the
        /// result should be the same as calling the char overload of this method.
        /// For instance in an amino acid encoding "Ala" or "A" could return the
        /// item representing Alanine.
        /// </summary>
        /// <param name="symbol">The byte value to look up.</param>
        ISequenceItem LookupBySymbol(string symbol);

        /// <summary>
        /// Gets the byte value of the complemented symbol of the symbol which has a given byte value.
        /// Ex: A = 65, T = 75. Complement of A is T => complement of 65 is 75
        /// </summary>
        /// <param name="value">Value of which complement has to be found</param>
        /// <returns>Complemented byte value</returns>
        byte GetComplement(byte value);

        /// <summary>
        /// Encodes the source sequence onto a byte array. The array will be the
        /// size of the source when returned.
        /// </summary>
        /// <param name="source">The data to be encoded (eg. "TAGGC")</param>
        /// <returns>The array into which the encoded values will be placed</returns>
        byte[] Encode(string source);
    }
}
