// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using MBF.Registration;

namespace MBF
{
    /// <summary>
    /// The currently supported and built-in alphabets for sequence items.
    /// </summary>
    public static class Alphabets
    {
        /// <summary>
        /// The DNA alphabet
        /// </summary>
        public static readonly DnaAlphabet DNA = DnaAlphabet.Instance;

        /// <summary>
        /// The RNA alphabet
        /// </summary>
        public static readonly RnaAlphabet RNA = RnaAlphabet.Instance;

        /// <summary>
        /// The protein alphabet consisting of amino acids
        /// </summary>
        public static readonly ProteinAlphabet Protein = ProteinAlphabet.Instance;

        /// <summary>
        /// List of all supported Alphabets.
        /// </summary>
        private static List<IAlphabet> all = new List<IAlphabet>(){
            Alphabets.DNA,
            Alphabets.RNA,
            Alphabets.Protein};

        /// <summary>
        ///  Gets the list of all Alphabets which is supported by the framework.
        /// </summary>
        public static IList<IAlphabet> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static Alphabets()
        {
            //get the registered alphabets
            IList<IAlphabet> registeredAlphabets = RegisteredAddIn.GetAlphabets(true);

            if (null != registeredAlphabets && registeredAlphabets.Count > 0)
            {
                foreach (IAlphabet alphabet in registeredAlphabets)
                {
                    if (alphabet != null && all.FirstOrDefault(IA => string.Compare(IA.Name, 
                        alphabet.Name, StringComparison.InvariantCultureIgnoreCase) == 0) == null)
                    {
                        all.Add(alphabet);
                    }
                }
                registeredAlphabets.Clear();
            }
        }

        /// <summary>
        /// Returns the highest character value found in the alphabet.  Because alphabets are a fixed size this is useful
        /// in order to create very fast lookups for high frequency operations.
        /// </summary>
        /// <param name="alphabet"></param>
        /// <returns></returns>
        public static int GetHighestChar(IAlphabet alphabet)
        {
           int maxValue = 0;
           foreach (var item in alphabet)
           {
              if (item.Symbol > maxValue)
              {
                 maxValue = item.Symbol;
              }
           }

           return maxValue;
        }
    }
}
