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
using System.ComponentModel;
using System.Globalization;
using MBF.Util.Logging;

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// Implements the pairwise overlap alignment algorithm described in Chapter 2 of
    /// Biological Sequence Analysis; Durbin, Eddy, Krogh and Mitchison; Cambridge Press; 1998.
    /// </summary>
    public class PairwiseOverlapAligner : DynamicProgrammingPairwiseAligner
    {
        // Overlap begins traceback at cell on last row or col with best score.  
        // Use these variables to track this in SetCell.

        /// <summary>
        /// Stores optimal score
        /// </summary>
        private int _optScore = int.MinValue;

        /// <summary>
        /// Stores details of all cells with best score
        /// </summary>
        private List<OptScoreCell> _optScoreCells = new List<OptScoreCell>();

        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns the Name of our algorithm i.e 
        /// Pairwise-Overlap algorithm.
        /// </summary>
        public override string Name
        {
            get
            {
                return Properties.Resource.PAIRWISE_NAME;
            }
        }

        /// <summary>
        /// Gets the description of the Pairwise-Overlap algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns a simple description of what 
        /// PairwiseOverlapAligner class implements.
        /// </summary>
        public override string Description
        {
            get
            {
                return Properties.Resource.PAIRWISE_DESCRIPTION;
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for Overlap – uses linear gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        protected override void FillCellSimple(int row, int col, int cell)
        {
            int score = SetCellValuesSimple(row, col, cell);
            _FScore[col] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == _nCols - 1 || row == _nRows - 1)
            {
                // Cell is in last column or row
                if (score > _optScore)
                {
                    // New high score found. Clear old cell lists.
                    // Update score and add this cell info
                    _optScoreCells.Clear();
                    _optScore = score;
                    _optScoreCells.Add(new OptScoreCell(row, col, cell));
                }
                else if (score == _optScore)
                {
                    // One more high scoring cell found.
                    // Add cell info to opt score cell list
                    _optScoreCells.Add(new OptScoreCell(row, col, cell));
                }
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for Overlap – Uses affine gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        protected override void FillCellAffine(int row, int col, int cell)
        {
            int score = SetCellValuesAffine(row, col, cell);
            _MaxScore[col] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == _nCols - 1 || row == _nRows - 1)
            {
                // Cell is in last column or row
                if (score > _optScore)
                {
                    // New high score found. Clear old cell lists.
                    // Update score and add this cell info
                    _optScoreCells.Clear();
                    _optScore = score;
                    _optScoreCells.Add(new OptScoreCell(row, col, cell));
                }
                else if (score == _optScore)
                {
                    // One more high scoring cell found.
                    // Add cell info to opt score cell list
                    _optScoreCells.Add(new OptScoreCell(row, col, cell));
                }
            }
        }

        /// <summary>
        /// Sets F matrix boundary condition for pairwise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses one gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionSimple()
        {
            for (int col = 0; col < _nCols; col++)
            {
                _FScore[col] = 0;

                // No source for cells, but set to up so that we can easily fill end 
                // gaps when doing traceback.
                _FSource[col] = SourceDirection.Left;
            }

            // Optimum score can be on a boundary.
            // Possible cells are col 0, last row; and last col, row 0.
            // These all have the same score, which is 0.
            // Track only cells with positive scores.
            _optScore = 1;
            _optScoreCells.Clear();
        }

        /// <summary>
        /// Sets F matrix boundary condition for pairwise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">cell number</param>
        protected override void SetColumnBoundaryConditionSimple(int row, int cell)
        {
            _FScoreDiagonal = _FScore[0];

            // No source for cells, but set to left so that we can easily fill end 
            // gaps when doing traceback.
            _FSource[cell] = SourceDirection.Up;
        }

        /// <summary>
        /// Sets matrix boundary conditions for pairwise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses affine gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionAffine()
        {
            for (int col = 0; col < _nCols; col++)
            {
                _MaxScore[col] = 0;
                _IxGapScore[col] = int.MinValue / 2;

                // No source for cells, but set to up so that we can easily fill end 
                // gaps when doing traceback.
                _FSource[col] = SourceDirection.Left;
            }

            // Optimum score can be on a boundary.
            // Possible cells are col 0, last row; and last col, row 0.
            // These all have the same score, which is 0.
            _optScore = 1;
            _optScoreCells.Clear();
        }

        /// <summary>
        /// Sets matrix boundary conditions for pairwise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">cell number</param>
        protected override void SetColumnBoundaryConditionAffine(int row, int cell)
        {
            _MaxScoreDiagonal = _MaxScore[0];
            _IyGapScore = int.MinValue / 2;  // should be -infinity, this value avoids underflow problems

            // No source for cells, but set to left so that we can easily fill end 
            // gaps when doing traceback.
            _FSource[cell] = SourceDirection.Up;
        }

        /// <summary>
        /// Optimal score updated in FillCellSimple. 
        /// So nothing to be done here
        /// </summary>
        protected override void SetOptimalScoreSimple() { }

        /// <summary>
        /// Optimal score updated in FillCellAffine. 
        /// So nothing to be done here
        /// </summary>
        protected override void SetOptimalScoreAffine() { }

        /// <summary>
        /// Resets the members used to track optimum score and cell.
        /// </summary>
        protected override void ResetSpecificAlgorithmMemberVariables()
        {
            _optScoreCells.Clear();
            _optScore = int.MinValue;
        }

        /// <summary>
        /// Performs traceback step for pairwise overlap alignment.
        /// </summary>
        /// <param name="alignedSequences">List of aligned sequences (output)</param>
        /// <param name="offsets">Offset is the starting position of alignment 
        /// of sequence1 with respect to sequence2.</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insetions made to the aligned sequences.</param>
        /// <returns>Optimum score.</returns>
        protected override int Traceback(out List<byte[]> alignedSequences, out List<int> offsets, out List<int> startOffsets, out List<int> endOffsets, out List<int> insertions)
        {
            alignedSequences = new List<byte[]>(_optScoreCells.Count * 2);
            offsets = new List<int>(_optScoreCells.Count * 2);
            startOffsets = new List<int>(_optScoreCells.Count * 2);
            endOffsets = new List<int>(_optScoreCells.Count * 2);
            insertions = new List<int>(_optScoreCells.Count * 2);

            // Find element with best score among the elements in the last column and bottom row.
            // Include top row and last column (that is, the boundary condition cells) since all gaps before the match is a valid result.

            int col, row;

            foreach (OptScoreCell optCell in _optScoreCells)
            {
                // Start at the best element found above and work backwards until we get the top row or left side
                // aligned array will be backwards, may be longer then original sequence due to gaps
                int guessLen = Math.Max(_a.Length, _b.Length);
                List<byte> aAlignedList = new List<byte>(guessLen);
                List<byte> bAlignedList = new List<byte>(guessLen);

                // Fill right end of overlap sequences with gaps as necessary before we get to the matching region.
                col = optCell.Column;
                row = optCell.Row;
                endOffsets.Add(col - 1);
                endOffsets.Add(row - 1);

                int colGaps = 0;
                int rowGaps = 0;
                // Fill matching section of overlap alignment until we reach top row
                // or left column.
                // stop when col or row are zero
                if (UseEARTHToFillMatrix)
                {
                    while (col > 0 && row > 0)
                    {
                        switch (_FSimpleSource[col, row])
                        {
                            case SourceDirection.Diagonal:
                                // Diagonal, Aligned
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(_b[row - 1]);
                                col = col - 1;
                                row = row - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in a
                                aAlignedList.Add(GapCode);
                                bAlignedList.Add(_b[row - 1]);
                                row = row - 1;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in b
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(GapCode);
                                col = col - 1;
                                rowGaps++;
                                break;

                            default:
                                string message = string.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.TracebackBadSource,
                                    "Overlap aligner");
                                Trace.Report(message);
                                throw new InvalidEnumArgumentException(message);
                        }
                    }
                }
                else
                {
                    int cell = optCell.Cell;
                    while (col > 0 && row > 0)
                    {
                        switch (_FSource[cell])
                        {
                            case SourceDirection.Diagonal:
                                // Diagonal, Aligned
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(_b[row - 1]);
                                col = col - 1;
                                row = row - 1;
                                cell = cell - _nCols - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in a
                                aAlignedList.Add(GapCode);
                                bAlignedList.Add(_b[row - 1]);
                                row = row - 1;
                                cell = cell - _nCols;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in b
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(GapCode);
                                col = col - 1;
                                cell = cell - 1;
                                rowGaps++;
                                break;

                            default:
                                string message = string.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.TracebackBadSource,
                                    "Overlap aligner");
                                Trace.Report(message);
                                throw new InvalidEnumArgumentException(message);
                        }
                    }
                }
                // Offset is start of alignment in input sequnce with respect to other sequence.
                if (col - row >= 0)
                {
                    offsets.Add(0);
                    offsets.Add(col - row);
                }
                else
                {
                    offsets.Add(-(col - row));
                    offsets.Add(0);
                }

                startOffsets.Add(col);
                startOffsets.Add(row);

                insertions.Add(colGaps);
                insertions.Add(rowGaps);

                // Prepare solution, copy diagnostic data, turn aligned sequences around, etc
                // Be nice, turn aligned solutions around so that they match the input sequences
                int i, j; // utility indices used to inverts aligned sequences
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
            }

            return _optScore;
        }

        /// <summary>
        /// Fills matrix cell specifically for Overlap – uses linear gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        protected override void FillCellSimple(int col, int row)
        {
            int score = SetCellValuesSimple(col, row);
            _FSimpleScore[col, row] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == _nCols - 1 || row == _nRows - 1)
            {
                // Cell is in last column or row
                if (score > _optScore)
                {
                    // New high score found. Clear old cell lists.
                    // Update score and add this cell info
                    _optScoreCells.Clear();
                    _optScore = score;
                    _optScoreCells.Add(new OptScoreCell(row, col));
                }
                else if (score == _optScore)
                {
                    // One more high scoring cell found.
                    // Add cell info to opt score cell list
                    _optScoreCells.Add(new OptScoreCell(row, col));
                }
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for Overlap – Uses affine gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        protected override void FillCellAffine(int col, int row)
        {
            int score = SetCellValuesAffine(col, row);
            _M[col, row] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == _nCols - 1 || row == _nRows - 1)
            {
                // Cell is in last column or row
                if (score > _optScore)
                {
                    // New high score found. Clear old cell lists.
                    // Update score and add this cell info
                    _optScoreCells.Clear();
                    _optScore = score;
                    _optScoreCells.Add(new OptScoreCell(row, col));
                }
                else if (score == _optScore)
                {
                    // One more high scoring cell found.
                    // Add cell info to opt score cell list
                    _optScoreCells.Add(new OptScoreCell(row, col));
                }
            }
        }

        /// <summary>
        /// Sets F matrix boundary condition for pairwise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected override void SetBoundaryConditionSimple(int col, int row)
        {
            if (col >= 0 && row == 0)
            {
                _FSimpleScore[col, 0] = 0;

                // No source for cells, but set to up so that we can easily fill end 
                // gaps when doing traceback.
                _FSimpleSource[col, 0] = SourceDirection.Left;
            }

            if (col == 0 && row == 0)
            {
                // Optimum score can be on a boundary.
                // Possible cells are col 0, last row; and last col, row 0.
                // These all have the same score, which is 0.
                // Track only cells with positive scores.
                _optScore = 1;
                _optScoreCells.Clear();
            }

            if (col == 0 && row > 0)
            {
                // No source for cells, but set to left so that we can easily fill end 
                // gaps when doing traceback.
                _FSimpleSource[0, row] = SourceDirection.Up;
            }
        }

        /// <summary>
        /// Sets matrix boundary conditions for pairwise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected override void SetBoundaryConditionAffine(int col, int row)
        {
            if (col >= 0 && row == 0)
            {
                _M[col, 0] = 0;
                _Ix[col, 0] = int.MinValue / 2;

                // No source for cells, but set to up so that we can easily fill end 
                // gaps when doing traceback.
                _FSimpleSource[col, 0] = SourceDirection.Left;
            }

            if (col == 0 && row == 0)
            {
                // Optimum score can be on a boundary.
                // Possible cells are col 0, last row; and last col, row 0.
                // These all have the same score, which is 0.
                _optScore = 1;
                _optScoreCells.Clear();
            }

            if (col == 0 && row > 0)
            {
                _Iy[0, row] = int.MinValue / 2;  // should be -infinity, this value avoids underflow problems

                // No source for cells, but set to left so that we can easily fill end 
                // gaps when doing traceback.
                _FSimpleSource[0, row] = SourceDirection.Up;
            }
        }
    }
}
