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
namespace SequenceAssembler
{
    #region -- Using Directives --

    using System;
    using System.Windows;
    using System.Windows.Input;

    #endregion -- Using Directives --

    /// <summary>
    /// Specifies which message box button that a user clicks. 
    /// </summary>
    public enum MessageDialogResult
    {
        /// <summary>
        /// The result value of the message box is Yes.
        /// </summary>
        Yes,

        /// <summary>
        /// The result value of the message box is No.
        /// </summary>
        No,

        /// <summary>
        /// The result value of the message box is OK.
        /// </summary>
        OK
    }  
}
