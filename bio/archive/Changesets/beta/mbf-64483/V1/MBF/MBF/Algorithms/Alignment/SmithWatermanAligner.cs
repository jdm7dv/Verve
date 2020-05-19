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
    /// Implements the SmithWaterman algorithm for partial alignment.
    /// See Chapter 2 in Biological Sequence Analysis; Durbin, Eddy, Krogh and Mitchison; 
    /// Cambridge Press; 1998.
    /// </summary>
    public class SmithWatermanAligner : DynamicProgrammingPairwiseAligner
    {
        // SW begins traceback at cell with optimum score.  Use these variables
        // to track this in FillCell overrides.

        /// <summary>
        /// Stores details of all cells with best score
        /// </summary>
        private List<OptScoreCell> _optScoreCells = new List<OptScoreCell>();

        /// <summary>
        /// Tracks optimal score for alignment
        /// </summary>
        private int _optScore = int.MinValue;

        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns the Name of our algorithm i.e 
        /// Smith-Waterman algorithm.
        /// </summary>
        public override string Name
        {
            get
            {
                return Properties.Resource.SMITH_NAME;
            }
        }

        /// <summary>
        /// Gets the Description of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns a simple description of what 
        /// SmithWatermanAligner class implements.
        /// </summary>
        public override string Description
        {
            get
            {
                return Properties.Resource.SMITH_DESCRIPTION;
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for SmithWaterman - Uses linear gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        protected override void FillCellSimple(int row, int col, int cell)
        {
            int score = SetCellValuesSimple(row, col, cell);

            // SmithWaterman does not use negative scores, instead, if score is <0
            // set scores to 0 and stop the alignment at that point.
            if (score < 0)
            {
                score = 0;
                _FSource[cell] = SourceDirection.Stop;
            }

            _FScore[col] = score;

            // SmithWaterman traceback begins at cell with optimum score, save it here.
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

        /// <summary>
        /// Fills matrix cell specifically for SmithWaterman - Uses affine gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        /// <param name="cell">cell number</param>
        protected override void FillCellAffine(int row, int col, int cell)
        {
            int score = SetCellValuesAffine(row, col, cell);

            // SmithWaterman does not use negative scores, instead, if score is < 0
            // set score to 0 and stop the alignment at that point.
            if (score < 0)
            {
                score = 0;
                _FSource[cell] = SourceDirection.Stop;
            }

            _MaxScore[col] = score;

            // SmithWaterman traceback begins at cell with optimum score, save it here.
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

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth row in SmithWaterman alignment.
        /// Uses one gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionSimple()
        {
            for (int col = 0; col < _nCols; col++)
            {
                _FScore[col] = 0;
                _FSource[col] = SourceDirection.Stop; // no source for cells with 0
            }

            // Optimum score can be anywhere in the matrix.
            // These all have the same score, 0.
            // Track only cells with positive scores.
            _optScore = 1;
            _optScoreCells.Clear();
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth column in SmithWaterman alignment.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">cell number</param>
        protected override void SetColumnBoundaryConditionSimple(int row, int cell)
        {
            _FScoreDiagonal = _FScore[0];
            _FSource[cell] = SourceDirection.Stop; // no source for cells with 0
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth row in SmithWaterman alignment.
        /// Uses affine gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionAffine()
        {
            for (int col = 0; col < _nCols; col++)
            {
                _IxGapScore[col] = int.MinValue / 2;
                _MaxScore[col] = 0;
                _FSource[col] = SourceDirection.Stop; // no source for cells with 0
            }

            // Optimum score can be anywhere in the matrix.
            // These all have the same score, 0.
            // Track only cells with positive scores.
            _optScore = 1;
            _optScoreCells.Clear();
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth column in SmithWaterman alignment.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">cell number</param>
        protected override void SetColumnBoundaryConditionAffine(int row, int cell)
        {
            _IyGapScore = int.MinValue / 2;
            _MaxScoreDiagonal = _MaxScore[0];
            _FSource[cell] = SourceDirection.Stop; // no source for cells with 0
        }

        /// <summary>
        /// Resets the members used to track optimum score and cell.
        /// </summary>
        protected override void ResetSpecificAlgorithmMemberVariables()
        {
            _optScoreCells.Clear();
            _optScore = int.MinValue;
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
        /// Performs traceback for SmithWaterman partial alignment.
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

            int col, row;
            foreach (OptScoreCell optCell in _optScoreCells)
            {
                // need an array we can extend if necessary
                // aligned array will be backwards, may be longer than original sequence due to gaps
                int guessLen = Math.Max(_a.Length, _b.Length);
                List<byte> aAlignedList = new List<byte>(guessLen);
                List<byte> bAlignedList = new List<byte>(guessLen);

                // Start at the optimum element of F and work backwards
                col = optCell.Column;
                row = optCell.Row;
                endOffsets.Add(col - 1);
                endOffsets.Add(row - 1);

                int colGaps = 0;
                int rowGaps = 0;

                bool done = false;
                if (UseEARTHToFillMatrix)
                {
                    while (!done)
                    {
                        // if next cell has score 0, we're done
                        switch (_FSimpleSource[col, row])
                        {
                            case SourceDirection.Stop:
                                done = true;
                                break;

                            case SourceDirection.Diagonal:
                                // Diagonal, Aligned
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(_b[row - 1]);
                                col = col - 1;
                                row = row - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in A
                                aAlignedList.Add(GapCode);
                                bAlignedList.Add(_b[row - 1]);
                                row = row - 1;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in B
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(GapCode);
                                col = col - 1;
                                rowGaps++;
                                break;

                            default:
                                // error condition, should never see this
                                string message = string.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.TracebackBadSource,
                                    "SmithWatermanAligner");
                                Trace.Report(message);
                                throw new InvalidEnumArgumentException(message);
                        }
                    }
                }
                else
                {
                    int cell = optCell.Cell;
                    while (!done)
                    {
                        // if next cell has score 0, we're done
                        switch (_FSource[cell])
                        {
                            case SourceDirection.Stop:
                                done = true;
                                break;

                            case SourceDirection.Diagonal:
                                // Diagonal, Aligned
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(_b[row - 1]);
                                col = col - 1;
                                row = row - 1;
                                cell = cell - _nCols - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in A
                                aAlignedList.Add(GapCode);
                                bAlignedList.Add(_b[row - 1]);
                                row = row - 1;
                                cell = cell - _nCols;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in B
                                aAlignedList.Add(_a[col - 1]);
                                bAlignedList.Add(GapCode);
                                col = col - 1;
                                cell = cell - 1;
                                rowGaps++;
                                break;

                            default:
                                // error condition, should never see this
                                string message = string.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.TracebackBadSource,
                                    "SmithWatermanAligner");
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

                // prepare solution, copy diagnostic data, turn aligned sequences around, etc
                // Be nice, turn aligned solutions around so that they match the input sequences
                int i, j; // utility indices used to invert aligned sequences
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
        /// Fills matrix cell specifically for SmithWaterman - Uses linear gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        protected override void FillCellSimple(int col, int row)
        {
            int score = SetCellValuesSimple(col, row);

            // SmithWaterman does not use negative scores, instead, if score is <0
            // set scores to 0 and stop the alignment at that point.
            if (score < 0)
            {
                score = 0;
                _FSimpleSource[col, row] = SourceDirection.Stop;
            }

            _FSimpleScore[col, row] = score;

            // SmithWaterman traceback begins at cell with optimum score, save it here.
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

        /// <summary>
        /// Fills matrix cell specifically for SmithWaterman - Uses affine gap penalty.
        /// </summary>
        /// <param name="row">row of cell</param>
        /// <param name="col">col of cell</param>
        protected override void FillCellAffine(int col, int row)
        {
            int score = SetCellValuesAffine(col, row);

            // SmithWaterman does not use negative scores, instead, if score is < 0
            // set score to 0 and stop the alignment at that point.
            if (score < 0)
            {
                score = 0;
                _FSimpleSource[col, row] = SourceDirection.Stop;
            }

            _M[col, row] = score;

            // SmithWaterman traceback begins at cell with optimum score, save it here.
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

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth row and zeroth column in SmithWaterman alignment.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected override void SetBoundaryConditionSimple(int col, int row)
        {
            if (col >= 0 && row == 0)
            {
                _FSimpleScore[col, 0] = 0;
                _FSimpleSource[col, 0] = SourceDirection.Stop; // no source for cells with 0
            }

            if (col == 0 && row == 0)
            {
                // Optimum score can be anywhere in the matrix.
                // These all have the same score, 0.
                // Track only cells with positive scores.
                _optScore = 1;
                _optScoreCells.Clear();
            }

            if (col == 0 && row > 0)
            {
                _FSimpleSource[0, row] = SourceDirection.Stop; // no source for cells with 0
            }
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth row and zeroth column in SmithWaterman alignment.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="col">col of cell to fill</param>
        /// <param name="row">row of cell to fill</param>
        protected override void SetBoundaryConditionAffine(int col, int row)
        {
            if (col >= 0 && row == 0)
            {
                _Ix[col, 0] = int.MinValue / 2;
                _M[col, 0] = 0;
                _FSimpleSource[col, 0] = SourceDirection.Stop; // no source for cells with 0
            }

            if (col == 0 && row == 0)
            {
                // Optimum score can be anywhere in the matrix.
                // These all have the same score, 0.
                // Track only cells with positive scores.
                _optScore = 1;
                _optScoreCells.Clear();
            }

            if (col == 0 && row > 0)
            {
                _Iy[0, row] = int.MinValue / 2;
                _FSimpleSource[0, row] = SourceDirection.Stop; // no source for cells with 0
            }
        }
    }
}
