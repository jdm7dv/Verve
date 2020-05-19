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
    public class MachineInvariantRandom : Random
    {
        private const uint Salt = unchecked((uint)-823483423);
        private const uint AfterWord = 724234833;

        /// <summary>
        /// Single random object to use if creating one isn't necessary.
        /// </summary>
        public static MachineInvariantRandom GlobalRand = new MachineInvariantRandom(new string[0]);

        private MachineInvariantRandom()
        {
        }

        /// <summary>
        /// Gives same result on 64-bit and 32-bit machines (unlike string's GetHashCode). Ignore's case
        /// </summary>
        public MachineInvariantRandom(params string[] seedStringArray)
            : this(Salt, seedStringArray)
        {
        }

        //<summary>
        //Gives same result on 64-bit and 32-bit machines (unlike string's GetHashCode). Ignore's case
        //</summary>
        public MachineInvariantRandom(uint seedUInt, params string[] seedStringArray)
            : base((int)GetSeedUInt(seedUInt, seedStringArray))
        {
        }


        public static uint GetSeedUInt(params string[] seedStringArray)
        {
            return GetSeedUInt(Salt, seedStringArray);
        }


        public static uint GetSeedUInt(uint seedUInt, params string[] seedStringArray)
        {
            foreach (string seedString in seedStringArray)
            {
                foreach (char c in seedString)
                {
                    //xor the rightrotated seed with the uppercase character
                    seedUInt = (((seedUInt >> 1) | ((seedUInt & 1) << 31)) ^ ((uint)char.ToUpper(c).GetHashCode()));
                }
                //After each word, do a right rotate to separate words
                seedUInt = ((seedUInt >> 1) | ((seedUInt & 1) << 31)) ^ AfterWord;
            }
            return seedUInt;
        }

    }
}
