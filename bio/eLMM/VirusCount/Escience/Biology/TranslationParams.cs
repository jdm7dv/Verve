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
using System.Linq;
using System.Text;

namespace MBT.Escience.Biology
{
    /// <summary>
    /// Specifies how a sequence should be translated
    /// </summary>
    public abstract class TranslationParams
    {
        /// <summary>
        /// If the translation will end up in a matrix, specifies how mixtures are dealt with.
        /// </summary>
        public MixtureSemantics MixtureSemantics = MixtureSemantics.Uncertainty;
    }

    /// <summary>
    /// Do nothing class.
    /// </summary>
    public class NoTranslation : TranslationParams { }

    public class Dna2Aa : TranslationParams
    {
        /// <summary>
        /// Which reading frame should be translated? 1-3 is primary. 4-6 is reverse complement.
        /// </summary>
        public int RF = 1;

        /// <summary>
        /// If true, then all indels will be marked as missing. If false, then dash will be a first-class character.
        /// </summary>
        public bool DashAsMissing = true;
    }
}
