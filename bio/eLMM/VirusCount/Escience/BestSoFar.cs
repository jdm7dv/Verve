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

namespace MBT.Escience
{

    //Merge with BestItems
    public class BestSoFar<TScore, TItem>
    {
        //public delegate bool IsBetterDelegate(TScore scoreChamp, TScore scoreChallenger);
        public TScore ChampsScore;
        public TItem Champ;
        public int ChangeCount = 0;

        private BestSoFar()
        {
        }

        internal Comparison<TScore> IsBetter;

        public static BestSoFar<TScore, TItem> GetInstance(Comparison<TScore> isBetter)
        {
            BestSoFar<TScore, TItem> bestSoFar = new BestSoFar<TScore, TItem>();
            bestSoFar.IsBetter = isBetter;
            return bestSoFar;
        }

        public virtual bool Compare(TScore scoreChallenger, TItem itemChallenger)
        {
            if (ChangeCount == 0 || IsBetter(scoreChallenger, ChampsScore) > 0)
            {
                ChampsScore = scoreChallenger;
                Champ = itemChallenger;
                ++ChangeCount;
                return true;
            }
            return false;
        }
    }
}
