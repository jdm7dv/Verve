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
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Parse;
using System.IO;

namespace MBT.Escience.Biology
{
    /// <summary>
    /// A class that allows direct parsing of sequences from a file.
    /// </summary>
    public class SequenceFile : List<NamedSequence>, IParsable
    {
        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo File { get; set; }

        public void FinalizeParse()
        {
            this.AddRange(SequenceFormatter.ParseWithMostLikelyFormatter(File));
        }
    }
}
