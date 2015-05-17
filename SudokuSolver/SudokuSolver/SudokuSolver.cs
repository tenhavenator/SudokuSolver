/// <summary>
/// This file contains the functions and classes used for solving sudokus and generating hints.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace SudokuSolver
{
    /// <summary>
    /// This class represents the componenets of the 9x9 sudoku grid. Entities are rows, columns, or 3x3 boxes.
    /// Squares are the indivinual cells in the 9x9 grid.
    /// </summary>
    public class Board
    { 
        private Box[,] mBoxes;
        private Row[] mRows;
        private Column[] mColumns;
        private Square[,] mGrid;
        private List<Entity> mEntities;

        private int mSquaresFilled;

        public Board()
        {
            mSquaresFilled = 0;
            mBoxes = new Box[Constants.BOX_SIZE, Constants.BOX_SIZE];
            mRows = new Row[Constants.BOARD_SIZE];
            mColumns = new Column[Constants.BOARD_SIZE];
            mEntities = new List<Entity>();

            // Initialize the grid of squares
            mGrid = new Square[Constants.BOARD_SIZE, Constants.BOARD_SIZE];
            
            for (int row = 0; row < Constants.BOARD_SIZE; row++)
            {
                for (int column = 0; column < Constants.BOARD_SIZE; column++)
                {
                    mGrid[row, column] = new Square(row, column);
                }
            }

            // Initialize the board entities
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                int boxRow = i / Constants.BOX_SIZE;
                int boxColumn = i % Constants.BOX_SIZE;
                int boxStartRow = boxRow * Constants.BOX_SIZE;
                int boxStartColumn = boxColumn * Constants.BOX_SIZE;

                // Create a new box that contains the correct squares
                Box box = new Box(new Square[] { 
                    mGrid[boxStartRow, boxStartColumn], mGrid[boxStartRow, boxStartColumn + 1], mGrid[boxStartRow, boxStartColumn + 2],
                    mGrid[boxStartRow + 1, boxStartColumn], mGrid[boxStartRow + 1, boxStartColumn + 1], mGrid[boxStartRow + 1, boxStartColumn + 2],
                    mGrid[boxStartRow + 2, boxStartColumn], mGrid[boxStartRow + 2, boxStartColumn + 1], mGrid[boxStartRow + 2, boxStartColumn + 2]});

                mBoxes[boxRow, boxColumn] = box;
                mEntities.Add(box);

                // Create a column that contains the correct squares
                Column column = new Column(new Square[] { 
                    mGrid[0,i], mGrid[1,i], mGrid[2,i], mGrid[3,i], mGrid[4,i], mGrid[5,i], mGrid[6,i], mGrid[7,i], mGrid[8,i]});

                mColumns[i] = column;
                mEntities.Add(column);

                // Create a row that contains the correct squares
                Row row = new Row(new Square[] { 
                    mGrid[i,0], mGrid[i,1], mGrid[i,2], mGrid[i,3], mGrid[i,4], mGrid[i,5], mGrid[i,6], mGrid[i,7], mGrid[i,8]});

                mRows[i] = row;
                mEntities.Add(row);
            
            }
        }

        /// <summary>
        /// Enters a known value into a square on the board
        /// </summary>
        /// <param name="pRow">The row value of the square</param>
        /// <param name="pColumn">The column value of the square</param>
        /// <param name="pValue">The value to be put in the square</param>
        /// <returns></returns>
        public Boolean setKnownValue(int pRow, int pColumn, byte pValue)
        {
            mSquaresFilled++;

            // Place the in the square
            mGrid[pRow, pColumn].Value = pValue;

            // Check that the three entities where the value is going do not have that value yet
            Boolean rowResult = mRows[pRow].setKnownValue(pValue);
            Boolean columnResult = mColumns[pColumn].setKnownValue(pValue);
            Boolean boxResult = mBoxes[pRow / Constants.BOX_SIZE, pColumn / Constants.BOX_SIZE].setKnownValue(pValue);

            return rowResult && columnResult && boxResult;
        }

        /// <summary>
        /// Check if the a value has been found on the board and insert it
        /// </summary>
        /// <returns></returns>
        public Boolean checkForKnownValue()
        {
            // Check if a value has been found
            foreach (Entity entity in mEntities)
            {
                // For each possible value in the entity, check if there is only one square where it can be
                foreach (byte value in entity.UnknownValues)
                {
                    List<Square> candidateSquares = new List<Square>();

                    foreach (Square square in entity.Squares)
                    {
                        if (square.PossibleValues.Contains(value))
                        {
                            candidateSquares.Add(square);
                        }
                    }

                    // Add value to the board if it has been found. 
                    if (candidateSquares.Count == 1)
                    {
                        setKnownValue(candidateSquares[0].Row, candidateSquares[0].Column, value);
                        return true;
                    }
                }
            }

            return false;
        }



        /// <summary>
        /// Checks if all squares have been assigned a value.
        /// </summary>
        /// <returns>Returns true if all square have a been assigned a value </returns>
        public Boolean isSolved()
        {
            if (mSquaresFilled < Constants.BOARD_SIZE * Constants.BOARD_SIZE)
            {
                return false;
            }

            // Check if the sudoku is solved
            for (int row = 0; row < Constants.BOARD_SIZE; row++)
            {
                for (int column = 0; column < Constants.BOARD_SIZE; column++)
                {
                    if (!mGrid[row, column].isFilled())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Property for accessing the 9x9 grid of squares
        /// </summary>
        public Square[,] Grid
        {
            get { return mGrid; }
        }

        /// <summary>
        /// Property for accessing the list of columns
        /// </summary>
        public Column[] Columns
        {
            get { return mColumns; }
        }

        /// <summary>
        /// Property for accessing the list of rows
        /// </summary>
        public Row[] Rows
        {
            get { return mRows; }
        }

        /// <summary>
        /// Property for accessing the list of boxes
        /// </summary>
        public Box[,] Boxes
        {
            get { return mBoxes; }
        }

        /// <summary>
        /// Property for the list accessing all entities (rows, columns, boxes)
        /// </summary>
        public List<Entity> Entities
        {
            get { return mEntities; }
        }
    }

    /// <summary>
    /// This class represents one square in the 9x9 sudoku grid
    /// </summary>
    public class Square
    {
        private int mRow;
        private int mColumn;
        private List<byte> mPossibleValues;
        private byte mFinalValue;

        public Square(int pRow, int pColumn)
        {
            mRow = pRow;
            mColumn = pColumn;
            mFinalValue = 0;
            mPossibleValues = new List<byte>() {1,2,3,4,5,6,7,8,9};
        }

        /// <summary>
        /// Removes a value from the list of values that can possible occupy this square
        /// </summary>
        /// <param name="pValue">The value to be removed</param>
        public void eliminatePossibleValue(byte pValue)
        {
            mPossibleValues.Remove(pValue);
        }

        /// <summary>
        /// Checks if the square has a value
        /// </summary>
        /// <returns> Returns true if the square has a value</returns>
        public Boolean isFilled()
        {
            return mFinalValue != 0;
        }

        /// <summary>
        /// Property to access and set the value of the square
        /// </summary>
        public byte Value
        {
            get { return mFinalValue;}
            set
            {
                mPossibleValues = new List<byte>();
                mFinalValue = value;
            }
        }

        /// <summary>
        /// Property to access the list of possible values for this square
        /// </summary>
        public List<byte> PossibleValues
        {
            get { return mPossibleValues; }
        }

        /// <summary>
        /// Property to access the row value of this square
        /// </summary>
        public int Row
        {
            get { return mRow; }
        }

        /// <summary>
        /// Property to access the column value of this square
        /// </summary>
        public int Column
        {
            get { return mColumn; }
        }
    }

    /// <summary>
    /// This class represents an entity (row, column, 3x3 box) that must have the all the values 1-9 once and only once.
    /// </summary>
    public class Entity
    { 
        private Square[] mSquares;
        private List<byte> mMissingValues;

        public Entity(Square[] pSquares)
        {
            mSquares = pSquares;
            mMissingValues = new List<byte>() {1,2,3,4,5,6,7,8,9};
        }

        /// <summary>
        /// Adds a value from 1-9 in the entity.
        /// </summary>
        /// <param name="pValue">The value to add</param>
        /// <returns>Returns true if the new value is valid and false if that value is already in this entity</returns>
        public Boolean setKnownValue(byte pValue)
        {
            foreach (Square square in mSquares)
            {
                square.eliminatePossibleValue(pValue);
            }

            return mMissingValues.Remove(pValue);
        }

        /// <summary>
        /// Property to access the list of values missing from the entity
        /// </summary>
        public List<byte> UnknownValues
        {
            get { return mMissingValues; }
        }

        /// <summary>
        /// Property to access that list of 9 squares in the entity
        /// </summary>
        public Square[] Squares
        {
            get { return mSquares; }
        }
    }

    /// <summary>
    /// Class to represent a box entity
    /// </summary>
    public class Box : Entity
    {
        public Box(Square[] pSquares) : base(pSquares)
        { 
        }
    }

    /// <summary>
    /// Class to represent a row entity
    /// </summary>
    public class Row : Entity
    { 
        public Row(Square[] pSquares) : base(pSquares)
        { 
        }
    }

    /// <summary>
    /// Class to represent a column entity
    /// </summary>
    public class Column : Entity
    { 
        public Column(Square[] pSquares) : base(pSquares)
        { 
            
        }
    }

    /// <summary>
    /// This class contains the static methods used to solve sudokus and generate hints
    /// </summary>
    public class SudokuSolver
    {
        /// <summary>
        /// Solves a given sudoku. 
        /// </summary>
        public static SolveResult solve(byte[] pSudokuValues, BackgroundWorker pBackgroundSudokuSolver)
        {
            try
            { 
                Board board = new Board();
                int progress = 0;

                // Set initial values
                for (int i = 0; i < pSudokuValues.Length; i++)
                {
                    if (pSudokuValues[i] != 0)
                    {
                        // Update progress
                        pBackgroundSudokuSolver.ReportProgress(++progress);


                        int row = i / Constants.BOARD_SIZE;
                        int column = i % Constants.BOARD_SIZE;

                        if (!board.setKnownValue(row, column, pSudokuValues[i]))
                        {
                            return SolveResult.createInvalidResult();
                        }
                    }
                }

                // Main solving technique loop
                while (true)
                {
                    // Check if operation was cancelled
                    if (pBackgroundSudokuSolver.CancellationPending)
                    {
                        return SolveResult.createCancelledResult();
                    }

                    // Check if the sudoku is solved
                    if (board.isSolved())
                    {

                        byte[] solvedValues = new byte[81];
                        for (int i = 0; i < pSudokuValues.Length; i++)
                        {
                            int row = i / Constants.BOARD_SIZE;
                            int column = i % Constants.BOARD_SIZE;

                            solvedValues[i] = board.Grid[row, column].Value;
                        }

                        return SolveResult.createSuccessResult(solvedValues);
                    }

                    // Check if a value has been found on the board
                    if (board.checkForKnownValue())
                    {
                        // Update progress
                        pBackgroundSudokuSolver.ReportProgress(++progress);
                        continue;
                    }

                    // ###############################################################################################################
                    // Only Possibility Technique
                    //  - There is only one value that can possibly go in a square
                    // ###############################################################################################################
                    foreach (Entity entity in board.Entities)
                    {
                        foreach (Square square in entity.Squares)
                        {
                            if (square.PossibleValues.Count == 1)
                            {
                                foreach (Square eliminateSquare in entity.Squares)
                                {
                                    if (eliminateSquare != square)
                                    {
                                        eliminateSquare.eliminatePossibleValue(square.PossibleValues[0]);
                                    }
                                }
                            }
                        }
                    }

                    if (board.checkForKnownValue())
                    {
                        // Update progress
                        pBackgroundSudokuSolver.ReportProgress(++progress);
                        continue;
                    }

                    // ###############################################################################################################
                    // Possible Pair Shadow Technique
                    //  - If there are only two possible squares where a value can go within an entity and those square are 
                    //    also part of a second entity, that value can go nowhere else but those two squares in the second entity 
                    //  - TODO: Make this a possible pair and triple shadow
                    // ###############################################################################################################
                    foreach (Entity entity in board.Entities)
                    {
                        // For each possible value in the entity, check if there is only two squares where it can be
                        foreach (byte value in entity.UnknownValues)
                        {
                            List<Square> candidateSquares = new List<Square>();

                            foreach (Square square in entity.Squares)
                            {
                                if (square.PossibleValues.Contains(value))
                                {
                                    candidateSquares.Add(square);
                                }
                            }

                            // If the candidate square are entirely within another entity then eliminate the possible value
                            // from all other squares in that entity
                            if (candidateSquares.Count == 2)
                            {
                                // Check if any other entity also contains these two squares
                                foreach (Entity overlapEntity in board.Entities)
                                {
                                    if (!candidateSquares.Except(overlapEntity.Squares).Any())
                                    {
                                        foreach (Square square in overlapEntity.Squares.Except(candidateSquares))
                                        {
                                            square.eliminatePossibleValue(value);                                       
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Check if a value has been found on the board
                    if (board.checkForKnownValue())
                    {
                        // Update progress
                        pBackgroundSudokuSolver.ReportProgress(++progress);
                        continue;
                    }

                    // ###############################################################################################################
                    // Possible Value Closure Technique
                    //  - If n values within an entity have the same n possible square where they can go, then nothing else can go
                    //    there. 
                    // ###############################################################################################################
                    foreach (Entity entity in board.Entities)
                    {
                        // If the entity has only 2 unknown values then we already know nothing else can go in its 2 free squares
                        if (entity.UnknownValues.Count < 3)
                        {
                            continue;
                        }

                        // Create dictionary of possible squares for each unknown value in the entity
                        Dictionary<byte, List<Square>> possibleSquareSets = new Dictionary<byte, List<Square>>();
                        foreach (byte value in entity.UnknownValues)
                        {
                            List<Square> candidateSquares = new List<Square>();

                            foreach (Square square in entity.Squares)
                            {
                                if (square.PossibleValues.Contains(value))
                                {
                                    candidateSquares.Add(square);
                                }
                            }
                            possibleSquareSets.Add(value, candidateSquares);
                        }
                        
                        // Create list of possible unknown value combinations. Size of combinations is n, where n is 2 - ((unkown values count) - 1)
                        List<List<byte>>[] combos = new List<List<byte>>[entity.UnknownValues.Count - 2];
                        for (int i = 0; i < entity.UnknownValues.Count - 2; i++)
                        {
                            combos[i] = new List<List<byte>>();
                        }

                        for (int n = 1; n < ((1 << entity.UnknownValues.Count) - 1); n++)
                        {
                            List<byte> combo = new List<byte>();
                            for (int i = 0; i < entity.UnknownValues.Count; i++)
                            {
                                if (((1 << i) & n) == 0)
                                {
                                    combo.Add(entity.UnknownValues[i]);
                                }
                            }
                          
                            if(combo.Count > 1)
                            {
                                combos[combo.Count - 2].Add(combo);
                            }
                        }

                        // Use the generated possible value combinations to see if a combination of size n has only n possible squares
                        for (int n = 0; n < combos.Length; n++)
                        {
                            foreach (List<byte> combosSizeN in combos[n])
                            {
                                IEnumerable<Square> union = new List<Square>();
                                foreach (byte value in combosSizeN)
                                {
                                    union = union.Union(possibleSquareSets[value]);
                                }
                                
                                List<Square> unionList = union.ToList();
                                if (unionList.Count == n + 2)
                                {
                                    foreach (Square square in unionList)
                                    {
                                        foreach (byte value in entity.UnknownValues.Except(combosSizeN))
                                        {
                                            square.eliminatePossibleValue(value);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Check if a value has been found on the board
                    if (board.checkForKnownValue())
                    {
                        // Update progress
                        pBackgroundSudokuSolver.ReportProgress(++progress);
                        continue;
                    }

                    // #############################################################################################################
                    // TODO - Think of a name for this technique
                    // #############################################################################################################

                    List<Dictionary<byte, List<Square>>> entityPossibleSquareDictionaries = new List<Dictionary<byte, List<Square>>>();
                    foreach (Entity entity in board.Entities)
                    {
                        Dictionary<byte, List<Square>> possibleSquareSets = new Dictionary<byte, List<Square>>();
                        foreach (byte value in entity.UnknownValues)
                        {
                            List<Square> candidateSquares = new List<Square>();

                            foreach (Square square in entity.Squares)
                            {
                                if (square.PossibleValues.Contains(value))
                                {
                                    candidateSquares.Add(square);
                                }
                            }
                            possibleSquareSets.Add(value, candidateSquares);
                        }

                        entityPossibleSquareDictionaries.Add(possibleSquareSets);
                    }

                    foreach (Dictionary<byte, List<Square>> possibleSquareSets in entityPossibleSquareDictionaries)
                    {
                    
                    
                    }



                    // Check if a value has been found on the board
                    if (board.checkForKnownValue())
                    {
                        // Update progress
                        pBackgroundSudokuSolver.ReportProgress(++progress);
                        continue;
                    }
                    else
                    {
                        return SolveResult.createInvalidResult();
                    }      
                }

              

            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                return SolveResult.createErrorResult(e.Message);
            }
        }
    }
}
