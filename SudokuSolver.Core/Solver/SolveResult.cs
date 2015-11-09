using SudokuSolver.Core.Model;
using System.Collections.Generic;

namespace SudokuSolver.Core.Solver
{
    /// <summary>
    /// Class that represents the result of trying to solve a sudoku
    /// </summary>
    internal class SolveResult
    {
        /// <summary>
        /// The list of <see cref="IFoundValue"/> that represents the solved sudoku. Will be null if there was an error or the
        /// sudoku was invalid.
        /// </summary>
        public List<IFoundValue> FoundValues { get; set; }

        /// <summary>
        /// Any error when trying to solve a sudoku.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The number of initial values provided.
        /// </summary>
        public int InitialCount { get; set; }
    }
}
