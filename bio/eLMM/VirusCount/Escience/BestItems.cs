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

namespace Msr.Adapt.Tabulate
{
    /// <summary>
    /// 
    /// </summary>
    ///!!!could use a heap data structure to make this more efficient
    public class BestItems<TItem>
    {
        List<TItem> _rgObjects;
        List<double> _rgScores;

        public static BestItems<TItem> GetInstance(int maxItemCount)
        {
            return new BestItems<TItem>(maxItemCount);
        }


        private BestItems(int cItems)
        {
            _rgObjects = new List<TItem>(cItems);
            _rgScores = new List<double>(cItems);
        }

        public double WorstBest
        {
            get
            {
                if (_rgScores.Capacity == 0)
                {
                    return double.PositiveInfinity;
                }
                else if (_rgScores.Count < _rgScores.Capacity)
                {
                    return double.NegativeInfinity;
                }
                else
                {
                    return -_rgScores[_rgScores.Count - 1];
                }
            }
        }
        public int Capacity
        {
            get
            {
                return _rgObjects.Capacity;
            }
        }

        public void Add(TItem item, double score)
        {
            bool bAdd = false;

            if (_rgScores.Count < _rgScores.Capacity)
            {
                bAdd = true;
            }
            else if (_rgScores.Capacity == 0)
            {
                bAdd = false;
            }
            else if (-score < _rgScores[_rgScores.Count - 1])
            {
                Debug.Assert(_rgScores.Count == _rgScores.Capacity); // real assert
                Debug.Assert(_rgObjects.Count == _rgObjects.Capacity); // real assert
                _rgScores.RemoveAt(_rgScores.Count - 1);
                _rgObjects.RemoveAt(_rgObjects.Count - 1);
                bAdd = true;
            }
            // else do nothing

            if (bAdd)
            {

                int iPos = _rgScores.BinarySearch(-score);
                if (iPos < 0)
                {
                    iPos = ~iPos;
                }

                _rgScores.Insert(iPos, -score);
                _rgObjects.Insert(iPos, item);
            }

            Debug.Assert(_rgScores.Count == _rgObjects.Count); // real assert

        }
        public List<double> Scores
        {
            get
            {
                return _rgScores;
            }
        }
        public List<TItem> Items
        {
            get
            {
                return _rgObjects;
            }
        }

    }


}
