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
using System.Runtime.Serialization;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// PairwiseAlignedSequence is a class containing the single aligned unit of pairwise alignment.
    /// </summary>
    [Serializable]
    public class PairwiseAlignedSequence : AlignedSequence
    {
        #region Private constants
        /// <summary>
        /// Constant string indicating consensus in meta-data
        /// </summary>
        private const string ConsensusKey = "Consensus";

        /// <summary>
        /// Constant string indicating alignment score in meta-data
        /// </summary>
        private const string ScoreKey = "Score";

        /// <summary>
        /// Constant string indicating offset of first sequence in alignment
        /// </summary>
        private const string FirstOffsetKey = "FirstOffset";

        /// <summary>
        /// Constant string indicating offset of second sequence in alignment
        /// </summary>
        private const string SecondOffsetKey = "SecondOffset";
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PairwiseAlignedSequence class
        /// Default constructor.
        /// </summary>
        public PairwiseAlignedSequence()
            : base()
        {
            // No impl.
        }

        /// <summary>
        /// Initializes a new instance of the PairwiseAlignedSequence class
        /// Internal constructor for creating new instance of 
        /// PairwiseAlignedSequence from specified IAlignedSequence.
        /// </summary>
        /// <param name="alignedSequence">IAlignedSequence instance.</param>
        internal PairwiseAlignedSequence(IAlignedSequence alignedSequence)
            : base(alignedSequence)
        {
            // Impl.
        }
        
        /// <summary>
        /// Initializes a new instance of the PairwiseAlignedSequence class
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected PairwiseAlignedSequence(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // no impl.
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets Alignment of First Sequence
        /// </summary>
        public ISequence FirstSequence
        {
            get
            {
                if (Sequences.Count > 0)
                {
                    return Sequences[0];
                }

                return null;
            }

            set
            {
                if (Sequences.Count == 0)
                {
                    Sequences.Add(null);
                }

                Sequences[0] = value;
            }
        }

        /// <summary>
        /// Gets or sets Alignment of Second Sequence
        /// </summary>
        public ISequence SecondSequence
        {
            get
            {
                if (Sequences.Count > 1)
                {
                    return Sequences[1];
                }

                return null;
            }

            set
            {
                if (Sequences.Count == 0)
                {
                    Sequences.Add(null);
                }

                if (Sequences.Count == 1)
                {
                    Sequences.Add(null);
                }

                Sequences[1] = value;
            }
        }

        /// <summary>
        /// Gets or sets Consensus of FirstSequence and SecondSequence
        /// </summary>
        public ISequence Consensus
        {
            get
            {
                if (Metadata.ContainsKey(ConsensusKey))
                {
                    return Metadata[ConsensusKey] as ISequence;
                }

                return null;
            }

            set
            {
                Metadata[ConsensusKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets Score of the alignment
        /// </summary>
        public int Score
        {
            get
            {
                int score = 0;
                if (Metadata.ContainsKey(ScoreKey))
                {
                    if (Metadata[ScoreKey] is int)
                    {
                        score = (int)Metadata[ScoreKey];
                    }
                }

                return score;
            }

            set
            {
                Metadata[ScoreKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets Offset of FirstSequence
        /// </summary>
        public int FirstOffset
        {
            get
            {
                int score = 0;
                if (Metadata.ContainsKey(FirstOffsetKey))
                {
                    if (Metadata[FirstOffsetKey] is int)
                    {
                        score = (int)Metadata[FirstOffsetKey];
                    }
                }

                return score;
            }

            set
            {
                Metadata[FirstOffsetKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets Offset of SecondSequence
        /// </summary>
        public int SecondOffset
        {
            get
            {
                int score = 0;
                if (Metadata.ContainsKey(SecondOffsetKey))
                {
                    if (Metadata[SecondOffsetKey] is int)
                    {
                        score = (int)Metadata[SecondOffsetKey];
                    }
                }

                return score;
            }

            set
            {
                Metadata[SecondOffsetKey] = value;
            }
        }
        #endregion
    }
}
