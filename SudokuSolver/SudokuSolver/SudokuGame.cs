using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    /// <summary>
    /// This class represents a Sudoku game
    /// </summary>
    public class SudokuGame
    {
        private byte[,] mSudokuGivenValues;
        private byte[,] mSudokuEnteredValues;
        private byte[,] mSudokuSolvedValues;
        
        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <param name="pSudokuGivenValues">The given values for the game</param>
        /// <param name="pSudokuSolvedValues">The solution to the game</param>
        public SudokuGame(byte[,] pSudokuGivenValues, byte[,] pSudokuSolvedValues) 
        {
            mSudokuGivenValues = pSudokuGivenValues;
            mSudokuEnteredValues = pSudokuGivenValues;
            mSudokuSolvedValues = pSudokuSolvedValues;
        }

        /// <summary>
        /// Solves all values in the sudoku
        /// </summary>
        public void solveAllValues()
        {
            mSudokuEnteredValues = mSudokuSolvedValues;
        }

        /// <summary>
        /// Solves the next value in the sudoku
        /// </summary>
        public void solveOneValue()
        {
            
        }

        /// <summary>
        /// Removes the last solved value from the sudoku
        /// </summary>
        public void unsolveOneValue()
        {
        
        }

        /// <summary>
        /// Property to access the solved sudoku values
        /// </summary>
        public byte[,] SolvedValues
        {
            get { return mSudokuSolvedValues; }
        }

        /// <summary>
        /// Property to access the given values of the sudoku
        /// </summary>
        public byte[,] GivenValues
        {
            get { return mSudokuGivenValues; }
        }

        /// <summary>
        /// Property to access the sudoku values that are on the board right now
        /// </summary>
        public byte[,] EnteredValues
        {
            get { return mSudokuEnteredValues; }
        }
    }
}
