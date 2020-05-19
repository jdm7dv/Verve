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
    /// Parses filepatterns of the form pattern1+pattern2+...+patternN
    /// </summary>
    [ParseAsNonCollection]
    [DoNotParseInherited]
    public class InputFileList : List<InputFile>
    {
        /// <summary>
        /// A patterns of file names. Each pattern can contain wild cards '*'. Multiple patterns can be appended with '+'. 
        /// Each pattern must identify at least one real file.
        /// </summary>
        [Parse(ParseAction.Required)]
        public string FilePattern
        {
            get
            {
                return this.Select(inputFile => inputFile.ToString()).StringJoin("+");
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        foreach (var filename in Bio.Util.FileUtils.GetFiles(value, false))
                        {
                            this.Add(new InputFile() { FullName = filename });
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ParseException(e.Message);
                    }
                }
            }
        }

        public InputFileList() : base() { }
        public InputFileList(IEnumerable<InputFile> inputFiles) : base(inputFiles) { }


        public static implicit operator List<FileInfo>(InputFileList inputFiles)
        {
            return inputFiles.Select(inFile => (FileInfo)inFile).ToList();
        }

        public static implicit operator InputFileList(List<FileInfo> fileInfos)
        {
            return new InputFileList(fileInfos.Select(fi => (InputFile)fi));
        }
    }
}
