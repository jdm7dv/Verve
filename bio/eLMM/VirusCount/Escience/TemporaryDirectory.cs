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
using System.IO;

namespace MBT.Escience
{
    public class TemporaryDirectory : IDisposable
    {
        private TemporaryDirectory()
        {
        }

        public string Name { private set; get; }
        public bool CleanUp { private set; get; }
        public static TemporaryDirectory GetInstance()
        {
            return GetInstance(Path.GetTempPath(), true);
        }

        public static TemporaryDirectory GetInstance(string parentOfTempDirectory, bool cleanUp)
        {
            TemporaryDirectory temporaryDirectory = new TemporaryDirectory();
            temporaryDirectory.Name = Path.Combine(parentOfTempDirectory, Path.GetRandomFileName());
            temporaryDirectory.CleanUp = cleanUp;
            Directory.CreateDirectory(temporaryDirectory.Name);
            return temporaryDirectory;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (CleanUp)
            {
                Directory.Delete(Name, true);
            }
        }

        #endregion

        override public string ToString()
        {
            return Name;
        }
    }
}
