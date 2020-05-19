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
using System.Collections.Generic;
using Bio.Util;

namespace MBT.Escience
{
    public class BestSoFarWithTies<TScore, TItem>
    {
        //public delegate bool IsBetterDelegate(TScore scoreChamp, TScore scoreChallenger);
        public TScore ChampsScore;
        public List<TItem> ChampList;
        public int ChangeCount = 0;
        internal Comparison<TScore> IsBetter;

        private BestSoFarWithTies()
        {
        }




        public static BestSoFarWithTies<TScore, TItem> GetInstance(Comparison<TScore> isBetter)
        {
            BestSoFarWithTies<TScore, TItem> bestSoFarWithTies = new BestSoFarWithTies<TScore, TItem>();
            bestSoFarWithTies.IsBetter = isBetter;
            return bestSoFarWithTies;
        }

        public virtual bool Compare(TScore scoreChallenger, TItem itemChallenger)
        {
            int isBetterComparison = (ChangeCount == 0) ? 1 : IsBetter(scoreChallenger, ChampsScore);
            switch (isBetterComparison)
            {
                case -1:
                    return false;
                case 1:
                    ChampsScore = scoreChallenger;
                    ChampList = new List<TItem>();
                    ChampList.Add(itemChallenger);
                    ++ChangeCount;
                    return true;
                case 0:
                    ChampList.Add(itemChallenger);
                    ++ChangeCount;
                    return true;
                default:
                    Helper.CheckCondition(false, "Comparison should return -1, 0, or 1");
                    return false;
            }
        }
    }
}
