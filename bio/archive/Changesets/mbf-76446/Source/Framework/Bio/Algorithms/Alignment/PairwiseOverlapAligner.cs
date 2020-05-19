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
    /// Implements the pair-wise overlap alignment algorithm described in Chapter 2 of
    /// Biological Sequence Analysis; Durbin, Eddy, Krogh and Mitchison; Cambridge Press; 1998.
    /// </summary>
    public class PairwiseOverlapAligner : DynamicProgrammingPairwiseAligner
    {
        #region Member Variables
        /// <summary>
        /// Stores optimal score
        /// </summary>
        private int optScore = int.MinValue;

        /// <summary>
        /// Stores details of all cells with best score
        /// </summary>
        private List<OptScoreCell> optScoreCells = new List<OptScoreCell>();
        #endregion Member Variables

        #region Properties
        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overridden property from the abstract parent.
        /// This property returns the Name of our algorithm i.e 
        /// pair-wise-Overlap algorithm.
        /// </summary>
        public override string Name
        {
            get
            {
                return Properties.Resource.PAIRWISE_NAME;
            }
        }

        /// <summary>
        /// Gets the description of the pair-wise-Overlap algorithm used.
        /// This is a overridden property from the abstract parent.
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
        #endregion Properties

        #region Methods
        /// <summary>
        /// Fills matrix cell specifically for Overlap – uses linear gap penalty.
        /// </summary>
        /// <param name="row">Row of cell</param>
        /// <param name="col">Col of cell</param>
        /// <param name="cell">Cell number</param>
        protected override void FillCellSimple(long row, long col, long cell)
        {
            int score = SetCellValuesSimple(row, col, cell);
            fScore[col] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == numberOfCols - 1 || row == numberOfRows - 1)
            {
                // Cell is in last column or row
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
        }

        /// <summary>
        /// Fills matrix cell specifically for Overlap – Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row of cell</param>
        /// <param name="col">Col of cell</param>
        /// <param name="cell">Cell number</param>
        protected override void FillCellAffine(long row, long col, long cell)
        {
            int score = SetCellValuesAffine(row, col, cell);
            maxScore[col] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == numberOfCols - 1 || row == numberOfRows - 1)
            {
                // Cell is in last column or row
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
        }

        /// <summary>
        /// Sets F matrix boundary condition for pair-wise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses one gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionSimple()
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                fScore[col] = 0;

                // No source for cells, but set to up so that we can easily fill end 
                // gaps when doing traceback.
                fSource[col] = SourceDirection.Left;
            }

            // Optimum score can be on a boundary.
            // Possible cells are col 0, last row; and last col, row 0.
            // These all have the same score, which is 0.
            // Track only cells with positive scores.
            optScore = 1;
            optScoreCells.Clear();
        }

        /// <summary>
        /// Sets F matrix boundary condition for pair-wise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">Cell number</param>
        protected override void SetColumnBoundaryConditionSimple(long row, long cell)
        {
            fScoreDiagonal = fScore[0];

            // No source for cells, but set to left so that we can easily fill end 
            // gaps when doing traceback.
            fSource[cell] = SourceDirection.Up;
        }

        /// <summary>
        /// Sets matrix boundary conditions for pair-wise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses affine gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionAffine()
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                maxScore[col] = 0;
                ixGapScore[col] = int.MinValue / 2;

                // No source for cells, but set to up so that we can easily fill end 
                // gaps when doing traceback.
                fSource[col] = SourceDirection.Left;
            }

            // Optimum score can be on a boundary.
            // Possible cells are col 0, last row; and last col, row 0.
            // These all have the same score, which is 0.
            optScore = 1;
            optScoreCells.Clear();
        }

        /// <summary>
        /// Sets matrix boundary conditions for pair-wise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell</param>
        /// <param name="cell">Cell number</param>
        protected override void SetColumnBoundaryConditionAffine(long row, long cell)
        {
            maxScoreDiagonal = maxScore[0];
            iyGapScore = int.MinValue / 2;  // should be -infinity, this value avoids underflow problems

            // No source for cells, but set to left so that we can easily fill end 
            // gaps when doing traceback.
            fSource[cell] = SourceDirection.Up;
        }

        /// <summary>
        /// Optimal score updated in FillCellSimple. 
        /// So nothing to be done here
        /// </summary>
        protected override void SetOptimalScoreSimple() 
        {
        }

        /// <summary>
        /// Optimal score updated in FillCellAffine. 
        /// So nothing to be done here
        /// </summary>
        protected override void SetOptimalScoreAffine() 
        {
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
        /// Performs traceback step for pair-wise overlap alignment.
        /// </summary>
        /// <param name="alignedSequences">List of aligned sequences (output)</param>
        /// <param name="offsets">Offset is the starting position of alignment 
        /// of sequence1 with respect to sequence2.</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insertions made to the aligned sequences.</param>
        /// <returns>Optimum score.</returns>
        protected override int Traceback(out List<byte[]> alignedSequences, out List<long> offsets, out List<long> startOffsets, out List<long> endOffsets, out List<long> insertions)
        {
            alignedSequences = new List<byte[]>(optScoreCells.Count * 2);
            offsets = new List<long>(optScoreCells.Count * 2);
            startOffsets = new List<long>(optScoreCells.Count * 2);
            endOffsets = new List<long>(optScoreCells.Count * 2);
            insertions = new List<long>(optScoreCells.Count * 2);

            // Find element with best score among the elements in the last column and bottom row.
            // Include top row and last column (that is, the boundary condition cells) since all gaps before the match is a valid result.
            long col, row;

            foreach (OptScoreCell optCell in optScoreCells)
            {
                // Start at the best element found above and work backwards until we get the top row or left side
                // aligned array will be backwards, may be longer then original sequence due to gaps
                List<byte> firstAlignedList = new List<byte>();
                List<byte> secondAlignedList = new List<byte>();
               
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
                                firstAlignedList.Add(firstInputSequence[col - 1]);
                                secondAlignedList.Add(secondInputSequence[row - 1]);
                                col = col - 1;
                                row = row - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in a
                                firstAlignedList.Add(gapCode);
                                secondAlignedList.Add(secondInputSequence[row - 1]);
                                row = row - 1;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in b
                                firstAlignedList.Add(firstInputSequence[col - 1]);
                                secondAlignedList.Add(gapCode);
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
                    long cell = optCell.Cell;
                    while (col > 0 && row > 0)
                    {
                        switch (fSource[cell])
                        {
                            case SourceDirection.Diagonal:
                                // Diagonal, Aligned
                                firstAlignedList.Add(firstInputSequence[col - 1]);
                                secondAlignedList.Add(secondInputSequence[row - 1]);
                                col = col - 1;
                                row = row - 1;
                                cell = cell - numberOfCols - 1;
                                break;

                            case SourceDirection.Up:
                                // up, gap in a
                                firstAlignedList.Add(gapCode);
                                secondAlignedList.Add(secondInputSequence[row - 1]);
                                row = row - 1;
                                cell = cell - numberOfCols;
                                colGaps++;
                                break;

                            case SourceDirection.Left:
                                // left, gap in b
                                firstAlignedList.Add(firstInputSequence[col - 1]);
                                secondAlignedList.Add(gapCode);
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

                // Prepare solution, copy diagnostic data, turn aligned sequences around, etc
                // Be nice, turn aligned solutions around so that they match the input sequences
                int i, j; // utility indices used to inverts aligned sequences
                int len = firstAlignedList.Count;
                byte[] firstAligned = new byte[len];
                byte[] secondAligned = new byte[len];
                for (i = 0, j = len - 1; i < len; i++, j--)
                {
                    firstAligned[i] = firstAlignedList[j];
                    secondAligned[i] = secondAlignedList[j];
                }

                alignedSequences.Add(firstAligned);
                alignedSequences.Add(secondAligned);
            }

            return optScore;
        }

        /// <summary>
        /// Fills matrix cell specifically for Overlap – uses linear gap penalty.
        /// </summary>
        /// <param name="col">Col of cell</param>
        /// <param name="row">Row of cell</param>
        protected override void FillCellSimple(long col, long row)
        {
            int score = SetCellValuesSimple(col, row);
            _FSimpleScore[col, row] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == numberOfCols - 1 || row == numberOfRows - 1)
            {
                // Cell is in last column or row
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
        }

        /// <summary>
        /// Fills matrix cell specifically for Overlap – Uses affine gap penalty.
        /// </summary>
        /// <param name="col">Col of cell</param>
        /// <param name="row">Row of cell</param>
        protected override void FillCellAffine(long col, long row)
        {
            int score = SetCellValuesAffine(col, row);
            _M[col, row] = score;

            // Overlap uses cell in last row or col with best score as starting point.
            if (col == numberOfCols - 1 || row == numberOfRows - 1)
            {
                // Cell is in last column or row
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
        }

        /// <summary>
        /// Sets F matrix boundary condition for pair-wise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses one gap penalty.
        /// </summary>
        /// <param name="col">Col of cell to fill</param>
        /// <param name="row">Row of cell to fill</param>
        protected override void SetBoundaryConditionSimple(long col, long row)
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
                optScore = 1;
                optScoreCells.Clear();
            }

            if (col == 0 && row > 0)
            {
                // No source for cells, but set to left so that we can easily fill end 
                // gaps when doing traceback.
                _FSimpleSource[0, row] = SourceDirection.Up;
            }
        }

        /// <summary>
        /// Sets matrix boundary conditions for pair-wise overlap.
        /// Use bc 0 so that there is no penalty for gaps at the ends.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="col">Col of cell to fill</param>
        /// <param name="row">Row of cell to fill</param>
        protected override void SetBoundaryConditionAffine(long col, long row)
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
                optScore = 1;
                optScoreCells.Clear();
            }

            if (col == 0 && row > 0)
            {
                _Iy[0, row] = int.MinValue / 2;  // should be -infinity, this value avoids underflow problems

                // No source for cells, but set to left so that we can easily fill end 
                // gaps when doing traceback.
                _FSimpleSource[0, row] = SourceDirection.Up;
            }
        }
        #endregion Methods
    }
}
