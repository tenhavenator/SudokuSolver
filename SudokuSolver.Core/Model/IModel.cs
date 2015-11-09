using System;
using System.Collections.Generic;

namespace SudokuSolver.Core.Model
{
    /// <summary>
    /// The interface used to represent the model for SudokuSolver. The model will try to solve a sudoku based on a list of
    /// intial values. The model makes <see cref="IFoundValue"/> available based on a "current value" that is updated based
    /// on the values accessed.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Any error when trying to solve a sudoku.
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Whether the sudoku was valid.
        /// </summary>
        bool Invalid { get; }

        /// <summary>
        /// Starts a new sudoku game.
        /// </summary>
        /// <param name="pSudokuValues">The array of initial values in the sudoku.</param>
        /// <param name="pOnSolved">The callback method for when the model is done trying to solve the sudoku.</param>
        void StartGame(char[] pSudokuValues, Action pOnSolved);

        /// <summary>
        /// Ends the current game. Returns the model to its initial state.
        /// </summary>
        void ClearGame();

        /// <summary>
        /// The intial values. The current value is now the last initial value.
        /// </summary>
        IEnumerable<IFoundValue> InitialValues();

        /// <summary>
        /// All values after the current value. The current value is now the last value in the sudoku.
        /// </summary>
        IEnumerable<IFoundValue> RemainingValues();

        /// <summary>
        /// The value after the current value. The current value is incremented by one.
        /// </summary>
        IFoundValue NextValue();

        /// <summary>
        /// The value before the current value. The current value is decremented by one.
        /// </summary>
        IFoundValue PrevValue();
    }
}
