/// <summary>
/// This class represents the result when solving a sudoku.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SolveResult
    {
        public const int SUCCESS = 0;
        public const int ERROR = 1;
        public const int INVALID = 2;
        public const int CANCELLED = 3;

        private int mResultType;
        private byte[,] mSolvedSudoku;
        private string mErrorMessage;

        private SolveResult(int pResultType)
        {
            mResultType = pResultType;
        }

        /// <summary>
        /// Creates a solve result for a successfully solved sudoku
        /// </summary>
        /// <param name="pSolvedSudoku">The values for the successfully solved sudoku</param>
        public static SolveResult createSuccessResult(byte[,] pSolvedSudoku)
        {
            SolveResult result = new SolveResult(SUCCESS);
            result.SolveResultValues = pSolvedSudoku;

            return result;
        }

        /// <summary>
        /// Creates a solve result incase an error has occurred
        /// </summary>
        /// <param name="pErrorMessage">The error message</param>
        public static SolveResult createErrorResult(string pErrorMessage)
        {
            SolveResult result = new SolveResult(ERROR);
            result.ErrorMessage = pErrorMessage;

            return result;
        }

        /// <summary>
        /// Creates a solve result for when an invalid sudoku is entered
        /// </summary>
        public static SolveResult createInvalidResult()
        {
            return new SolveResult(INVALID);
        }

        /// <summary>
        /// Creates a new solve result for when the solve operation is cancelled. 
        /// </summary>
        public static SolveResult createCancelledResult()
        {
            return new SolveResult(CANCELLED);
        }

        /// <summary>
        /// Property to access the result type
        /// </summary>
        public int ResultType
        {
            get { return mResultType; }
        }

        /// <summary>
        /// Property to access the result of solved sudoku
        /// </summary>
        public byte[,] SolveResultValues
        {
            get { return mSolvedSudoku; }
            set { mSolvedSudoku = value; }
        }

        /// <summary>
        /// Property to access the error message 
        /// </summary>
        public string ErrorMessage
        {
            get { return mErrorMessage; }
            set { mErrorMessage = value; }
        }
    }
}
