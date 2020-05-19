//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;

//namespace MBT.Escience
//{
//    //Unordered pair, a pair in which the two items are not ordered
//    public struct UOPair<T> : IEnumerable<T>, IComparable<UOPair<T>> where T : IComparable<T>
//    {

//        public T First { get; private set; }
//        public T Second { get; private set; }

//        //private UOPair()
//        //{
//        //}


//        public static UOPair<T> GetInstance(T e1, T e2) //The two elements can be the same
//        {
//            UOPair<T> uOPair = new UOPair<T>();

//            if (null == e1)
//            {
//                uOPair.First = e1;
//                uOPair.Second = e2;
//            }
//            else if (null == e2)
//            {
//                uOPair.First = e2;
//                uOPair.Second = e1;
//            }
//            else
//            {
//                Helper.CheckCondition(e1 is IComparable<T>, "Runtime error: UoPair's element are not IComparable");
//                if (((IComparable<T>)e1).CompareTo(e2) < 1)
//                {
//                    uOPair.First = e1;
//                    uOPair.Second = e2;
//                }
//                else
//                {
//                    uOPair.First = e2;
//                    uOPair.Second = e1;
//                }
//            }

//            return uOPair;
//        }

///// <summary>
/////  The elements must already be sorted according to ((IComparable&lt T&gt)e1).CompareTo(e2)
///// </summary>
//public static UOPair<T> GetInstanceGuaranteeSorted(T e1, T e2) //The two elements can be the same
//{
//    Debug.Assert(((IComparable<T>)e1).CompareTo(e2) < 1, "GetInstanceGuaranteeSorted requires elements to be sorted");
//    return new UOPair<T> { First = e1, Second = e2 };
//}


//        public IEnumerator<T> GetEnumerator()
//        {
//            yield return First;
//            yield return Second;
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return this.GetEnumerator();
//        }


//        public override bool Equals(object obj)
//        {
//            if (!(obj is UOPair<T>))
//            {
//                return false;
//            }

//            UOPair<T> other = (UOPair<T>)obj;

//            return First.Equals(other.First) && Second.Equals(other.Second);
//        }

//        public bool ElementsAreSame
//        {
//            get
//            {
//                if (null == First)
//                {
//                    return null == (object)Second;
//                }
//                else
//                {
//                    return First.Equals(Second);
//                }
//            }
//        }

//        static int uopairStringHashCode = (int)MachineInvariantRandom.GetSeedUInt("UOPair");

//        /// <summary>
//        /// Depending on the subtypes, the hash code may be different on 32-bit and 64-bit machines
//        /// </summary>
//        public override int GetHashCode()
//        {
//            return First.GetHashCode() ^ Second.GetHashCode() ^ uopairStringHashCode ^ typeof(T).GetHashCode();
//        }

//        public override string ToString()
//        {
//            if (ElementsAreSame)
//            {
//                return string.Format("(UO 2x {0})", First.ToString());
//            }
//            else
//            {
//                return string.Format("(UO {0}, {1})", First.ToString(), Second.ToString());
//            }
//        }


//        // 

//        // {A1,A2} -> {<A1>,<A2>}
//        // {A1,A2},{B1,B2} -> {<A1,B1>,<A2,B2>}, {<A1,B2>,<A2,B1>}
//        // {A1,A2},{B1,B2},{C1,C2} -> 
//        //          foreach pair in C({B1,B2},{C1,C2})
//        //            If duplex
//        //              yeild return pair.First + c1, pair.second + c2
//        //              yeild return pair.First + c2, pair.second + c1
//        //            else
//        //              yeild return pair.First + c2, pair.second + c1

//        // {A1},{B1,B2} -> {<A1,B1>,<A1,B2>}
//        // {A1} -> {<A1>,<A1>} -> {<A1>}
//        // {A1},{B1} -> {<A1,B1>,<A1,B1>} -> {<A1,B1>}

//        //Every item enumerated is unique
//        static public IEnumerable<UOPair<LinkedList1<T>>> PhaseEnumeration(LinkedList1<UOPair<T>> uOPairList)
//        {
//            //Use of a singlely-linked list makes this very efficient.
//            if (null == uOPairList)
//            {
//                yield return UOPair<LinkedList1<T>>.GetInstance(null, null);
//                yield break;
//            }

//            UOPair<T> firstPair = uOPairList.First();
//            foreach (UOPair<LinkedList1<T>> resultsFromRest in PhaseEnumeration(uOPairList.RestOrNull))
//            {
//                if (resultsFromRest.ElementsAreSame)
//                {
//                    if (firstPair.ElementsAreSame)
//                    {
//                        LinkedList1<T> a0b0 = LinkedList1<T>.GetInstance(firstPair.First, resultsFromRest.First);
//                        yield return UOPair<LinkedList1<T>>.GetInstance(a0b0, a0b0);
//                    }
//                    else
//                    {
//                        LinkedList1<T> a1b0 = LinkedList1<T>.GetInstance(firstPair.First, resultsFromRest.First);
//                        LinkedList1<T> a2b0 = LinkedList1<T>.GetInstance(firstPair.Second, resultsFromRest.First);
//                        yield return UOPair<LinkedList1<T>>.GetInstance(a1b0, a2b0);
//                    }
//                }
//                else
//                {
//                    if (firstPair.ElementsAreSame)
//                    {
//                        LinkedList1<T> a0b1 = LinkedList1<T>.GetInstance(firstPair.First, resultsFromRest.First);
//                        LinkedList1<T> a0b2 = LinkedList1<T>.GetInstance(firstPair.First, resultsFromRest.Second);
//                        yield return UOPair<LinkedList1<T>>.GetInstance(a0b1, a0b2);
//                    }
//                    else
//                    {
//                        LinkedList1<T> a1b1 = LinkedList1<T>.GetInstance(firstPair.First, resultsFromRest.First);
//                        LinkedList1<T> a2b2 = LinkedList1<T>.GetInstance(firstPair.Second, resultsFromRest.Second);
//                        yield return UOPair<LinkedList1<T>>.GetInstance(a1b1, a2b2);

//                        LinkedList1<T> a1b2 = LinkedList1<T>.GetInstance(firstPair.First, resultsFromRest.Second);
//                        LinkedList1<T> a2b1 = LinkedList1<T>.GetInstance(firstPair.Second, resultsFromRest.First);
//                        yield return UOPair<LinkedList1<T>>.GetInstance(a1b2, a2b1);
//                    }
//                }
//            }
//        }

//        /* Excepted output:
//                IN: (UO C1, C2)
//                (UO <C2>, <C1>)
//                IN: (UO B1, B2),(UO C1, C2)
//                (UO <B2,C1>, <B1,C2>)
//                (UO <B1,C1>, <B2,C2>)
//                IN: (UO A1, A2),(UO B1, B2),(UO C1, C2)
//                (UO <A1,B2,C1>, <A2,B1,C2>)
//                (UO <A2,B2,C1>, <A1,B1,C2>)
//                (UO <A2,B2,C2>, <A1,B1,C1>)
//                (UO <A2,B1,C1>, <A1,B2,C2>)
//                IN: (UO A1, A2),(UO 2x B1),(UO C1, C2)
//                (UO <A1,B1,C1>, <A2,B1,C2>)
//                (UO <A2,B1,C1>, <A1,B1,C2>)
//                IN: (UO 2x A1),(UO 2x B1),(UO 2x C1)
//                (UO 2x <A1,B1,C1>)
//        */
//        static public void TestPhaseEnumeration()
//        {
//            Helper.CheckCondition(UOPair<string>.GetInstance("A1", "A2").Equals(UOPair<string>.GetInstance("A2", "A1")), "real assert");
//            Helper.CheckCondition(UOPair<string>.GetInstance("A1", "A1").ElementsAreSame, "real assert");
//            Helper.CheckCondition(!UOPair<string>.GetInstance("A1", "A2").ElementsAreSame, "real assert");
//            Helper.CheckCondition(UOPair<string>.GetInstance("A1", "A2").ToString() == "(UO A1, A2)" || UOPair<string>.GetInstance("A1", "A2").ToString() == "(UO A2, A1)", "real assert");
//            Helper.CheckCondition(UOPair<string>.GetInstance("A1", "A1").ToString() == "(UO 2x A1)", "real assert");

//            LinkedList1<UOPair<string>> small = LinkedList1<UOPair<string>>.GetInstance(UOPair<string>.GetInstance("C1", "C2"));
//            Debug.WriteLine("IN: " + small.StringJoin(","));
//            foreach (var phase in UOPair<String>.PhaseEnumeration(small))
//            {
//                PhasePrinter(phase);
//            }

//            LinkedList1<UOPair<string>> mid = LinkedList1<UOPair<string>>.GetInstance(UOPair<string>.GetInstance("B1", "B2"), UOPair<string>.GetInstance("C1", "C2"));
//            Debug.WriteLine("IN: " + mid.StringJoin(","));
//            foreach (var phase in UOPair<String>.PhaseEnumeration(mid))
//            {
//                PhasePrinter(phase);
//            }


//            LinkedList1<UOPair<string>> big = LinkedList1<UOPair<string>>.GetInstance(UOPair<string>.GetInstance("A1", "A2"), UOPair<string>.GetInstance("B1", "B2"), UOPair<string>.GetInstance("C1", "C2"));
//            Debug.WriteLine("IN: " + big.StringJoin(","));
//            foreach (var phase in UOPair<String>.PhaseEnumeration(big))
//            {
//                PhasePrinter(phase);
//            }


//            LinkedList1<UOPair<string>> dup1 = LinkedList1<UOPair<string>>.GetInstance(UOPair<string>.GetInstance("A1", "A2"), UOPair<string>.GetInstance("B1", "B1"), UOPair<string>.GetInstance("C1", "C2"));
//            Debug.WriteLine("IN: " + dup1.StringJoin(","));
//            foreach (var phase in UOPair<String>.PhaseEnumeration(dup1))
//            {
//                PhasePrinter(phase);
//            }

//            LinkedList1<UOPair<string>> dup2 = LinkedList1<UOPair<string>>.GetInstance(UOPair<string>.GetInstance("A1", "A1"), UOPair<string>.GetInstance("B1", "B1"), UOPair<string>.GetInstance("C1", "C1"));
//            Debug.WriteLine("IN: " + dup2.StringJoin(","));
//            foreach (var phase in UOPair<String>.PhaseEnumeration(dup2))
//            {
//                PhasePrinter(phase);
//            }
//        }

//        private static void PhasePrinter(UOPair<LinkedList1<string>> phase)
//        {
//            if (phase.ElementsAreSame)
//            {
//                Debug.WriteLine(string.Format("(UO 2x <{0}>)", phase.First.StringJoin(",")));
//            }
//            else
//            {
//                Debug.WriteLine(string.Format("(UO <{0}>, <{1}>)", phase.First.StringJoin(","), phase.Second.StringJoin(",")));
//            }
//        }

//        public int CompareTo(UOPair<T> other)
//        {
//            //If we are the same object in memory, we are equal
//            if ((object)this == (object)other)
//            {
//                return 0;
//            }

//            //If our hash codes are different, sort on it
//            int hashCodeComp = GetHashCode().CompareTo(other.GetHashCode());
//            if (0 != hashCodeComp)
//            {
//                return hashCodeComp;
//            }


//            int compFirst = ((IComparable)First).CompareTo((IComparable)other.First);
//            if (compFirst != 0)
//            {
//                return compFirst;
//            }

//            return ((IComparable)Second).CompareTo((IComparable)other.Second);
//        }


//    }
//}
