// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio.SimilarityMatrices;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
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
        /// Signifies gap in aligned sequence (stored as int[]) during trace back.
        /// </summary>
        protected byte gapCode = 255;

        /// <summary> Similarity matrix for use in alignment algorithms. </summary>
        protected SimilarityMatrix internalSimilarityMatrix;

        /// <summary> 
        /// Gap open penalty for use in alignment algorithms. 
        /// For alignments using a linear gap penalty, this is the gap penalty.
        /// For alignments using an affine gap, this is the penalty to open a new gap.
        /// This is a negative number, for example _gapOpenCost = -8, not +8.
        /// </summary>
        protected int internalGapOpenCost;

        /// <summary> 
        /// Gap extension penalty for use in alignment algorithms. 
        /// Not used for alignments using a linear gap penalty.
        /// For alignments using an affine gap, this is the penalty to extend an existing gap.
        /// This is a negative number, for example _gapExtensionCost = -2, not +2.
        /// </summary>
        protected int internalGapExtensionCost;

        /// <remark>
        /// The F matrix holds the scores and source.  See Durbin et al. for details.
        /// The F matrix uses the first column and first row to store boundary condition.
        /// This means the the input sequence at, for example, col 4 will map to col 5 in the F matrix.
        /// Gotoh's optimization is used to reduce memory usage.
        /// </remark>
        /// <summary>
        /// FScore is the score for each cell, used for linear gap penalty.
        /// </summary>
        protected int[] fScore;

        /// <summary>
        /// FScoreDiagonal is used to store diagonal value from previous row.
        /// Used for Gotoh optimization of linear gap penalty.
        /// </summary>
        protected int fScoreDiagonal;

        // Note that the dynamic programming matrix is coded as separate arrays.  Structs allocate space on 4 byte
        // boundaries, leading to some memory inefficiency.

        /// <summary>
        /// FSource stores the source for the each cell in the F matrix.
        /// Source is coded as 0 diagonal, 1 up, 2 left, see enum SourceDirection below.
        /// </summary>
        protected sbyte[] fSource; // source for cell

        /// <summary>
        /// MaxScore stores the maximum value for the affine gap penalty implementation.
        /// </summary>
        protected int[] maxScore; // best score of alignment x1...xi to y1...yi

        /// <summary>
        /// MaxScoreDiagonal is used to store maximum value from previous row.
        /// Used for Gotoh optimization of affine gap penalty.
        /// </summary>
        protected int maxScoreDiagonal;

        /// <summary>
        /// Stores alignment score for putting gap in 'x' sequence for affine gap penalty implementation.
        /// Alignment score if xi aligns to a gap after yi.
        /// </summary>
        protected int[] ixGapScore;

        /// <summary>
        /// Stores alignment score for putting gap in 'y' sequence for affine gap penalty implementation.
        /// Alignment score if yi aligns to a gap after xi.
        /// </summary>
        protected int iyGapScore;

        /// <summary>
        /// Number of rows in the dynamic programming matrix.
        /// </summary>
        protected long numberOfRows;

        /// <summary>
        /// Number of columns in the dynamic programming matrix.
        /// </summary>
        protected long numberOfCols;

        /// <summary>
        /// First input sequence.
        /// </summary>
        protected ISequence firstInputSequence;

        /// <summary>
        /// Second input sequence.
        /// </summary>
        protected ISequence secondInputSequence;

        /// <summary>
        /// Row size of the block of Matrix.
        /// </summary>
        private long rowBlockSize = 0;

        /// <summary>
        /// Column size of the block of Matrix.
        /// </summary>
        private long colBlockSize = 0;
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
            internalSimilarityMatrix = new DiagonalSimilarityMatrix(2, -2);
            internalGapOpenCost = -8;
            internalGapExtensionCost = -1;
        }

        #endregion

        #region Properties

        /// <summary> Gets or sets similarity matrix for use in alignment algorithms. </summary>
        public SimilarityMatrix SimilarityMatrix
        {
            get { return internalSimilarityMatrix; }
            set { internalSimilarityMatrix = value; }
        }

        /// <summary> 
        /// Gets or sets gap open penalty for use in alignment algorithms. 
        /// For alignments using a linear gap penalty, this is the gap penalty.
        /// For alignments using an affine gap, this is the penalty to open a new gap.
        /// This is a negative number, for example GapOpenCost = -8, not +8.
        /// </summary>
        public int GapOpenCost
        {
            get { return internalGapOpenCost; }
            set { internalGapOpenCost = value; }
        }

        /// <summary> 
        /// Gets or sets gap extension penalty for use in alignment algorithms. 
        /// Not used for alignments using a linear gap penalty.
        /// For alignments using an affine gap, this is the penalty to extend an existing gap.
        /// This is a negative number, for example GapExtensionCost = -2, not +2.
        /// </summary>
        public int GapExtensionCost
        {
            get { return internalGapExtensionCost; }
            set { internalGapExtensionCost = value; }
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
        /// Gets or sets a value indicating whether to use parallelization using 
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
        /// "-" block in progress (parallel in tasks)
        /// Fill Matrix till Anti-Diagonal
        /// Iteration 1
        /// -
        /// Iteration 2
        /// + -
        /// -
        /// Iteration 3
        /// + + -
        /// + -
        /// -
        /// Iteration 4
        /// + + +
        /// + + -
        /// + -
        /// Iteration 5
        /// + + +
        /// + + +
        /// + + -.
        /// </summary>
        public bool UseEARTHToFillMatrix { get; set; }

        /// <remark>
        /// Gets or sets the scores and source.  See Durbin et al. for details.
        /// The F matrix uses the first column and first row to store boundary condition.
        /// This means the the input sequence at, for example, col 4 will map to col 5 in the F matrix.
        /// </remark>
        /// <summary>
        /// Gets or sets the score for each cell, used for single gap penalty.
        /// </summary>
        protected int[,] _FSimpleScore { get; set; }

        /// <summary>
        /// Gets or sets the source for the each cell in the F matrix.
        /// Source is coded as 0 diagonal, 1 up, 2 left, see enum SourceDirection below.
        /// </summary>
        protected sbyte[,] _FSimpleSource { get; set; } // source for cell

        /// <summary>
        /// Gets or sets the diagonal value for the affine gap penalty implementation.
        /// See Durbin et al. for details.
        /// </summary>
        protected int[,] _M { get; set; }

        /// <summary>
        /// Gets or sets the gap in x value for the affine gap penalty implementation.
        /// </summary>
        protected int[,] _Ix { get; set; }

        /// <summary>
        /// Gets or sets the gap in y value for the affine gap penalty implementation.
        /// </summary>
        protected int[,] _Iy { get; set; }

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
            return AlignSimple(internalSimilarityMatrix, internalGapOpenCost, sequence1, sequence2);
        }

        /// <summary>
        /// Aligns two sequences using a linear gap parameter, using existing gap open cost and similarity matrix.
        /// Set these using GapOpenCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(IEnumerable<ISequence> inputSequences)
        {
            if (inputSequences.Count() != 2)
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.PairwiseAlignerWrongArgumentCount,
                        inputSequences.Count());
                Trace.Report(message);
                throw new ArgumentException(message, "inputSequences");
            }

            ISequence firstSequence = null;
            ISequence secondSequence = null;

            firstSequence = inputSequences.ElementAt(0);
            secondSequence = inputSequences.ElementAt(1);

            return AlignSimple(internalSimilarityMatrix, internalGapOpenCost, firstSequence, secondSequence);
        }

        /// <summary>
        /// Aligns two sequences using a linear gap parameter, using existing gap open cost and similarity matrix.
        /// Set these using GapOpenCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        IList<ISequenceAlignment> ISequenceAligner.AlignSimple(IEnumerable<ISequence> inputSequences)
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
            return Align(internalSimilarityMatrix, internalGapOpenCost, internalGapExtensionCost, sequence1, sequence2);
        }

        /// <summary>
        /// Aligns two sequences using the affine gap metric, a gap open penalty and a gap extension penalty.
        /// This method uses the existing gap open and extension penalties and similarity matrix.
        /// Set these using GapOpenCost, GapExtensionCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> Align(IEnumerable<ISequence> inputSequences)
        {
            if (inputSequences.Count() != 2)
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.PairwiseAlignerWrongArgumentCount,
                        inputSequences.Count());
                Trace.Report(message);
                throw new ArgumentException(message, "inputSequences");
            }

            ISequence firstSequence = null;
            ISequence secondSequence = null;

            firstSequence = inputSequences.ElementAt(0);
            secondSequence = inputSequences.ElementAt(1);

            return Align(internalSimilarityMatrix, internalGapOpenCost, internalGapExtensionCost, firstSequence, secondSequence);
        }

        /// <summary>
        /// Aligns two sequences using the affine gap metric, a gap open penalty and a gap extension penalty.
        /// This method uses the existing gap open and extension penalties and similarity matrix.
        /// Set these using GapOpenCost, GapExtensionCost and SimilarityMatrix properties before calling this method.
        /// </summary>
        /// <param name="inputSequences">List of sequences to align.  Must contain exactly two sequences.</param>
        /// <returns>A list of sequence alignments.</returns>
        IList<ISequenceAlignment> ISequenceAligner.Align(IEnumerable<ISequence> inputSequences)
        {
            return this.Align(inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Pairwise alignment of two sequences using a linear gap penalty.  The various algorithms in derived classes (NeedlemanWunsch, 
        /// SmithWaterman, and PairwiseOverlap) all use this general engine for alignment with a linear gap penalty.
        /// </summary>
        /// <param name="localSimilarityMatrix">Scoring matrix.</param>
        /// <param name="gapPenalty">Gap penalty (by convention, use a negative number for this.).</param>
        /// <param name="inputA">First input sequence.</param>
        /// <param name="inputB">Second input sequence.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(SimilarityMatrix localSimilarityMatrix, int gapPenalty, ISequence inputA, ISequence inputB)
        {
            if (inputA == null)
            {
                throw new ArgumentNullException("inputA");
            }

            inputA.Alphabet.TryGetDefaultGapSymbol(out gapCode);

            // Initialize and perform validations for simple alignment
            SimpleAlignPrimer(localSimilarityMatrix, gapPenalty, inputA, inputB);

            FillMatrixSimple();

            List<byte[]> alignedSequences;
            List<long> offsets;
            List<long> endOffsets;
            List<long> insertions;
            List<long> startOffsets;
            int optScore = Traceback(out alignedSequences, out offsets, out startOffsets, out endOffsets, out insertions);
            return CollateResults(inputA, inputB, alignedSequences, offsets, optScore, startOffsets, endOffsets, insertions);
        }

        /// <summary>
        /// Pairwise alignment of two sequences using an affine gap penalty.  The various algorithms in derived classes (NeedlemanWunsch, 
        /// SmithWaterman, and PairwiseOverlap) all use this general engine for alignment with an affine gap penalty.
        /// </summary>
        /// <param name="localSimilarityMatrix">Scoring matrix.</param>
        /// <param name="gapOpenPenalty">Gap open penalty (by convention, use a negative number for this.).</param>
        /// <param name="gapExtensionPenalty">Gap extension penalty (by convention, use a negative number for this.).</param>
        /// <param name="inputA">First input sequence.</param>
        /// <param name="inputB">Second input sequence.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> Align(
            SimilarityMatrix localSimilarityMatrix,
            int gapOpenPenalty,
            int gapExtensionPenalty,
            ISequence inputA,
            ISequence inputB)
        {
            if (inputA == null)
            {
                throw new ArgumentNullException("inputA");
            }

            inputA.Alphabet.TryGetDefaultGapSymbol(out gapCode);

            // Initialize and perform validations for alignment
            // In addition, initialize gap extension penalty.
            SimpleAlignPrimer(localSimilarityMatrix, gapOpenPenalty, inputA, inputB);
            internalGapExtensionCost = gapExtensionPenalty;

            FillMatrixAffine();

            List<byte[]> alignedSequences;
            List<long> offsets;
            List<long> startOffsets;
            List<long> endOffsets;
            List<long> insertions;
            int optScore = Traceback(out alignedSequences, out offsets, out startOffsets, out endOffsets, out insertions);
            return CollateResults(inputA, inputB, alignedSequences, offsets, optScore, startOffsets, endOffsets, insertions);
        }

        /// <summary>
        /// Return the starting position of alignment of sequence1 with respect to sequence2.
        /// </summary>
        /// <param name="aligned">Aligned sequence.</param>
        /// <returns>The number of initial gap characters.</returns>
        protected int GetOffset(IEnumerable<byte> aligned)
        {
            if (aligned == null)
            {
                throw new ArgumentNullException("aligned");
            }

            int ret = 0;
            foreach (byte item in aligned)
            {
                if (item != gapCode)
                {
                    return ret;
                }

                ++ret;
            }

            return ret;
        }

        // These routines will be different for the various variations --SmithWaterman, NeedlemanWunsch, etc.
        // Additional variations can be built by modifying these functions.

        /// <summary>
        /// Sets cell (col,row) of the F matrix.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// </summary>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected abstract void FillCellSimple(long col, long row);

        /// <summary>
        /// Sets cell (row,col) of the F matrix.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// Uses linear gap penalty.
        /// </summary>
        /// <param name="row">Row of cell to fill.</param>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="cell">Cell number.</param>
        protected abstract void FillCellSimple(long row, long col, long cell);

        /// <summary>
        /// Resets member variables that are unique to a specific algorithm.
        /// These must be reset for each alignment, initialization in the constructor
        /// only works for the first call to AlignSimple.  This routine is called at the beginning
        /// of each AlignSimple method.
        /// </summary>
        protected abstract void ResetSpecificAlgorithmMemberVariables();

        /// <summary>
        /// Allows each algorithm to set optimal score at end of matrix construction
        /// Used for linear gap penalty.
        /// </summary>
        protected abstract void SetOptimalScoreSimple();

        /// <summary>
        /// Allows each algorithm to set optimal score at end of matrix construction
        /// Used for affine gap penalty.
        /// </summary>
        protected abstract void SetOptimalScoreAffine();

        /// <summary>
        /// Sets cell (col,row) of the matrix for affine gap implementation.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// </summary>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected abstract void FillCellAffine(long col, long row);

        /// <summary>
        /// Sets cell (row,col) of the matrix for affine gap implementation.  Different algorithms will use different scoring
        /// and traceback methods and therefore will override this method.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row of cell to fill.</param>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="cell">Cell number.</param>
        protected abstract void FillCellAffine(long row, long col, long cell);

        /// <summary>
        /// Sets boundary conditions in the F matrix for the one gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected abstract void SetBoundaryConditionSimple(long col, long row);

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
        /// <param name="row">Row number of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected abstract void SetColumnBoundaryConditionSimple(long row, long cell);

        /// <summary>
        /// Sets boundary conditions for the dynamic programming matrix for the affine gap penalty case.  
        /// As in the FillCell methods, different algorithms will use different 
        /// boundary conditions and will override this method.
        /// </summary>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected abstract void SetBoundaryConditionAffine(long col, long row);

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
        /// <param name="row">Row number of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected abstract void SetColumnBoundaryConditionAffine(long row, long cell);

        /// <summary>
        /// Performs traceback step for the relevant algorithm.  Each algorithm must override this
        /// since the traceback differs for the different algorithms.
        /// </summary>
        /// <param name="alignedSequences">List of aligned sequences (output).</param>
        /// <param name="offsets">Offset is the starting position of alignment .
        /// of sequence1 with respect to sequence2.</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insertions made to the aligned sequences.</param>
        /// <returns>Optimum score for this alignment.</returns>
        protected abstract int Traceback(out List<byte[]> alignedSequences, out List<long> offsets, out List<long> startOffsets, out List<long> endOffsets, out List<long> insertions); // perform algorithm's traceback step

        /// <summary>
        /// Fills F matrix for linear gap penalty implementation.
        /// </summary>
        protected virtual void FillMatrixSimple()
        {
            numberOfCols = firstInputSequence.Count + 1;
            numberOfRows = secondInputSequence.Count + 1;

            try
            {
                if (UseEARTHToFillMatrix)
                {
                    _FSimpleScore = new int[numberOfCols, numberOfRows];
                    _FSimpleSource = new sbyte[numberOfCols, numberOfRows];
                    _M = null;  // affine matrices not used for simple model
                    _Ix = null;
                    _Iy = null;
                }
                else
                {
                    fScore = new int[numberOfCols];
                    try
                    {
                        long size = (long)numberOfRows * (long)numberOfCols;
                        fSource = new sbyte[size];
                    }
                    catch (ArithmeticException)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.SequenceLengthExceedsLimit,
                                numberOfRows,
                                numberOfCols,
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
            long row, col, cell;

            if (UseEARTHToFillMatrix)
            {
                // Calculate row & col size for each block of Matrix
                CalculateOptimalBlockSize();

                if (rowBlockSize < 1)
                {
                    rowBlockSize = 1;
                }

                if (colBlockSize < 1)
                {
                    colBlockSize = 1;
                }

                long maxCol = 0, initialRow, initialCol;
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
                while (row < numberOfRows || col < numberOfCols)
                {
                    initialRow = 0;
                    initialCol = col;

                    while (initialCol >= 0)
                    {
                        long taskRow = initialRow;
                        long taskCol = initialCol;

                        simpleMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixSimpleBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        if (initialCol > maxCol)
                        {
                            maxCol = initialCol;
                        }

                        initialRow += rowBlockSize;
                        initialCol -= colBlockSize;

                        if (initialRow > numberOfRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(simpleMatrixTasks.ToArray());
                    simpleMatrixTasks.Clear();

                    row += rowBlockSize;
                    col += colBlockSize;
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
                col = numberOfCols;
                row = rowBlockSize;
                while (row < numberOfRows || col >= 0)
                {
                    initialRow = row;
                    initialCol = maxCol;

                    while (initialCol >= 0)
                    {
                        long taskRow = initialRow;
                        long taskCol = initialCol;

                        simpleMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixSimpleBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        initialRow += rowBlockSize;
                        initialCol -= colBlockSize;

                        if (initialRow > numberOfRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(simpleMatrixTasks.ToArray());
                    simpleMatrixTasks.Clear();

                    row += rowBlockSize;
                    col -= colBlockSize;

                    if (row > numberOfRows)
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

                for (row = 1, cell = numberOfCols; row < numberOfRows; row++)
                {
                    SetColumnBoundaryConditionSimple(row, cell);
                    cell++;
                    for (col = 1; col < numberOfCols; col++, cell++)
                    {
                        FillCellSimple(row, col, cell);
                    }
                }

                SetOptimalScoreSimple();
            }
        }

        /// <summary>
        /// Fills matrix data for affine gap penalty implementation.
        /// </summary>
        protected virtual void FillMatrixAffine()
        {
            numberOfCols = firstInputSequence.Count + 1;
            numberOfRows = secondInputSequence.Count + 1;
            try
            {
                if (UseEARTHToFillMatrix)
                {
                    _FSimpleScore = null; // not used for affine model
                    _FSimpleSource = new sbyte[numberOfCols, numberOfRows];
                    _M = new int[numberOfCols, numberOfRows];
                    _Ix = new int[numberOfCols, numberOfRows];
                    _Iy = new int[numberOfCols, numberOfRows];
                }
                else
                {
                    ixGapScore = new int[numberOfCols];
                    maxScore = new int[numberOfCols];
                    try
                    {
                        long size = (long)numberOfRows * (long)numberOfCols;
                        fSource = new sbyte[size];
                    }
                    catch (ArithmeticException)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.SequenceLengthExceedsLimit,
                                numberOfRows,
                                numberOfCols,
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
            long row, col, cell;
            if (UseEARTHToFillMatrix)
            {
                // Calculate row & col size for each block of Matrix
                CalculateOptimalBlockSize();

                if (rowBlockSize < 1)
                {
                    rowBlockSize = 1;
                }

                if (colBlockSize < 1)
                {
                    colBlockSize = 1;
                }

                long maxCol = 0, initialRow, initialCol;
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
                while (row < numberOfRows || col < numberOfCols)
                {
                    initialRow = 0;
                    initialCol = col;

                    while (initialCol >= 0)
                    {
                        long taskRow = initialRow;
                        long taskCol = initialCol;

                        affineMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixAffineBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        if (initialCol > maxCol)
                        {
                            maxCol = initialCol;
                        }

                        initialRow += rowBlockSize;
                        initialCol -= colBlockSize;

                        if (initialRow > numberOfRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(affineMatrixTasks.ToArray());
                    affineMatrixTasks.Clear();

                    row += rowBlockSize;
                    col += colBlockSize;
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
                col = numberOfCols;
                row = rowBlockSize;
                while (row < numberOfRows || col > 0)
                {
                    initialRow = row;
                    initialCol = maxCol;

                    while (initialCol >= 0)
                    {
                        long taskRow = initialRow;
                        long taskCol = initialCol;

                        affineMatrixTasks.Add(Task.Factory.StartNew(
                                t => FillMatrixAffineBlock(taskRow, taskCol),
                                TaskCreationOptions.None));

                        initialRow += rowBlockSize;
                        initialCol -= colBlockSize;

                        if (initialRow > numberOfRows)
                        {
                            break;
                        }
                    }

                    Task.WaitAll(affineMatrixTasks.ToArray());
                    affineMatrixTasks.Clear();

                    row += rowBlockSize;
                    col -= colBlockSize;

                    if (row > numberOfRows)
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

                for (row = 1, cell = numberOfCols; row < numberOfRows; row++)
                {
                    SetColumnBoundaryConditionAffine(row, cell);
                    cell++;
                    for (col = 1; col < numberOfCols; col++, cell++)
                    {
                        FillCellAffine(row, col, cell);
                    }
                }

                SetOptimalScoreAffine();
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
        /// <param name="inputA">First input sequence.</param>
        /// <param name="inputB">Second input sequence.</param>
        protected void ValidateAlignInput(ISequence inputA, ISequence inputB)
        {
            if (inputA == null)
            {
                throw new ArgumentNullException("inputA");
            }

            if (inputB == null)
            {
                throw new ArgumentNullException("inputB");
            }

            if (!Alphabets.CheckIsFromSameBase(inputA.Alphabet, inputB.Alphabet))
            {
                Trace.Report(Properties.Resource.InputAlphabetsMismatch);
                throw new ArgumentException(Properties.Resource.InputAlphabetsMismatch);
            }

            if (null == internalSimilarityMatrix)
            {
                Trace.Report(Properties.Resource.SimilarityMatrixCannotBeNull);
                throw new ArgumentException(Properties.Resource.SimilarityMatrixCannotBeNull);
            }

            if (!internalSimilarityMatrix.ValidateSequence(inputA))
            {
                Trace.Report(Properties.Resource.FirstInputSequenceMismatchSimilarityMatrix);
                throw new ArgumentException(Properties.Resource.FirstInputSequenceMismatchSimilarityMatrix);
            }

            if (!internalSimilarityMatrix.ValidateSequence(inputB))
            {
                Trace.Report(Properties.Resource.SecondInputSequenceMismatchSimilarityMatrix);
                throw new ArgumentException(Properties.Resource.SecondInputSequenceMismatchSimilarityMatrix);
            }

            // Warning if gap penalty > 0
            if (internalGapOpenCost > 0)
            {
                ApplicationLog.WriteLine("Gap Open Penalty {0} > 0, possible error", internalGapOpenCost);
            }

            if (internalGapExtensionCost > 0)
            {
                ApplicationLog.WriteLine("Gap Extension Penalty {0} > 0, possible error", internalGapExtensionCost);
            }
        }

        /// <summary>
        /// Sets general case cell score and source for one gap parameter.
        /// </summary>
        /// <param name="col">Col of cell.</param>
        /// <param name="row">Row of cell.</param>
        /// <returns>Score for cell.</returns>
        protected int SetCellValuesSimple(long col, long row)
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
            int diagScore = _FSimpleScore[col - 1, row - 1] + internalSimilarityMatrix[firstInputSequence[col - 1], secondInputSequence[row - 1]];
            int scoreUp = _FSimpleScore[col, row - 1] + internalGapOpenCost; // (row - 1, col) of scoring matrix
            int leftScore = _FSimpleScore[col - 1, row] + internalGapOpenCost; // (row, col - 1) of scoring matrix

            int score = 0;
            if (diagScore >= scoreUp)
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
            else if (scoreUp >= leftScore)
            {
                // use up
                score = scoreUp;
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
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Col of cell.</param>
        /// <param name="cell">Cell number.</param>
        /// <returns>Score for cell.</returns>
        protected int SetCellValuesSimple(long row, long col, long cell)
        {
            // _FScoreDiagonal has the value of (row - 1, col -1) cell in the scoring matrix.
            int diagScore = fScoreDiagonal + internalSimilarityMatrix[secondInputSequence[row - 1], firstInputSequence[col - 1]];
            int scoreUp = fScore[col] + internalGapOpenCost; // (row - 1, col) of scoring matrix
            int leftScore = fScore[col - 1] + internalGapOpenCost; // (row, col - 1) of scoring matrix

            // Store current value in _FScoreDiagonal, before overwriting with new value
            fScoreDiagonal = fScore[col];

            int score = 0;
            if (diagScore >= scoreUp)
            {
                if (diagScore >= leftScore)
                {
                    // use diag
                    score = diagScore;
                    fSource[cell] = SourceDirection.Diagonal;
                }
                else
                {
                    // use left
                    score = leftScore;
                    fSource[cell] = SourceDirection.Left;
                }
            }
            else if (scoreUp >= leftScore)
            {
                // use up
                score = scoreUp;
                fSource[cell] = SourceDirection.Up;
            }
            else
            {
                // use left
                score = leftScore;
                fSource[cell] = SourceDirection.Left;
            }

            return score;
        }

        /// <summary>
        /// Sets general case cell score and matrix elements for general affine gap case.
        /// </summary>
        /// <param name="col">Col of cell.</param>
        /// <param name="row">Row of cell.</param>
        /// <returns>Score for cell.</returns>
        protected int SetCellValuesAffine(long col, long row)
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
            int m, Ix, Iy;

            // _MaxScoreDiagonal is max(M[row-1,col-1], Iy[row-1,col-1], Iy[row-1,col-1])
            m = Math.Max(_M[col - 1, row - 1], _Ix[col - 1, row - 1]);
            m = Math.Max(m, _Iy[col - 1, row - 1]);
            m += internalSimilarityMatrix[firstInputSequence[col - 1], secondInputSequence[row - 1]];

            // ~ Ix = _M[row - 1, col] + _gapOpenCost, _Ix[row - 1, col] + _gapExtensionCost);
            Ix = Math.Max(_M[col, row - 1] + internalGapOpenCost, _Ix[col, row - 1] + internalGapExtensionCost);
            _Ix[col, row] = Ix;

            // ~ Iy = Max(_M[row, col - 1] + _gapOpenCost, _Iy[row, col - 1] + _gapExtensionCost);
            Iy = Math.Max(_M[col - 1, row] + internalGapOpenCost, _Iy[col - 1, row] + internalGapExtensionCost);
            _Iy[col, row] = Iy;

            if (m >= Ix)
            {
                if (m >= Iy)
                {
                    score = m;
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
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Col of cell.</param>
        /// <param name="cell">Cell number.</param>
        /// <returns>Score for cell.</returns>
        protected int SetCellValuesAffine(long row, long col, long cell)
        {
            int score;
            int extnScore, openScore;

            // _MaxScoreDiagonal is max(M[row-1,col-1], Iy[row-1,col-1], Iy[row-1,col-1])
            int diagScore = maxScoreDiagonal + internalSimilarityMatrix[secondInputSequence[row - 1], firstInputSequence[col - 1]];

            // ~ Ix = _M[row - 1, col] + _gapOpenCost, _Ix[row - 1, col] + _gapExtensionCost);
            extnScore = ixGapScore[col] + internalGapExtensionCost;
            openScore = maxScore[col] + internalGapOpenCost;
            int scoreX = (extnScore >= openScore) ? extnScore : openScore;
            ixGapScore[col] = scoreX;

            // ~ Iy = Max(_M[row, col - 1] + _gapOpenCost, _Iy[row, col - 1] + _gapExtensionCost);
            extnScore = iyGapScore + internalGapExtensionCost;
            openScore = maxScore[col - 1] + internalGapOpenCost;
            iyGapScore = (extnScore >= openScore) ? extnScore : openScore;

            maxScoreDiagonal = maxScore[col];

            if (diagScore >= scoreX)
            {
                if (diagScore >= iyGapScore)
                {
                    score = diagScore;
                    fSource[cell] = SourceDirection.Diagonal;
                }
                else
                {
                    score = iyGapScore;
                    fSource[cell] = SourceDirection.Left;
                }
            }
            else
            {
                if (iyGapScore >= scoreX)
                {
                    score = iyGapScore;
                    fSource[cell] = SourceDirection.Left;
                }
                else
                {
                    score = scoreX;
                    fSource[cell] = SourceDirection.Up;
                }
            }

            return score;
        }

        /// <summary>
        /// Builds detailed error message for OutOfMemory exception.
        /// </summary>
        /// <param name="ex">Exception to throw.</param>
        /// <param name="isAffine">True for affine case, false for one gap penalty.</param>
        /// <returns>Message to send to user.</returns>
        protected string BuildOutOfMemoryMessage(Exception ex, bool isAffine)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);

            // Memory required is about 5 * N*M for linear gap penalty, 13 * N*M for affine gap.
            sb.AppendFormat("Sequence lengths are {0:N0} and {1:N0}.", firstInputSequence.Count, secondInputSequence.Count);
            sb.AppendLine();
            sb.AppendLine("Dynamic programming algorithms are order NxM in memory use, with N and M the sequence lengths.");

            // Large sequences can easily overflow an int.  Use intermediate variables to avoid hard-to-read casts.
            long factor = isAffine ? 13 : 5;
            long estimatedMemory = (long)numberOfCols * (long)numberOfRows * factor;
            double estimatedGig = estimatedMemory / 1073741824.0;
            sb.AppendFormat("Current problem requires about {0:N0} bytes (approx {1:N2} Gbytes) of free memory.", estimatedMemory, estimatedGig);
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Sets cell (initialRow, initialColumn) to 
        /// (initialRow + _rowBlockSize, initialColumn + _colBlockSize) of the F matrix.
        /// </summary>
        /// <param name="initialRow">Initial row in block.</param>
        /// <param name="initialColumn">Initial column in block.</param>
        private void FillMatrixSimpleBlock(long initialRow, long initialColumn)
        {
            for (long col = initialColumn; col < numberOfCols && col < initialColumn + colBlockSize; col++)
            {
                for (long row = initialRow; row < numberOfRows && row < initialRow + rowBlockSize; row++)
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
        /// Calculates the optimal block size.
        /// </summary>
        private void CalculateOptimalBlockSize()
        {
            // Calculate optimal row & col size for each block of Matrix
            // Max # of task should be equal to # of cores.
            rowBlockSize = numberOfRows / Environment.ProcessorCount;
            colBlockSize = numberOfCols / Environment.ProcessorCount;
        }

        /// <summary>
        /// Sets cell (initialRow, initialColumn) to 
        /// (initialRow + _rowBlockSize, initialColumn + _colBlockSize) of the F matrix.
        /// </summary>
        /// <param name="initialRow">Intial row in block.</param>
        /// <param name="initialColumn">Intial column in block.</param>
        private void FillMatrixAffineBlock(long initialRow, long initialColumn)
        {
            for (long col = initialColumn; col < numberOfCols && col < initialColumn + colBlockSize; col++)
            {
                for (long row = initialRow; row < numberOfRows && row < initialRow + rowBlockSize; row++)
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
        /// Initializations to be done before aligning sequences.
        /// Sets consensus resolver property to correct alphabet.
        /// </summary>
        /// <param name="inputSequence">Input sequence.</param>
        private void InitializeAlign(ISequence inputSequence)
        {
            // Initializations
            if (ConsensusResolver == null)
            {
                ConsensusResolver = new SimpleConsensusResolver(Alphabets.AmbiguousAlphabetMap[inputSequence.Alphabet]);
            }
            else
            {
                ConsensusResolver.SequenceAlphabet = Alphabets.AmbiguousAlphabetMap[inputSequence.Alphabet];
            }
        }

        /// <summary>
        /// Performs initializations and validations required 
        /// before carrying out sequence alignment.
        /// Initializes only gap open penalty. Initialization for
        /// gap extension, if required, has to be done separately. 
        /// </summary>
        /// <param name="similarityMatrix">Scoring matrix.</param>
        /// <param name="gapPenalty">Gap open penalty (by convention, use a negative number for this.).</param>
        /// <param name="inputA">First input sequence.</param>
        /// <param name="inputB">Second input sequence.</param>
        private void SimpleAlignPrimer(SimilarityMatrix similarityMatrix, int gapPenalty, ISequence inputA, ISequence inputB)
        {
            InitializeAlign(inputA);
            ResetSpecificAlgorithmMemberVariables();

            // Set Gap Penalty and Similarity Matrix
            internalGapOpenCost = gapPenalty;

            // note that _gapExtensionCost is not used for linear gap penalty
            this.internalSimilarityMatrix = similarityMatrix;

            ValidateAlignInput(inputA, inputB);  // throws exception if input not valid

            // Convert input strings to 0-based int arrays using similarity matrix mapping
            firstInputSequence = inputA;
            secondInputSequence = inputB;
        }

        /// <summary>
        /// Convert aligned sequences back to Sequence objects, load output SequenceAlignment object.
        /// </summary>
        /// <param name="inputA">First input sequence.</param>
        /// <param name="inputB">Second input sequence.</param>
        /// <param name="alignedSequences">List of aligned sequences.</param>
        /// <param name="offsets">List of offsets for each aligned sequence.</param>
        /// <param name="optScore">Optimum alignment score.</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insertions made to the aligned sequences.</param>
        /// <returns>SequenceAlignment with all alignment information.</returns>
        private IList<IPairwiseSequenceAlignment> CollateResults(ISequence inputA, ISequence inputB, List<byte[]> alignedSequences, List<long> offsets, int optScore, List<long> startOffsets, List<long> endOffsets, List<long> insertions)
        {
            if (alignedSequences.Count > 0)
            {
                PairwiseSequenceAlignment alignment = new PairwiseSequenceAlignment(inputA, inputB);
                byte[] alignedA, alignedB;

                for (int i = 0; i < alignedSequences.Count; i += 2)
                {
                    alignedA = alignedSequences[i];
                    alignedB = alignedSequences[i + 1];

                    PairwiseAlignedSequence result = new PairwiseAlignedSequence();
                    result.Score = optScore;

                    // If alphabet of inputA is DnaAlphabet then alphabet of alignedA may be Dna or AmbiguousDna.
                    IAlphabet alphabet = Alphabets.AutoDetectAlphabet(alignedA, 0, alignedA.LongLength, inputA.Alphabet);
                    Sequence seq = new Sequence(alphabet, alignedA, false);
                    seq.ID = inputA.ID;
                    // seq.DisplayID = aInput.DisplayID;
                    result.FirstSequence = seq;

                    alphabet = Alphabets.AutoDetectAlphabet(alignedB, 0, alignedB.LongLength, inputB.Alphabet);
                    seq = new Sequence(alphabet, alignedB, false);
                    seq.ID = inputB.ID;
                    // seq.DisplayID = bInput.DisplayID;
                    result.SecondSequence = seq;

                    AddSimpleConsensusToResult(result);
                    result.FirstOffset = offsets[i];
                    result.SecondOffset = offsets[i + 1];

                    result.Metadata["StartOffsets"] = new List<long> { startOffsets[i], startOffsets[i + 1] };
                    result.Metadata["EndOffsets"] = new List<long> { endOffsets[i], endOffsets[i + 1] };
                    result.Metadata["Insertions"] = new List<long> { insertions[i], insertions[i + 1] };
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

            byte[] consensus = new byte[seq0.Count];
            for (int i = 0; i < seq0.Count; i++)
            {
                consensus[i] = ConsensusResolver.GetConsensus(
                        new byte[] { seq0[i], seq1[i] });
            }

            IAlphabet consensusAlphabet = Alphabets.AutoDetectAlphabet(consensus, 0, consensus.LongLength, seq0.Alphabet);
            alignment.Consensus = new Sequence(consensusAlphabet, consensus, false);
        }

        #region Nested Enums, Structs and Classes
        /// <summary>
        /// Position details of cell with best score.
        /// </summary>
        protected struct OptScoreCell
        {
            /// <summary>
            /// Column number of cell with optimal score.
            /// </summary>
            public long Column;

            /// <summary>
            /// Row number of cell with optimal score.
            /// </summary>
            public long Row;

            /// <summary>
            /// Cell number of cell with optimal score.
            /// </summary>
            public long Cell;

            /// <summary>
            /// Initializes a new instance of the OptScoreCell struct.
            /// Creates best score cell with the input position values.
            /// </summary>
            /// <param name="row">Row Number.</param>
            /// <param name="column">Column Number.</param>
            public OptScoreCell(long row, long column)
            {
                Row = row;
                Column = column;
                Cell = 0;
            }

            /// <summary>
            /// Initializes a new instance of the OptScoreCell struct.
            /// Creates best score cell with the input position values.
            /// </summary>
            /// <param name="row">Row Number.</param>
            /// <param name="column">Column Number.</param>
            /// <param name="cell">Cell Number.</param>
            public OptScoreCell(long row, long column, long cell)
                : this(row, column)
            {
                Cell = cell;
            }

            /// <summary>
            /// Overrides == Operator.
            /// </summary>
            /// <param name="cell1">First cell.</param>
            /// <param name="cell2">Second cell.</param>
            /// <returns>Result of comparison.</returns>
            public static bool operator ==(OptScoreCell cell1, OptScoreCell cell2)
            {
                return
                    cell1.Row == cell2.Row &&
                    cell1.Column == cell2.Column &&
                    cell1.Cell == cell2.Cell;
            }

            /// <summary>
            /// Overrides != Operator.
            /// </summary>
            /// <param name="cell1">First cell.</param>
            /// <param name="cell2">Second cell.</param>
            /// <returns>Result of comparison.</returns>
            public static bool operator !=(OptScoreCell cell1, OptScoreCell cell2)
            {
                return !(cell1 == cell2);
            }

            /// <summary>
            /// Override Equals method.
            /// </summary>
            /// <param name="obj">Object for comparison.</param>
            /// <returns>Result of comparison.</returns>
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
            /// Returns the Hash code.
            /// </summary>
            /// <returns>Hash code of OptScoreCell.</returns>
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
