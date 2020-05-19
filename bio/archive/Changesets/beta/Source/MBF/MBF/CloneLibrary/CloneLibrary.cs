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
using System.Globalization;
using System.IO;
using System.Linq;
using MBF.Library.Resources;
using MBF.Properties;

namespace MBF
{
    /// <summary>
    /// Class created for reading data from resource file having library information.
    /// Singleton design pattern is used to create only one instance of class. 
    /// </summary>
    public class CloneLibrary
    {
        #region Fields

        /// <summary>
        /// Private Instance
        /// </summary>
        private static CloneLibrary _instance;

        /// <summary>
        /// object to use for lock.
        /// </summary>
        private static object lockobj = new object();

        /// <summary>
        /// List of Information about Clone libraries
        /// Duplicate libraries not allowed.
        /// </summary>
        private Dictionary<string, CloneLibraryInformation> _libraries = new Dictionary<string, CloneLibraryInformation>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the information about libraries.
        /// </summary>
        public IList<CloneLibraryInformation> GetLibraries
        {
            get
            {
                return _libraries.Values.ToList();
            }
        }

        #endregion
        #region Singleton

        /// <summary>
        /// Gets an instance of this class.
        /// Property to make sure only one Instance of this class is created
        /// </summary>
        public static CloneLibrary Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new CloneLibrary();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Prevents a default instance of the CloneLibrary class from being created.
        /// Initializes a instance of the CloneLibrary class.
        /// </summary>
        private CloneLibrary()
        {
            ReadLibrary();
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Returns information about Library
        /// </summary>
        /// <param name="libraryName"> Name of Library</param>
        /// <returns>Struct containing Information about Library</returns>
        public CloneLibraryInformation GetLibraryInformation(string libraryName)
        {
            CloneLibraryInformation cloneLibrary;

            if (!_libraries.TryGetValue(libraryName, out cloneLibrary))
            {
                throw new ArgumentException(Resource.LibraryExist);
            }

            return cloneLibrary;
        }

        /// <summary>
        /// Add Library to existing list of libraries.
        /// </summary>
        /// <param name="library">Libarary information.</param>
        public void AddLibrary(CloneLibraryInformation library)
        {
            if (null != library && !String.IsNullOrEmpty(library.LibraryName) &&
               library.MeanLengthOfInsert >= 0 && library.StandardDeviationOfInsert >= 0)
            {
                _libraries[library.LibraryName] = library;
            }
            else
            {
                throw new ArgumentException(Resource.LibraryInvalidParameters);
            }           
        }

        /// <summary>
        /// Add Library to existing list of libraries.
        /// </summary>
        /// <param name="libraryName">Name of Library.</param>
        /// <param name="mean">Mean Length Of Insert.</param>
        /// <param name="standardDeviation">Standard Deviation Of Insert.</param>
        public void AddLibrary(string libraryName, float mean, float standardDeviation)
        {
            if (String.IsNullOrEmpty(libraryName) || mean < 0 || standardDeviation < 0)
            {
                throw new ArgumentException(Resource.LibraryInvalidParameters);
            }

            CloneLibraryInformation library = new CloneLibraryInformation()
            {
                LibraryName = libraryName,
                MeanLengthOfInsert = mean,
                StandardDeviationOfInsert = standardDeviation
            };
            
            _libraries[libraryName] = library;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Read Libraries from file
        /// </summary>
        private void ReadLibrary()
        {
            string library = LibraryResource.Library;
            using (TextReader reader = new StringReader(library))
            {
                library = reader.ReadLine();
                while (!string.IsNullOrEmpty(library))
                {
                    Parse(library);
                    library = reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Parse Library and convert parsed data into structure
        /// </summary>
        /// <param name="library">Name of Library</param>
        private void Parse(string library)
        {
            string[] libraryInformation = library.Split(new char[] { ' ' }, 3);
            CloneLibraryInformation information = new CloneLibraryInformation()
            {
                LibraryName = libraryInformation[0],
                MeanLengthOfInsert = float.Parse(libraryInformation[1], CultureInfo.InvariantCulture),
                StandardDeviationOfInsert = float.Parse(libraryInformation[2], CultureInfo.InvariantCulture)
            };

            _libraries.Add(information.LibraryName, information);
        }

        #endregion
    }
}