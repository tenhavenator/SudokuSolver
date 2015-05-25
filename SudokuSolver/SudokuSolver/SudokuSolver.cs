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

        public Board(byte [,] pInitialValues)
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
                mEntities.Add(new Box(new List<Square>() { 
                    mGrid[boxStartRow, boxStartColumn], mGrid[boxStartRow, boxStartColumn + 1], mGrid[boxStartRow, boxStartColumn + 2],
                    mGrid[boxStartRow + 1, boxStartColumn], mGrid[boxStartRow + 1, boxStartColumn + 1], mGrid[boxStartRow + 1, boxStartColumn + 2],
                    mGrid[boxStartRow + 2, boxStartColumn], mGrid[boxStartRow + 2, boxStartColumn + 1], mGrid[boxStartRow + 2, boxStartColumn + 2]}));

                // Create a column that contains the correct squares
                 mEntities.Add(new Column(new List<Square>() { 
                    mGrid[0,i], mGrid[1,i], mGrid[2,i], mGrid[3,i], mGrid[4,i], mGrid[5,i], mGrid[6,i], mGrid[7,i], mGrid[8,i]}));

                // Create a row that contains the correct squares
                 mEntities.Add(new Row(new List<Square>() { 
                    mGrid[i,0], mGrid[i,1], mGrid[i,2], mGrid[i,3], mGrid[i,4], mGrid[i,5], mGrid[i,6], mGrid[i,7], mGrid[i,8]}));
            }

            // Insert the initial values
            foreach (Square square in mGrid)
            {
                byte value = pInitialValues[square.Row, square.Column];
                if (value != 0)
                { 
                    applyGivenValueTechnique(value, square.Row, square.Column);
                }
            }
        }

        /// <summary>
        /// Checks if all squares have been assigned a value.
        /// </summary>
        /// <returns>Returns true if all squares have a been assigned a value </returns>
        public Boolean isSolved()
        {
            // Check if the sudoku is solved
            foreach (Square square in mGrid)
            {
                if (!square.isFilled())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Inserts a value into a square on the board
        /// </summary>
        /// <param name="pValue">The value to insert</param>
        /// <param name="pSquare">The suqare to insert the value into</param>
        /// <param name="pTechnique">The technique used to find the value</param>
        private void insertValue(byte pValue, Square pSquare, SolvingTechnique pTechnique)
        {
            if (!pSquare.isFilled())
            {
                pSquare.setValue(pValue, ++mFoundValues, pTechnique);

                int boxStartRow = (pSquare.Row / Constants.BOX_SIZE) * Constants.BOX_SIZE;
                int boxStartColumn = (pSquare.Column / Constants.BOX_SIZE) * Constants.BOX_SIZE;

                for (int i = 0; i < Constants.BOARD_SIZE; i++)
                {
                    mGrid[boxStartRow + (i / Constants.BOX_SIZE), boxStartColumn + (i % Constants.BOX_SIZE)].eliminatePossibleValue(pValue, pTechnique);
                    mGrid[pSquare.Row, i].eliminatePossibleValue(pValue, pTechnique);
                    mGrid[i, pSquare.Column].eliminatePossibleValue(pValue, pTechnique);
                }

                foreach (byte value in new List<byte>(pSquare.PossibleValues))
                {
                    pSquare.eliminatePossibleValue(value, new SquareOccupiedTechnique());
                }
            }
        }

        /// <summary>
        /// Apply the given value technique to insert a given value into the board
        /// </summary>
        /// <param name="pValue">The value to insert</param>
        /// <param name="pRow">The row to insert it in</param>
        /// <param name="pColumn">The column to insert it in</param>
        private void applyGivenValueTechnique(byte pValue, int pRow, int pColumn)
        {
            insertValue(pValue, mGrid[pRow, pColumn], new GivenValueTechnique());
        }

        /// <summary>
        /// Apply the found value technique to the board. Found Value Technique: A value has been found in row, column, 
        /// or box by following the basic principle that each must only have the values 1-9 or there is a square with 
        /// only one possible value
        /// </summary>
        /// <returns>The number of values found</returns>
        public int applyFoundValueTechnique()
        {
            int foundValues = 0;

            // Check for values with only onw possible square where they can go
            foreach (Entity entity in mEntities)
            {
                foreach (byte value in entity.MissingValues)
                {
                    List<Square> candidateSquares = entity.ValueCandidateSquares(value);

                    if (candidateSquares.Count == 1)
                    {
                        insertValue(value, candidateSquares.First(), entity.EntityTechnique());
                        foundValues++;
                    }
                }
            }

            // Check for quares that can only have one value
            foreach (Square square in mGrid)
            {
                if (square.PossibleValues.Count == 1)
                {
                    insertValue(square.PossibleValues.First(), square, new OnlyPossibleValueTechnique());
                    foundValues++;
                }
            }

            return foundValues;
        }

        /// <summary>
        /// Apply the Possible Pair Overlap technique to the board. Possible Pair Overlap Technique: If there are only
        /// two possible squares where a value can go within an entity (overlapper) and both those square are also part of
        /// a second entity (overlapee), that value can go nowhere else but those two squares in the overlapee. Note that 
        /// the overlapper or overlapee, but not both, will be a box
        /// </summary>
        public void applyPossibleValueOverlapTechnique()
        {
            foreach (Entity entity in mEntities)
            {
                foreach (byte value in entity.MissingValues)
                {
                    List<Square> candidateSquares = entity.ValueCandidateSquares(value);

                    // For each possible value in the entity, check if there is only two squares where it can be
                    if (candidateSquares.Count == 2)
                    {
                        // Check if any other entity also contains these two squares and is missing the value
                        Func<Entity, bool> entityFilter = e => !candidateSquares.Except(e.ValueCandidateSquares(value)).Any() && e != entity;

                        foreach (Entity overlapee in mEntities.Where(entityFilter))
                        {
                            PossibleValueOverlapTechnique overlapTechnique = new PossibleValueOverlapTechnique();

                            foreach (Square square in overlapee.Squares.Except(candidateSquares))
                            {
                                square.eliminatePossibleValue(value, overlapTechnique);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply the Possible Value Closure technique to the board. Possible Value Closure technique: If n values within 
        /// an entity have the same n possible square where they can go, then nothing else can go there. Right now this is 
        /// only done for 2 or 3 values.
        /// </summary>
        public void applyPossibleValueClosureTechnique()
        {
          foreach (Entity entity in mEntities.Where(e => e.MissingValues.Count > 2))
            {
                List<byte> missingCandidates = entity.MissingValues.Where(mv => entity.ValueCandidateSquares(mv).Count < 4).ToList();

                // Use binary counting to generate each of the possible combinations of missing values
                // There will be (1 << missingCandidates.Count) combinations, but we through away any combination that
                // is not size two or three
                for (int n = 0; n < (1 << missingCandidates.Count); n++)
                {
                    List<byte> combo = new List<byte>();
                    for (int i = 0; i < missingCandidates.Count; i++)
                    {
                        if (((1 << i) & n) == 0)
                        {
                            combo.Add(missingCandidates[i]);
                        }
                    }

                    // Check if the values in generated combination occupy the same set of squares
                    if (combo.Count == 2 || combo.Count == 3)
                    {
                        IEnumerable<Square> squareUnion = new List<Square>();
                        foreach (byte value in combo)
                        {
                            squareUnion = squareUnion.Concat(entity.ValueCandidateSquares(value));
                        }

                        List<Square> closureSquares = squareUnion.Distinct().ToList() ;
                        if (closureSquares.Count() == combo.Count)
                        {
                            PossibleValueClosureTechnique closureTechnique = new PossibleValueClosureTechnique();

                            foreach (byte value in closureSquares.SelectMany(s => s.PossibleValues).Except(combo).ToList())
                            {
                                foreach (Square square in closureSquares)
                                {
                                    square.eliminatePossibleValue(value, closureTechnique);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply the Possible Value Row/Column shadow technique to the board. Possible Value Row/Column Shadow technique: 
        /// If two entities have only two possible squares for a value and those possible squares are in the same two columns
        /// or the same two rows, then those two rows or columns cannot have that value anywhere else except in those two 
        /// squares
        /// </summary>
        public void applyRowColumnShadowTechnique()
        {
            foreach (Entity entityA in mEntities)
            {
                foreach (byte value in entityA.MissingValues)
                {
                    List<Square> candidateSquaresA = entityA.ValueCandidateSquares(value);
                    if (candidateSquaresA.Count == 2)
                    {
                        Func<Entity, bool> entityFilter = e =>
                        {
                            IEnumerable<Square> squares = e.ValueCandidateSquares(value).Concat(candidateSquaresA);
                            return squares.Count() == 4 && squares.Distinct().Count() == 4;
                        };

                        // Check if there is another entity that is missing the value and has a different set of possible
                        // squares for the value
                        foreach (Entity entityB in mEntities.Where(entityFilter))
                        {
                            List<Square> squares = candidateSquaresA.Concat(entityB.ValueCandidateSquares(value)).ToList();

                            List<int> rows = squares.Select(s => s.Row).ToList();
                            List<int> columns = squares.Select(s => s.Column).ToList();

                            // Check if the two entities create a row shadow for the value
                            if (rows.Distinct().Count() == 2 && rows.Skip(2).Distinct().Count() == 2)
                            {
                                PossibleValueRowShadow rowShadowTechnique = new PossibleValueRowShadow();

                                for (int i = 0; i < Constants.BOARD_SIZE; i++)
                                {
                                    if (!squares.Contains(mGrid[rows[0], i]))
                                    {
                                        mGrid[rows[0], i].eliminatePossibleValue(value, rowShadowTechnique);
                                    }

                                    if (!squares.Contains(mGrid[rows[1], i]))
                                    {
                                        mGrid[rows[1], i].eliminatePossibleValue(value, rowShadowTechnique);
                                    }
                                }
                            }

                            // Check if the two entities create a column shadow for the value
                            if (columns.Distinct().Count() == 2 && columns.Skip(2).Distinct().Count() == 2)
                            {
                                PossibleValueColumnShadow columnShadowTechnique = new PossibleValueColumnShadow();

                                for (int i = 0; i < Constants.BOARD_SIZE; i++)
                                {
                                    if (!squares.Contains(mGrid[i, columns[0]]))
                                    {
                                        mGrid[i, columns[0]].eliminatePossibleValue(value, columnShadowTechnique);
                                    }

                                    if (!squares.Contains(mGrid[i, columns[1]]))
                                    {
                                        mGrid[i, columns[1]].eliminatePossibleValue(value, columnShadowTechnique);
                                    }
                                } 
                            }
                        }
                    }
                }
            }
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
        private int mOrderSolved;
        private List<byte> mPossibleValues;

        private Dictionary<byte, SolvingTechnique> mEliminatedValueTechniques;
        private SolvingTechnique mTechnique;

        public Square(int pRow, int pColumn)
        {
            mRow = pRow;
            mColumn = pColumn;
            mFinalValue = 0;
            mOrderSolved = 0;
            mPossibleValues = new List<byte>() {1,2,3,4,5,6,7,8,9};
            mEliminatedValueTechniques = new Dictionary<byte, SolvingTechnique>();
        }

        /// <summary>
        /// Removes a possible value from a sqaure
        /// </summary>
        /// <param name="pValue">The value to remove</param>
        /// <param name="pTechnique">The technique used to remove the value</param>
        public void eliminatePossibleValue(byte pValue, SolvingTechnique pTechnique)
        {
            if (mPossibleValues.Contains(pValue))
            {
                mPossibleValues.Remove(pValue);
                mEliminatedValueTechniques.Add(pValue, pTechnique);
            }
        }

        /// <summary>
        /// Sets a value for this square
        /// </summary>
        /// <param name="pValue">The value to set</param>
        /// <param name="pOrderSolved">The order in which the value was found on the board</param>
        /// <param name="pTechnique">The technique used to find the value</param>
        public void setValue(byte pValue, int pOrderSolved, SolvingTechnique pTechnique)
        {
            mFinalValue = pValue;
            mOrderSolved = pOrderSolved;
            mTechnique = pTechnique;
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
        /// Property to access the list of possible values for this square
        /// </summary>
        public List<byte> PossibleValues
        {
            get { return mPossibleValues; }
        }

        /// <summary>
        /// Property to access and set the value of the square
        /// </summary>
        public byte Value
        {
            get { return mFinalValue;}
        }

        /// <summary>
        /// Property to access the order in which this square was solved
        /// </summary>
        public int Order
        {
            get { return mOrderSolved;  }
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
    /// The possible squares for each value in the entity are tracked and eliminated one by one.
    /// </summary>
    public abstract class Entity
    { 
        private List<Square> mSquares;
        private List<byte> mMissingValues;
        private Dictionary<byte, List<Square>> mCandidateSquares;
        
        public Entity(List<Square> pSquares)
        {
            mSquares = pSquares;
            mMissingValues = new List<byte>(){ 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            mCandidateSquares = new Dictionary<byte, List<Square>>();
            mMissingValues.ForEach(value => mCandidateSquares.Add(value, new List<Square>(mSquares)));
        }

        /// <summary>
        /// Get the set of candidate squares for a certain value. This set is updated when this method is called
        /// </summary>
        /// <param name="pValue">The value to retreive candidate squares for</param>
        /// <returns>The list of candidate squares</returns>
        public List<Square> ValueCandidateSquares(byte pValue)
        {
            return mCandidateSquares[pValue] = mCandidateSquares[pValue].Where(square => square.PossibleValues.Contains(pValue)).ToList();;
        }

        /// <summary>
        /// Property to access the list of missing values for this entity. This list is updated when this method is called.
        /// </summary>
        public List<byte> MissingValues
        {
            get
            {
                return mMissingValues = mMissingValues.Where(value => this.ValueCandidateSquares(value).Any()).ToList();
            }
        }

        /// <summary>
        /// Property to access that list of 9 squares in the entity
        /// </summary>
        public List<Square> Squares
        {
            get { return mSquares; }
        }

        /// <summary>
        /// Creates a solving technique for the entity. This is abstract so the correct technique for each type of entity
        /// (bow, row, column) can be created.
        /// </summary>
        /// <returns>The solving technique created</returns>
        public abstract SolvingTechnique EntityTechnique();

    }

    /// <summary>
    /// This class represents the 3x3 Box entity
    /// </summary>
    public class Box : Entity
    {
        public Box(List<Square> pSquares) : base(pSquares) { }

        /// <summary>
        ///  Creates a solving technique for a Box
        /// </summary>
        /// <returns>The solving technique created</returns>
        public override SolvingTechnique EntityTechnique()
        {
            return new BoxFoundValueTechnique();
        }
    }

    /// <summary>
    /// This class respresents the row entity
    /// </summary>
    public class Row : Entity
    {
        public Row(List<Square> pSquares) : base(pSquares) { }

        /// <summary>
        ///  Creates a solving technique for a Row
        /// </summary>
        /// <returns>The solving technique created</returns>
        public override SolvingTechnique EntityTechnique()
        {
            return new RowFoundValueTechnique();
        }

    }

    /// <summary>
    /// This class represents the column entity
    /// </summary>
    public class Column : Entity
    {
        public Column(List<Square> pSquares) : base(pSquares) { }

        /// <summary>
        ///  Creates a solving technique for a Column
        /// </summary>
        /// <returns>The solving technique created</returns>
        public override SolvingTechnique EntityTechnique()
        {
            return new ColumnFoundValueTechnique();
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
                Board board = new Board(pSudokuValues);
                double progress = 0.0;

                // Main solving loop. A value should be found on every iteration
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
                        return SolveResult.createSuccessResult(board.Grid);
                    }

                    // Check for known values
                    int foundValues = board.applyFoundValueTechnique();
                    if (foundValues > 0)
                    {
                        progress += foundValues * VALUE_PERCENT;
                        pBackgroundSudokuSolver.ReportProgress(Convert.ToInt32(progress));
                        Thread.Sleep(SLEEP_TIME_MS);
                        continue;
                    }

                    // Apply Possible overlap technique
                    board.applyPossibleValueOverlapTechnique();

                    // Check for known values
                    foundValues = board.applyFoundValueTechnique();
                    if (foundValues > 0)
                    {
                        progress += foundValues * VALUE_PERCENT;
                        pBackgroundSudokuSolver.ReportProgress(Convert.ToInt32(progress));
                        Thread.Sleep(SLEEP_TIME_MS);
                        continue;
                    }

                    // Apply Possible value closure technique
                    board.applyPossibleValueClosureTechnique();

                    // Check for known values
                    foundValues = board.applyFoundValueTechnique();
                    if (foundValues > 0)
                    {
                        progress += foundValues * VALUE_PERCENT;
                        pBackgroundSudokuSolver.ReportProgress(Convert.ToInt32(progress));
                        Thread.Sleep(SLEEP_TIME_MS);
                        continue;
                    }

                    // Apply Possible value closure technique
                    board.applyRowColumnShadowTechnique();

                    // Check for known values
                    foundValues = board.applyFoundValueTechnique();
                    if (foundValues > 0)
                    {
                        progress += foundValues * VALUE_PERCENT;
                        pBackgroundSudokuSolver.ReportProgress(Convert.ToInt32(progress));
                        Thread.Sleep(SLEEP_TIME_MS);
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
