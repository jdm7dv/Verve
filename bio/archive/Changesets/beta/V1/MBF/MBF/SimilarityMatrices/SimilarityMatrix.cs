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
using System.Text;
using MBF.Encoding;
using MBF.SimilarityMatrices.Resources;
using MBF.Util.Logging;

namespace MBF.SimilarityMatrices
{
    /// <summary>
    /// Representation of a matrix that contains similarity scores for every 
    /// pair of symbols in an alphabet. BLOSUM and PAM are well-known examples.
    /// </summary>
    public class SimilarityMatrix
    {
        #region Member variables

        /// <summary>
        /// Gap character used in aligned sequence strings.
        /// </summary>
        protected const char GapChar = '-';

        /// <summary>
        /// Value used in the similarity matrix and alignment codes when a new gap is created.
        /// </summary>
        protected const byte GapCode = 255;

        /// <summary>
        /// Encoding that maps the symbols and ordering used in the similarity matrix to the symbols used in the sequences.
        /// </summary>
        private IEncoding _encoding;

        /// <summary>
        /// Array containing the scores for each pair of symbols.
        /// The indices of the array are byte values of alphabet symbols.
        /// The byte values are obtained based on the encoding used.
        /// </summary>
        private int[][] _similarityMatrix;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SimilarityMatrix class
        /// </summary>
        public SimilarityMatrix()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SimilarityMatrix class
        /// Constructs one of the standard similarity matrices.
        /// </summary>
        /// <param name="matrixId">
        /// Matrix to load, BLOSUM and PAM currently supported.
        /// The enum StandardSimilarityMatrices contains list of available matrices.
        /// </param>
        public SimilarityMatrix(StandardSimilarityMatrix matrixId)
        {
            // MoleculeType.Protein for BLOSUM and PAM series supported matrices
            MoleculeType moleculeType = MoleculeType.Protein;
            string matrixText = null;

            switch (matrixId)
            {
                case StandardSimilarityMatrix.Blosum45:
                    matrixText = SimilarityMatrixResources.Blosum45;
                    break;
                case StandardSimilarityMatrix.Blosum50:
                    matrixText = SimilarityMatrixResources.Blosum50;
                    break;
                case StandardSimilarityMatrix.Blosum62:
                    matrixText = SimilarityMatrixResources.Blosum62;
                    break;
                case StandardSimilarityMatrix.Blosum80:
                    matrixText = SimilarityMatrixResources.Blosum80;
                    break;
                case StandardSimilarityMatrix.Blosum90:
                    matrixText = SimilarityMatrixResources.Blosum90;
                    break;
                case StandardSimilarityMatrix.Pam250:
                    matrixText = SimilarityMatrixResources.Pam250;
                    break;
                case StandardSimilarityMatrix.Pam30:
                    matrixText = SimilarityMatrixResources.Pam30;
                    break;
                case StandardSimilarityMatrix.Pam70:
                    matrixText = SimilarityMatrixResources.Pam70;
                    break;
                case StandardSimilarityMatrix.AmbiguousDna:
                    matrixText = SimilarityMatrixResources.AmbiguousDna;
                    moleculeType = MoleculeType.DNA;
                    break;
                case StandardSimilarityMatrix.AmbiguousRna:
                    matrixText = SimilarityMatrixResources.AmbiguousRna;
                    moleculeType = MoleculeType.RNA;
                    break;
                case StandardSimilarityMatrix.DiagonalScoreMatrix:
                    matrixText = SimilarityMatrixResources.DiagonalScoreMatrix;
                    break;
            }

            using (TextReader reader = new StringReader(matrixText))
            {
                LoadFromStream(reader, moleculeType);
            }
        }

        /// <remarks>
        /// File or stream format:
        /// There are two slightly different formats.
        /// <para>
        /// For custom similarity matrices:
        /// First line is descriptive name, will be stored as a string.
        /// Second line define the molecule type.  Must be "DNA", "RNA", or "Protein".
        /// Third line is alphabet (symbol map). It contains n characters and optional white space.
        /// Following lines are values for each row of matrix
        /// Must be n numbers per line, n lines
        /// </para>
        /// <para>
        /// In some cases the molecule type is implicit in the matrix.  This is true for the
        /// supported standard matrices (BLOSUM and PAM series at this point), and for the standard
        /// encodings IUPACna, NCBIA2na, NCBI2na, NCBI4na, and NCBIeaa.
        /// For these cases:
        /// First line is descriptive name, will be stored as a string.
        /// Second line is the encoding name for the standard encodings (IUPACna, NCBIA2na, NCBI2na, NCBI4na, or NCBIeaa)
        ///     or the alphabet (symbol map) for the standard matrices.
        /// Following lines are values for each row of matrix
        /// Must be n numbers per line, n lines; or in the case of the supported encoding, sufficient
        /// entries to handle all possible indices (0 through max index value).
        /// </para>
        /// </remarks>
        /// <summary>
        /// Initializes a new instance of the SimilarityMatrix class.
        /// Constructs SimilarityMatrix from an input stream.
        /// </summary>
        /// <param name="reader">Text reader associated with the input sequence stream</param>
        public SimilarityMatrix(TextReader reader)
        {
            LoadFromStream(reader, MoleculeType.NA);
        }

        /// <summary>
        /// Initializes a new instance of the SimilarityMatrix class
        /// Constructs SimilarityMatrix from an input file.
        /// </summary>
        /// <param name="fileName">File name of input sequence</param>
        public SimilarityMatrix(string fileName)
        {
            using (TextReader reader = new StreamReader(fileName))
            {
                LoadFromStream(reader, MoleculeType.NA);
            }
        }

        /// <summary>
        /// Initializes a new instance of the SimilarityMatrix class.
        /// Constructs a custom SimilarityMatrix.
        /// </summary>
        /// <param name="similarityMatrix">2-d array containing the correlation scoring matrix.</param>
        /// <param name="symbolMap">Symbols (alphabet) associated with the array.</param>
        /// <param name="name">Description of the custom matrix.</param>
        /// <param name="moleculeType">Type of molecule for which this matrix is designed.  Must be DNA, RNA or Protein.</param>
        /// <remarks>
        /// The array must be NxN, where there are N symbols in the symbolMap.
        /// The ordering of the symbolMap must match the ordering in the array.
        /// </remarks>
        public SimilarityMatrix(int[][] similarityMatrix, string symbolMap, string name, MoleculeType moleculeType)
        {
            Name = name;
            _encoding = new BasicSmEncoding(symbolMap, name, moleculeType);
            _similarityMatrix = similarityMatrix;
        }
        #endregion

        #region Nested enums

        /// <summary>
        /// List of available standard similarity matrices.
        /// </summary>
        /// <remarks>
        /// BLOSUM matrices reference:
        /// S Henikoff and J G Henikoff,
        /// "Amino acid substitution matrices from protein blocks."
        /// Proc Natl Acad Sci U S A. 1992 November 15; 89(22): 10915–10919.  PMCID: PMC50453
        /// <para>
        /// Available at:
        /// <![CDATA[http://www.pubmedcentral.nih.gov/articlerender.fcgi?tool=EBI&pubmedid=1438297]]>
        /// </para>
        /// <para>
        /// PAM matrices reference:
        /// Dayhoff, M.O., Schwartz, R. and Orcutt, B.C. (1978), 
        /// "A model of Evolutionary Change in Proteins", 
        /// Atlas of protein sequence and structure (volume 5, supplement 3 ed.), 
        /// Nat. Biomed. Res. Found., p. 345-358, ISBN 0912466073
        /// </para>
        /// </remarks>
        public enum StandardSimilarityMatrix
        {
            /// <summary>
            /// BLOSUM45 Similarity Matrix
            /// </summary>
            Blosum45,

            /// <summary>
            /// BLOSUM50 Similarity Matrix
            /// </summary>
            Blosum50,

            /// <summary>
            /// BLOSUM62 Similarity Matrix
            /// </summary>
            Blosum62,

            /// <summary>
            /// BLOSUM80 Similarity Matrix
            /// </summary>
            Blosum80,

            /// <summary>
            /// BLOSUM90 Similarity Matrix
            /// </summary>
            Blosum90,

            /// <summary>
            /// PAM250 Similarity Matrix
            /// </summary>
            Pam250,

            /// <summary>
            /// PAM30 Similarity Matrix
            /// </summary>
            Pam30,

            /// <summary>
            /// PAM70 Similarity Matrix
            /// </summary>
            Pam70,

            /// <summary>
            /// Simple DNA Similarity Matrix
            /// </summary>
            AmbiguousDna,

            /// <summary>
            /// RNA with ambiguous
            /// </summary>
            AmbiguousRna,

            /// <summary>
            /// Diagonal matrix
            /// </summary>
            DiagonalScoreMatrix
        }

        #endregion

        #region Properties
        /// <summary> 
        /// Gets or sets descriptive name of the particular SimilarityMatrix being used. 
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets similarity matrix values in a 2-D integer array.
        /// </summary>
        public int[][] Matrix
        {
            get
            {
                return _similarityMatrix;
            }

            protected set
            {
                _similarityMatrix = value;
            }
        }

        /// <summary>
        /// Gets or sets value of encoding
        /// </summary>
        protected IEncoding MatrixEncoding
        {
            get
            {
                return _encoding;
            }

            set
            {
                _encoding = value;
            }
        }

        /// <summary>
        /// Gets or sets molecule type for matrix.
        /// DNA, RNA, NA or Protein; used to determine whether encoding items are AminoAcid or Nucleotide
        /// </summary>
        public MoleculeType MatrixMoleculeType { get; protected set; }
                    
        #endregion

        #region Methods
        /// <summary>
        /// Returns value of matrix at [row, col].
        /// </summary>
        /// <param name="row">
        /// Row number. This is same as byte value
        /// corresponding to sequence symbol on the row
        /// </param>
        /// <param name="col">
        /// Column number. This is same as byte value
        /// corresponding to sequence symbol on the column
        /// </param>
        /// <returns>Score value of matrix at [row, col]</returns>
        public virtual int this[int row, int col]
        {
            // Override this method for any "matrix" that is function based 
            // instead of implemented with an actual matrix.
            get
            {
                return _similarityMatrix[row][col];
            }
        }

        /// <summary>
        /// Returns value of matrix at row, column corresponding to input ISequenceItems.
        /// </summary>
        /// <param name="rowItem">ISequenceItem on the row</param>
        /// <param name="colItem">ISequenceItem on the column</param>
        /// <returns>Score at matrix[row, col]</returns>
        public virtual int this[ISequenceItem rowItem, ISequenceItem colItem]
        {
            // Override this method for any "matrix" that is function based 
            // instead of implemented with an actual matrix.
            get
            {
                int row = _encoding.LookupBySymbol(rowItem.Symbol).Value;
                int col = _encoding.LookupBySymbol(colItem.Symbol).Value;
                return _similarityMatrix[row][col];
            }
        }

        /// <summary>
        /// Confirms that there is a symbol in the similarity matrix for every
        /// symbol in the sequence.
        /// </summary>
        /// <param name="sequence">Sequence to validate.</param>
        /// <returns>true if sequence is valid.</returns>
        public bool ValidateSequence(ISequence sequence)
        {
            foreach (ISequenceItem item in sequence)
            {
                if (!_encoding.Contains(item)) 
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Converts sequence string to byte array for use by alignment engines.
        /// Mapping from character to integer uses index ordering associated with matrix.
        /// </summary>
        /// <param name="sequence">String of sequence symbols</param>
        /// <returns>Byte array representation of input sequence</returns>
        public byte[] ToByteArray(string sequence)
        {
            byte[] seq = new byte[sequence.Length];
            int i = 0;
            foreach (char c in sequence)
            {
                seq[i] = _encoding.LookupBySymbol(c).Value;
                i++;
            }

            return seq;
        }

        /// <summary>
        /// Converts character symbol of SequenceItem to byte for use by alignment engines.
        /// </summary>
        /// <param name="symbol">character symbol of SequenceItem</param>
        /// <returns>Byte representation of input symbol</returns>
        public byte ToByte(char symbol)
        {
            return _encoding.LookupBySymbol(symbol).Value;
        }

        /// <summary>
        /// Converts sequence as integer array to string using SimilarityMatrix symbol map.
        /// </summary>
        /// <param name="array">Byte array corresponding to sequence</param>
        /// <returns>String representation of sequence</returns>
        public string ToString(byte[] array)
        {
            StringBuilder s = new StringBuilder();
            foreach (byte i in array)
            {
                char c;
                if (i == GapCode)
                {
                    c = GapChar;
                }
                else
                {
                    c = _encoding.LookupByValue(i).Symbol;
                }

                s.Append(c);
            }

            return s.ToString();
        }

        /// <summary>
        /// Reads similarity matrix from a stream.  File (or stream) format defined
        /// above with constructors to create SimilarityMatrix from stream or file.
        /// </summary>
        /// <param name="reader">Text reader associated with the input sequence stream</param>
        /// <param name="moleculeType">Molecule type supported by SimilarityMatrix</param>
        private void LoadFromStream(TextReader reader, MoleculeType moleculeType)
        {
            char[] delimiters = { '\t', ' ', ',' }; // basic white space, comma, can add more items later if necessary

            Name = reader.ReadLine();
            if (String.IsNullOrEmpty(Name))
            {
                string message = Properties.Resource.SimilarityMatrix_NameMissing;
                Trace.Report(message);
                throw new InvalidDataException(message);
            }

            // Second line is the alphabet ot the alphabet code
            // Valid codes are: IUPACna, NCBIA2na, NCBI2na, NCBI4na, and NCBIeaa, case insensitive.
            //                                0          1          2          3            4
            string[] alphabetCodes = { "IUPACna", "NCBI2na", "NCBI4na", "NCBIeaa", "NCBIstdaa" };

            string line = reader.ReadLine();
            if (String.IsNullOrEmpty(line))
            {
                string message = Properties.Resource.SimilarityMatrix_SecondLineMissing;
                Trace.Report(message);
                throw new InvalidDataException(message);
            }

            // If second line is a supported mapping, we can set molecule type here and do not have to read an alphabet.
            // If the second line is Protein, DNA or RNA, we can set molecule type here and will have to read the alphabet from the third line.
            // If it is a standard supported mapping (BLOSUM and PAM at this point), molecule type is Protein and the second line is the alphabet.

            // First find out if the second line is one of the supported encodings.
            string secondLine = line.ToUpper(CultureInfo.InvariantCulture).Trim();
            bool hasSupportedEncoding = false;
            int codeIndex;
            for (codeIndex = 0; codeIndex < alphabetCodes.Length; codeIndex++)
            {
                if (alphabetCodes[codeIndex].ToUpper(CultureInfo.InvariantCulture) == secondLine)
                {
                    hasSupportedEncoding = true;
                    break;
                }
            }

            string alphabetLine;

            if (moleculeType != MoleculeType.NA)
            {
                MatrixMoleculeType = moleculeType;
                alphabetLine = secondLine;
            }
            else if (hasSupportedEncoding)
            {
                alphabetLine = null;

                // do not need to read alphabet, it's implicit in the encoding
                // will set molecule type below when we know which encoding it is
            }
            else
            {
                // must be molecule type, case is not significant
                if (secondLine == "DNA")
                {
                    MatrixMoleculeType = MoleculeType.DNA;
                }
                else if (secondLine == "RNA")
                {
                    MatrixMoleculeType = MoleculeType.RNA;
                }
                else if (secondLine == "PROTEIN")
                {
                    MatrixMoleculeType = MoleculeType.Protein;
                }
                else
                {
                    string message = String.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resource.SimilarityMatrix_InvalidMoleculeType,
                            secondLine);
                    Trace.Report(message);
                    throw new InvalidDataException(message);
                }

                // Third line will be the alphabet, read it now.
                alphabetLine = reader.ReadLine().ToUpper(CultureInfo.InvariantCulture).Trim();
            }

            // We have read the two or three line header, including the alphabet if required.
            int symbolCount = 0;  // number of symbols in alphabet map
            if (hasSupportedEncoding)
            {
                // We have a valid encoding
                switch (codeIndex)
                {
                    case 0:
                        _encoding = Encodings.IupacNA;
                        MatrixMoleculeType = MoleculeType.NA;  // could be DNA or RNA
                        break;
                    case 1:
                        _encoding = Encodings.Ncbi2NA;
                        MatrixMoleculeType = MoleculeType.NA;  // could be DNA or RNA
                        break;
                    case 2:
                        _encoding = Encodings.Ncbi4NA;
                        MatrixMoleculeType = MoleculeType.NA;  // could be DNA or RNA
                        break;
                    case 3:
                        _encoding = Encodings.NcbiEAA;
                        MatrixMoleculeType = MoleculeType.Protein;
                        break;
                    case 4:
                        _encoding = Encodings.NcbiStdAA;
                        MatrixMoleculeType = MoleculeType.Protein;
                        break;
                }

                // We need the maximum value of a symbol so we know how many rows and cols we need in the similarity matrix
                byte valueMax = 0;
                List<ISequenceItem> list = _encoding.ToList<ISequenceItem>();
                foreach (ISequenceItem item in list)
                {
                    if (item.Value > valueMax)
                    {
                        valueMax = item.Value;
                    }
                }

                symbolCount = valueMax;
            }
            else
            {
                // We need to parse the alphabet line.
                string[] entries = alphabetLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                char[] holdChars = new char[entries.Length];
                foreach (string s in entries)
                {
                    holdChars[symbolCount] = s[0];
                    symbolCount++;
                }

                _encoding = new BasicSmEncoding(new string(holdChars), Name, MatrixMoleculeType);
            }

            // We have the symbol map, nSymbols symbols in it.  Matrix must be int[nSymbols,nSymbols]
            int rowCount = symbolCount; // number of rows in the matrix
            int columnCount = symbolCount; // number of columns in the matrix.
            int[][] similarityMatrix = new int[rowCount][];
            for (int x = 0; x < rowCount; x++)
                similarityMatrix[x] = new int[columnCount];
            int row, col; // row and col indices
            for (row = 0; row < rowCount; row++)
            {
                line = reader.ReadLine();
                if (line == null)
                {
                    string message = Properties.Resource.SimilarityMatrix_FewerMatrixLines;
                    Trace.Report(message);
                    throw new InvalidDataException(message);
                }

                string[] rowValues = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                for (col = 0; col < columnCount; col++)
                {
                    try
                    {
                        similarityMatrix[row][col] = Convert.ToInt32(rowValues[col], CultureInfo.InvariantCulture);  // here's where to catch a non-integer
                    }
                    catch (Exception e)
                    {
                        string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.SimilarityMatrix_BadOrMissingValue, 
                                line, 
                                e.Message);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                    }
                }
            }

            _similarityMatrix = similarityMatrix;
        }
        #endregion
    }
}
