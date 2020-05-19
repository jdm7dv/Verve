// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace Bio
{
    /// <summary>
    /// This class gives the Sequence Equality Comparer.
    /// </summary>
    public class SequenceEqualityComparer : IEqualityComparer<ISequence>
    {
        /// <summary>
        /// Two sequences data are equal or not.
        /// </summary>
        /// <param name="x">First sequence.</param>
        /// <param name="y">Second sequence.</param>
        /// <returns>Returns true if both sequence have equal data.</returns>
        public bool Equals(ISequence x, ISequence y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (x.Count == y.Count)
            {
                string seq1 = new string(x.Select(a => (char)a).ToArray());
                string seq2 = new string(y.Select(a => (char)a).ToArray());
                return string.Compare(seq1, seq2, CultureInfo.InvariantCulture, CompareOptions.Ordinal) == 0 ? true : false;
            }

            return false;
        }

        /// <summary>
        /// Gets hash code.
        /// </summary>
        /// <param name="obj">The sequence object.</param>
        /// <returns>Returns the hash code.</returns>
        public int GetHashCode(ISequence obj)
        {
            return new string(obj.Select(a => (char)a).ToArray()).GetHashCode();
        }
    }
}
