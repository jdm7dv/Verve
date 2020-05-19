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
using System.IO;
using Bio.Encoding;


namespace Bio.IO
{
    /// <summary>
    /// Implements common methods for parsing SNPs from a SnpReader into ISequences. 
    /// This reads Snp items from the SnpReader and stores either of the two alleles
    /// in a sparse sequence at the same position as the chromosome position.
    /// Extending classes have to implement the 
    /// SnpReader GetSnpReader(TextReader reader) method that returns a
    /// SnpReader for the given TextReader.
    /// 
    /// This class is based on the Bio.IO.BasicSequenceParser.
    /// 
    /// </summary>
    public abstract class SnpParser : ISequenceParser
    {
        #region Properties implementing ISequenceParser

        /// <summary>
        /// The alphabet to use for parsed ISequence objects.
        /// </summary>
        public IAlphabet Alphabet
        {
            get;
            set;
        }

        /// <summary>
        /// The encoding to use for parsed ISequence objects. 
        /// </summary>
        public IEncoding Encoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the parser. Intended to be filled in 
        /// by classes deriving from BasicSequenceParser class
        /// with the exact name of the parser type.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the description of the parser. Intended to be filled in 
        /// by classes deriving from BasicSequenceParser class
        /// with the exact details of the parser.
        /// </summary>
        public abstract string Description
        {
            get;
        }

        /// <summary>
        /// Gets the filetypes supported by the parser. Intended to be filled in 
        /// by classes deriving from BasicSequenceParser class
        /// with the exact details of the filetypes supported.
        /// </summary>
        public abstract string FileTypes
        {
            get;
        }

        #endregion Properties


        #region Properties introduced by SnpParser

        /// <summary>
        /// If set to false, this will parse AlleleTwo. If true, this will parse AlleleOne from the SnpReader.
        /// </summary>
        public bool ParseAlleleOne
        {
            get;
            set;
        }

        #endregion Properties introduced by SnpParser


        #region Constructor

        /// <summary>
        /// Creates a SNP parser with the given encoding and alphabet
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="alphabet"></param>
        protected SnpParser(IEncoding encoding, IAlphabet alphabet)
        {
            this.Encoding = encoding;
            this.Alphabet = alphabet;
            this.ParseAlleleOne = true;
        }

        #endregion Constructor


        #region Methods implementing ISequenceParser

        /// <summary>
        /// Parses a list of sparse sequences from the reader, one per contiguous 
        /// chromosome present in the reader. There is one SequenceItem per SnpItem with 
        /// either of the two alleles in the SnpItem (determined by the ParseAlleleOne property)
        /// and at the same position in the sequence as the SnpItem.Position.
        /// </summary>
        /// <param name="reader">Text reader to read the Snpitems from using a SnpReader created for it</param>
        /// <returns>Returns a list of sparse sequences containing Snp items that were read 
        /// from the reader, one sequence per contiguous chromosome number and
        /// retaining the same position in the sequence as the chromosome position.</returns>
        public IList<ISequence> Parse(TextReader reader)
        {
            return Parse(reader, true);
        }

        /// <summary>
        /// Parses a list of sparse sequences from the reader, one per contiguous 
        /// chromosome present in the reader. There is one SequenceItem per SnpItem with 
        /// either of the two alleles in the SnpItem (determined by the ParseAlleleOne property)
        /// and at the same position in the sequence as the SnpItem.Position.
        /// </summary>
        /// <param name="reader">Text reader to read the Snpitems from using a SnpReader created for it</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>Returns a list of sparse sequences containing Snp items that were read 
        /// from the reader, one sequence per contiguous chromosome number and
        /// retaining the same position in the sequence as the chromosome position.</returns>
        public IList<ISequence> Parse(TextReader reader, bool isReadOnly)
        {
            ISnpReader snpReader = GetSnpReader(reader);
            snpReader.MoveNext();

            List<ISequence> sequenceList = new List<ISequence>();
            while (snpReader.Current != null)
                sequenceList.Add(ParseOne(snpReader, isReadOnly));
            return sequenceList;
        }

        /// <summary>
        /// Creates a TextReader for the file and calls Parse(TextReader)
        /// </summary>
        /// <param name="filename">Name of file to read the SnpItems from using a SnpReader created for it</param>
        /// <returns>Returns a list of sparse sequences containing Snp items that were read 
        /// from the file, one sequence per contiguous chromosome number and
        /// retaining the same position in the sequence as the chromosome position.</returns>
        public IList<ISequence> Parse(string filename)
        {
            return Parse(filename, true);
        }

        /// <summary>
        /// Creates a TextReader for the file and calls Parse(TextReader)
        /// </summary>
        /// <param name="filename">Name of file to read the SnpItems from using a SnpReader created for it</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>Returns a list of sparse sequences containing Snp items that were read 
        /// from the file, one sequence per contiguous chromosome number and
        /// retaining the same position in the sequence as the chromosome position.</returns>
        public IList<ISequence> Parse(string filename, bool isReadOnly)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                return Parse(reader, isReadOnly);
            }
        }

        /// <summary>
        /// Creates a TextReader for the file and calls ParseOne(TextReader)
        /// </summary>
        /// <param name="filename">Name of file to read the SnpItems from using a SnpReader created for it</param>
        /// <returns>Returns a single sparse sequence containing Snp items from a single contiguous
        /// chromosome read from the file. The SnpItems retain the same position in 
        /// the sequence as the chromosome position.</returns>
        public ISequence ParseOne(string filename)
        {
            return ParseOne(filename, true);
        }

        /// <summary>
        /// Creates a TextReader for the file and calls ParseOne(TextReader)
        /// </summary>
        /// <param name="filename">Name of file to read the SnpItems from using a SnpReader created for it</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>Returns a single sparse sequence containing Snp items from a single contiguous
        /// chromosome read from the file. The SnpItems retain the same position in 
        /// the sequence as the chromosome position.</returns>
        public ISequence ParseOne(string filename, bool isReadOnly)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                return ParseOne(reader, isReadOnly);
            }
        }

        /// <summary>
        /// Returns a sparse sequence composed of one of the two alleles for the current 
        /// chromosome pointed to by the SnpReader. If ParseAlleleOne is set to true (default), 
        /// this parses AlleleOne, else AlleleTwo.
        /// 
        /// The Sequence Items correspond to AlleleOne or Two in the reader and their position 
        /// in the sparse sequence corresponds to their Position in the SnpItem.
        /// 
        /// This starts parsing from the current line and continues until EOF or the chromosome 
        /// number changes.
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Returns a SparseSequence containing Snp items from the first contiguous 
        /// chromosome number read from the reader, retaining the same position in the sparse sequence
        /// as the chromosome position.</returns>
        public ISequence ParseOne(TextReader reader)
        {
            return ParseOne(reader, true);
        }

        /// <summary>
        /// Returns a sparse sequence composed of one of the two alleles for the current 
        /// chromosome pointed to by the SnpReader. If ParseAlleleOne is set to true (default), 
        /// this parses AlleleOne, else AlleleTwo.
        /// 
        /// The Sequence Items correspond to AlleleOne or Two in the reader and their position 
        /// in the sparse sequence corresponds to their Position in the SnpItem.
        /// 
        /// This starts parsing from the current line and continues until EOF or the chromosome 
        /// number changes.
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>Returns a SparseSequence containing Snp items from the first contiguous 
        /// chromosome number read from the reader, retaining the same position in the sparse sequence
        /// as the chromosome position.</returns>
        public ISequence ParseOne(TextReader reader, bool isReadOnly)
        {
            ISnpReader snpReader = GetSnpReader(reader);
            snpReader.MoveNext();
            return ParseOne(snpReader, isReadOnly);
        }

        #endregion Methods implementing ISequenceParser


        #region Abstract Methods introduced by SnpParser

        /// <summary>
        /// This method has to be overriden by an implementing class to return a SnpReader 
        /// for the given TextReader
        /// </summary>
        /// <param name="reader">TextReader containing snps that is wrapped by the returned ISnpReader</param>
        /// <returns></returns>
        protected abstract ISnpReader GetSnpReader(TextReader reader);

        #endregion Abstract Methods introduced by SnpParser


        #region Protected Methods of SnpParser
        /// <summary>
        /// The common ParseOne method called for parsing SNPs
        /// NOTE: The snpReader.MoveNext must have already been called and 
        /// the ISnpReader.Current have the first SnpItem to parse into the sequence
        /// </summary>
        /// <param name="snpReader">The ISnpReader to read a Snp chromosome sequence from</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>Returns a SparseSequence containing Snp items from the first contiguous 
        /// chromosome number read from the snp reader.</returns>
        protected ISequence ParseOne(ISnpReader snpReader, bool isReadOnly)
        {
            if (snpReader == null)
            {
                throw new ArgumentNullException("snpReader");
            }

            if (snpReader.Current == null)
                return new SparseSequence(Alphabet)
                {
                    ID = "Empty"
                };

            int sequenceChromosome = snpReader.Current.Chromosome;
            SparseSequence sequence = new SparseSequence(Alphabet);
            sequence.ID = ("Chr" + sequenceChromosome);

            do
            {
                SnpItem snp = snpReader.Current;
                // increase the size of the sparse sequence
                if (sequence.Count <= snp.Position)
                    sequence.Count = snp.Position + 1;
                sequence[snp.Position] = ParseAlleleOne
                                             ? Alphabet.LookupBySymbol(snp.AlleleOne)
                                             : Alphabet.LookupBySymbol(snp.AlleleTwo);
            } while (snpReader.MoveNext() && snpReader.Current.Chromosome == sequenceChromosome);

            sequence.IsReadOnly = isReadOnly;
            return sequence;
        }

        #endregion Protected Methods of SnpParser

    }
}
