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



namespace Bio.SimilarityMatrices
{
    /// <summary>
    /// Diagonal similarity matrix is a special case and needs its own class.
    /// It does not have an actual matrix, instead using a test "if (col == row)" and
    /// returning the diagonal value if true, and the off diagonal value if false.
    /// </summary>
    public class DiagonalSimilarityMatrix : SimilarityMatrix
    {
        /// <summary>
        /// Score value at diagonals. To be used when (col == row)
        /// </summary>
        private int _diagonalValue;

        /// <summary>
        /// Score value off diagonals. To be used when (col != row)
        /// </summary>
        private int _offDiagonalValue;

        /// <summary>
        /// Initializes a new instance of the DiagonalSimilarityMatrix class.
        /// Creates a SimilarityMatrix with one value for match and one for mis-match.
        /// </summary>
        /// <param name="matchValue">diagonal score for (col == row)</param>
        /// <param name="mismatchValue">off-diagonal score for (col != row)</param>
        /// <param name="moleculeType">DNA, RNA or Protein</param>
        public DiagonalSimilarityMatrix(int matchValue, int mismatchValue, MoleculeType moleculeType)
        {
            _diagonalValue = matchValue;
            _offDiagonalValue = mismatchValue;
            Matrix = null; // not used

            // Don't really need a symbol map for a diagonal matrix, but the code needs one to convert sequences
            // to and from integer arrays.  Simple alphabet below.
            // Can map all 256 single byte chars if we need to.
            string symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ*-";
            MatrixEncoding = new BasicSmEncoding(symbols, "Diagonal", moleculeType);

            ////= new Basic(symbols);
            Name = "Diagonal: match value " + _diagonalValue + ", non-match value " + _offDiagonalValue;
        }

        /// <summary>
        /// Gets or sets the diagonal value (match value) for the diagonal similarity matrix.
        /// </summary>
        public int DiagonalValue
        {
            get { return _diagonalValue; }
            set { _diagonalValue = value; }
        }

        /// <summary>
        /// Gets or sets the off diagonal value (mis-match value for the diagonal similarity matrix.
        /// </summary>
        public int OffDiagonalValue
        {
            get { return _offDiagonalValue; }
            set { _offDiagonalValue = value; }
        }

        /// <summary>
        /// Returns value of diagonal similarity matrix at [row,col].
        /// </summary>
        /// <param name="row">
        /// Row number. This is same as byte value
        /// corresponding to sequence symbol on the row
        /// </param>
        /// <param name="col">
        /// Column number. This is same as byte value
        /// corresponding to sequence symbol on the column
        /// </param>
        /// <returns>Score value of matrix at [row,col]</returns>
        public override int this[int row, int col]
        {
            get
            {
                return (col == row) ? _diagonalValue : _offDiagonalValue;
            }
        }

        /// <summary>
        /// Returns value of matrix at row, column corresponding to input ISequenceItems.
        /// </summary>
        /// <param name="rowItem">ISequenceItem on the row</param>
        /// <param name="colItem">ISequenceItem on the column</param>
        /// <returns>Score at matrix[row, col]</returns>
        public override int this[ISequenceItem rowItem, ISequenceItem colItem]
        {
            get
            {
                return (colItem.Value == rowItem.Value) ? _diagonalValue : _offDiagonalValue;
            }
        }
    }
}
