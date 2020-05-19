// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.Registration
{
    /// <summary>
    /// Self registerable mechanism's attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RegistrableAttribute : Attribute
    {
        /// <summary>
        /// If its registrable or not
        /// </summary>
        public bool IsRegistrable { get; private set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isRegistrable">Registrable or not</param>
        public RegistrableAttribute(bool isRegistrable)
        {
            this.IsRegistrable = isRegistrable;
        }
    }
}
