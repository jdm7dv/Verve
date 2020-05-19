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

using System.Collections.Generic;
using System.Diagnostics;

namespace MBT.Escience
{

    public class EverySubsetBySizeWithNumberLimit : EverySubsetBySize
    {
        internal int SubsetSizeLimit;

        override public IEnumerable<List<int>> Collection()
        {
            for (int size = 0; size <= SubsetSizeLimit; ++size)
            {
                foreach (List<int> indexCollection in CollectionInternal(size))
                {
                    yield return indexCollection;
                }
            }
        }
    }

    internal class EverySubsetBySizeWithBySetSize : EverySubsetBySize
    {

        internal int StartingSetSize;
        internal int NumberOfSetSizes;

        override public IEnumerable<List<int>> Collection()
        {
            for (int size = StartingSetSize; size < StartingSetSize + NumberOfSetSizes; ++size)
            {
                foreach (List<int> indexCollection in CollectionInternal(size))
                {
                    yield return indexCollection;
                }
            }
        }
    }

    abstract public class EverySubsetBySize
    {
        internal EverySubsetBySize()
        {
        }

        private int NumberOfElements;

        static public EverySubsetBySize GetInstance(int numberOfElements, int subsetSizeLimit)
        {
            EverySubsetBySizeWithNumberLimit aEverySubsetBySizeWithNumberLimit = new EverySubsetBySizeWithNumberLimit();
            aEverySubsetBySizeWithNumberLimit.NumberOfElements = numberOfElements;
            aEverySubsetBySizeWithNumberLimit.SubsetSizeLimit = subsetSizeLimit;
            return aEverySubsetBySizeWithNumberLimit;
        }

        public static EverySubsetBySize GetInstance(int numberOfElements, int startingSetSize, int numberOfSetSizes)
        {
            EverySubsetBySizeWithBySetSize aEverySubsetBySizeWithBySetSize = new EverySubsetBySizeWithBySetSize();
            aEverySubsetBySizeWithBySetSize.NumberOfElements = numberOfElements;
            aEverySubsetBySizeWithBySetSize.StartingSetSize = startingSetSize;
            aEverySubsetBySizeWithBySetSize.NumberOfSetSizes = numberOfSetSizes;
            return aEverySubsetBySizeWithBySetSize;
        }



        public static void Test(int numberOfElements, int subsetSizeLimit)
        {
            EverySubsetBySize aEverySubsetBySize = EverySubsetBySize.GetInstance(numberOfElements, subsetSizeLimit);
            foreach (List<int> indexCollection in aEverySubsetBySize.Collection())
            {
                Debug.Write(">");
                foreach (int index in indexCollection)
                {
                    Debug.Write('\t' + index.ToString());
                }
                Debug.WriteLine("");
            }
        }


        abstract public IEnumerable<List<int>> Collection();

        internal IEnumerable<List<int>> CollectionInternal(int size)
        {
            if (size == 0)
            {
                yield return new List<int>();
            }
            else
            {
                foreach (List<int> shortIndexCollection in CollectionInternal(size - 1))
                {
                    int highestPrevious = (size == 1) ? -1 : shortIndexCollection[size - 2];
                    shortIndexCollection.Add(-1);
                    for (int newItem = highestPrevious + 1; newItem < NumberOfElements; ++newItem)
                    {
                        shortIndexCollection[size - 1] = newItem;
                        Debug.Assert(shortIndexCollection.Count == size);
                        yield return shortIndexCollection;
                    }
                    shortIndexCollection.RemoveAt(size - 1);
                }
            }
        }

    }

}






