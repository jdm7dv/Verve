//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Bio.Properties;

namespace Bio
{
    /// <summary>
    /// This class is intended for incapsulation use within the various
    /// implementations of ISequence to centralize metadata storage and
    /// logic code.
    /// 
    /// This class implements ICloneable interface. To create a copy 
    /// of the BasicSequenceInfo call Clone() method. For example:
    /// 
    /// BasicSequenceInfo basicInfo = new BasicSequenceInfo();
    /// BasicSequenceInfo basicInfoCopy = basicInfo.Clone();
    ///
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files can be 
    /// de-serialized to restore the instances.
    /// While serializing any non serializable object in the metadata are ignored 
    /// thus the restored BasicSequenceInfo instance will contains null in place 
    /// of non serializable object.
    /// </summary>
    [Serializable]
    public class BasicSequenceInfo : ICloneable, ISerializable
    {
        #region Fields
        private string id = String.Empty;
        private string displayID = null;
        private Dictionary<string, object> metadata = new Dictionary<string, object>();
        private IAlphabet alphabet;
        private MoleculeType _moleculeType;
        #endregion  Fields

        #region Properties
        internal string ID
        {
            get { return id; }
            set { id = value; }
        }

        internal string DisplayID
        {
            get
            {
                if (String.IsNullOrEmpty(displayID))
                    return ID;
                return displayID;
            }
            set
            {
                displayID = value;
            }
        }

        internal IAlphabet Alphabet
        {
            get { return alphabet; }
            set { alphabet = value; }
        }

        internal MoleculeType MoleculeType
        {
            get { return _moleculeType; }
            set { _moleculeType = value; }
        }

        internal Dictionary<string, object> Metadata
        {
            get { return metadata; }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasicSequenceInfo()
        {
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Gets the index of first non gap character in the specified sequence.
        /// </summary>
        /// <param name="sequence">ISequence instance.</param>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public static int IndexOfNonGap(ISequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            if (sequence.Count > 0)
            {
                return IndexOfNonGap(sequence, 0);
            }

            return -1;
        }

        /// <summary>
        /// Returns the position of the first item in the specified sequence from startPos that does not 
        /// have a Gap character.
        /// </summary>
        /// <param name="sequence">ISequence instance.</param>
        /// <param name="startPos">Index value above which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public static int IndexOfNonGap(ISequence sequence, int startPos)
        {
            int index = -1;
            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            if (startPos < 0 || startPos >= sequence.Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameStartPos,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            for (int i = startPos; i < sequence.Count; i++)
            {
                if (sequence[i] != null && !sequence[i].IsGap)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Gets the index of last non gap character in the specified sequence.
        /// </summary>
        /// <param name="sequence">ISequence instance.</param>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public static int LastIndexOfNonGap(ISequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            if (sequence.Count > 0)
            {
                return LastIndexOfNonGap(sequence,sequence.Count - 1);
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of last non gap character in the specified sequence within the specified end position.
        /// </summary>
        /// <param name="sequence">ISequence instance.</param>
        /// <param name="endPos">Index value below which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public static int LastIndexOfNonGap(ISequence sequence, int endPos)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            int index = -1;

            if (endPos < 0 || endPos >= sequence.Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameEndPos,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            for (int i = endPos; i >= 0; i--)
            {
                if (sequence[i] != null && !sequence[i].IsGap)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Creates a new BasicSequenceInfo that is a copy of the current BasicSequenceInfo.
        /// </summary>
        /// <returns>A new BasicSequenceInfo that is a copy of this BasicSequenceInfo.</returns>
        public BasicSequenceInfo Clone()
        {
            BasicSequenceInfo cloneObject = new BasicSequenceInfo();
            cloneObject.id = id;
            cloneObject.displayID = displayID;
            cloneObject._moleculeType = _moleculeType;
            cloneObject.alphabet = alphabet;

            foreach (KeyValuePair<string, object> metadataKeyValue in metadata)
            {
                object value = metadataKeyValue.Value;

                // If the object in the medata is ICloneable then get the clone copy.
                ICloneable cloneValue = value as ICloneable;
                if (cloneValue != null)
                {
                    value = cloneValue.Clone();
                }

                cloneObject.metadata.Add(metadataKeyValue.Key, value);
            }

            return cloneObject;
        }
        #endregion Methods

        #region ICloneable Members
        /// <summary>
        /// Creates a new BasicSequenceInfo that is a copy of the current BasicSequenceInfo.
        /// </summary>
        /// <returns>A new object that is a copy of this BasicSequenceInfo.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected BasicSequenceInfo(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            id = info.GetString("ID");
            displayID = info.GetString("DID");

            // Get the alphabet from alphabet name.
            string alphabetName = info.GetString("AN");

            if (!string.IsNullOrEmpty(alphabetName))
            {
                alphabet = Alphabets.All.Single(A => A.Name.Equals(alphabetName));
            }

            _moleculeType = (MoleculeType)info.GetValue("MT", typeof(int));

            if (info.GetBoolean("M"))
            {
                metadata = (Dictionary<string, object>)info.GetValue("MD", typeof(Dictionary<string, object>));
            }
        }

        /// <summary>
        /// Method for serializing the BasicSequenceInfo.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ID", id);
            info.AddValue("DID", displayID);
            string alphabetName = string.Empty;

            if (alphabet != null)
            {
                alphabetName = alphabet.Name;
            }

            info.AddValue("AN", alphabetName);
            info.AddValue("MT", (int)_moleculeType);

            Dictionary<string, object> tempMetadata = new Dictionary<string, object>();

            // Ignore non serializable objects in the metadata.
            foreach (KeyValuePair<string, object> kvp in metadata)
            {
                if ((kvp.Value.GetType().Attributes & System.Reflection.TypeAttributes.Serializable)
                    == System.Reflection.TypeAttributes.Serializable)
                {
                    tempMetadata.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    tempMetadata.Add(kvp.Key, null);
                }
            }

            if (tempMetadata.Count > 0)
            {
                info.AddValue("M", true);
                info.AddValue("MD", tempMetadata);
            }
            else
            {
                info.AddValue("M", false);
            }
        }

        #endregion
    }
}
