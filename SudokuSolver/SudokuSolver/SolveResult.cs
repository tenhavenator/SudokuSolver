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
        private byte[] mSolvedSudoku;
        private string mErrorMessage;

        public SolveResult(int pResultType)
        {
            mResultType = pResultType;
        }

        /// <summary>
        /// Returns the result when trying to solve the sudoku
        /// </summary>
        public int getResultType()
        {
            return mResultType;
        }

        /// <summary>
        /// Returns the solved sudoku if it was successfully solved
        /// </summary>
        public byte[] getSolvedSudoku()
        {
            return mSolvedSudoku;
        }

        /// <summary>
        /// Sets the solved sudoku values
        /// </summary>
        /// <param name="pSolvedSudoku">The solved sudoku</param>
        public void setSolvedSudoku(byte[] pSolvedSudoku)
        {
            mSolvedSudoku = pSolvedSudoku;
        }

        /// <summary>
        /// Returns the error message if there was an error when solving the sudoku
        /// </summary>
        public string getErrorMessage() 
        {
            return mErrorMessage;
        }

        /// <summary>
        /// Sets the error message if there was an error when trying to solve the sudoku
        /// </summary>
        /// <param name="pErrorMessage">The error message</param>
        public void setErrorMessage(string pErrorMessage)
        {
            mErrorMessage = pErrorMessage;
        }
    }
}
