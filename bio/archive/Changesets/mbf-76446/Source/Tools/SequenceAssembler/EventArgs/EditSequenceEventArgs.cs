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
    public class EditSequenceEventArgs : EventArgs
    {
        #region -- Private Members --

        /// <summary>
        /// Describes the updated sequence
        /// </summary>
        private string sequence;

        #endregion

        #region -- Constructor --

        /// <summary>
        /// Initiliazes the EditSequenceEventArgs with the 
        /// editted Sequence.        
        /// </summary>
        /// <param name="editSequence">Edited Sequence string</param>
        /// <param name="seq">Edited Sequence object</param>
        public EditSequenceEventArgs(string editSequence, ISequence seq)
        {
            this.sequence = editSequence;
            this.Sequence = seq;
        }
        #endregion

        #region -- Public Properties --

        /// <summary>
        /// Gets the updated sequence string.
        /// </summary>
        public string SequenceString
        {
            get
            {
                return this.sequence;
            }
        }

        /// <summary>
        /// Gets or sets the sequence
        /// </summary>
        public ISequence Sequence
        {
            get;
            set;
        }
        #endregion
    }
}