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
using System.Runtime.Serialization;
using System.Text;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// Implements IEdge interface for PersistentMWaySuffixTree. Represents an edge in suffix tree which exposes all the properties required by IEdge and pointers to child edges.
    /// </summary>
    [Serializable]
    public class PersistentMultiWaySuffixEdge : IPersistentEdge
    {
        #region -- Member Variables --

        /// <summary>
        /// Pointers to all the child edges
        /// </summary>
        private long[] _children = null;

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the Edge class
        ///     Index of First Character
        ///     Index of Last Character
        ///     Start node of edge
        /// </summary>
        /// <param name="startIndex">Index of the First Character</param>
        /// <param name="endIndex">Index of the Last Character</param>
        /// <param name="childrenCount">Number of allowed child edges</param>
        public PersistentMultiWaySuffixEdge(int startIndex, int endIndex, int childrenCount)
            : this(childrenCount)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        /// <summary>
        /// Initializes a new instance of the Edge class
        /// </summary>
        /// <param name="childrenCount">Number of allowed child edges</param>
        public PersistentMultiWaySuffixEdge(int childrenCount)
        {
            Key = -1;
            _children = new long[childrenCount];
            for (int index = 0; index < _children.Length; index++)
            {
                _children[index] = -1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Edge class
        /// </summary>
        /// <param name="data">Deserialize given data into persistent suffix tree edge</param>
        public PersistentMultiWaySuffixEdge(string data)
        {
            Deserialize(data);
        }
        #endregion

        #region -- IPersistentEdge --

        /// <summary>
        /// Serialization format
        /// </summary>
        private const string SERIALIZE_FORMAT =
            "K:{0,20};SI:{1,11};EI:{2,11};L:{3};C:{4};";

        /// <summary>
        /// Property seperator
        /// </summary>
        private readonly static char[] PROPERTY_SEPERATOR = new char[] { ';' };

        /// <summary>
        /// Data seperator
        /// </summary>
        private readonly static char[] DATA_SEPERATOR = new char[] { ':' };

        /// <summary>
        /// Children seperator
        /// </summary>
        private readonly static char[] CHILDREN_SEPERATOR = new char[] { ',' };

        /// <summary>
        /// Serialize the given object to string format.
        /// Build a semicolon (;) seperator string in following format
        /// Key:{N}StartIndex:{N};EndIndex:{N};IsLeaf:{T/F};Children:{N},{N},...;
        /// </summary>
        /// <returns>Serialized data</returns>
        public string Serialize()
        {
            StringBuilder children = new StringBuilder();

            if (_children != null)
            {
                for (int index = 0; index < _children.Length; index++)
                {
                    children.Append(string.Format(
                        CultureInfo.InvariantCulture,
                        "{0,20}",
                        _children[index]));
                    children.Append(",");
                }
            }

            return string.Format(CultureInfo.InvariantCulture,
                    SERIALIZE_FORMAT,
                        Key,
                        StartIndex,
                        EndIndex,
                        IsLeaf ? 'T' : 'F',
                        children.ToString().TrimEnd(','));
        }

        /// <summary>
        /// Deserialize given data into persistent suffix tree edge
        /// </summary>
        /// <param name="data">Serialized data</param>
        public void Deserialize(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            string[] properties = data.Split(PROPERTY_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);

            // First is Key
            string[] values = properties[0].Split(DATA_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
            Key = long.Parse(values[1], CultureInfo.InvariantCulture);

            // Next is Start Index
            values = properties[1].Split(DATA_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
            StartIndex = int.Parse(values[1], CultureInfo.InvariantCulture);

            // Next is End Index
            values = properties[2].Split(DATA_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
            EndIndex = int.Parse(values[1], CultureInfo.InvariantCulture);

            // Next is IsLeaf, Ignore this
            // Next is Children
            values = properties[4].Split(DATA_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 1)
            {
                string[] children = values[1].Split(CHILDREN_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
                if (children.Length > 0)
                {
                    _children = new long[children.Length];
                    for (int index = 0; index < children.Length; index++)
                    {
                        _children[index] = long.Parse(children[index], CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        #endregion

        #region -- ISerializable Members --

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected PersistentMultiWaySuffixEdge(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Key = info.GetInt64("PersistentMultiWaySuffixEdge:Key");
            _children = (long[])info.GetValue("PersistentMultiWaySuffixEdge:Children", typeof(long[]));
            StartIndex = info.GetInt32("PersistentMultiWaySuffixEdge:StartIndex");
            EndIndex = info.GetInt32("PersistentMultiWaySuffixEdge:EndIndex");
        }

        /// <summary>
        /// Method for serializing the sequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("PersistentMultiWaySuffixEdge:Key", Key);
            info.AddValue("PersistentMultiWaySuffixEdge:Children", GetChildren());
            info.AddValue("PersistentMultiWaySuffixEdge:StartIndex", StartIndex);
            info.AddValue("PersistentMultiWaySuffixEdge:EndIndex", EndIndex);
            info.AddValue("PersistentMultiWaySuffixEdge:IsLeaf", IsLeaf);
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets or sets the unique key representing the edge in persistent storage
        /// </summary>
        public long Key { get; set; }

        /// <summary>
        /// Gets or sets index of last character
        /// </summary>
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the edge is at the leaf.
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                for (int index = 0; index < _children.Length; index++)
                {
                    if (_children[index] != -1)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets or sets index of first character.
        /// </summary>
        public int StartIndex { get; set; }

        #endregion

        #region -- Public methods --

        /// <summary>
        /// Gets pointers to all the child edges
        /// </summary>
        public long[] GetChildren()
        {
            // Return children
            return _children;
        }

        /// <summary>
        /// Add an edge to children array
        /// </summary>
        /// <param name="key">Key of edge to be added</param>
        public void AddChild(long key)
        {
            for (int index = 0; index < _children.Length; index++)
            {
                if (_children[index] == -1)
                {
                    _children[index] = key;
                    return;
                }
            }

            // No more children edge can be added.
            throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot add more than {0} child nodes to edge.",
                    _children.Length));

        }

        /// <summary>
        /// Replace the current set of children with given set of children
        /// </summary>
        /// <param name="children">new set of children</param>
        public void ReplaceChildren(long[] children)
        {
            _children = children;
        }

        /// <summary>
        /// Clear the children list of current edge
        /// </summary>
        public void ClearChildren()
        {
            for (int index = 0; index < _children.Length; index++)
            {
                _children[index] = -1;
            }
        }

        #endregion
    }
}