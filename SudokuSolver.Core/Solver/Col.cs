using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core.Solver
{
    /// <summary>
    /// Class that represents a column.
    /// </summary>
    internal class Col : Entity
    {
        /// <summary>
        /// Instantiates a column.
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pIndex">The index of the column (1-9).</param>
        public Col(IEnumerable<Square> pGrid, int pIndex) : base(pGrid, pIndex) { }

        /// <summary>
        /// Creates a column method for finding a value.
        /// </summary>
        /// <param name="pValue"> The value that has been found</param>
        protected override Method CreateMethod(char pValue)
        {
            return Method.CreateColMethod(pValue, mSquares.Except(mValueSquares[pValue]));
        }

        /// <summary>
        /// Creates an column technique for eliminating a possible value for a square.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square where the value is located.</param>
        protected override ITechnique CreateTechnique(char pValue, int pIndex)
        {
            return Technique.CreateBoxTechnique(pValue, pIndex, mSquares.Select(s => s.Index));
        }

        /// <summary>
        /// Chooses the squares that will be part of this column.
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pEntityIndex">The index of the column (1-9).</param>
        protected override IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex)
        {
            return pGrid.Where((s, i) => i % Constants.ENTITY_SIZE == pEntityIndex);
        }
    }
}
