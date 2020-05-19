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
    /// Convertable to and from type FileInfo. Verifies that the file exists.
    /// </summary>
    //[Parsable]
    public class InputFile : ParsableFile, IParsable
    {
        public void FinalizeParse()
        {
            Helper.CheckCondition<ParseException>(_fileInfo.Name == "-" || _fileInfo.Exists, "File {0} does not exist.", FullName);
        }

        public static implicit operator FileInfo(InputFile inputFile)
        {
            return inputFile._fileInfo;
        }

        public static implicit operator InputFile(FileInfo fileInfo)
        {
            return new InputFile { _fileInfo = fileInfo };
        }

    }
}
