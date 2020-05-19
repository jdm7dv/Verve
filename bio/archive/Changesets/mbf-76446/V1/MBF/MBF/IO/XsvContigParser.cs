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
using System.IO;
using System.Linq;
using System.Text;
using MBF.Algorithms.Assembly;
using MBF.Encoding;


namespace MBF.IO
{

    /// <summary>
    /// This creates a contig parser that uses an XSV sparse reader to parse
    /// a contig from a list of sparse sequences, where the first sequence is the
    /// consensus and the rest are sequences aligned to it.
    /// </summary>
    public class XsvContigParser : XsvSparseParser
    {
        #region Private Fields

        /// <summary>
        /// The separator character passed to the XsvSparseReader
        /// </summary>
        private readonly char separator;

        /// <summary>
        /// The sequence ID prefix character passed to the XsvSparseReader
        /// </summary>
        private readonly char sequenceIdPrefix;

        #endregion

        #region Constructor

        ///<summary>
        /// Creates a contig parser that parses Contigs using the given encoding
        /// and alphabet, by creating an XsvSparseReader that uses the given separator 
        /// and sequenceIdPrefix characters.
        ///</summary>
        ///<param name="encoding">Encoding to use for the consensus and assembled sequences that are parsed.</param>
        ///<param name="alphabet">Alphabet to use for the consensus and assembled sequences that are parsed.</param>
        ///<param name="separator_">Character used to separate sequence item position and symbol in the Xsv file</param>
        ///<param name="sequenceIdPrefix_">Character used at the beginning of the sequence start line.</param>
        public XsvContigParser(IEncoding encoding, IAlphabet alphabet,
                               char separator_, char sequenceIdPrefix_)
            : base(encoding, alphabet)
        {
            separator = separator_;
            sequenceIdPrefix = sequenceIdPrefix_;
        }

        #endregion

        #region Methods
        /// <summary>
        /// This converts a list of sparse sequences read from the Text reader into a contig.
        /// Assumes the first sequence is the consensus and the rest are assembled sequences.
        /// The positions of the assembed sequences are the offsets of the sparse sequences in
        /// the sequence start line. The positions of the sequence items are the same as their
        /// position field value in each character separated line 
        /// (i.e. they are not incremented by the offset)
        /// </summary>
        /// <param name="reader">Text reader with the formatted contig</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the contig should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed contig with consensus and assembled sequences, all represented 
        /// as SparseSequences. 
        /// Null if no lines were present in the reader. Exception if valid sparse sequences
        /// were not present. 
        /// NOTE: This does not check if the assembled sequence positions are valid with respect to the consensus.
        /// </returns>
        public Contig ParseContig(TextReader reader, bool isReadOnly)
        {
            // Check input arguments
            if (reader == null) 
            {
                throw new ArgumentNullException("reader", "Text reader to read contig from cannot be null");
            }

            // parse the consensus
            XsvSparseReader sparseReader = GetSparseReader(reader);
            ISequence consensus = ParseOne(sparseReader, isReadOnly);
            if (consensus == null)
                return null;

            Contig contig = new Contig();
            contig.Consensus = consensus;
            contig.Sequences = ParseAssembledSequence(sparseReader, isReadOnly);
            return contig;
        }

        /// <summary>
        /// Parses a list of assembled sparse sequences from the reader.
        /// </summary>
        /// <param name="contigReader">The reader to read the assembled sparse sequences from</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns></returns>
        protected IList<Contig.AssembledSequence> ParseAssembledSequence(XsvSparseReader contigReader, bool isReadOnly)
        {
            // Check input arguments
            if (contigReader == null) 
            {
                throw new ArgumentNullException("contigReader", "Sparse contig reader cannot be null");
            }

            List<Contig.AssembledSequence> sequenceList = new List<Contig.AssembledSequence>();
            while (contigReader.HasLines)
            {
                Contig.AssembledSequence aseq = new Contig.AssembledSequence();
                int offset;
                var sequenceWithOffset = ParseOneWithOffset(contigReader, isReadOnly);
                aseq.Sequence = sequenceWithOffset.Item1;
                offset = sequenceWithOffset.Item2;
                aseq.Position = offset;
                sequenceList.Add(aseq);
            }
            return sequenceList;
        }

        #endregion

        #region Overrides of XsvSparseParser

        /// <summary>
        /// Creates and returns an XsvSparseReader for the given text reader
        /// that uses the separator and sequenceIdPrefix characters passed
        /// in the constructor for the Contig Parser.
        /// </summary>
        /// <param name="reader">Text reader to create a sparse reader for</param>
        /// <returns>Sparse parser that can parse the text reader as a sequence of 
        /// items using the separator characters defined for this contig.</returns>
        protected override XsvSparseReader GetSparseReader(TextReader reader)
        {
            return new XsvSparseReader(reader, separator, sequenceIdPrefix);
        }

        #endregion
    }
}
