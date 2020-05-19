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

using Bio.Registration;
using Bio.Algorithms.Alignment;

namespace Bio.Test.AddIns
{
    /// <summary>
    /// Add-in for the Negative test:
    /// the add-in that cannot be constructed by the Registration API and results in exception thrown.
    /// </summary>
    [RegistrableAttribute(true)] 
    public class AlignerNoDefaultConstructor : NeedlemanWunschAligner
    {
        /// <summary>
        /// Private default constructor
        /// </summary>
        private AlignerNoDefaultConstructor()
        {
        }

        /// <summary>
        /// Public constructor with parameter;
        /// not accessible via default Activator.CreateInstance
        /// </summary>
        /// <param name="gapCost"></param>
        public AlignerNoDefaultConstructor(int gapCost)
        {
            this.GapOpenCost = gapCost;
        }
    }
}
