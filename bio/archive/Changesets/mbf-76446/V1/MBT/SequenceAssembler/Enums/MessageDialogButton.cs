// ------------------------------------------------------------------------------
// <copyright file="MessageDialogButton.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//  Specifies the buttons that are displayed on a MessageDialog box.
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
    /// Specifies the buttons that are displayed on a MessageDialog box. 
    /// </summary>
    public enum MessageDialogButton
    {
        /// <summary>
        ///  The MessageDialog displays Yes and No buttons.
        /// </summary>
        YesNo,

        /// <summary>
        /// The MessageDialog displays an OK button.
        /// </summary>
        OK
    }
}
