// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF.Web
{
    /// <summary>
    /// Function pointer to a method that has to be invoked after the asynchronous web call completes.
    /// </summary>
    /// <param name="response">Response of asynchronous web method call.</param>
    public delegate void AsyncWebMethodCompleted(AsyncWebMethodResponse response);
}
