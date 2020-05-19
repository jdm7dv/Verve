// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bio;

namespace Bio.Tests
{
    /// <summary>
    /// Tests the AmbiguousRnaAlphabet class.
    /// </summary>
    [TestClass]
    public class AmbiguousAmbiguousRnaAlphabetTests
    {
        /// <summary>
        /// Tests the AmbiguousRNAAlphabet class.
        /// </summary>
        [TestMethod]
        public void TestAmbiguousRnaAlphabetCompareSymbols()
        {
            AmbiguousRnaAlphabet ambiguousRnaAlphabet = AmbiguousRnaAlphabet.Instance;
            Assert.AreEqual(false, ambiguousRnaAlphabet.CompareSymbols((byte)'A', (byte)'M'));
            Assert.AreEqual(true, ambiguousRnaAlphabet.CompareSymbols((byte)'A', (byte)'A'));
        }
    }
}
