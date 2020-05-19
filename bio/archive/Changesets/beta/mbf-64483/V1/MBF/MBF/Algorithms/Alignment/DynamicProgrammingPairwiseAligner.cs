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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBF.SimilarityMatrices;
using MBF.Util.Logging;

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// Base class for dynamic programming alignment algorithms, including NeedlemanWunsch, 
    /// SmithWaterman and PairwiseOverlap.
    /// The basic reference for this code (including NW, SW and Overlap) is Chapter 2 in 
    /// Biological Sequence Analysis; Durbin, Eddy, Krogh and Mitchison; Cambridge Press; 1998
    /// The variable names in these classes follow the notation in Durbin et al.
    /// </summary>
    public abstract class DynamicProgrammingPairwiseAligner : IPairwiseSequenceAligner
    {
        #region Member variables
        /// <summary>
        /// Signifies gap in aligned sequence (stored as int[]) during traceback.
        /// </summary>
        protected const byte GapCode = 255;  // Used in internal sequences.  ISequenceItem uses byte, so cannot use -1.

        /// <summary> Similarity matrix for use in alignment algorithms. </summary>
        protected SimilarityMatrix _similarityMatrix;

        /// <summary> 
        /// Gap open penalty for use in alignment algorithms. 
        /// For alignments using a linear gap penalty, this is the gap penalty.
        /// For alignments using an affine gap, this is the penalty to open a new gap.
        /// This is a negative number, for example _gapOpenCost = -8, not +8.
        /// </summary>
        protected int _gapOpenCost;

        /// <summary> 
        /// Gap extension penalty for use in alignment algorithms. 
        /// Not used for alignments using a linear gap penalty.
        /// For alignments using an affine gap, this is the penalty to extend an existing gap.
        /// This is a negative number, for example _gapExtensionCost = -2, not +2.
        /// </summary>
        protected int _gapExtensionCost;

        /// <remark>
        /// The F matrix holds the scores and source.  See Durbin et al. for details.
        /// The F matrix uses the first column and first row to store boundary condition.
        /// This means the the input sequence at, for example, col 4 will map to col 5 in the F matrix.
        /// Gotoh's optimization is used to reduce memory usage
        /// </remark>
        /// <summary>
        /// _FScore is the score for each cell, used for linear gap penalty.
        /// </summary>
        protected int[] _FScore;

        /// <remark>
        /// The F matrix holds the scores and source.  See Durbin et al. for details.
        /// The F matrix uses the first column and first row to store boundary condition.
        /// This means the the input sequence at, for example, col 4 will map to col 5 in the F matrix.
        /// </remark>
        /// <summary>
        /// _FScore is the score for each cell, used for single gap penalty.
        /// </summary>
        protected int[,] _FSimpleScore { get; set; }

        /// <summary>
        /// _FScoreDiagonal is used to store diagonal value from previous row.
        /// Used for Gotoh optimization of linear gap penalty
        /// </summary>
        protected int _FScoreDiagonal;

        // Note that the dynamic programming matrix is coded as separate arrays.  Structs allocate space on 4 byte
        // boundaries, leading to some memory inefficiency.

        /// <summary>
        /// _FSource stores the source for the each cell in the F matrix.
        /// Source is coded as 0 diagonal, 1 up, 2 left, see enum SourceDirection below
        /// </summary>
        protected sbyte[] _FSource; // source for cell

        /// <summary>
        /// _FSource stores the source for the each cell in the F matrix.
        /// Source is coded as 0 diagonal, 1 up, 2 left, see enum SourceDirection below
        /// </summary>
        protected sbyte[,] _FSimpleSource { get; set; } // source for cell

        /// <summary>
        /// _M stores the diagonal value for the affine gap penalty implementation.
        /// See Durbin et al. for details.
        /// </summary>
        protected int[,] _M { get; set; }

        /// <summary>
        /// _Ix stores the gap in x value for the affine gap penalty implementation.
        /// </summary>
        protected int[,] _Ix { get; set; }

        /// <summary>
        /// _Iy stores the gap in y value for the affine gap penalty implementation.
        /// </summary>
        protected int[,] _Iy { get; set; }

        /// <summary>
        /// _MaxScore stores the maximum value for the affine gap penalty implementation.
        /// </summary>
        protected int[] _MaxScore; // best score of alignment x1...xi to y1...yi

        /// <summary>
        /// _MaxScoreDiagonal is used to store maximum value from previous row.
        /// Used for Gotoh optimization of affine gap penalty
        /// </summary>
        protected int _MaxScoreDiagonal;

        /// <summary>
        /// Stores alignment score for putting gap in 'x' sequence for affine gap penalty implementation.
        /// Alignment score if xi aligns to a gap after yi
        /// </summary>
        protected int[] _IxGapScore;

        /// <summary>
        /// Stores alignment score for putting gap in 'y' sequence for affine gap penalty implementation.
        /// Alignment score if yi aligns to a gap after xi
        /// </summary>
        protected int _IyGapScore;

        /// <summary>
        /// Number of rows in the dynamic programming matrix.
        /// </summary>
        protected int _nRows;

        /// <summary>
        /// Number of columns in the dynamic programming matrix.
        /// </summary>
        protected int _nCols;

        /// <summary>
        /// First input sequence as byte arrays, for internal use.
        /// This is zero based, so use care when working with F matrix.
        /// <see cref="_FSource"/>.
        /// </summary>
        protected byte[] _a;

        /// <summary>
        /// second input sequence as byte arrays, for internal use.
        /// This is zero based, so use care when working with F matrix.
        /// <see cref="_FSource"/>.
        /// </summary>
        protected byte[] _b;

        /// <summary>
        /// Row size of the block of Matrix
        /// </summary>
        private int _rowBlockSize = 0;

        /// <summary>
        /// Column size of the block of Matrix
        /// </summary>
        private int _colBlockSize = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the DynamicProgrammingPairwiseAligner class.
        /// Constructor for all the pairwise aligner (NeedlemanWunsch, SmithWaterman, Overlap).
        /// Sets default similarity matrix and gap penalties.
        /// Users will typically reset these using parameters specific to their particular sequences and needs.
        /// </summary>
        protected DynamicProgrammingPairwiseAligner()
        {
            // Set default similarity matrix and gap penalty.
            // User will typically choose their own parameters, these defaults are reasonable for many cases.
            // Molecule type is set to protein, since this will also work for DNA and RNA in the
            // special case of a diagonal similarity matrix.
            _similarityMatrix = new DiagonalSimilarityMatrix(2, -2, MoleculeType.Protein);
            _gapOpenCost = -8;
            _gapExtensionCost = -1;
        }

        #endregion

        #region Properties

        /// <summary> Gets or sets similarity matrix for use in alignment algorithms. </summary>
        public SimilarityMatrix SimilarityMatrix
        {
            get { return _similarityMatrix; }
            set { _similarityMatrix = value; }
        }

        /// <summary> 
        /// Gets or sets gap open penalty for use in alignment algorithms. 
        /// For alignments using a linear gap penalty, this is the gap penalty.
        /// For alignments using an affine gap, this is the penalty to open a new gap.
        /// This is a negative number, for example GapOpenCost = -8, not +8.
        /// </summary>
        public int GapOpenCost
        {
            get { return _gapOpenCost; }
            set { _gapOpenCost = value; }
        }

        /// <summary> 
        /// Gets or sets gap extension penalty for use in alignment algorithms. 
        /// Not used for alignments using a linear gap penalty.
        /// For alignments using an affine gap, this is the penalty to extend an existing gap.
        /// This is a negative number, for example GapExtensionCost = -2, not +2.
        /// </summary>
        public int GapExtensionCost
        {
            get { return _gapExtensionCost; }
            set { _gapExtensionCost = value; }
        }

        /// <summary>
        /// Gets or sets the object that will be used to compute the alignment's consensus.
        /// </summary>
        public IConsensusResolver ConsensusResolver { get; set; }

        /// <summary>
        /// Gets the name of the Aligner. Intended to be filled in 
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact name of the Alignment algorithm.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of the Aligner. Intended to be filled in 
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact details of the Alignment algorithm.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets or sets an value indicating whether to use parallelization using 
        /// EARTH technique to fill the Simple and Affine matrices.
        /// Reference: http://helix-web.stanford.edu/psb01/martins.pdf
        /// Steps: 
        ///  1. Calculate the number of rows and column for the smaller blocks of matrix.
        ///  2. Start the task to build the block of matrix for which all the dependent blocks are available.
        ///   2.1 Fill the matrix in the block.
        ///   2.2 Repeat step (2) for the rest of block in both vertical and horizontal direction.
        /// Assuming there are 3 * 3 block in Matrix
        /// Following shows the progress of blocks in Matrix in each iteration
        /// "+" block completed
        /// "-" block in progress (parallelly in tasks)
        /// Fill Matrix till Anti-Diagonal
        /// 
        /// Iteration 1
        /// -
        /// 
        /// Iteration 2
        /// + -
        /// -
        /// 
        /// Iteration 3
        /// + + -
        /// + -
        /// -
        /// 
        /// Iteration 4
        /// + + +
        /// + + -
        /// + -
        /// 
        /// Iteration 5
        /// + + +
        /// + + +
        /// + + -
        /// </summary>
        public bool UseEARTHToFillMatrix { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Aligns two sequences using a linear gap parameter, using existing gap open cost and similarity matrix.
        /// Set these using GapOpenCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="sequence1">First input sequence.</param>
        /// <param name="sequence2">Second input sequence.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(ISequence sequence1, ISequence sequence2)
        {
            return AlignSimple(_similarityMatrix, _gapOpenCost, sequence1, sequence2);
        }

        /// <summary>
        /// Aligns two sequences using a linear gap parameter, using existing gap open cost and similarity matrix.
        /// Set these using GapOpenCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(IList<ISequence> inputSequences)
        {
            if (inputSequences.Count != 2)
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.PairwiseAlignerWrongArgumentCount,
                        inputSequences.Count);
                Trace.Report(message);
                throw new ArgumentException(message, "inputSequences");
            }

            return AlignSimple(_similarityMatrix, _gapOpenCost, inputSequences[0], inputSequences[1]);
        }

        /// <summary>
        /// Aligns two sequences using a linear gap parameter, using existing gap open cost and similarity matrix.
        /// Set these using GapOpenCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        IList<ISequenceAlignment> ISequenceAligner.AlignSimple(IList<ISequence> inputSequences)
        {
            return this.AlignSimple(inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Aligns two sequences using the affine gap metric, a gap open penalty and a gap extension penalty.
        /// This method uses the existing gap open and extension penalties and similarity matrix.
        /// Set these using GapOpenCost, GapExtensionCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="sequence1">First input sequence.</param>
        /// <param name="sequence2">Second input sequence.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> Align(ISequence sequence1, ISequence sequence2)
        {
            return Align(_similarityMatrix, _gapOpenCost, _gapExtensionCost, sequence1, sequence2);
        }

        /// <summary>
        /// Aligns two sequences using the affine gap metric, a gap open penalty and a gap extension penalty.
        /// This method uses the existing gap open and extension penalties and similarity matrix.
        /// Set these using GapOpenCost, GapExtensionCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> Align(IList<ISequence> inputSequences)
        {
            if (inputSequences.Count != 2)
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.PairwiseAlignerWrongArgumentCount,
                        inputSequences.Count);
                Trace.Report(message);
                throw new ArgumentException(message, "inputSequences");
            }

            return Align(_similarityMatrix, _gapOpenCost, _gapExtensionCost, inputSequences[0], inputSequences[1]);
        }

        /// <summary>
        /// Aligns two sequences using the affine gap metric, a gap open penalty and a gap extension penalty.
        /// This method uses the existing gap open and extension penalties and similarity matrix.
        /// Set these using GapOpenCost, GapExtensionCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        IList<ISequenceAlignment> ISequenceAligner.Align(IList<ISequence> inputSequences)
        {
            return this.Align(inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Pairwise alignment of two sequences using a linear gap penalty.  The various algorithms in derived classes (NeedlemanWunsch, 
        /// SmithWaterman, and PairwiseOverlap) all use this general engine for alignment with a linear gap penalty.
        /// </summary>
        /// <param name="similarityMatrix">Scoring matrix.</param>
        /// <param name="gapPenalty">Gap penalty (by convention, use a negative number for this.)</param>
        /// <param name="aInput">First input sequence.</param>
        /// <param name="bInput">Second input sequence.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(SimilarityMatrix similarityMatrix, int gapPenalty, ISequence aInput, ISequence bInput)
        {
            // Initialize and perform validations for simple alignment
            SimpleAlignPrimer(similarityMatrix, gapPenalty, aInput, bInput);

            FillMatrixSimple();

            List<byte[]> alignedSequences;
            List<int> offsets;
            List<int> endOffsets;
            List<int> insertions;
            List<int> startOffsets;
            int optScore = Traceback(out alignedSequences, out offsets, out startOffsets, out endOffsets, out insertions);
            return CollateResults(aInput, bInput, alignedSequences, offsets, optScore, startOffsets, endOffsets, insertions);
        }

        /// <summary>
        /// Pairwise alignment of two sequences using an affine gap penalty.  The various algorithms in derived classes (NeedlemanWunsch, 
        /// SmithWaterman, and PairwiseOverlap) all use this general engine for alignment with an affine gap penalty.
        /// </summary>
        /// <param name="similarityMatrix">Scoring matrix.</param>
        /// <param name="gapOpenPenalty">Gap open penalty (by convention, use a negative number for this.)</param>
        /// <param name="gapExtensionPenalty">Gap extension penalty (by convention, use a negative number for this.)</param>
        /// <param name="aInput">First input sequence.</param>
        /// <param name="bInput">Second input sequence.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> Align(
            SimilarityMatrix similarityMatrix,
            int gapOpenPenalty,
            int gapExtensionPenalty,
            ISequence aInput,
            ISequence bInput)
        {
            // Initialize and perform validations for alignment
            // In addition, initialize gap extension penalty.
            SimpleAlignPrimer(similarityMatrix, gapOpenPenalty, aInput, bInput);
            _gapExtensionCost = gapExtensionPenalty;

            FillMatrixAffine();

            List<byte[]> alignedSequences;
            List<int> offsets;
            List<int> startOffsets;
            List<int> endOffsets;
            List<int> insertions;
            int optScore = Traceback(out alignedSequences, out offsets, out startOffsets, out endOffsets, out insertions);
            return CollateResults(aInput, bInput, alignedSequences, offsets, optScore, startOffsets, endOffsets, insertions);
        }

        /// <summary>
        /// Return the starting position of alignment of sequence1 with respect to sequence2.
        /// </summary>
        /// <param name="aligned">Aligned sequence.</param>
        /// <returns>The number of initial gap characters.</returns>
        protected static int GetOffset(byte[] aligned)
        {
            int ret = 0;
            foreach (byte item in aligned)
            {
                if (item != GapCode)
                {
                    return ret;
                }

                ++ret;
            }

            return ret;
        }

        // These routines will be different for the various variations --SmithWaterman, NeedlemanWunsch, etc.
        // Additonal variations can be built by modifying these functions.

        /// <summary>
        /// Sets cell (col,row) of the F matrix.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected abstract void FillCellSimple(int col, int row);

        /// <summary>
        /// Sets cell (row,col) of the F matrix.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// Uses linear gap penalty.
        /// </summary>
        /// <param name="row">row of cell to fill</param>
        /// <param name="col">col of cell to fill</param>
        /// <param name="cell">cell number</param>
        protected abstract void FillCellSimple(int row, int col, int cell);

        /// <summary>
        /// Resets member variables that are unique to a specific algorithm.
        /// These must be reset for each alignment, initialization in the constructor
        /// only works for the first call to AlignSimple.  This routine is called at the beginning
        /// of each AlignSimple method.
        /// </summary>
        protected abstract void ResetSpecificAlgorithmMemberVariables();

        /// <summary>
        /// Allows each algorithm to set optimal score at end of matrix construction
        /// Used for linear gap penalty
        /// </summary>
        protected abstract void SetOptimalScoreSimple();

        /// <summary>
        /// Allows each algorithm to set optimal score at end of matrix construction
        /// Used for affine gap penalty
        /// </summary>
        protected abstract void SetOptimalScoreAffine();

        /// <summary>
        /// Sets cell (col,row) of the matrix for affine gap implementation.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected abstract void FillCellAffine(int col, int row);

        /// <summary>
        /// Sets cell (row,col) of the matrix for affine gap implementation.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">row of cell to fill</param>
        /// <param name="col">col of cell to fill</param>
        /// <param name="cell">cell number</param>
        protected abstract void FillCellAffine(int row, int col, int cell);

        /// <summary>
        /// Sets boundary conditions in the F matrix for the one gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected abstract void SetBoundaryConditionSimple(int col, int row);

        /// <summary>
        /// Sets boundary conditions for first row in F matrix for linear gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        protected abstract void SetRowBoundaryConditionSimple();

        /// <summary>
        /// Sets boundary conditions for zeroth column in F matrix for linear gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">Cell number</param>
        protected abstract void SetColumnBoundaryConditionSimple(int row, int cell);

        /// <summary>
        /// Sets boundary conditions for the dynamic programming matrix for the affine gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected abstract void SetBoundaryConditionAffine(int col, int row);

        /// <summary>
        /// Sets boundary conditions for first row in dynamic programming matrix for affine gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        protected abstract void SetRowBoundaryConditionAffine();

        /// <summary>
        /// Sets boundary conditions for the zeroth column in dynamic programming 
        /// matrix for affine gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">Cell number</param>
        protected abstract void SetColumnBoundaryConditionAffine(int row, int cell);

        /// <summary>
        /// Performs traceback step for the relevant algorithm.  Each algorithm must override this
        /// since the traceback differs for the different algorithms.
        /// </summary>
        /// <param name="alignedSequences">List of aligned sequences (output)</param>
        /// <param name="offsets">Offset is the starting position of alignment 
        /// of sequence1 with respect to sequence2.</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insetions made to the aligned sequences.</param>
        /// <returns>Optimum score for this alignment</returns>
        protected abstract int Traceback(out List<byte[]> alignedSequences, out List<int> offsets, out List<int> startOffsets, out List<int> endOffsets, out List<int> insertions); // perform algorithm's traceback step

        /// <summary>
        /// Fills F matrix for linear gap penalty implementation.
        /// </summary>
        protected virtual void FillMatrixSimple()
        {
            _nCols = _a.Length + 1;
            _nRows = _b.Length + 1;

            try
            {
                if (UseEARTHToFillMatrix)
                {
                    _FSimpleScore = new int[_nCols, _nRows];
                    _FSimpleSource = new sbyte[_nCols, _nRows];
                    _M = null;  // affine matrices not used for simple model
                    _Ix = null;
                    _Iy = null;
                }
                else
                {
                    _FScore = new int[_nCols];
                    try
                    {
                        long size = (long)_nRows * (long)_nCols;
                        _FSource = new sbyte[size];
                    }
                    catch (ArithmeticException)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.SequenceLengthExceedsLimit,
                                _nRows,
                                _nCols,
                                int.MaxValue));
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                string msg = BuildOutOfMemoryMessage(ex, false);
                Trace.Report(msg);
                throw new OutOfMemoryException(msg, ex);
            }

            // Fill by columns
            int row, col, cell;

            if (UseEARTHToFillMatrix)
            {
                // Calculate row & col size for each block of Matrix
                CalculateOptimalBlockSize();

                if (_rowBlockSize < 1)
                {
                    _rowBlockSize = 1;
                }

                if (_colBlockSize < 1)
                {
                    _colBlockSize = 1;
                }

                int maxCol = 0, initialRow, initialCol;
                IList<Task> simpleMatrixTasks = new List<Task>();

                // Fill the Matrix in blocks
                // Assuming there are 3 * 3 block in Matrix
                // Following shows the progress of blocks in Matrix in each iteration
                // "+" block completed
                // "-" block in progress (parallelly in tasks)

                /* 
                 * Fill Matrix till Anti-Diagonal
                 * Iteration 1
                 * -
                 * 
                 * Iteration 2
                 * + -
                 * -
                 * 
                 * Iteration 3
                 * + + -
                 * + -
                 * -
                 */
                col = 0;
                row = 0;
                while (row < _nRows || col < _nCols)
                {
                    initialRow = 0;
                    initialCol = col;

                    while (initialCol >= 0)
                    {
                        int taskRow = initialRow;
                        int taskCol = initialCol;

                        simpleMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixSimpleBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        if (initialCol > maxCol)
                        {
                            maxCol = initialCol;
                        }

                        initialRow += _rowBlockSize;
                        initialCol -= _colBlockSize;

                        if (initialRow > _nRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(simpleMatrixTasks.ToArray());
                    simpleMatrixTasks.Clear();

                    row += _rowBlockSize;
                    col += _colBlockSize;
                }

                /* 
                 * Fill the Matrix after Anti-Diagonal
                 * Iteration 1 
                 * + + +
                 * + + -
                 * + -
                 * 
                 * Iteration 2 
                 * + + +
                 * + + +
                 * + + -
                 */
                col = _nCols;
                row = _rowBlockSize;
                while (row < _nRows || col >= 0)
                {
                    initialRow = row;
                    initialCol = maxCol;

                    while (initialCol >= 0)
                    {
                        int taskRow = initialRow;
                        int taskCol = initialCol;

                        simpleMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixSimpleBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        initialRow += _rowBlockSize;
                        initialCol -= _colBlockSize;

                        if (initialRow > _nRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(simpleMatrixTasks.ToArray());
                    simpleMatrixTasks.Clear();

                    row += _rowBlockSize;
                    col -= _colBlockSize;

                    if (row > _nRows)
                    {
                        break;
                    }
                }

                SetOptimalScoreSimple();
            }
            else
            {
                // Set matrix bc along top row and left column.
                SetRowBoundaryConditionSimple();

                for (row = 1, cell = _nCols; row < _nRows; row++)
                {
                    SetColumnBoundaryConditionSimple(row, cell);
                    cell++;
                    for (col = 1; col < _nCols; col++, cell++)
                    {
                        FillCellSimple(row, col, cell);
                    }
                }

                SetOptimalScoreSimple();
            }
        }

        /// <summary>
        /// Sets cell (initialRow, initialColumn) to 
        /// (initialRow + _rowBlockSize, initialColumn + _colBlockSize) of the F matrix
        /// </summary>
        /// <param name="initialRow">Intial row in block.</param>
        /// <param name="initialColumn">Intial column in block.</param>
        private void FillMatrixSimpleBlock(int initialRow, int initialColumn)
        {
            for (int col = initialColumn; col < _nCols && col < initialColumn + _colBlockSize; col++)
            {
                for (int row = initialRow; row < _nRows && row < initialRow + _rowBlockSize; row++)
                {
                    if (row == 0 || col == 0)
                    {
                        // Set matrix bc along top row and left column.
                        SetBoundaryConditionSimple(col, row);

                    }
                    else
                    {
                        FillCellSimple(col, row);
                    }
                }
            }
        }

        /// <summary>
        /// Fills matrix data for affine gap penalty implementation.
        /// </summary>
        protected virtual void FillMatrixAffine()
        {
            _nCols = _a.Length + 1;
            _nRows = _b.Length + 1;
            try
            {
                if (UseEARTHToFillMatrix)
                {
                    _FSimpleScore = null; // not used for affine model
                    _FSimpleSource = new sbyte[_nCols, _nRows];
                    _M = new int[_nCols, _nRows];
                    _Ix = new int[_nCols, _nRows];
                    _Iy = new int[_nCols, _nRows];
                }
                else
                {
                    _IxGapScore = new int[_nCols];
                    _MaxScore = new int[_nCols];
                    try
                    {
                        long size = (long)_nRows * (long)_nCols;
                        _FSource = new sbyte[size];
                    }
                    catch (ArithmeticException)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.SequenceLengthExceedsLimit,
                                _nRows,
                                _nCols,
                                int.MaxValue));
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                string msg = BuildOutOfMemoryMessage(ex, true);
                Trace.Report(msg);
                throw new OutOfMemoryException(msg, ex);
            }

            // Fill by rows
            int row, col, cell;
            if (UseEARTHToFillMatrix)
            {
                // Calculate row & col size for each block of Matrix
                CalculateOptimalBlockSize();

                if (_rowBlockSize < 1)
                {
                    _rowBlockSize = 1;
                }

                if (_colBlockSize < 1)
                {
                    _colBlockSize = 1;
                }

                int maxCol = 0, initialRow, initialCol;
                IList<Task> affineMatrixTasks = new List<Task>();

                // Fill the Matrix in blocks
                // Assuming there are 3 * 3 block in Matrix
                // Following shows the progress of blocks in Matrix in each iteration
                // "+" block completed
                // "-" block in progress (parallelly in tasks)

                /* 
                 * Fill Matrix till Anti-Diagonal
                 * Iteration 1
                 * -
                 * 
                 * Iteration 2
                 * + -
                 * -
                 * 
                 * Iteration 3
                 * + + -
                 * + -
                 * -
                 */
                col = 0;
                row = 0;
                while (row < _nRows || col < _nCols)
                {
                    initialRow = 0;
                    initialCol = col;

                    while (initialCol >= 0)
                    {
                        int taskRow = initialRow;
                        int taskCol = initialCol;

                        affineMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixAffineBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        if (initialCol > maxCol)
                        {
                            maxCol = initialCol;
                        }

                        initialRow += _rowBlockSize;
                        initialCol -= _colBlockSize;

                        if (initialRow > _nRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(affineMatrixTasks.ToArray());
                    affineMatrixTasks.Clear();

                    row += _rowBlockSize;
                    col += _colBlockSize;
                }

                /* 
                 * Fill the Matrix after Anti-Diagonal
                 * Iteration 1 
                 * + + +
                 * + + -
                 * + -
                 * 
                 * Iteration 2 
                 * + + +
                 * + + +
                 * + + -
                 */
                col = _nCols;
                row = _rowBlockSize;
                while (row < _nRows || col > 0)
                {
                    initialRow = row;
                    initialCol = maxCol;

                    while (initialCol >= 0)
                    {
                        int taskRow = initialRow;
                        int taskCol = initialCol;

                        affineMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixAffineBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        initialRow += _rowBlockSize;
                        initialCol -= _colBlockSize;

                        if (initialRow > _nRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(affineMatrixTasks.ToArray());
                    affineMatrixTasks.Clear();

                    row += _rowBlockSize;
                    col -= _colBlockSize;

                    if (row > _nRows)
                    {
                        break;
                    }
                }

                SetOptimalScoreAffine();
            }
            else
            {
                // Set matrix bc along top row and left column.
                SetRowBoundaryConditionAffine();

                for (row = 1, cell = _nCols; row < _nRows; row++)
                {
                    SetColumnBoundaryConditionAffine(row, cell);
                    cell++;
                    for (col = 1; col < _nCols; col++, cell++)
                    {
                        FillCellAffine(row, col, cell);
                    }
                }

                SetOptimalScoreAffine();
            }
        }

        /// <summary>
        /// Calculates the optimal block size.
        /// </summary>
        private void CalculateOptimalBlockSize()
        {
            // Calculate optimal row & col size for each block of Matrix
            // Max # of task should be equal to # of cores.
            _rowBlockSize = _nRows / Environment.ProcessorCount;
            _colBlockSize = _nCols / Environment.ProcessorCount;
        }

        /// <summary>
        /// Sets cell (initialRow, initialColumn) to 
        /// (initialRow + _rowBlockSize, initialColumn + _colBlockSize) of the F matrix
        /// </summary>
        /// <param name="initialRow">Intial row in block.</param>
        /// <param name="initialColumn">Intial column in block.</param>
        private void FillMatrixAffineBlock(int initialRow, int initialColumn)
        {
            for (int col = initialColumn; col < _nCols && col < initialColumn + _colBlockSize; col++)
            {
                for (int row = initialRow; row < _nRows && row < initialRow + _rowBlockSize; row++)
                {
                    if (row == 0 || col == 0)
                    {
                        // Set matrix bc along top row and left column.
                        SetBoundaryConditionAffine(col, row);
                    }
                    else
                    {
                        FillCellAffine(col, row);
                    }
                }
            }
        }

        /// <summary>
        /// Validates input sequences and gap penalties.
        /// Checks that input sequences use the same alphabet.
        /// Checks that each symbol in the input sequences exists in the similarity matrix.
        /// Checks that gap penalties are less than or equal to 0.
        /// Throws exception if sequences fail these checks.
        /// Writes warning to ApplicationLog if gap penalty or penalties are positive.
        /// </summary>
        /// <param name="aInput">First input sequence.</param>
        /// <param name="bInput">Second input sequence.</param>
        protected void ValidateAlignInput(ISequence aInput, ISequence bInput)
        {
            if (aInput.Alphabet != bInput.Alphabet)
            {
                string message = Properties.Resource.InputAlphabetsMismatch;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            if (null == _similarityMatrix)
            {
                string message = Properties.Resource.SimilarityMatrixCannotBeNull;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            if (!_similarityMatrix.ValidateSequence(aInput))
            {
                string message = Properties.Resource.FirstInputSequenceMismatchSimilarityMatrix;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            if (!_similarityMatrix.ValidateSequence(bInput))
            {
                string message = Properties.Resource.SecondInputSequenceMismatchSimilarityMatrix;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            // Warning if gap penalty > 0
            if (_gapOpenCost > 0)
            {
                ApplicationLog.WriteLine("Gap Open Penalty {0} > 0, possible error", _gapOpenCost);
            }

            if (_gapExtensionCost > 0)
            {
                ApplicationLog.WriteLine("Gap Extension Penalty {0} > 0, possible error", _gapExtensionCost);
            }
        }

        /// <summary>
        /// Sets general case cell score and source for one gap parameter.
        /// </summary>
        /// <param name="col">col of cell</param>
        /// <param name="row">row of cell</param>
        /// <returns>score for cell</returns>
        protected int SetCellValuesSimple(int col, int row)
        {
            if (col < 0)
            {
                throw new ArgumentOutOfRangeException("col");
            }

            if (row < 0)
            {
                throw new ArgumentOutOfRangeException("row");
            }

            // (row - 1, col -1) cell in the scoring matrix.
            int diagScore = _FSimpleScore[col - 1, row - 1] + _similarityMatrix[_a[col - 1], _b[row - 1]];
            int upScore = _FSimpleScore[col, row - 1] + _gapOpenCost; // (row - 1, col) of scoring matrix
            int leftScore = _FSimpleScore[col - 1, row] + _gapOpenCost; // (row, col - 1) of scoring matrix

            int score = 0;
            if (diagScore >= upScore)
            {
                if (diagScore >= leftScore)
                {
                    // use diag
                    score = diagScore;
                    _FSimpleSource[col, row] = SourceDirection.Diagonal;
                }
                else
                {
                    // use left
                    score = leftScore;
                    _FSimpleSource[col, row] = SourceDirection.Left;
                }
            }
            else if (upScore >= leftScore)
            {
                // use up
                score = upScore;
                _FSimpleSource[col, row] = SourceDirection.Up;
            }
            else
            {
                // use left
                score = leftScore;
                _FSimpleSource[col, row] = SourceDirection.Left;
            }

            return score;
        }

        /// <summary>
        /// Sets general case cell score and source for one gap parameter.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        /// <returns>score for cell</returns>
        protected int SetCellValuesSimple(int row, int col, int cell)
        {
            // _FScoreDiagonal has the value of (row - 1, col -1) cell in the scoring matrix.
            int diagScore = _FScoreDiagonal + _similarityMatrix[_b[row - 1], _a[col - 1]];
            int upScore = _FScore[col] + _gapOpenCost; // (row - 1, col) of scoring matrix
            int leftScore = _FScore[col - 1] + _gapOpenCost; // (row, col - 1) of scoring matrix

            // Store current value in _FScoreDiagonal, before overwriting with new value
            _FScoreDiagonal = _FScore[col];

            int score = 0;
            if (diagScore >= upScore)
            {
                if (diagScore >= leftScore)
                {
                    // use diag
                    score = diagScore;
                    _FSource[cell] = SourceDirection.Diagonal;
                }
                else
                {
                    // use left
                    score = leftScore;
                    _FSource[cell] = SourceDirection.Left;
                }
            }
            else if (upScore >= leftScore)
            {
                // use up
                score = upScore;
                _FSource[cell] = SourceDirection.Up;
            }
            else
            {
                // use left
                score = leftScore;
                _FSource[cell] = SourceDirection.Left;
            }

            return score;
        }

        /// <summary>
        /// Sets general case cell score and matrix elements for general affine gap case.
        /// </summary>
        /// <param name="col">col of cell</param>
        /// <param name="row">row of cell</param>
        /// <returns>score for cell</returns>
        protected int SetCellValuesAffine(int col, int row)
        {
            if (col < 0)
            {
                throw new ArgumentOutOfRangeException("col");
            }

            if (row < 0)
            {
                throw new ArgumentOutOfRangeException("row");
            }

            int score;
            int M, Ix, Iy;

            // _MaxScoreDiagonal is max(M[row-1,col-1], Iy[row-1,col-1], Iy[row-1,col-1])
            M = Math.Max(_M[col - 1, row - 1], _Ix[col - 1, row - 1]);
            M = Math.Max(M, _Iy[col - 1, row - 1]);
            M += _similarityMatrix[_a[col - 1], _b[row - 1]];

            // ~ Ix = _M[row - 1, col] + _gapOpenCost, _Ix[row - 1, col] + _gapExtensionCost);
            Ix = Math.Max(_M[col, row - 1] + _gapOpenCost, _Ix[col, row - 1] + _gapExtensionCost);
            _Ix[col, row] = Ix;

            // ~ Iy = Max(_M[row, col - 1] + _gapOpenCost, _Iy[row, col - 1] + _gapExtensionCost);
            Iy = Math.Max(_M[col - 1, row] + _gapOpenCost, _Iy[col - 1, row] + _gapExtensionCost);
            _Iy[col, row] = Iy;

            if (M >= Ix)
            {
                if (M >= Iy)
                {
                    score = M;
                    _FSimpleSource[col, row] = SourceDirection.Diagonal;
                }
                else
                {
                    score = Iy;
                    _FSimpleSource[col, row] = SourceDirection.Left;
                }
            }
            else
            {
                if (Iy >= Ix)
                {
                    score = Iy;
                    _FSimpleSource[col, row] = SourceDirection.Left;
                }
                else
                {
                    score = Ix;
                    _FSimpleSource[col, row] = SourceDirection.Up;
                }
            }

            return score;
        }

        /// <summary>
        /// Sets general case cell score and matrix elements for general affine gap case.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        /// <returns>score for cell</returns>
        protected int SetCellValuesAffine(int row, int col, int cell)
        {
            int score;
            int extnScore, openScore;

            // _MaxScoreDiagonal is max(M[row-1,col-1], Iy[row-1,col-1], Iy[row-1,col-1])
            int diagScore = _MaxScoreDiagonal + _similarityMatrix[_b[row - 1], _a[col - 1]];

            // ~ Ix = _M[row - 1, col] + _gapOpenCost, _Ix[row - 1, col] + _gapExtensionCost);
            extnScore = _IxGapScore[col] + _gapExtensionCost;
            openScore = _MaxScore[col] + _gapOpenCost;
            int xScore = (extnScore >= openScore) ? extnScore : openScore;
            _IxGapScore[col] = xScore;

            // ~ Iy = Max(_M[row, col - 1] + _gapOpenCost, _Iy[row, col - 1] + _gapExtensionCost);
            extnScore = _IyGapScore + _gapExtensionCost;
            openScore = _MaxScore[col - 1] + _gapOpenCost;
            _IyGapScore = (extnScore >= openScore) ? extnScore : openScore;

            _MaxScoreDiagonal = _MaxScore[col];

            if (diagScore >= xScore)
            {
                if (diagScore >= _IyGapScore)
                {
                    score = diagScore;
                    _FSource[cell] = SourceDirection.Diagonal;
                }
                else
                {
                    score = _IyGapScore;
                    _FSource[cell] = SourceDirection.Left;
                }
            }
            else
            {
                if (_IyGapScore >= xScore)
                {
                    score = _IyGapScore;
                    _FSource[cell] = SourceDirection.Left;
                }
                else
                {
                    score = xScore;
                    _FSource[cell] = SourceDirection.Up;
                }
            }

            return score;
        }

        /// <summary>
        /// Builds detailed error message for OutOfMemory exception.
        /// </summary>
        /// <param name="ex">Exception to throw</param>
        /// <param name="isAffine">True for affine case, false for one gap penalty.</param>
        /// <returns>Message to send to user.</returns>
        protected string BuildOutOfMemoryMessage(Exception ex, bool isAffine)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);

            // Memory required is about 5 * N*M for linear gap penalty, 13 * N*M for affine gap.
            sb.AppendFormat("Sequence lengths are {0:N0} and {1:N0}.", _a.Length, _b.Length);
            sb.AppendLine();
            sb.AppendLine("Dynamic programming algorithms are order NxM in memory use, with N and M the sequence lengths.");

            // Large sequences can easily overflow an int.  Use intermediate variables to avoid hard-to-read casts.
            long factor = isAffine ? 13 : 5;
            long estimatedMemory = (long)_nCols * (long)_nRows * factor;
            double estimatedGig = estimatedMemory / 1073741824.0;
            sb.AppendFormat("Current problem requires about {0:N0} bytes (approx {1:N2} Gbytes) of free memory.", estimatedMemory, estimatedGig);
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Initializations to be done before aligning sequences.
        /// Sets consensus resolver property to correct alphabet.
        /// </summary>
        /// <param name="inputSequence">input sequence</param>
        private void InitializeAlign(ISequence inputSequence)
        {
            // Initializations
            if (ConsensusResolver == null)
            {
                ConsensusResolver = new SimpleConsensusResolver(inputSequence.Alphabet);
            }
            else
            {
                ConsensusResolver.SequenceAlphabet = inputSequence.Alphabet;
            }
        }

        /// <summary>
        /// Performs initializations and validations required 
        /// before carrying out sequence alignment.
        /// Initializes only gap open penalty. Initialization for
        /// gap extension, if required, has to be done seperately. 
        /// </summary>
        /// <param name="similarityMatrix">Scoring matrix.</param>
        /// <param name="gapPenalty">Gap open penalty (by convention, use a negative number for this.)</param>
        /// <param name="aInput">First input sequence.</param>
        /// <param name="bInput">Second input sequence.</param>
        private void SimpleAlignPrimer(SimilarityMatrix similarityMatrix, int gapPenalty, ISequence aInput, ISequence bInput)
        {
            InitializeAlign(aInput);
            ResetSpecificAlgorithmMemberVariables();

            // Set Gap Penalty and Similarity Matrix
            _gapOpenCost = gapPenalty;

            // note that _gapExtensionCost is not used for linear gap penalty
            _similarityMatrix = similarityMatrix;

            ValidateAlignInput(aInput, bInput);  // throws exception if input not valid

            // Convert input strings to 0-based int arrays using similarity matrix mapping
            _a = similarityMatrix.ToByteArray(aInput.ToString());
            _b = similarityMatrix.ToByteArray(bInput.ToString());
        }

        /// <summary>
        /// Convert aligned sequences back to Sequence objects, load output SequenceAlignment object
        /// </summary>
        /// <param name="aInput">First input sequence.</param>
        /// <param name="bInput">Second input sequence.</param>
        /// <param name="alignedSequences">List of aligned sequences</param>
        /// <param name="offsets">List of offsets for each aligned sequence</param>
        /// <param name="optScore">Optimum alignment score</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insetions made to the aligned sequences.</param>
        /// <returns>SequenceAlignment with all alignment information</returns>
        private IList<IPairwiseSequenceAlignment> CollateResults(ISequence aInput, ISequence bInput, List<byte[]> alignedSequences, List<int> offsets, int optScore, List<int> startOffsets, List<int> endOffsets, List<int> insertions)
        {
            if (alignedSequences.Count > 0)
            {
                PairwiseSequenceAlignment alignment = new PairwiseSequenceAlignment(aInput, bInput);
                byte[] aAligned, bAligned;

                for (int i = 0; i < alignedSequences.Count; i += 2)
                {
                    aAligned = alignedSequences[i];
                    bAligned = alignedSequences[i + 1];

                    PairwiseAlignedSequence result = new PairwiseAlignedSequence();
                    result.Score = optScore;

                    Sequence seq = new Sequence(aInput.Alphabet, _similarityMatrix.ToString(aAligned));
                    seq.ID = aInput.ID;
                    seq.DisplayID = aInput.DisplayID;
                    result.FirstSequence = seq;

                    seq = new Sequence(bInput.Alphabet, _similarityMatrix.ToString(bAligned));
                    seq.ID = bInput.ID;
                    seq.DisplayID = bInput.DisplayID;
                    result.SecondSequence = seq;

                    AddSimpleConsensusToResult(result);
                    result.FirstOffset = offsets[i];
                    result.SecondOffset = offsets[i + 1];

                    result.Metadata["StartOffsets"] = new List<int> { startOffsets[i], startOffsets[i + 1] };
                    result.Metadata["EndOffsets"] = new List<int> { endOffsets[i], endOffsets[i + 1] };
                    result.Metadata["Insertions"] = new List<int> { insertions[i], insertions[i + 1] };
                    alignment.PairwiseAlignedSequences.Add(result);
                }

                return new List<IPairwiseSequenceAlignment>() { alignment };
            }
            else
            {
                return new List<IPairwiseSequenceAlignment>();
            }
        }

        /// <summary>
        /// Adds consensus to the alignment result.  At this point, it is a very simple algorithm
        /// which puts an ambiguity character where the two aligned sequences do not match.
        /// Uses X and N for protein and DNA/RNA alignments, respectively.
        /// </summary>
        /// <param name="alignment">
        /// Alignment to which to add the consensus.  This is the result returned by the main Align
        /// or AlignSimple method, which contains the aligned sequences but not yet a consensus sequence.
        /// </param>
        private void AddSimpleConsensusToResult(PairwiseAlignedSequence alignment)
        {
            ISequence seq0 = alignment.FirstSequence;
            ISequence seq1 = alignment.SecondSequence;

            Sequence consensus = new Sequence(seq0.Alphabet);
            for (int i = 0; i < seq0.Count; i++)
            {
                consensus.Add(
                    ConsensusResolver.GetConsensus(
                        new List<ISequenceItem>() { seq0[i], seq1[i] }));
            }

            alignment.Consensus = consensus;
        }

        #region Nested Enums, Structs and Classes
        /// <summary>
        /// Position details of cell with best score
        /// </summary>
        protected struct OptScoreCell
        {
            /// <summary>
            /// Column number of cell with optimal score
            /// </summary>
            public int Column;

            /// <summary>
            /// Row number of cell with optimal score
            /// </summary>
            public int Row;

            /// <summary>
            /// Cell number of cell with optimal score
            /// </summary>
            public int Cell;

            /// <summary>
            /// Initializes a new instance of the OptScoreCell struct.
            /// Creates best score cell with the input position values
            /// </summary>
            /// <param name="row">Row Number</param>
            /// <param name="column">Column Number</param>
            public OptScoreCell(int row, int column)
            {
                Row = row;
                Column = column;
                Cell = 0;
            }

            /// <summary>
            /// Initializes a new instance of the OptScoreCell struct.
            /// Creates best score cell with the input position values
            /// </summary>
            /// <param name="row">Row Number</param>
            /// <param name="column">Column Number</param>
            /// <param name="cell">Cell Number</param>
            public OptScoreCell(int row, int column, int cell)
                : this(row, column)
            {
                Cell = cell;
            }

            /// <summary>
            /// Overrides == Operator
            /// </summary>
            /// <param name="cell1">First cell</param>
            /// <param name="cell2">Second cell</param>
            /// <returns>Result of comparison</returns>
            public static bool operator ==(OptScoreCell cell1, OptScoreCell cell2)
            {
                return
                    cell1.Row == cell2.Row &&
                    cell1.Column == cell2.Column &&
                    cell1.Cell == cell2.Cell;
            }

            /// <summary>
            /// Overrides != Operator
            /// </summary>
            /// <param name="cell1">First cell</param>
            /// <param name="cell2">Second cell</param>
            /// <returns>Result of comparison</returns>
            public static bool operator !=(OptScoreCell cell1, OptScoreCell cell2)
            {
                return !(cell1 == cell2);
            }

            /// <summary>
            /// Override Equals method
            /// </summary>
            /// <param name="obj">Object for comparison</param>
            /// <returns>Result of comparison</returns>
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                OptScoreCell other = (OptScoreCell)obj;
                return this == other;
            }

            /// <summary>
            /// Returns the Hash code
            /// </summary>
            /// <returns>Hash code of OptScoreCell</returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        /// <summary> Direction to source of cell value, used during traceback. </summary>
        protected static class SourceDirection
        {
            // This is coded as a set of consts rather than using an enum.  Enums are ints and 
            // referring to these in the code requires casts to and from (sbyte), which makes
            // the code more difficult to read.

            /// <summary> Source was up and left from current cell. </summary>
            public const sbyte Diagonal = 0;

            /// <summary> Source was up from current cell. </summary>
            public const sbyte Up = 1;

            /// <summary> Source was left of current cell. </summary>
            public const sbyte Left = 2;

            /// <summary> During traceback, stop at this cell (used by SmithWaterman). </summary>
            public const sbyte Stop = -1;

            /// <summary> Error code, if cell has code Invalid error has occurred. </summary>
            public const sbyte Invalid = -2;
        }
        #endregion

        #endregion
    }
}