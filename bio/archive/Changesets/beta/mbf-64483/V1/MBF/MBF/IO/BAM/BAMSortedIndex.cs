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

namespace MBF.IO.BAM
{
    /// <summary>
    /// This class implements indexer for Sorted BAM Index.
    /// Reads
    ///  Index for a file (contains data sorted by index) and return index
    ///  Or
    ///  Indices from multiple file (contains data sorted by index in each file) and returns smallest index.
    /// </summary>
    public class BAMSortedIndex :IEnumerable<int>, IEnumerator<int>
    {
        /// <summary>
        /// List of file readers.
        /// </summary>
        private IList<StreamReader> _readers = null;

        /// <summary>
        /// Next data object to processed in each file.
        /// </summary>
        private IList<Tuple<string, int>> _data = null;

        /// <summary>
        /// holds filenames (sorted files) like chr1_1, chr1_2, chr2 etc.
        /// </summary>
        private IList<string> _filenames;

        /// <summary>
        /// Type of sort needed.
        /// </summary>
        private BAMSortByFields _sortType;

        /// <summary>
        /// Holds current sorted index.
        /// </summary>
        private int _current;

        /// <summary>
        /// Constructor to initialize an instance of BAMSortedIndex class with specified list of filenames.
        /// </summary>
        /// <param name="filenames">Sorted filenames.</param>
        /// <param name="sortType">Type of sort required.</param>
        public BAMSortedIndex(IList<string> filenames, BAMSortByFields sortType)
        {
            _filenames = filenames;
            _sortType = sortType;
        }

        /// <summary>
        /// Constructor to initialize an instance of BAMSortedIndex class with specified filename.
        /// </summary>
        /// <param name="filename">Sorted filename.</param>
        /// <param name="sortType">Type of sort required.</param>
        public BAMSortedIndex(string filename, BAMSortByFields sortType)
        {
            _filenames = new List<string>();
            _filenames.Add(filename);
            _sortType = sortType;
        }

        /// <summary>
        /// Gets or sets the Chromosome name of this Sorted BAM Indexer
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets the current sorted index.
        /// </summary>
        public int Current
        {
            get 
            {
                return _current; 
            }
        }

        /// <summary>
        /// Disposes this object by discording any resources held.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose field instances
        /// </summary>
        /// <param name="disposeManaged">If disposeManaged equals true, clean all resources</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                // close all file pointers here.
                if (_readers != null)
                {
                    for (int index = 0; index < _readers.Count; index++)
                    {
                        _readers[index].Close();
                        _readers[index].Dispose();
                        _readers[index] = null;
                    }
                }

                if (_filenames != null)
                {
                    foreach (string filename in _filenames)
                    {
                        if (File.Exists(filename))
                        {
                            try
                            {
                                File.Delete(filename);
                            }
                            catch (IOException)
                            {
                                // Ignore the exception
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current sorted index.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get
            {

                return _current;
            }
        }

        /// <summary>
        /// Fetches next sorted index.
        /// </summary>
        /// <returns>Returns true on successful fetch, else return false.</returns>
        public bool MoveNext()
        {
            // code here to get the next smaller index.
            if (null == _data)
            {
                if (_filenames == null || _filenames.Count == 0)
                {
                    return false;
                }

                StreamReader reader = null;
                string[] data;
                Tuple<string, int> dataObject;

                _readers = new List<StreamReader>();
                _data = new List<Tuple<string, int>>();
                foreach (string filename in _filenames)
                {
                    reader = new StreamReader(filename);
                    data = reader.ReadLine().Split(',');
                    dataObject = new Tuple<string, int>(data[0], int.Parse(data[1], CultureInfo.InvariantCulture));
                    _readers.Add(reader);
                    _data.Add(dataObject);
                }
            }

            int smallestIndex = -1;
            for(int index = 0; index < _data.Count; index++)
            {
                if (_data[index] != null)
                {
                    if (smallestIndex == -1)
                    {
                        smallestIndex = index;
                    }
                    else
                    {
                        switch (_sortType)
                        {
                            case BAMSortByFields.ReadNames:
                                if (0 < string.Compare(_data[smallestIndex].Item1, _data[index].Item1, 
                                        StringComparison.OrdinalIgnoreCase))
                                {
                                    smallestIndex = index;
                                }
                                break;

                            case BAMSortByFields.ChromosomeCoordinates:
                                if (int.Parse(_data[index].Item1, CultureInfo.CurrentCulture) 
                                        < int.Parse(_data[smallestIndex].Item1, CultureInfo.CurrentCulture))
                                {
                                    smallestIndex = index;
                                }
                                break;
                        }
                    }
                }
            }

            if (smallestIndex > -1)
            {
                _current = _data[smallestIndex].Item2;

                string[] data;
                Tuple<string, int> dataObject = null;

                if (!_readers[smallestIndex].EndOfStream)
                {
                    data = _readers[smallestIndex].ReadLine().Split(',');
                    dataObject = new Tuple<string, int>(data[0], int.Parse(data[1], CultureInfo.InvariantCulture));
                }

                _data[smallestIndex] = dataObject;
            }
            else
            {
                Reset();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets this instance to initial state.
        /// </summary>
        public void Reset()
        {
            // code here to reset this object to initial state.
            _data = null;

            foreach (StreamReader reader in _readers)
            {
                reader.Close();
            }

            _readers = null;
        }

        /// <summary>
        /// Returns the enumerator object
        /// </summary>
        /// <returns>enumerator object</returns>
        public IEnumerator<int> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Returns the enumerator object
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
