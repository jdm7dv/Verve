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
    /// Implements the NeedlemanWunsch algorithm for global alignment.
    /// See Chapter 2 in Biological Sequence Analysis; Durbin, Eddy, Krogh and Mitchison; 
    /// Cambridge Press; 1998.
    /// </summary>
    public class NeedlemanWunschAligner : DynamicProgrammingPairwiseAligner
    {
        /// <summary>
        /// Tracks optimal score for alignment.
        /// </summary>
        private int optScore = int.MinValue;

        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overridden property from the abstract parent.
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
        /// This is a overridden property from the abstract parent.
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
        /// Fills matrix cell specifically for NeedlemanWunsch - Uses linear gap penalty.
        /// Required because method is abstract in DynamicProgrammingPairwise
        /// To be removed once changes are made in SW, Pairwise algorithms.
        /// </summary>
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Col of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void FillCellSimple(long row, long col, long cell)
        {
            fScore[col] = SetCellValuesSimple(row, col, cell);
        }

        /// <summary>
        /// Sets the score in last cell of _FScore to be the optimal score.
        /// </summary>
        protected override void SetOptimalScoreSimple()
        {
            // Trace back starts at lower right corner.
            // Save the score from this point.
            if (UseEARTHToFillMatrix)
            {
                optScore = _FSimpleScore[numberOfCols - 1, numberOfRows - 1];
            }
            else
            {
                optScore = fScore[numberOfCols - 1];
            }
        }

        /// <summary>
        /// Sets the score in last cell of _MaxScore to be the optimal score.
        /// </summary>
        protected override void SetOptimalScoreAffine()
        {
            // Trace back starts at lower right corner.
            // Save the score from this point.
            if (UseEARTHToFillMatrix)
            {
                optScore = _M[numberOfCols - 1, numberOfRows - 1];
            }
            else
            {
                optScore = maxScore[numberOfCols - 1];
            }
        }

        /// <summary>
        /// Fills matrix cell specifically for NeedlemanWunsch - Uses affine gap penalty.
        /// Required because method is abstract in DynamicProgrammingPairwise
        /// To be removed once changes are made in SW, Pairwise algorithms.
        /// </summary>
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Col of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void FillCellAffine(long row, long col, long cell)
        {
            maxScore[col] = SetCellValuesAffine(row, col, cell);
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth row in NeedlemanWunsch global alignment.
        /// Uses linear gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionSimple()
        {
            for (int col = 0; col < numberOfCols; col++)
            {
                fScore[col] = col * internalGapOpenCost;
                fSource[col] = SourceDirection.Left;
            }

            fScore[0] = internalGapOpenCost;
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth column in NeedlemanWunsch global alignment.
        /// Uses linear gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void SetColumnBoundaryConditionSimple(long row, long cell)
        {
            // TODO: Remove below cast to int in the next two lines
            fScore[0] = (int)row * internalGapOpenCost; // (row, 0) for F Matrix is set
            fScoreDiagonal = ((int)row - 1) * internalGapOpenCost; // _FScoreDiagonal is set to previous row's _FScore[0]
            fSource[cell] = SourceDirection.Up;
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth row in NeedlemanWunsch global alignment.
        /// Uses affine gap penalty.
        /// </summary>
        protected override void SetRowBoundaryConditionAffine()
        {
            // Column 0
            ixGapScore[0] = int.MinValue / 2;
            maxScore[0] = 0;
            fSource[0] = SourceDirection.Left;

            for (int col = 1; col < numberOfCols; col++)
            {
                ixGapScore[col] = int.MinValue / 2;
                maxScore[col] = internalGapOpenCost + ((col - 1) * internalGapExtensionCost);
                fSource[col] = SourceDirection.Left;
            }
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth column in NeedlemanWunsch global alignment.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="row">Row number of cell.</param>
        /// <param name="cell">Cell number.</param>
        protected override void SetColumnBoundaryConditionAffine(long row, long cell)
        {
            iyGapScore = int.MinValue / 2; // Iy set to -infinity
            maxScoreDiagonal = maxScore[0]; // stored 0th cell of previous row.

            // sets (row, 0) for _MaxScore
            // TODO: Remove below cast to int
            maxScore[0] = internalGapOpenCost + (((int)row - 1) * internalGapExtensionCost);
            fSource[cell] = SourceDirection.Up;
        }

        /// <summary>
        /// Resets the members used to track optimum score and cell.
        /// </summary>
        protected override void ResetSpecificAlgorithmMemberVariables()
        {
            // Not strictly necessary since this will be set in the FillCell methods, 
            // but it is good practice to initialize correctly.
            this.optScore = int.MinValue;
        }

        /// <summary>
        /// Performs traceback for global alignment.
        /// </summary>
        /// <param name="alignedSequences">List of aligned sequences (output).</param>
        /// <param name="offsets">Offset is the starting position of alignment
        /// of sequence1 with respect to sequence2.</param>
        /// <param name="startOffsets">Start indices of aligned sequences with respect to input sequences.</param>
        /// <param name="endOffsets">End indices of aligned sequences with respect to input sequences.</param>
        /// <param name="insertions">Insertions made to the aligned sequences.</param>
        /// <returns>Optimum score.</returns>
        protected override int Traceback(out List<byte[]> alignedSequences, out List<long> offsets, out List<long> startOffsets, out List<long> endOffsets, out List<long> insertions)
        {
            alignedSequences = new List<byte[]>(2);
            startOffsets = new List<long>(2);
            endOffsets = new List<long>(2);
            insertions = new List<long>(2);

            // For NW, aligned sequence will be at least as long as longest input sequence.
            // May be longer if there are gaps in both aligned sequences.
            long guessLen = Math.Max(firstInputSequence.Count, secondInputSequence.Count);

            endOffsets.Add(firstInputSequence.Count - 1);
            endOffsets.Add(secondInputSequence.Count - 1);

            // TODO: Remove below cast
            List<byte> alignedListA = new List<byte>((int)guessLen);
            List<byte> alignedListB = new List<byte>((int)guessLen);

            // Start at the bottom left element of F and work backwards until we get to upper left
            long col, row;
            col = numberOfCols - 1;
            row = numberOfRows - 1;

            int colGaps = 0;
            int rowGaps = 0;
            if (UseEARTHToFillMatrix)
            {
                // stop when col and row are both zero
                while (row > 0 || col > 0)
                {
                    switch (_FSimpleSource[col, row])
                    {
                        case SourceDirection.Diagonal:
                            // diagonal, no gap, use both sequence residues
                            alignedListA.Add(firstInputSequence[col - 1]);
                            alignedListB.Add(secondInputSequence[row - 1]);

                            col = col - 1;
                            row = row - 1;
                            break;

                        case SourceDirection.Up:
                            // up, gap in a
                            alignedListA.Add(gapCode);
                            alignedListB.Add(secondInputSequence[row - 1]);

                            row = row - 1;
                            colGaps++;
                            break;

                        case SourceDirection.Left:
                            // left, gap in b
                            alignedListA.Add(firstInputSequence[col - 1]);
                            alignedListB.Add(gapCode);

                            col = col - 1;
                            rowGaps++;
                            break;

                        default:
                            string message = string.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.TracebackBadSource,
                                "NeedlemanWunschAligner");
                            Trace.Report(message);
                            throw new InvalidEnumArgumentException(message);
                    }
                }
            }
            else
            {
                long cell = (numberOfCols * numberOfRows) - 1;
                // stop when col and row are both zero
                while (cell > 0)
                {
                    switch (fSource[cell])
                    {
                        case SourceDirection.Diagonal:
                            // diagonal, no gap, use both sequence residues
                            alignedListA.Add(firstInputSequence[col - 1]);
                            alignedListB.Add(secondInputSequence[row - 1]);

                            col = col - 1;
                            row = row - 1;
                            cell = cell - numberOfCols - 1;
                            break;

                        case SourceDirection.Up:
                            // up, gap in a
                            alignedListA.Add(gapCode);
                            alignedListB.Add(secondInputSequence[row - 1]);

                            row = row - 1;
                            cell = cell - numberOfCols;
                            colGaps++;
                            break;

                        case SourceDirection.Left:
                            // left, gap in b
                            alignedListA.Add(firstInputSequence[col - 1]);
                            alignedListB.Add(gapCode);

                            col = col - 1;
                            cell = cell - 1;
                            rowGaps++;
                            break;

                        default:
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

            insertions.Add(colGaps);
            insertions.Add(rowGaps);

            offsets = new List<long>(2);
            offsets.Add(GetOffset(alignedA) - GetOffset(firstInputSequence));
            offsets.Add(GetOffset(alignedB) - GetOffset(secondInputSequence));

            startOffsets.Add(0);
            startOffsets.Add(0);
            return this.optScore;
        }

        /// <summary>
        /// Fills matrix cell specifically for NeedlemanWunsch - Uses linear gap penalty.
        /// Required because method is abstract in DynamicProgrammingPairwise
        /// To be removed once changes are made in SW, Pairwise algorithms.
        /// </summary>
        /// <param name="col">Col of cell.</param>
        /// <param name="row">Row of cell.</param>
        protected override void FillCellSimple(long col, long row)
        {
            _FSimpleScore[col, row] = SetCellValuesSimple(col, row);
        }

        /// <summary>
        /// Fills matrix cell specifically for NeedlemanWunsch - Uses affine gap penalty.
        /// Required because method is abstract in DynamicProgrammingPairwise
        /// To be removed once changes are made in SW, Pairwise algorithms.
        /// </summary>
        /// <param name="col">Col of cell.</param>
        /// <param name="row">Row of cell.</param>
        protected override void FillCellAffine(long col, long row)
        {
            _M[col, row] = SetCellValuesAffine(col, row);
        }

        /// <summary>
        /// Sets F matrix boundary conditions for zeroth row and zeroth column in NeedlemanWunsch global alignment.
        /// Uses linear gap penalty.
        /// </summary>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected override void SetBoundaryConditionSimple(long col, long row)
        {
            if (col >= 0 && row == 0)
            {
                // TODO: Remove below cast to int
                _FSimpleScore[col, 0] = (int)col * internalGapOpenCost;
                _FSimpleSource[col, 0] = SourceDirection.Left;
            }

            if (col == 0 && row > 0)
            {
                // TODO: Remove below cast to int
                _FSimpleScore[0, row] = (int)row * internalGapOpenCost; // (row, 0) for F Matrix is set
                _FSimpleSource[0, row] = SourceDirection.Up;
            }
        }

        /// <summary>
        /// Sets matrix boundary conditions for zeroth row and zeroth column in NeedlemanWunsch global alignment.
        /// Uses affine gap penalty.
        /// </summary>
        /// <param name="col">Col of cell to fill.</param>
        /// <param name="row">Row of cell to fill.</param>
        protected override void SetBoundaryConditionAffine(long col, long row)
        {
            if (col == 0 && row == 0)
            {
                _Ix[0, 0] = int.MinValue / 2;
                _Iy[0, 0] = int.MinValue / 2;
                _M[0, 0] = 0;
                _FSimpleSource[0, 0] = SourceDirection.Left;
            }

            if (col > 0 && row == 0)
            {
                _Ix[col, 0] = int.MinValue / 2;
                _Iy[col, 0] = int.MinValue / 2;
                // TODO: Remove below cast to int
                _M[col, 0] = internalGapOpenCost + (((int)col - 1) * internalGapExtensionCost);
                _FSimpleSource[col, 0] = SourceDirection.Left;
            }

            if (col == 0 && row > 0)
            {
                _Ix[0, row] = int.MinValue / 2;
                _Iy[0, row] = int.MinValue / 2;
                // TODO: Remove below cast to int
                _M[0, row] = internalGapOpenCost + (((int)row - 1) * internalGapExtensionCost);
                _FSimpleSource[0, row] = SourceDirection.Up;
            }
        }
    }
}
