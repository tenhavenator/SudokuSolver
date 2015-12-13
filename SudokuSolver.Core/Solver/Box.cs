using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core.Solver
{
    using Model;
    using Techniques;

    /// <summary>
    /// Class that represents a box.
    /// </summary>
    internal class Box : Entity
    {
        /// <summary>
        /// Instantiates a box.
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pIndex">The index of the box (1-9).</param>
        public Box(IEnumerable<Square> pGrid, int pIndex) : base(pGrid, pIndex) { }

        /// <summary>
        /// Creates a box method for finding a value.
        /// </summary>
        /// <param name="pValue"> The value that has been found</param>
        protected override Method CreateMethod(char pValue)
        {
            return Method.CreateBoxMethod(pValue, mSquares.Except(mValueSquares[pValue]));
        }

        /// <summary>
        /// Creates an box technique for eliminating a possible value for a square.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square where the value is located.</param>
        protected override ITechnique CreateTechnique(char pValue, int pIndex)
        {
            return Technique.CreateBoxTechnique(pValue, pIndex, mSquares.Select(s => s.Index));
        }

        /// <summary>
        /// Chooses the squares that will be part of this box.
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pEntityIndex">The index of the box (1-9).</param>
        protected override IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex)
        {
            var boxRow = pEntityIndex / Constants.BOX_SIZE;
            var boxCol = pEntityIndex % Constants.BOX_SIZE;
            var boxRowIndexes = Constants.BOX_SIZE * Constants.ENTITY_SIZE;

            return pGrid.Where((s, i) => (i / (boxRowIndexes)) == boxRow
                && (i / Constants.BOX_SIZE) % Constants.BOX_SIZE == boxCol);
        }
    }
}
