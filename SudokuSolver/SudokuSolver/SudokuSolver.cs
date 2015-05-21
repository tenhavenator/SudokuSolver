/// <summary>
/// This file contains the functions and classes used for solving sudokus.
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
        private Square[,] mGrid;
        private List<Entity> mEntities;
        private int mFoundValues;

        public Board()
        {
            mEntities = new List<Entity>();
            mFoundValues = 0;

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
                mEntities.Add(new Entity(new List<Square>() { 
                    mGrid[boxStartRow, boxStartColumn], mGrid[boxStartRow, boxStartColumn + 1], mGrid[boxStartRow, boxStartColumn + 2],
                    mGrid[boxStartRow + 1, boxStartColumn], mGrid[boxStartRow + 1, boxStartColumn + 1], mGrid[boxStartRow + 1, boxStartColumn + 2],
                    mGrid[boxStartRow + 2, boxStartColumn], mGrid[boxStartRow + 2, boxStartColumn + 1], mGrid[boxStartRow + 2, boxStartColumn + 2]}));

                // Create a column that contains the correct squares
                 mEntities.Add(new Entity(new List<Square>() { 
                    mGrid[0,i], mGrid[1,i], mGrid[2,i], mGrid[3,i], mGrid[4,i], mGrid[5,i], mGrid[6,i], mGrid[7,i], mGrid[8,i]}));

                // Create a row that contains the correct squares
                 mEntities.Add(new Entity(new List<Square>() { 
                    mGrid[i,0], mGrid[i,1], mGrid[i,2], mGrid[i,3], mGrid[i,4], mGrid[i,5], mGrid[i,6], mGrid[i,7], mGrid[i,8]}));
            }
        }

        public void setGivenValue(int pRow, int pColumn, byte pValue)
        {
            mGrid[pRow, pColumn].setFoundValue(pValue);
        }

        /// <summary>
        /// Enters a known value into a square on the board
        /// </summary>
        /// <param name="pRow">The row value of the square</param>
        /// <param name="pColumn">The column value of the square</param>
        /// <param name="pValue">The value to be put in the square</param>
        public void setFoundValue(int pRow, int pColumn, byte pValue)
        {
            mGrid[pRow, pColumn].setFoundValue(pValue, ++mFoundValues);
        }

        /// <summary>
        /// Check if a value has been found on the board and if so insert it
        /// </summary>
        /// <returns>True if a value was found and false if not</returns>
        public Boolean checkForKnownValue()
        {
            foreach (Entity entity in mEntities)
            {
                // For each possible value in the entity, check if there is only one square where it can be
                foreach (byte value in entity.UnknownValues)
                {
                    List<Square> candidateSquares = entity.candidateSquaresForValue(value);

                    // Add value to the board if it has been found. 
                    if (candidateSquares.Count == 1)
                    {
                        setFoundValue(candidateSquares[0].Row, candidateSquares[0].Column, value);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if all squares have been assigned a value.
        /// </summary>
        /// <returns>Returns true if all squares have a been assigned a value </returns>
        public Boolean isSolved()
        {
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
        private byte mFinalValue;
        private List<Action<byte, Square>> mEntityNotifiers;
        private int mOrderSolved;

        public Square(int pRow, int pColumn)
        {
            mRow = pRow;
            mColumn = pColumn;
            mFinalValue = 0;
            mEntityNotifiers = new List<Action<byte, Square>>();
            mOrderSolved = 0;
        }

        /// <summary>
        /// Removes a value from the list of values that can possibly occupy this square. Notifies all listening entities that the
        /// value cannot go in this square anymore.
        /// </summary>
        /// <param name="pValue">The value to be removed</param>
        public void eliminatePossibleValue(byte pValue)
        {
            if (!isFilled())
            {
                mEntityNotifiers.ForEach(notifier => notifier(pValue, this));
            }
        }

        /// <summary>
        /// Sets a value to be in this square. Sets the order in which the value was found. Notifies all listening
        /// entities that there is now a value in this square.
        /// </summary>
        /// <param name="pValue">The value to be placed in this square</param>
        /// <param name="pOrderSolved">The order in which the values was found</param>
        public void setFoundValue(byte pValue, int pOrderSolved)
        {
            mOrderSolved = pOrderSolved;
            setFoundValue(pValue);
        }

        /// <summary>
        /// Sets a value to be in this square. Notifies all listening entities that there is now a value in this square.
        /// </summary>
        /// <param name="pValue">The value to be placed in this square</param>
        public void setFoundValue(byte pValue)
        {
            mFinalValue = pValue;
            mEntityNotifiers.ForEach(notifier => notifier(pValue, this));
        }

        /// <summary>
        /// Adds a notifier to this square that will be activated when the square publishes a notification
        /// </summary>
        /// <param name="pNotifier">The notifier to activate</param>
        public void addEntityNotifier(Action<byte, Square> pNotifier)
        {
            mEntityNotifiers.Add(pNotifier);
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
    /// This class represents an entity (row, column, 3x3 box) that must have the all the values 1-9 once and only once. The possible 
    /// squares for each value in the entity are tracked and eliminated one by one.
    /// </summary>
    public class Entity
    { 
        private List<Square> mSquares;
        private List<byte> mMissingValues;
        private Dictionary<byte, List<Square>> mCandidateSquares;
        private Dictionary<byte, List<Tuple<Square, SolvingTechnique>>> mEliminatedSquares;

        public Entity(List<Square> pSquares)
        {
            mSquares = pSquares;
            mMissingValues = new List<byte>() {1,2,3,4,5,6,7,8,9};
            mCandidateSquares = new Dictionary<byte, List<Square>>();
            mEliminatedSquares = new Dictionary<byte, List<Tuple<Square, SolvingTechnique>>>();

            mSquares.ForEach(square => square.addEntityNotifier(this.squareNotificationHandler));
            mMissingValues.ForEach(value => mCandidateSquares.Add(value, new List<Square>(mSquares)));
        }

        /// <summary>
        /// Tiggered when a square publishes a notification indicating that it has been filled or that a 
        /// possible value has been eliminated
        /// </summary>
        /// <param name="pValue">The value final value entered or the possible value removed from a square</param>
        /// <param name="pSquare">The square that published the notification</param>
        private void squareNotificationHandler(byte pValue, Square pSquare)
        {
            if (mMissingValues.Contains(pValue))
            {
                // The square has been filled
                if (pSquare.isFilled())
                {
                    mCandidateSquares.Remove(pValue);
                    mMissingValues.Remove(pValue);
                    mMissingValues.ForEach(value => mCandidateSquares[value].Remove(pSquare));
                    mSquares.ForEach(square => square.eliminatePossibleValue(pValue));
                }
                // A possible value has been eliminated from the square
                else
                {
                    if (mCandidateSquares.ContainsKey(pValue))
                    {
                        mCandidateSquares[pValue].Remove(pSquare);
                    }
                }
            }
          
        }

        /// <summary>
        /// Get the set of candidate squares for a certain value
        /// </summary>
        /// <param name="pValue">The value to retreive candidate squares for</param>
        /// <returns>The list of candidate squares</returns>
        public List<Square> candidateSquaresForValue(byte pValue)
        {
            List<Square> candidateSqaures;
            if (mCandidateSquares.TryGetValue(pValue, out candidateSqaures))
            {
                return candidateSqaures;
            }

            return new List<Square>();
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
        public List<Square> Squares
        {
            get { return mSquares; }
        }
    }




    /// <summary>
    /// This class contains the static method used to solve sudokus
    /// </summary>
    public class SudokuSolver
    {
        private const int SLEEP_TIME_MS = 62;
        private const double VALUE_PERCENT  = 1.23;

        /// <summary>
        /// Solves a given sudoku. 
        /// </summary>
        public static SolveResult solve(byte[,] pSudokuValues, BackgroundWorker pBackgroundSudokuSolver)
        {
            try
            { 
                Board board = new Board();
                double progress = 0.0;

                // Set initial values
                for (int row = 0; row < Constants.BOARD_SIZE; row++)
                {
                    for (int column = 0; column < Constants.BOARD_SIZE; column++)
                    {
                        // Check if operation was cancelled
                        if (pBackgroundSudokuSolver.CancellationPending)
                        {
                            return SolveResult.createCancelledResult();
                        }

                        // An intial values has been found and will be entered on the board
                        if (pSudokuValues[row, column] != 0)
                        {
                            // Update progress
                            progress += VALUE_PERCENT;
                            pBackgroundSudokuSolver.ReportProgress(Convert.ToInt32(progress));
                            Thread.Sleep(SLEEP_TIME_MS);

                            board.setGivenValue(row, column, pSudokuValues[row, column]);
                        }
                    }
                }

                // Main solving technique loop
                while (true)
                {
                    // Update progress
                    progress += VALUE_PERCENT;
                    pBackgroundSudokuSolver.ReportProgress(Convert.ToInt32(progress));
                    Thread.Sleep(SLEEP_TIME_MS);

                    // Check if operation was cancelled
                    if (pBackgroundSudokuSolver.CancellationPending)
                    {
                        return SolveResult.createCancelledResult();
                    }

                    // Check if the sudoku is solved
                    if (board.isSolved())
                    {
                        byte[,] solvedValues = new byte[Constants.BOARD_SIZE,Constants.BOARD_SIZE];
                        for (int row = 0; row < Constants.BOARD_SIZE; row++)
                        {
                            for (int column = 0; column < Constants.BOARD_SIZE; column++)
                            {
                                solvedValues[row, column] = board.Grid[row, column].Value;
                            }
                        }

                        return SolveResult.createSuccessResult(solvedValues);
                    }

                    // Check if a value has been found on the board
                    if (board.checkForKnownValue())
                    {
                        continue;
                    }

                    // ###############################################################################################################
                    // Only Possibility Technique
                    //  - There is only one value that can possibly go in a square
                    // ###############################################################################################################
                    Boolean valueFound = false;
                    foreach (Entity entity in board.Entities)
                    {
                        foreach (Square square in entity.Squares)
                        {
                            List<byte> possibleValues = new List<byte>();

                            // Check how many of the values in the entity can possibly go in this square
                            foreach (byte value in entity.UnknownValues)
                            {
                                if (entity.candidateSquaresForValue(value).Contains(square))
                                {
                                    possibleValues.Add(value);
                                }
                            }

                            // If only one value can go in this square, then insert that value into the square
                            if (possibleValues.Count == 1)
                            {
                                board.setFoundValue(square.Row, square.Column, possibleValues[0]);
                                valueFound = true;
                            }

                        }
                    }

                    // Check if a value was found on the board
                    if (valueFound)
                    {
                        continue;
                    }

                    // ###############################################################################################################
                    // Possible Value Overrlap Technique
                    //  - If there are only two possible squares where a value can go within an entity and both those square are 
                    //    also part of a second entity, that value can go nowhere else but those two squares in the second entity 
                    //  - TODO: Make this a possible pair and triple shadow
                    // ###############################################################################################################
                    foreach (Entity entity in board.Entities)
                    {
                        foreach (byte value in entity.UnknownValues)
                        {
                            List<Square> candidateSquares = entity.candidateSquaresForValue(value);

                            // For each possible value in the entity, check if there is only two squares where it can be
                            if (candidateSquares.Count == 2)
                            {
                                // Check if any other entity also contains these two squares
                                foreach (Entity overlapEntity in board.Entities)
                                {
                                    // If the candidate square are entirely within another entity then eliminate the possible value
                                    // from all other squares in that entity
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

                        // Create list of possible unknown value combinations. Size of combinations is n, where n is 2 - ((unkown values count) - 1)
                        List<List<byte>>[] combos = new List<List<byte>>[entity.UnknownValues.Count - 2];
                        for (int i = 0; i < entity.UnknownValues.Count - 2; i++)
                        {
                            combos[i] = new List<List<byte>>();
                        }

                        // Use binary counting to generate each of the possible combinations
                        // There will be (1 << entity.UnknownValues.Count) combinations, but we through away the empty combination,
                        // the full combination, and any combinations with only one element
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
                                IEnumerable<Square> squareUnion = new List<Square>();
                                combosSizeN.ForEach(value => squareUnion = squareUnion.Concat(entity.candidateSquaresForValue(value)));

                                squareUnion = squareUnion.Distinct();
                                if (squareUnion.Count() == n + 2)
                                {
                                    foreach (Square square in squareUnion)
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
                        continue;
                    }

                    // #############################################################################################################
                    // Dual Enitity Shadow Technique
                    // - If two entities have only two possible squares for a value and those possible squares are in the same two columns
                    //   or the same two rows, then those two rows or columns cannot have that value anywhere else except in those squares
                    // #############################################################################################################
                    foreach (Entity entityA in board.Entities)
                    {
                        foreach (byte value in entityA.UnknownValues)
                        {
                            List<Square> candidateSquaresA = entityA.candidateSquaresForValue(value);
                            if (candidateSquaresA.Count == 2)
                            { 
                                foreach (Entity entityB in board.Entities)
                                {
                                    List<Square> candidateSquaresB = entityB.candidateSquaresForValue(value);
                                    if (!candidateSquaresA.SequenceEqual(candidateSquaresB) && candidateSquaresB.Count == 2)
                                    {
                                        // Check if the two possible value sets share the same two rows
                                        if ((candidateSquaresA[0].Row == candidateSquaresB[0].Row
                                            && candidateSquaresA[1].Row == candidateSquaresB[1].Row))
                                        {
                                            for (int i = 0; i < Constants.BOARD_SIZE; i++)
                                            { 
                                                if(i != candidateSquaresA[0].Column && i != candidateSquaresB[0].Column)
                                                {
                                                    board.Grid[candidateSquaresA[0].Row, i].eliminatePossibleValue(value);
                                                }

                                                if (i != candidateSquaresA[1].Column && i != candidateSquaresB[1].Column)
                                                {
                                                    board.Grid[candidateSquaresA[1].Row, i].eliminatePossibleValue(value);
                                                }
                                            }
                                        }

                                        // Check if the two possible value sets share the same two rows
                                        if ((candidateSquaresA[0].Row == candidateSquaresB[1].Row
                                          && candidateSquaresA[1].Row == candidateSquaresB[0].Row))
                                        {
                                            for (int i = 0; i < Constants.BOARD_SIZE; i++)
                                            {
                                                if (i != candidateSquaresA[0].Column && i != candidateSquaresB[1].Column)
                                                {
                                                    board.Grid[candidateSquaresA[0].Row, i].eliminatePossibleValue(value);
                                                }

                                                if (i != candidateSquaresA[1].Column && i != candidateSquaresB[0].Column)
                                                {
                                                    board.Grid[candidateSquaresA[1].Row, i].eliminatePossibleValue(value);
                                                }
                                            }
                                        }

                                        // Check if the two possible value sets share the same two columns
                                        if ((candidateSquaresA[0].Column == candidateSquaresB[0].Column
                                            && candidateSquaresA[1].Column == candidateSquaresB[1].Column))
                                        {
                                            for (int i = 0; i < Constants.BOARD_SIZE; i++)
                                            {
                                                if (i != candidateSquaresA[0].Row && i != candidateSquaresB[0].Row)
                                                {
                                                    board.Grid[i, candidateSquaresA[0].Column].eliminatePossibleValue(value);
                                                }

                                                if (i != candidateSquaresA[1].Row && i != candidateSquaresB[1].Row)
                                                {
                                                    board.Grid[i, candidateSquaresA[1].Column].eliminatePossibleValue(value);
                                                }
                                            }
                                        }

                                        // Check if the two possible value sets share the same two columns
                                        if ((candidateSquaresA[0].Column == candidateSquaresB[1].Column
                                          && candidateSquaresA[1].Column == candidateSquaresB[0].Column))
                                        {
                                            for (int i = 0; i < Constants.BOARD_SIZE; i++)
                                            {
                                                if (i != candidateSquaresA[0].Row && i != candidateSquaresB[1].Row)
                                                {
                                                    board.Grid[i, candidateSquaresA[0].Column].eliminatePossibleValue(value);
                                                }

                                                if (i != candidateSquaresA[1].Row && i != candidateSquaresB[0].Row)
                                                {
                                                    board.Grid[i, candidateSquaresA[1].Column].eliminatePossibleValue(value);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
     
                    // Check if a value has been found on the board
                    if (board.checkForKnownValue())
                    {
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
