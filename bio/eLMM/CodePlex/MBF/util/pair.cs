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
namespace Bio.Util
{
    /// <summary>
    /// A pair object that hashes (and equals) based on the values of its elements.
    /// </summary>
    /// <typeparam name="T1">The type of its first element.</typeparam>
    /// <typeparam name="T2">The type of its second element.</typeparam>
    [Obsolete("Tuple<T1,T2>")]
    public class Pair<T1, T2> : Tuple<T1, T2>
    {
        /// <summary>
        /// The first element.
        /// </summary>
        public T1 First
        {
            get
            {
                return Item1;
            }
        }

        /// <summary>
        /// The second element.
        /// </summary>
        public T2 Second
        {
            get
            {
                return Item2;
            }
        }

        /// <summary>
        /// Creates a pair.
        /// </summary>
        /// <param name="first">The first element of the pair</param>
        /// <param name="second">The second element of the pair</param>
        public Pair(T1 first, T2 second)
            :base(first,second)
        {
        }

        ///// <summary>
        ///// Creates an instance of the class with default values.
        ///// </summary>
        //public Pair()
        //{
        //    First = default(T1);
        //    Second = default(T2);
        //}


        //#pragma warning disable 1591
        //public override bool Equals(object obj)
        //#pragma warning restore 1591
        //{
        //    Pair<T1, T2> other = obj as Pair<T1, T2>;
        //    if (null == other)
        //        return false;

        //    return First.Equals(other.First) && Second.Equals(other.Second);
        //}

        //static int pairStringHashCode = 937292331;

        ///// <summary>
        ///// Depending on the subtypes, the hash code may be different on 32-bit and 64-bit machines
        ///// </summary>
        //public override int GetHashCode()
        //{
        //    return pairStringHashCode
        //        ^ First.GetHashCode() ^ typeof(T1).GetHashCode()
        //        ^ WrapAroundLeftShift(Second.GetHashCode(), 1) ^ typeof(T2).GetHashCode(); //Do the shift so that <"A","B"> hashs different than <"B","A">
        //}

        //private static int WrapAroundLeftShift(int someInt, int count)
        //{
        //    //Tip: Use "?Convert.ToString(WrapAroundLeftShift(someInt,count),2)" to see this work
        //    int result = (someInt << count) | ((~(-1 << count)) & (someInt >> (8 * sizeof(int) - count)));
        //    return result;
        //}

        //#pragma warning disable 1591
        //public override string ToString()
        //#pragma warning restore 1591
        //{
        //    return string.Format("({0}, {1})", First, Second);
        //}

    }
}
