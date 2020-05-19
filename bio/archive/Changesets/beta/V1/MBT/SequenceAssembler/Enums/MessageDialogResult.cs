// ------------------------------------------------------------------------------
// <copyright file="MessageDialogResult.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//  Specifies which message box button that a user clicks. 
// </summary>
// ------------------------------------------------------------------------------
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
