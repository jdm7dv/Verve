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
    using System.Collections.ObjectModel;
    using Bio;
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