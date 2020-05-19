// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// Implements ISuffixEdgeStorage interface for Data base driven suffix edges storage. 
    /// This class has required methods to Write and Read the Persistent Edge in a file.
    /// </summary>
    public class FileSuffixEdgeStorage : ISuffixEdgeStorage, IDisposable
    {
        #region -- Constants --

        /// <summary>
        /// Buffer size
        /// </summary>
        private const int BUFFER_SIZE = 4096;

        #endregion

        #region -- Member variables --

        /// <summary>
        /// Stream to underlying file object
        /// </summary>
        private string _fileName = string.Empty;

        /// <summary>
        /// Stream to underlying file object
        /// </summary>
        private FileStream _stream = null;

        /// <summary>
        /// Object to be locked for any file operation
        /// </summary>
        private object lockObject = new object();

        #endregion

        #region -- Constructor --

        /// <summary>
        /// Default Constructor: Initialize an instance of FileSuffixEdgeStorage
        /// </summary>
        public FileSuffixEdgeStorage()
        {
            _fileName = string.Format(CultureInfo.InvariantCulture,
                    "MultiWaySuffixTree_{0}.csv",
                    DateTime.Now.Ticks);
            _stream = new FileStream(_fileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Write the edge to database storage.
        /// 1. Serialize the given edge.
        /// 2. Write it to the database.
        /// 3. Return the Key(Id) of newly inserted row.
        /// If the edge does not have key, insert a edge in storage
        /// Else update the existing edge in storage
        /// </summary>
        /// <param name="edge">Edge to be stored</param>
        /// <returns>Index / Key / Id / Offset of new edge</returns>
        public long Write(IPersistentEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException("edge");
            }

            if (edge.Key == -1)
            {
                edge.Key = Insert(edge);
            }
            else
            {
                Update(edge.Key, edge);
            }

            return edge.Key;
        }

        /// <summary>
        /// Read an edge from the storage device using ginen index (offset/Id) and return the same.
        /// 1. Read the serialized data from storage.
        /// 2. De-serialize it back to IEdge.
        /// 3. Return the edge
        /// </summary>
        /// <param name="index">Index / Id / Key of the required edge</param>
        /// <returns>Edge found with given Key</returns>
        public IPersistentEdge Read(long index)
        {
            StringBuilder lineBuilder = new StringBuilder();
            byte[] buffer = null;

            lock (lockObject)
            {
                _stream.Seek(index, SeekOrigin.Begin);

                buffer = new byte[BUFFER_SIZE];
                _stream.Read(buffer, 0, BUFFER_SIZE);
            }

            for (int counter = 0; counter < BUFFER_SIZE; counter++)
            {
                if ((buffer[counter] == '\0')
                    || (buffer[counter] == '\r')
                    || (buffer[counter] == '\n'))
                {
                    break;
                }

                lineBuilder.Append((char)buffer[counter]);
            }

            IPersistentEdge edge = new PersistentMultiWaySuffixEdge(lineBuilder.ToString());
            edge.Key = index;
            return edge;
        }

        /// <summary>
        /// Remove an edge from the storage device using ginen index (offset/Id).
        /// </summary>
        /// <param name="index">Index (offset/Id) of edge to be removed</param>
        /// <returns>Success flag</returns>
        public bool Remove(long index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region -- Private Methods --

        /// <summary>
        /// Insert a new row in data base and return the row identity
        /// </summary>
        /// <param name="edge">Edge to be inserted</param>
        /// <returns>Id of new inserted row</returns>
        private long Insert(IPersistentEdge edge)
        {
            // Append the new line character to each line
            lock (lockObject)
            {
                _stream.Seek(0, SeekOrigin.End);
                edge.Key = _stream.Position;
                string edgeString = edge.Serialize();
                edgeString = string.Concat(edgeString, Environment.NewLine);
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(edgeString);

                _stream.Write(buffer, 0, edgeString.Length);
                _stream.Flush();
            }

            return edge.Key;
        }

        /// <summary>
        /// Update new row in data base and return the row identity
        /// </summary>
        /// <param name="id">Identity of row to be updated</param>
        /// <param name="edge">Edge to be updated</param>
        /// <returns>Success flag</returns>
        private bool Update(long id, IPersistentEdge edge)
        {
            string edgeString = edge.Serialize();
            edgeString = string.Concat(edgeString, Environment.NewLine);
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes(edgeString);

            lock (lockObject)
            {
                _stream.Seek(id, SeekOrigin.Begin);
                _stream.Write(buffer, 0, edgeString.Length);
                _stream.Flush();
            }

            return true;
        }

        #endregion

        #region -- IDisposable --

        /// <summary>
        /// Dispose the resources
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
                if (_stream != null)
                {
                    _stream.Flush();
                    _stream.Dispose();

                     // Delete the file
                     File.Delete(_fileName);
                }
            }
        }

        #endregion
    }
}