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
using System.ComponentModel;
using System.Globalization;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
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
        /// Stores details of all cells with best score.
        /// </summary>
        private List<OptScoreCell> optScoreCells = new List<OptScoreCell>();

        /// <summary>
        /// Tracks optimal score for alignment.
        /// </summary>
        private int optScore = int.MinValue;

        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overridden property from the abstract parent.
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
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Col of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void FillCellSimple(long row, long col, long cell)
        {
            int score = SetCellValuesSimple(row, col, cell);

            // SmithWaterman does not use negative scores, instead, if score is <0
            // set scores to 0 and stop the alignment at that point.
            if (score < 0)
            {
                score = 0;
                fSource[cell] = SourceDirection.Stop;
            }

            fScore[col] = score;

            // SmithWaterman traceback begins at cell with optimum score, save it here.
            if (score > optScore)
            {
                // New high score found. Clear old cell lists.
                // Update score and add this cell info
                optScoreCells.Clear();
                optScore = score;
                optScoreCells.Add(new OptScoreCell(row, col, cell));
            }
            else if (score == optScore)
            {
                // One more high scoring cell found.
                // Add cell info to opt score cell list
                optScoreCells.Add(new OptScoreCell(row, col, cell));
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for SmithWaterman - Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Col of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void FillCellAffine(long row, long col, long cell)
        {
            int score = SetCellValuesAffine(row, col, cell);

            // SmithWaterman does not use negative scores, instead, if score is < 0
            // set score to 0 and stop the alignment at that point.
            if (score < 0)
            {
                score = 0;
                fSource[cell] = SourceDirection.Stop;
            }

            maxScore[col] = score;

            // SmithWaterman traceback begins at cell with optimum score, save it here.
            if (score > optScore)
            {
                // New high score found. Clear old cell lists.
                // Update score and add this cell info
                optScoreCells.Clear();
                optScore = score;
                optScoreCells.Add(new OptScoreCell(row, col, cell));
            }
            else if (score == optScore)
            {
                // One more high scoring cell found.
                // Add cell info to opt score cell list
                optScoreCells.Add(new OptScoreCell(row, col, cell));
            }
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth row in SmithWaterman alignment.
        /// Uses one gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionSimple()
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                fScore[col] = 0;
                fSource[col] = SourceDirection.Stop; // no source for cells with 0
            }

            // Optimum score can be anywhere in the matrix.
            // These all have the same score, 0.
            // Track only cells with positive scores.
            optScore = 1;
            optScoreCells.Clear();
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth column in SmithWaterman alignment.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void SetColumnBoundaryConditionSimple(long row, long cell)
        {
            fScoreDiagonal = fScore[0];
            fSource[cell] = SourceDirection.Stop; // no source for cells with 0
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth row in SmithWaterman alignment.
        /// Uses affine gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionAffine()
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                ixGapScore[col] = int.MinValue / 2;
                maxScore[col] = 0;
                fSource[col] = SourceDirection.Stop; // no source for cells with 0
            }

            // Optimum score can be anywhere in the matrix.
            // These all have the same score, 0.
            // Track only cells with positive scores.
            optScore = 1;
            optScoreCells.Clear();
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth column in SmithWaterman alignment.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void SetColumnBoundaryConditionAffine(long row, long cell)
        {
            iyGapScore = int.MinValue / 2;
            maxScoreDiagonal = maxScore[0];
            fSource[cell] = SourceDirection.Stop; // no source for cells with 0
        }

        /// <summary>
        /// Resets the members used to track optimum score and cell.
        /// </summary>
        protected override void ResetSpecificAlgorithmMemberVariables()
        {
            optScoreCells.Clear();
            optScore = int.MinValue;
        }

        /// <summary>
        /// Optimal score updated in FillCellSimple. 
        /// So nothing to be done here.
        /// </summary>
        protected override void SetOptimalScoreSimple() 
        { 
        }

        /// <summary>
        /// Optimal score updated in FillCellAffine. 
        /// So nothing to be done here.
        /// </summary>
        protected override void SetOptimalScoreAffine() 
        { 
        }

        /// <summary>
        /// Performs traceback for SmithWaterman partial alignment.
        /// </summary>
        /// <param name="alignedSequences">List of aligned sequences (output).</param>
        /// <param name="offsets">Offset is the starting position of alignment
        /// of sequence1 with respect to sequence2.</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insetions made to the aligned sequences.</param>
        /// <returns>Optimum score.</returns>
        protected override int Traceback(out List<byte[]> alignedSequences, out List<long> offsets, out List<long> startOffsets, out List<long> endOffsets, out List<long> insertions)
        {
            alignedSequences = new List<byte[]>(optScoreCells.Count * 2);
            offsets = new List<long>(optScoreCells.Count * 2);
            startOffsets = new List<long>(optScoreCells.Count * 2);
            endOffsets = new List<long>(optScoreCells.Count * 2);
            insertions = new List<long>(optScoreCells.Count * 2);

            long col, row;
            foreach (OptScoreCell optCell in optScoreCells)
            {
                // need an array we can extend if necessary
                // aligned array will be backwards, may be longer than original sequence due to gaps
                long guessLen = Math.Max(firstInputSequence.Count, secondInputSequence.Count);

                // TODO: Remove below cast
                List<byte> alignedListA = new List<byte>((int)guessLen);
                List<byte> alignedListB = new List<byte>((int)guessLen);

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
                                alignedListA.Add(firstInputSequence[col - 1]);
                                alignedListB.Add(secondInputSequence[row - 1]);

                                col = col - 1;
                                row = row - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in A
                                alignedListA.Add(gapCode);
                                alignedListB.Add(secondInputSequence[row - 1]);

                                row = row - 1;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in B
                                alignedListA.Add(firstInputSequence[col - 1]);
                                alignedListB.Add(gapCode);

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
                    long cell = optCell.Cell;
                    while (!done)
                    {
                        // if next cell has score 0, we're done
                        switch (fSource[cell])
                        {
                            case SourceDirection.Stop:
                                done = true;
                                break;

                            case SourceDirection.Diagonal:
                                // Diagonal, Aligned
                                alignedListA.Add(firstInputSequence[col - 1]);
                                alignedListB.Add(secondInputSequence[row - 1]);

                                col = col - 1;
                                row = row - 1;
                                cell = cell - numberOfCols - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in A
                                alignedListA.Add(gapCode);
                                alignedListB.Add(secondInputSequence[row - 1]);

                                row = row - 1;
                                cell = cell - numberOfCols;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in B
                                alignedListA.Add(firstInputSequence[col - 1]);
                                alignedListB.Add(gapCode);

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

                // Offset is start of alignment in input sequence with respect to other sequence.
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
                int len = alignedListA.Count;
                byte[] alignedA = new byte[len];
                byte[] alignedB = new byte[len];
                for (i = 0, j = len - 1; i < len; i++, j--)
                {
                    alignedA[i] = alignedListA[j];
                    alignedB[i] = alignedListB[j];
                }

                alignedSequences.Add(alignedA);
                alignedSequences.Add(alignedB);
            }

            return optScore;
        }

        /// <summary>
        /// Fills matrix cell specifically for SmithWaterman - Uses linear gap penalty.
        /// </summary>
        /// <param name="col">Col of cell.</param>
        /// <param name="row">Row of cell.</param>
        protected override void FillCellSimple(long col, long row)
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
            if (score > optScore)
            {
                // New high score found. Clear old cell lists.
                // Update score and add this cell info
                optScoreCells.Clear();
                optScore = score;
                optScoreCells.Add(new OptScoreCell(row, col));
            }
            else if (score == optScore)
            {
                // One more high scoring cell found.
                // Add cell info to opt score cell list
                optScoreCells.Add(new OptScoreCell(row, col));
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for SmithWaterman - Uses affine gap penalty.
        /// </summary>
        /// <param name="col">Cow of cell.</param>
        /// <param name="row">Rol of cell.</param>
        protected override void FillCellAffine(long col, long row)
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
            if (score > optScore)
            {
                // New high score found. Clear old cell lists.
                // Update score and add this cell info
                optScoreCells.Clear();
                optScore = score;
                optScoreCells.Add(new OptScoreCell(row, col));
            }
            else if (score == optScore)
            {
                // One more high scoring cell found.
                // Add cell info to opt score cell list
                optScoreCells.Add(new OptScoreCell(row, col));
            }
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth row and zeroth column in SmithWaterman alignment.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected override void SetBoundaryConditionSimple(long col, long row)
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
                optScore = 1;
                optScoreCells.Clear();
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
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected override void SetBoundaryConditionAffine(long col, long row)
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
                optScore = 1;
                optScoreCells.Clear();
            }

            if (col == 0 && row > 0)
            {
                _Iy[0, row] = int.MinValue / 2;
                _FSimpleSource[0, row] = SourceDirection.Stop; // no source for cells with 0
            }
        }
    }
}
