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
using System.Runtime.Serialization;
using MBF.Algorithms.Alignment;
using MBF.Util;

namespace MBF.IO.SAM
{
    /// <summary>
    /// Class to hold Reads or aligned sequence or query sequence in SAM Alignment.
    /// </summary>
    [Serializable]
    public class SAMAlignedSequence : IAlignedSequence
    {
        #region Fields
        /// <summary>
        /// SAM aligned sequence header.
        /// </summary>
        private SAMAlignedSequenceHeader _seqHeader;

        /// <summary>
        /// Holds metadata of this aligned sequence.
        /// </summary>
        private Dictionary<string, object> _metadata;

        /// <summary>
        /// Holds aligned sequence.
        /// </summary>
        private List<ISequence> _sequences;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets read/query/aligned sequence.
        /// </summary>
        public ISequence QuerySequence
        {
            get
            {
                return _sequences[0];
            }
            set
            {
                _sequences[0] = value;
            }
        }

        /// <summary>
        /// Query pair name if paired; or Query name if unpaired.
        /// </summary>  
        public string QName
        {
            get
            {
                return _seqHeader.QName;
            }
            set
            {
                _seqHeader.QName = value;
            }
        }

        /// <summary>
        /// SAM flags.
        /// <see cref="SAMFlags"/>
        /// </summary>
        public SAMFlags Flag
        {
            get
            {
                return _seqHeader.Flag;
            }
            set
            {
                _seqHeader.Flag = value;
            }
        }

        /// <summary>
        /// Reference sequence name.
        /// </summary>
        public string RName
        {
            get
            {
                return _seqHeader.RName;
            }
            set
            {
                _seqHeader.RName = value;
            }
        }
        /// <summary>
        /// One-based leftmost position/coordinate of the clipped sequence.
        /// </summary>
        public int Pos
        {
            get
            {
                return _seqHeader.Pos;
            }
            set
            {
                _seqHeader.Pos = value;
            }
        }

        /// <summary>
        /// Gets bin depending on POS and CIGAR values.
        /// </summary>
        public int Bin
        {
            get
            {
                return _seqHeader.Bin;
            }

            internal set
            {
                _seqHeader.Bin = value;
            }
        }

        /// <summary>
        /// Gets Query length depending on CIGAR value.
        /// </summary>
        public int QueryLength
        {
            get
            {
                return _seqHeader.QueryLength;
            }
        }

        /// <summary>
        /// Mapping quality (phred-scaled posterior probability that the 
        /// mapping position of this read is incorrect).
        /// </summary>
        public int MapQ
        {
            get
            {

                return _seqHeader.MapQ;
            }
            set
            {
                _seqHeader.MapQ = value;
            }
        }

        /// <summary>
        /// Extended CIGAR string.
        /// </summary>
        public string CIGAR
        {
            get
            {
                return _seqHeader.CIGAR;
            }

            set
            {
                _seqHeader.CIGAR = value;
            }
        }

        /// <summary>
        /// Mate reference sequence name. 
        /// </summary>
        public string MRNM
        {
            get
            {
                return _seqHeader.MRNM;
            }

            set
            {
                _seqHeader.MRNM = value;
            }
        }

        /// <summary>
        /// One-based leftmost mate position of the clipped sequence.
        /// </summary>
        public int MPos
        {
            get
            {
                return _seqHeader.MPos;
            }

            set
            {
                _seqHeader.MPos = value;
            }
        }

        /// <summary>
        /// Inferred insert size.
        /// </summary>
        public int ISize
        {
            get
            {
                return _seqHeader.ISize;
            }

            set
            {
                _seqHeader.ISize = value;
            }
        }

        /// <summary>
        /// Contains the list of indices of "." symbols present in the aligned sequence.
        /// As "." is not supported by DNA, RNA and Protien alphabets, while creating aligned 
        /// sequence "." symbols are replaced by "N" which has the same meaning of ".".
        /// </summary>
        public IList<int> DotSymbolIndexes
        {
            get
            {
                return _seqHeader.DotSymbolIndices;
            }
        }

        /// <summary>
        /// Contains the list of "=" symbol indices present in the aligned sequence.
        /// The "=" symbol in aligned sequence indicates that the symbol at this index 
        /// is equal to the symbol present in the reference sequence. As "=" is not 
        /// supported by DNA, RNA and Protien alphabets, while creating aligned 
        /// sequence "=" symbols are replaced by the symbol present in the reference 
        /// sequence at the same index.
        /// </summary>
        public IList<int> EqualSymbolIndexes
        {
            get
            {
                return _seqHeader.EqualSymbolIndices;
            }
        }

        /// <summary>
        /// Optional fields.
        /// </summary>
        public IList<SAMOptionalField> OptionalFields
        {
            get
            {
                return _seqHeader.OptionalFields;
            }
        }

        /// <summary>
        /// Metadata of this aligned sequence.
        /// SAMAlignedSequenceHeader is stored with the key "SAMAlignedSequenceHeader".
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Always returns QuerySequence in a list.
        /// </summary>
        public IList<ISequence> Sequences
        {
            get { return _sequences.AsReadOnly(); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates new instance of SAMAlignedSequence.
        /// </summary>
        public SAMAlignedSequence() : this(new SAMAlignedSequenceHeader()) { }

        /// <summary>
        /// Creates new instance of SAMAlignedSequence with specified SAMAlignedSequenceHeader.
        /// </summary>
        /// <param name="seqHeader"></param>
        public SAMAlignedSequence(SAMAlignedSequenceHeader seqHeader)
        {
            _seqHeader = seqHeader;
            _metadata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            _metadata.Add(Helper.SAMAlignedSequenceHeaderKey, _seqHeader);
            _sequences = new List<ISequence>();
            _sequences.Add(null);
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected SAMAlignedSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _seqHeader = (SAMAlignedSequenceHeader)info.GetValue("header", typeof(SAMAlignedSequenceHeader));
            _metadata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            _metadata.Add(Helper.SAMAlignedSequenceHeaderKey, _seqHeader);
            _sequences = new List<ISequence>();
            _sequences.Add(null);
            QuerySequence = (ISequence)info.GetValue("sequence", typeof(ISequence));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets quality values.
        /// </summary>
        public byte[] GetQualityValues()
        {
            IQualitativeSequence seq = QuerySequence as IQualitativeSequence;
            if (seq != null)
            {
                return seq.Scores;
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// Method for serializing the SAMAlignedSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("header", _seqHeader);
            info.AddValue("sequence", QuerySequence);
        }
        #endregion
    }
}
