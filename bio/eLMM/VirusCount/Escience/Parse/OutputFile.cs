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
using System.IO;
using Bio.Util;

namespace MBT.Escience.Parse
{
    /// <summary>
    /// Convertable to and from type FileInfo. Creates the output directory, if needed.
    /// </summary>
    //[Parsable]
    public class OutputFile : ParsableFile, IParsable
    {
        public void FinalizeParse()
        {
            if (FullName != null && FullName != "-")
                _fileInfo.CreateDirectoryForFileIfNeeded();
        }

        public static implicit operator FileInfo(OutputFile outputFile)
        {
            return outputFile._fileInfo;
        }

        public static implicit operator OutputFile(FileInfo fileInfo)
        {
            return new OutputFile { _fileInfo = fileInfo };
        }

    }
}
