using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core.Solver
{
    using Model;
    using Techniques;

    /// <summary>
    /// Class that represents a row.
    /// </summary>
    internal class Row : Entity
    {
        /// <summary>
        /// Instantiates a row.
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pIndex">The index of the row (1-9).</param>
        public Row(IEnumerable<Square> pGrid, int pIndex) : base(pGrid, pIndex) { }

        /// <summary>
        /// Creates a row method for finding a value.
        /// </summary>
        /// <param name="pValue"> The value that has been found</param>
        protected override Method CreateMethod(char pValue)
        {
            return Method.CreateRowMethod(pValue, mSquares.Except(mValueSquares[pValue]));
        }

        /// <summary>
        /// Creates an row technique for eliminating a possible value for a square.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square where the value is located.</param>
        protected override ITechnique CreateTechnique(char pValue, int pIndex)
        {
            return Technique.CreateRowTechnique(pValue, pIndex, mSquares.Select(s => s.Index));
        }

        /// <summary>
        /// Chooses the squares that will be part of this row.
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pEntityIndex">The index of the row (1-9).</param>
        protected override IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex)
        {
            var startIndex = pEntityIndex * Constants.ENTITY_SIZE;
            return pGrid.Where((s, i) => i >= startIndex && i < startIndex + Constants.ENTITY_SIZE);
        }
    }
}
