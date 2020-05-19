//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************


using System;

namespace Bio.Registration
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
