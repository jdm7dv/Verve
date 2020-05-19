// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using MBF.Properties;

namespace MBF.Algorithms.Assembly
{
    /// <summary>
    /// Stores mate pair information 
    /// </summary>
    public class MatePair
    {
        #region Field Variables

        /// <summary>
        /// Stores sequence ID of forward read.
        /// </summary>
        private string _forwardRead;

        /// <summary>
        /// Stores sequence ID of reverse read.
        /// </summary>
        private string _reverseRead;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MatePair class with specified library name.
        /// </summary>
        /// <param name="library">Library name</param>
        public MatePair(string library)
        {
            if (Validate(library))
            {
                Library = library;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MatePair class with specified library name,
        /// forward read and reverse read.
        /// </summary>
        /// <param name="forwardRead">Forward Read</param>
        /// <param name="reverseRead">Reverse Read</param>
        /// <param name="library">Library used to sequence reads</param>
        public MatePair(ISequence forwardRead, ISequence reverseRead, string library)
        {
            if (null == forwardRead)
            {
                throw new ArgumentNullException("forwardRead");
            }

            if (null == reverseRead)
            {
                throw new ArgumentNullException("reverseRead");
            }

            if (string.IsNullOrEmpty(library))
            {
                throw new ArgumentNullException("library");
            }

            if(Validate(forwardRead, reverseRead, library))
            {
                _forwardRead = forwardRead.DisplayID;
                _reverseRead = reverseRead.DisplayID;
                Library = library;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MatePair class with specified library name,
        /// forward read and reverse read.
        /// </summary>
        /// <param name="forwardReadID">ID of forward read.</param>
        /// <param name="reverseReadID">ID of reverse read.</param>
        /// <param name="library">Library used to sequence reads</param>
        public MatePair(string forwardReadID, string reverseReadID, string library)
        {
            if(Validate(forwardReadID, reverseReadID, library))
            {
                _forwardRead = forwardReadID;
                _reverseRead = reverseReadID;
                Library = library;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets sequence of Forward Read
        /// </summary>
        public string ForwardReadID
        {
            get
            {
                return _forwardRead;
            }

            set
            {
                if (Validate(value))
                {
                    _forwardRead = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets sequence for Reverse Read
        /// </summary>
        public string ReverseReadID
        {
            get
            {
                return _reverseRead;
            }

            set
            {
                if (Validate(value))
                {
                    _reverseRead = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets Name of Library
        /// </summary>
        public string Library
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets mean length of Insert for Library
        /// </summary>
        public float MeanLengthOfLibrary
        {
            get
            {
                return CloneLibrary.Instance.GetLibraryInformation(Library).MeanLengthOfInsert;
            }
        }

        /// <summary>
        /// Gets standard deviation of insert lengths for a library
        /// </summary>
        public float StandardDeviationOfLibrary
        {
            get
            {
                return CloneLibrary.Instance.GetLibraryInformation(Library).StandardDeviationOfInsert;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sequence of forward read.
        /// </summary>
        /// <param name="sequences">List of input reads.</param>
        /// <returns>Sequence of forwrad read.</returns>
        public ISequence GetForwardRead(IList<ISequence> sequences)
        {
            return sequences.First(t => t.DisplayID == _forwardRead);
        }

        /// <summary>
        /// Gets the Sequence of reverse read from given list.
        /// </summary>
        /// <param name="sequences">List of input reads.</param>
        /// <returns>Sequence of reverse read.</returns>
        public ISequence GetReverseRead(IList<ISequence> sequences)
        {
            return sequences.First(t => t.DisplayID == _reverseRead);
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Validate library information.
        /// </summary>
        /// <param name="library">Name of library.</param>
        /// <returns>Is Input Valid.</returns>
        private static bool Validate(String library)
        {
            if (string.IsNullOrEmpty(library))
            {
                throw new ArgumentNullException("library");
            }

            return true;
        }

        /// <summary>
        /// Validates the Input.
        /// </summary>
        /// <param name="forwardReadID">ID of forward read</param>
        /// <param name="reverseReadID">ID of reverse read</param>
        /// <param name="library">Name of Library</param>
        /// <returns>Are inputs valid</returns>
        private static bool Validate(string forwardReadID, string reverseReadID, string library)
        {
            if (string.IsNullOrEmpty(forwardReadID))
            {
                throw new ArgumentNullException("forwardReadID");
            }

            if (string.IsNullOrEmpty(reverseReadID))
            {
                throw new ArgumentNullException("reverseReadID");
            }

            Validate(library);

            return true;
        }

        /// <summary>
        /// Validates the Input.
        /// </summary>
        /// <param name="forwardRead">Sequence of forward read.</param>
        /// <param name="reverseRead">Sequence of reverse read.</param>
        /// <param name="library">Name of libarary.</param>
        /// <returns>Are inputs valid</returns>
        private static bool Validate(ISequence forwardRead, ISequence reverseRead, string library)
        {
            if (0 == forwardRead.Count)
            {
                throw new ArgumentException(Resource.ForwardReadCount);
            }

            if (0 == reverseRead.Count)
            {
                throw new ArgumentException(Resource.ReverseReadCount);
            }

            Validate(library);
            return true;
        }

        #endregion

    }
}
