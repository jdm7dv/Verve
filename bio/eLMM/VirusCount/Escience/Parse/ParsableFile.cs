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
    //[Parsable]
    public class ParsableFile
    {
        /// <summary>
        /// Can set with a short file name, but the 'get' will return the FullName;
        /// </summary>
        [Parse(ParseAction.Required)]
        public string FullName
        {
            set
            {
                _fileInfo = (value == null) ? null : new FileInfo(value);
            }
            get
            {
                return (_fileInfo == null) ? null : _fileInfo.ToString();
            }
        }

        //private string _relativePath = null;
        //public string RelativePath
        //{
        //    get
        //    {
        //        if (_relativePath == null)
        //        {
        //            string workingDir = Environment.CurrentDirectory;
        //            string filePath = _fileInfo.FullName;

        //            int pointOfDivergence = -1;
        //            while (++pointOfDivergence < workingDir.Length && pointOfDivergence < filePath.Length && workingDir[pointOfDivergence] == filePath[pointOfDivergence]) ;

        //            if (pointOfDivergence == workingDir.Length)
        //                _relativePath = filePath.Substring(pointOfDivergence + 1);
        //            else
        //                _relativePath = Enumerable.Repeat("..\\", CountBackSlashes(workingDir.Substring(pointOfDivergence - 1))).StringJoin() + filePath.Substring(pointOfDivergence);
        //        }
        //        return _relativePath;
        //    }

        //}



        [Parse(ParseAction.Ignore)]
        protected FileInfo _fileInfo = null;

        public override string ToString()
        {
            return _fileInfo.ToString();
        }

        public static implicit operator FileInfo(ParsableFile inputFile)
        {
            return inputFile._fileInfo;
        }

        public static implicit operator ParsableFile(FileInfo fileInfo)
        {
            return new ParsableFile { _fileInfo = fileInfo };
        }

        //public override string ToString()
        //{
        //    return RelativePath;
        //}

        private int CountBackSlashes(string path)
        {
            int count = 0;
            foreach (char c in path)
                if (c == '\\')
                    count++;
            return count;
        }
    }
}
