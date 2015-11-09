using System.Collections.Generic;

namespace SudokuSolver.Core.Model
{
    /// <summary>
    /// Interface that represents a value found in the Sudoku.
    /// </summary>
    public interface IFoundValue
    {
        /// <summary>
        /// The value found (1-9).
        /// </summary>
        char Value { get; }

        /// <summary>
        /// The index of the square where the value was found (0-80).
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The methods that can be used to find the value.
        /// </summary>
        IEnumerable<IMethod> Methods { get; }
    }
}
