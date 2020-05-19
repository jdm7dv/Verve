// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF
{

    /// <summary>
    /// Represents a single nucleotide polymporphism (Snp) at a particular
    /// position for a certain chromosome, with the two possible allele
    /// values for that position.
    /// </summary>
    public class SnpItem : IEquatable<SnpItem>
    {

        #region Properties

        /// <summary>
        /// Contains the chromosome number for the SNP
        /// </summary>
        public byte Chromosome
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the position for the SNP within the sequence 
        /// (may be position in full sequence or offset within chromosome)
        /// </summary>
        public int Position
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the first allele character for the SNP.
        /// </summary>
        public char AlleleOne
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the second allele character for the SNP.
        /// </summary>
        public char AlleleTwo
        {
            get;
            set;
        }

        #endregion Properties


        #region Implementation of IEquatable interface

        ///<summary>
        /// Indicates whether the SnpItem is equal to another SnpItem. This compares
        /// the exact values of all four properties. 
        /// AlleleOne is compared with other.AlleleOne, and 
        /// AlleleTwo is compared with other.AlleleTwo.
        ///</summary>
        ///
        ///<returns>
        ///true if the current SnpItem is equal to the other SnpItem; otherwise, false.
        ///</returns>
        ///
        ///<param name="other">A SnpItem to compare with this SnpItem.</param>
        public bool Equals (SnpItem other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Chromosome == other.Chromosome && Position == other.Position &&
                   AlleleOne == other.AlleleOne && AlleleTwo == other.AlleleTwo;
        }

        #endregion Implementation of IEquatable interface


        #region Override of Object.Equals
        ///<summary>
        /// Indicates whether the SnpItem is equal to another SnpItem. This compares
        /// the exact values of all four properties. 
        /// AlleleOne is compared with other.AlleleOne, and 
        /// AlleleTwo is compared with other.AlleleTwo.
        ///</summary>
        ///
        ///<returns>
        ///true if the current SnpItem is equal to the other SnpItem; otherwise, false.
        ///</returns>
        ///
        ///<param name="other">
        ///A SnpItem to compare with this SnpItem. If this parameter
        ///is not of type SnpItem, then false is returned.
        ///</param>
        public override bool Equals (object other)
        {
            return (other.GetType() != typeof(SnpItem)) ? false : Equals((SnpItem)other);
        }

        /// <summary>
        /// Generates a unique hashcode based on the data members in the item.
        /// </summary>
        public override int GetHashCode ()
        {
            unchecked
            {
                return Chromosome << 16 + Position + AlleleOne.GetHashCode() + AlleleTwo.GetHashCode();
            }
        }

        #endregion Override of Object.Equals


    }
}