// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
namespace Bio.Registration
{
    using System;

    /// <summary>
    /// Self registrable  mechanism's attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class RegistrableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the RegistrableAttribute class.
        /// </summary>
        /// <param name="isRegistrable">Registrable or not.</param>
        public RegistrableAttribute(bool isRegistrable)
        {
            this.IsRegistrable = isRegistrable;
        }

        /// <summary>
        /// Gets a value indicating whether its registrable or not.
        /// </summary>
        public bool IsRegistrable { get; private set; }
    }
}
