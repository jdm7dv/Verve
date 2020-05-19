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
using System.ComponentModel;
using System.Globalization;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// Implements the NeedlemanWunsch algorithm for global alignment.
    /// See Chapter 2 in Biological Sequence Analysis; Durbin, Eddy, Krogh and Mitchison; 
    /// Cambridge Press; 1998.
    /// </summary>
    public class NeedlemanWunschAligner : DynamicProgrammingPairwiseAligner
    {
        /// <summary>
        /// Tracks optimal score for alignment
        /// </summary>
        private int _optScore = int.MinValue;

        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns the Name of our algorithm i.e 
        /// Needleman-Wunsch algorithm.
        /// </summary>
        public override string Name
        {
            get
            {
                return Properties.Resource.NEEDLEMAN_NAME;
            }
        }

        /// <summary>
        /// Gets the description of the NeedlemanWunsch algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns a simple description of what 
        /// NeedlemanWunschAligner class implements.
        /// </summary>
        public override string Description
        {
            get
            {
                return Properties.Resource.NEEDLEMAN_DESCRIPTION;
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for NeedlemanWunsch
        /// Required because method is abstract in DynamicProgrammingPairwise
        /// To be removed once changes are made in SW, Pairwise algorithms
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        protected override void FillCellSimple(int row, int col, int cell)
        {
            _FScore[col] = SetCellValuesSimple(row, col, cell);
        }
        
        /// <summary>
        /// Sets the score in last cell of _FScore to be the optimal score
        /// </summary>
        protected override void SetOptimalScoreSimple()
        {
            // Traceback starts at lower right corner.
            // Save the score from this point.
            _optScore = _FScore[_nCols - 1];
        }

        /// <summary>
        /// Sets the score in last cell of _MaxScore to be the optimal score
        /// </summary>
        protected override void SetOptimalScoreAffine()
        {
            // Traceback starts at lower right corner.
            // Save the score from this point.
            _optScore = _MaxScore[_nCols - 1];
        }

        /// <summary>
        /// Fills matrix cell specifically for NeedlemanWunsch
        /// Required because method is abstract in DynamicProgrammingPairwise
        /// To be removed once changes are made in SW, Pairwise algorithms
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        protected override void FillCellAffine(int row, int col, int cell)
        {
            _MaxScore[col] = SetCellValuesAffine(row, col, cell);
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth row in NeedlemanWunsch global alignment.
        /// Uses single gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionSimple()
        {
            for (int col = 0; col < _nCols; col++)
            {
                _FScore[col] = col * _gapOpenCost;
                _FSource[col] = SourceDirection.Left; 
            }

            _FScore[0] = _gapOpenCost;
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth column in NeedlemanWunsch global alignment.
        /// Uses single gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">cell number</param>
        protected override void SetColumnBoundaryConditionSimple(int row, int cell)
        {
            _FScore[0] = row * _gapOpenCost; // (row, 0) for F Matrix is set
            _FScoreDiagonal = (row - 1) * _gapOpenCost; // _FScoreDiagonal is set to previous row's _FScore[0]
            _FSource[cell] = SourceDirection.Up; 
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth row in NeedlemanWunsch global alignment.
        /// Uses affine gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionAffine()
        {
            // Column 0
            _IxGapScore[0] = int.MinValue / 2;
            _MaxScore[0] = 0;
            _FSource[0] = SourceDirection.Left;

            for (int col = 1; col < _nCols; col++)
            {
                _IxGapScore[col] = int.MinValue / 2;
                _MaxScore[col] = _gapOpenCost + ((col - 1) * _gapExtensionCost);
                _FSource[col] = SourceDirection.Left;
            }
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth column in NeedlemanWunsch global alignment.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">cell number</param>
        protected override void SetColumnBoundaryConditionAffine(int row, int cell)
        {
            _IyGapScore = int.MinValue / 2; // Iy set to -infinity
            _MaxScoreDiagonal = _MaxScore[0]; // stored 0th cell of previous row.

            // sets (row, 0) for _MaxScore
            _MaxScore[0] = _gapOpenCost + ((row - 1) * _gapExtensionCost); 
            _FSource[cell] = SourceDirection.Up; 
        }

        /// <summary>
        /// Resets the members used to track optimum score and cell.
        /// </summary>
        protected override void ResetSpecificAlgorithmMemberVariables()
        {
            // Not strictly necessary since this will be set in the FillCell methods, 
            // but it is good practice to initialize correctly.
            this._optScore = int.MinValue;
        }

        /// <summary>
        /// Performs traceback for global alignment.
        /// </summary>
        /// <param name="alignedSequences">List of aligned sequences (output)</param>
        /// <param name="offsets">Offset is the starting position of alignment
        /// of sequence1 with respect to sequence2</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insetions made to the aligned sequences.</param>
        /// <returns>Optimum score.</returns>
        protected override int Traceback(out List<byte[]> alignedSequences, out List<int> offsets, out List<int> startOffsets, out List<int> endOffsets, out List<int> insertions)
        {
            alignedSequences = new List<byte[]>(2);
            startOffsets = new List<int>(2);
            endOffsets = new List<int>(2);
            insertions = new List<int>(2);


            // For NW, aligned sequence will be at least as long as longest input sequence.
            // May be longer if there are gaps in both aligned sequences.
            int guessLen = Math.Max(_a.Length, _b.Length);

            endOffsets.Add(_a.Length -1);
            endOffsets.Add(_b.Length -1);

            List<byte> aAlignedList = new List<byte>(guessLen);
            List<byte> bAlignedList = new List<byte>(guessLen);

            // Start at the bottom left element of F and work backwards until we get to upper left
            int col, row, cell;
            col = _nCols - 1;
            row = _nRows - 1;
            cell = (_nCols * _nRows) - 1;

            int colGaps = 0;
            int rowGaps = 0;
            // stop when col and row are both zero
            while (cell > 0) 
            {
                switch (_FSource[cell])
                {
                    case SourceDirection.Diagonal:
                        {
                            // diagonal, no gap, use both sequence residues
                            aAlignedList.Add(_a[col - 1]);
                            bAlignedList.Add(_b[row - 1]);
                            col = col - 1;
                            row = row - 1;
                            cell = cell - _nCols - 1;
                            break;
                        }

                    case SourceDirection.Up:
                        {
                            // up, gap in a
                            aAlignedList.Add(GapCode);
                            bAlignedList.Add(_b[row - 1]);
                            row = row - 1;
                            cell = cell - _nCols;
                            colGaps++;
                            break;
                        }

                    case SourceDirection.Left:
                        {
                            // left, gap in b
                            aAlignedList.Add(_a[col - 1]);
                            bAlignedList.Add(GapCode);
                            col = col - 1;
                            cell = cell - 1;
                            rowGaps++;
                            break;
                        }

                    default:
                        {
                            string message = string.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.TracebackBadSource,
                                "NeedlemanWunschAligner");
                            Trace.Report(message);
                            throw new InvalidEnumArgumentException(message);
                        }
                }
            }

            // Prepare solution, copy diagnostic data, turn aligned sequences around, etc
            // Be nice, turn aligned solutions around so that they match the input sequences
            int i, j; // utility indices used to reverse aligned sequences
            int len = aAlignedList.Count;
            byte[] aAligned = new byte[len];
            byte[] bAligned = new byte[len];
            for (i = 0, j = len - 1; i < len; i++, j--)
            {
                aAligned[i] = aAlignedList[j];
                bAligned[i] = bAlignedList[j];
            }

            alignedSequences.Add(aAligned);
            alignedSequences.Add(bAligned);

            insertions.Add(colGaps);
            insertions.Add(rowGaps);

            offsets = new List<int>(2);
            offsets.Add(GetOffset(aAligned) - GetOffset(_a));
            offsets.Add(GetOffset(bAligned) - GetOffset(_b));

            startOffsets.Add(0);
            startOffsets.Add(0);
            return this._optScore;
        }
    }
}
