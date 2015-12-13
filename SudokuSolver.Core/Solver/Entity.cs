using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core.Solver
{
    using Model;
    using Techniques;
    /// <summary>
    /// Class that represents a Box, Row, or Column. Tracks the possible squares for each value (1-9).
    /// </summary>
    internal abstract class Entity
    {
        protected IEnumerable<Square> mSquares;
        protected IDictionary<char, ICollection<Square>> mValueSquares;
        private char? mInserting;

        /// <summary>
        /// Instantiates an entity. Will choose a list of squares that it contains based on <paramref name="pGrid"/> and 
        /// <paramref name="pIndex"/>.
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pIndex">The index of the entity (1-9).</param>
        public Entity(IEnumerable<Square> pGrid, int pIndex)
        {
            mSquares = PickSquares(pGrid, pIndex);
            mValueSquares = new Dictionary<char, ICollection<Square>>();

            // Adds this entity to each squares entity list
            foreach (Square square in mSquares)
            {
                square.Entities.Add(this);
            }

            // Initializes the possible squares for each value
            foreach (var value in Constants.ALL_VALUES)
            {
                mValueSquares.Add(value, new List<Square>(mSquares));
            }
        }

        /// <summary>
        /// The squares of this entity
        /// </summary>
        public IEnumerable<Square> Squares
        {
            get { return mSquares; }
        }

        /// <summary>
        /// Prepares the entity to have a value inserted.
        /// </summary>
        /// <param name="pValue">The values that will be inserted.</param>
        public void PreInsertion(char pValue)
        {
            mInserting = pValue;
        }

        /// <summary>
        /// Inserts a value into this entity. <see cref="PreInsertion"/> must be called first.
        /// </summary>
        /// <param name="pIndex">The index of the square to insert the value in.</param>
        public void StartInsertion(int pIndex)
        {
            mValueSquares[mInserting.Value].Clear();

            // Eliminate the value being inserted as a possible value from all squares in the entity
            var technique = CreateTechnique(mInserting.Value, pIndex);
            foreach (Square square in mSquares)
            {
                square.EliminatePossibility(mInserting.Value, technique);
            }
        }

        /// <summary>
        /// Notifies the entity that insertion has completed.
        /// </summary>
        public void EndInsertion()
        {
            mInserting = null;
        }

        /// <summary>
        /// Eliminates a square as a possible location for a value.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pSquare">The square to eliminate.</param>
        public void EliminatePossibility(char pValue, Square pSquare)
        {
            var squares = mValueSquares[pValue];
            squares.Remove(pSquare);

            // If there is only one place left for the value to go then generate a method
            if (squares.Count() == 1 && mInserting != pValue)
            {
                squares.Single().AddMethod(pValue, CreateMethod(pValue));
            }
        }

        /// <summary>
        /// Creates a method for finding a value.
        /// </summary>
        /// <param name="pValue"> The value that has been found</param>
        protected abstract Method CreateMethod(char pValue);

        /// <summary>
        /// Creates an entity technique for eliminating a possible value for a square.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square where the value is located.</param>
        /// <returns></returns>
        protected abstract ITechnique CreateTechnique(char pValue, int pIndex);

        /// <summary>
        /// Chooses the squares that will be part of this entity
        /// </summary>
        /// <param name="pGrid">The grid of all squares in the sudoku.</param>
        /// <param name="pEntityIndex">The index of the entity (1-9).</param>
        protected abstract IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex);
    }
}
