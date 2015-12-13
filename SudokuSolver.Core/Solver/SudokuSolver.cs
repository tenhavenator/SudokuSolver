using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core.Solver
{
    using Model;
    using Techniques;

    /// <summary>
    /// Class for solving a sudoku.
    /// </summary>
    internal class SudokuSolver
    {
        private int mInitialCount;
        private List<Entity> mEntities;
        private List<Square> mSquares;
        private List<IFoundValue> mFoundValues;

        /// <summary>
        /// Instantiates a SudokuSolver. Private so that uses must access statically.
        /// </summary>
        /// <param name="pInitialValues">The list of initial values.</param>
        private SudokuSolver(char[] pInitialValues)
        {
            mFoundValues = new List<IFoundValue>();
            mEntities = new List<Entity>();
            mSquares = new List<Square>();

            // Initialize the squares
            for (int i = 0; i < Constants.SUDOKU_SIZE; i++)
            {
                mSquares.Add(new Square() { Index = i, Filled = false });
            }

            // Initialize the entities
            for (int i = 0; i < Constants.ENTITY_SIZE; i++)
            {
                mEntities.Add(new Box(mSquares, i));
                mEntities.Add(new Row(mSquares, i));
                mEntities.Add(new Col(mSquares, i));
            }

            // Insert the initial values
            for (int i = 0; i < Constants.SUDOKU_SIZE; i++)
            {
                char value = pInitialValues[i];
                if (value != '\0')
                {
                    InsertValue(new FoundValue()
                    {
                        Value = value,
                        Index = i,
                        Methods = new IMethod[] { Method.CreateGivenValue(value, i) }
                    });
                }
            }

            mInitialCount = mFoundValues.Count();
        }

        /// <summary>
        /// Static method used to solve a sudoku.
        /// </summary>
        /// <param name="pInitialValues">The list of initial values.</param>
        /// <returns>The result of the solving operation.</returns>
        public static SolveResult Solve(char[] pInitialValues)
        {
            return new SudokuSolver(pInitialValues).Solve();
        }

        /// <summary>
        /// Inserts a value into a square.
        /// </summary>
        /// <param name="pFoundValue">The found value to insert</param>
        /// <returns></returns>
        private void InsertValue(IFoundValue pFoundValue)
        {
            mFoundValues.Add(pFoundValue);

            Square square = mSquares[pFoundValue.Index];
            square.InsertValue(pFoundValue.Value);
        }

        /// <summary>
        /// Check all squares for found values.
        /// </summary>
        /// <returns>A list of values that have been found.</returns>
        private IEnumerable<FoundValue> CheckValues()
        {
            List<FoundValue> foundValues = new List<FoundValue>();
            foreach (var square in mSquares.Where(s => !s.Filled))
            {
                if (square.HasFoundValue())
                {
                    foundValues.Add(square.GetFoundValue());
                }
            }

            return foundValues;
        }

        /// <summary>
        /// The main algorithm for solving a sudoku.
        /// </summary>
        /// <returns>The result of the solving operation.</returns>
        private SolveResult Solve()
        {
            try
            {
                while (true)
                {
                    int foundValuesCount = mFoundValues.Count;

                    // Check if the sudoku has been solved
                    if (foundValuesCount == Constants.SUDOKU_SIZE)
                    {
                        return new SolveResult { FoundValues = mFoundValues, InitialCount = mInitialCount };
                    }

                    // Insert the found value with the lowest ranked methods
                    IEnumerable<FoundValue> foundValues = CheckValues();
                    if (foundValues.Any())
                    {
                        InsertValue(foundValues.OrderByDescending(v => v.Rank).First());
                        continue;                    
                    }
                    
                    // Apply advaneed techniques and check for found values again.

                    return new SolveResult();
                   
                }
            }
            catch (Exception e)
            {
                return new SolveResult { Error = e.Message };
            }
        }
    }
}
