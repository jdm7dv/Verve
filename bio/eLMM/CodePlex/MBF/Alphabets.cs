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
using System.Collections.Generic;
using System.Linq;
using Bio.Registration;

namespace Bio
{
    /// <summary>
    /// The currently supporteed and built-in alphabets for sequence items.
    /// </summary>
    public static class Alphabets
    {
        /// <summary>
        /// The DNA alphabet
        /// </summary>
        public static readonly DnaAlphabet DNA = DnaAlphabet.Instance;

        /// <summary>
        /// The DNA alphabet
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
    }
}
