using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solver = SudokuSolver.Core.Solver;

namespace SudokuSolver.Core.Model
{
    public class Model : IModel
    {
        private int mInitalValues;
        private int mShownValues;
        private List<IFoundValue> mValues;

        private Model()
        {

        }

        public string Error { get; set; }

        public bool Invalid { get; set; }

        public static IModel CreateModel()
        {
            return new Model();
        }

        public async void StartGame(char[] pSudokuValues, Action pOnSolved)
        {
            Solver.SolveResult result = await Task.Run(() => { return Solver.SudokuSolver.Solve(pSudokuValues); });

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

            pOnSolved();
        }

        public void ClearGame()
        {
            mValues = null;
            mShownValues = 0;
            mInitalValues = 0;
            Error = null;
            Invalid = false;
        }

        public IEnumerable<IFoundValue> InitialValues()
        {
            mShownValues = mInitalValues - 1;
            return mValues.GetRange(mShownValues + 1, Constants.SUDOKU_SIZE - mInitalValues);
        }

        public IEnumerable<IFoundValue> RemainingValues()
        {
            int currentShown = mShownValues + 1;
            mShownValues = Constants.SUDOKU_SIZE - 1;

            return mValues.GetRange(currentShown, Constants.SUDOKU_SIZE - currentShown);
        }

        public IFoundValue NextValue()
        {
            if (mShownValues < Constants.SUDOKU_SIZE - 1)
            {
                mShownValues++;
            }

            return mValues[mShownValues];
        }

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
