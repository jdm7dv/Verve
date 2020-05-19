// -------------------------------------------------------------------------------------
// <copyright file="PopupEventArgs.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Popup event args describes the custom event Args which contains 
// the status of the popup, this Status would be used to describe 
// as whether it was a successful close of the pop up or failed 
// closure of the pop up.
// </summary>
// -------------------------------------------------------------------------------------
namespace SequenceAssembler
{
    #region -- Using Directives --
    using System;
    using System.Collections.ObjectModel;
    using MBF;
    #endregion
    /// <summary>
    /// This defines the custom Event Arguments for describing closure of 
    /// pop up occurred with success or failure status.
    /// </summary>
    public class PopupEventArgs : EventArgs
    {
        #region -- Private Members --

        /// <summary>
        /// Describes the the status of the Popup
        /// </summary>
        private bool status;

        #endregion

        #region -- Constructor --

        /// <summary>
        /// Initiliazes the PopupEventArgs with the 
        /// State of the Pop up.        
        /// </summary>
        /// <param name="state">Popup State</param>      
        public PopupEventArgs(bool state)
        {
            this.status = state;
        }
        #endregion

        #region -- Public Properties --

        /// <summary>
        /// Gets a value indicating whether the status of the Popup is true or false.
        /// </summary>
        public bool Status
        {
            get
            {
                return this.status;
            }
        }
        #endregion
    }
}