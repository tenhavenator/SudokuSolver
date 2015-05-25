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
        private Square[,] mSudokuSolvedValues;

        private int mCurrentValue;
        
        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <param name="pSudokuGivenValues">The given values for the game</param>
        /// <param name="pSudokuSolvedValues">The solution to the game</param>
        public SudokuGame(byte[,] pSudokuGivenValues, Square[,] pSudokuSolvedValues) 
        {
            mCurrentValue = 0;
            mSudokuEnteredValues = new byte[Constants.BOARD_SIZE, Constants.BOARD_SIZE];
            for (int row = 0; row < Constants.BOARD_SIZE; row++)
            {
                for (int column = 0; column < Constants.BOARD_SIZE; column++)
                {
                    byte value = pSudokuGivenValues[row, column];
                    if (value != 0)
                    {
                        mSudokuEnteredValues[row, column] = value;
                        mCurrentValue++;
                    }
                }
            }

            mSudokuGivenValues = pSudokuGivenValues;
            mSudokuSolvedValues = pSudokuSolvedValues;
        }

        /// <summary>
        /// Solves all values in the sudoku
        /// </summary>
        public void solveAllValues()
        {
            foreach (Square square in mSudokuSolvedValues)
            {
                mSudokuEnteredValues[square.Row, square.Column] = square.Value;
            }

            mCurrentValue = 81;
        }

        /// <summary>
        /// Solves the next value in the sudoku
        /// </summary>
        public void solveOneValue()
        {
            foreach (Square square in mSudokuSolvedValues)
            {
                if (square.Order == mCurrentValue + 1)
                {
                    mSudokuEnteredValues[square.Row, square.Column] = square.Value;
                    mCurrentValue++;
                    break;
                }
            }
        }

        /// <summary>
        /// Removes the last solved value from the sudoku
        /// </summary>
        public void unsolveOneValue()
        {
            foreach (Square square in mSudokuSolvedValues)
            {
                if (square.Order == mCurrentValue && mSudokuGivenValues[square.Row, square.Column] == 0)
                {
                    mSudokuEnteredValues[square.Row, square.Column] = 0;
                    mCurrentValue--;
                    break;
                }
            }
        }

        /// <summary>
        /// Property to access the solved sudoku values
        /// </summary>
        public Square[,] SolvedValues
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
