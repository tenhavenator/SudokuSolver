using System.Collections.Generic;

namespace SudokuSolver.Core
{
    /// <summary>
    /// Class to hold the global constants used by SudokuSolver.Core
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The number of squares in the sudoku.
        /// </summary>
        public const int SUDOKU_SIZE = 81;

        /// <summary>
        /// The number squares in an entity.
        /// </summary>
        public const int ENTITY_SIZE = 9;

        /// <summary>
        /// The number of squares in a box.
        /// </summary>
        public const int BOX_SIZE = 3;

        /// <summary>
        /// The possible values for a sudoku (1-9).
        /// </summary>
        public static IEnumerable<char> ALL_VALUES
        {
            get
            {
                yield return '1';
                yield return '2';
                yield return '3';
                yield return '4';
                yield return '5';
                yield return '6';
                yield return '7';
                yield return '8';
                yield return '9';
            }
        }
    }
}
