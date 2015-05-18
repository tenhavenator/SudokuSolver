/// <summary>
/// This class represents a Sudoku game
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuGame
    {
        private byte[,] mSudokuGivenValues;
        private byte[,] mSudokuEnteredValues;
        private byte[,] mSudokuSolvedValues;
        private Boolean mSaved;
        
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
            mSaved = false;
        }

        /// <summary>
        /// Returns true if the game has been saved since the last square changed
        /// </summary>
        public Boolean isSaved()
        {
            return mSaved;
        }

        /// <summary>
        /// Saves the current game by writing it to a save file. Returns true if the game is saved successfully. TODO Make this actually write to a file.
        /// </summary>
        /// <returns></returns>
        public Boolean save() 
        {
            mSaved = true;
            return true;
        }

        public byte[,] SolvedValues
        {
            get { return mSudokuSolvedValues; }
        }

        public byte[,] GivenValues
        {
            get { return mSudokuGivenValues; }
        }
    }
}
