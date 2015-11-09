using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SudokuSolver.Core.Model
{
    /// <summary>
    /// The concrete implementation of <see cref="IModel"/>. The model will try to solve a sudoku based on a list of
    /// intial values. The model makes <see cref="IFoundValue"/> available based on a "current value" that is updated based
    /// on the values accessed.
    /// </summary>
    public class Model : IModel
    {
        private int mInitalValues;
        private int mShownValues;
        private List<IFoundValue> mValues;

        private Model()
        {
            // Private constructor ensures that only the model can instantiate concrete instances of itself
        }

        /// <summary>
        /// Any error when trying to solve a sudoku.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Whether the sudoku was valid.
        /// </summary>
        public bool Invalid { get; set; }

        /// <summary>
        /// Creates a concrete instance of the model
        /// </summary>
        /// <returns></returns>
        public static IModel CreateModel()
        {
            return new Model();
        }

        /// <summary>
        /// Starts a new sudoku game.
        /// </summary>
        /// <param name="pSudokuValues">The array of initial values in the sudoku.</param>
        /// <param name="pOnSolved">The callback method for when the model is done trying to solve the sudoku.</param>
        public async void StartGame(char[] pSudokuValues, Action pOnSolved)
        {
            // Try to solve the sudoku asyncronsously
            Solver.SolveResult result = await Task.Run(() => { return Solver.SudokuSolver.Solve(pSudokuValues); });

            // Process the result of solving
            if (result.FoundValues != null)
            {
                mValues = result.FoundValues;
                mInitalValues = result.InitialCount;
                mShownValues = mInitalValues - 1;
                Invalid = false;
                Error = null;
            }
            else if (result.Error != null)
            {
                Error = result.Error;
            }
            else
            {
                Invalid = true;
            }

            // Invoke the callback method
            pOnSolved();
        }

        /// <summary>
        /// Ends the current game. Returns the model to its initial state.
        /// </summary>
        public void ClearGame()
        {
            mValues = null;
            mShownValues = 0;
            mInitalValues = 0;
            Error = null;
            Invalid = false;
        }

        /// <summary>
        /// The intial values. The current value is now the last initial value.
        /// </summary>
        public IEnumerable<IFoundValue> InitialValues()
        {
            mShownValues = mInitalValues - 1;
            return mValues.GetRange(mShownValues + 1, Constants.SUDOKU_SIZE - mInitalValues);
        }

        /// <summary>
        /// All values after the current value. The current value is now the last value in the sudoku.
        /// </summary>
        public IEnumerable<IFoundValue> RemainingValues()
        {
            int currentShown = mShownValues + 1;
            mShownValues = Constants.SUDOKU_SIZE - 1;

            return mValues.GetRange(currentShown, Constants.SUDOKU_SIZE - currentShown);
        }

        /// <summary>
        /// The value after the current value. The current value is incremented by one.
        /// </summary>
        public IFoundValue NextValue()
        {
            if (mShownValues < Constants.SUDOKU_SIZE - 1)
            {
                mShownValues++;
            }

            return mValues[mShownValues];
        }

        /// <summary>
        /// The value before the current value. The current value is decremented by one.
        /// </summary>
        public IFoundValue PrevValue()
        {
            if (mShownValues >= mInitalValues)
            {
                mShownValues--;
            }

            return mValues[mShownValues + 1];
        }
    }
}
