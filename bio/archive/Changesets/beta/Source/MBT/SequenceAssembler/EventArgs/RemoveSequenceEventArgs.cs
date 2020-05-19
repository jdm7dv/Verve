// -------------------------------------------------------------------------------------
// <copyright file="RemoveSequenceEventArgs.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// RemoveSequenceEventArgs describes the custom event Args
// given sequence would be removed from the collection of the Selected Sequences
// and from the UI.
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
    /// This defines the custom Event Arguments for Removing the 
    /// Sequence from the UI and selected sequence collection.
    /// </summary>
    public class RemoveSequenceEventArgs : EventArgs
    {
        #region -- Private Members --

        /// <summary>
        /// Describes the Sequence to be removed
        /// </summary>
        private ISequence sequence;
        #endregion

        #region -- Constructor --

        /// <summary>
        /// Initiliazes the RemoveSequenceEventArgs with the 
        /// sequence to be removed
        /// </summary>
        /// <param name="removedSequence">Sequence to be removed</param>       
        public RemoveSequenceEventArgs(ISequence removedSequence)
        {
            this.sequence = removedSequence;
        }
        #endregion

        #region -- Public Properties --

        /// <summary>
        /// Gets the sequence to be removed
        /// </summary>
        public ISequence Sequence
        {
            get
            {
                return this.sequence;
            }
        }
        #endregion
    }
}