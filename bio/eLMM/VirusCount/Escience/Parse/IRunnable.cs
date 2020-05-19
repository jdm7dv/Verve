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
using System.Runtime.Serialization;
#if !SILVERLIGHT
using System.Runtime.Serialization.Formatters.Binary;
#endif
using Bio.Util;
using System.Collections;

namespace MBT.Escience.Parse
{
    public interface IRunnable
    {
        //IResultFormatter Out { get; }
        void Run();
    }




}
