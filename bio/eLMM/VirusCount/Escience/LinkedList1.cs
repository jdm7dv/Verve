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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Bio.Util;

namespace MBT.Escience
{
    /// <summary>
    /// A Lisp-like, singlely-linked list.
    /// Its hashcode is set once, at construction time, and is a function of the contents. So, don't change the contents if you want to use hashcode.
    /// Likewise, equality is based on the contents.
    /// </summary>
    public class LinkedList1<T> : IEnumerable<T>, IEnumerable, IComparable<LinkedList1<T>> where T : IComparable<T>
    {
        private LinkedList1()
        {
        }

        public static LinkedList1<T> GetInstanceFromList(IList<T> elementList)
        {
            LinkedList1<T> linkedList1 = null;
            for (int i = elementList.Count - 1; i >= 0; --i)
            {
                linkedList1 = GetInstance(elementList[i], linkedList1);
            }
            return linkedList1;
        }


        public static LinkedList1<T> GetInstance(params T[] elementList)
        {
            return GetInstanceFromList(elementList);
        }

        public static LinkedList1<T> GetInstance(T first, LinkedList1<T> restOrNull)
        {
            LinkedList1<T> linkedList1 = new LinkedList1<T>();
            linkedList1.First = first;
            linkedList1.RestOrNull = restOrNull;

            linkedList1.HashCode = LinkedList1StringHashCode ^ first.GetHashCode();
            if (null != restOrNull)
            {
                linkedList1.HashCode ^= Helper.WrapAroundLeftShift(restOrNull.GetHashCode(), 1);
            }

            return linkedList1;
        }

        public T First { get; private set; }
        public LinkedList1<T> RestOrNull { get; private set; }

        public override int GetHashCode()
        {
            return HashCode;
        }
        private int HashCode;
        static private int LinkedList1StringHashCode = (int)MachineInvariantRandom.GetSeedUInt("LinkedList1<T>");

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            LinkedList1<T> other = obj as LinkedList1<T>;
            if (null == other)
                return false;

            if (!First.Equals(other.First))
            {
                return false;
            }

            if (null == RestOrNull)
            {
                return null == other.RestOrNull;
            }

            return RestOrNull.Equals(other.RestOrNull);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("<LL1 ");
            ToStringWithoutOpening(ref sb);
            return sb.ToString();
        }

        private void ToStringWithoutOpening(ref StringBuilder sb)
        {
            sb.Append(First);
            if (RestOrNull == null)
            {
                sb.Append(">");
            }
            else
            {
                sb.Append(", ");
                RestOrNull.ToStringWithoutOpening(ref sb);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return First;
            if (null != RestOrNull)
            {
                foreach (T t in RestOrNull)
                    yield return t;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public int CompareTo(LinkedList1<T> other)
        {
            //If we are the same object in memory, we are equal
            if ((object)this == (object)other)
            {
                return 0;
            }

            //I'm an object, so if they other guy is null, so sort me after him
            if (null == other)
            {
                return 1;
            }

            //If our hash codes are different, sort on it (this will be fast because hashcode are pre-computed
            int hashCodeComp = GetHashCode().CompareTo(other.GetHashCode());
            if (0 != hashCodeComp)
            {
                return hashCodeComp;
            }


            //We are IComparable<T>. Sort on our first items if they are different.
            int compFirst = ((IComparable<T>)First).CompareTo(other.First);
            if (compFirst != 0)
            {
                return compFirst;
            }

            //If your first items are equal, compare our Rest's
            if (RestOrNull == null)
            {
                if (other.RestOrNull == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }

            return RestOrNull.CompareTo(other.RestOrNull);
        }




    }
}
