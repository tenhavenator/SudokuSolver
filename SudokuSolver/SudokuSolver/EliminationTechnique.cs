/// <summary>
/// This file contains the classes used to represent elimination techniques
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SudokuSolver
{
    /// <summary>
    /// The interface used to represent a value elimination technique
    /// </summary>
    public interface EliminationTechnique
    {
        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        void apply(TextBox[,] pTextBoxes);

        /// <summary>
        /// The rank of a technique represents its necesscity in solving values on the boad. The rank increases with the 
        /// complexity of the elimination technique
        /// </summary>
        int Rank
        {
            get;
        }

        /// <summary>
        /// The order of the technique is equivalent to the number of solved values on the board when the technique was used
        /// to eliminate a possible value
        /// </summary>
        int Order
        {
            get;
            set;
        }
    }

    /// <summary>
    /// This class represents the elimination technique where a value cannot go in a square because the square is already 
    /// occupied
    /// </summary>
    public class OccupiedEliminationTechnique : EliminationTechnique
    {
        private Square mSquare;
        private int mOrder;

        public OccupiedEliminationTechnique(Square pSquare)
        {
            mSquare = pSquare;
        }

        /// <summary>
        /// The rank of this technique is 1 because it is the most basic technique
        /// </summary>
        public int Rank
        {
            get { return 1; }
        }

        /// <summary>
        /// Property to access the order in which this technique was used to eliminate a value
        /// </summary>
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public void apply(TextBox[,] pTextBoxes)
        {
            TextBox textBox = pTextBoxes[mSquare.Row, mSquare.Column];
            textBox.Text = Convert.ToString(mSquare.Value);
            textBox.BackColor = Color.LightSalmon;
        }
    }

    /// <summary>
    /// This class represents the elimination technique where a value is eliminated because it already exists in the same box.
    /// </summary>
    public class BoxEliminationTechnique : EliminationTechnique
    {
        private Square mSquare;
        private int mOrder;

        public BoxEliminationTechnique(Square pSquare)
        {
            mSquare = pSquare;
        }

        /// <summary>
        /// The rank of the box technique is 2 because it is a basic solving rule and will always eliminate a value from 3 squares
        /// of each overlapping column and row.
        /// </summary>
        public int Rank
        {
            get { return 2; }
        }

        /// <summary>
        /// Property to access the order in which this technique was used to eliminate a value
        /// </summary>
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public void apply(TextBox[,] pTextBoxes)
        {
            int boxStartRow = (mSquare.Row / Constants.BOX_SIZE) * Constants.BOX_SIZE;
            int boxStartColumn = (mSquare.Column / Constants.BOX_SIZE) * Constants.BOX_SIZE;

            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                pTextBoxes[boxStartRow + (i / Constants.BOX_SIZE), boxStartColumn + (i % Constants.BOX_SIZE)].BackColor = Color.LightSalmon;
            }

            pTextBoxes[mSquare.Row, mSquare.Column].Text = Convert.ToString(mSquare.Value);
        }
    }

    /// <summary>
    /// This class represents the elimination technique where a value is eliminated because it already exists in the same row.
    /// </summary>
    public class RowEliminationTechnique : EliminationTechnique
    {
        private Square mSquare;
        private int mOrder;

        public RowEliminationTechnique(Square pSquare)
        {
            mSquare = pSquare;
        }

        /// <summary>
        /// The rank of this technique is 3 because it a basic solving rule but is still supercede by the box found technique.
        /// </summary>
        public int Rank
        {
            get { return 3; }
        }

        /// <summary>
        /// Property to access the order in which this technique was used to eliminate a value
        /// </summary>
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public void apply(TextBox[,] pTextBoxes) 
        {
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                pTextBoxes[mSquare.Row, i].BackColor = Color.LightSalmon;
            }

            pTextBoxes[mSquare.Row, mSquare.Column].Text = Convert.ToString(mSquare.Value);
        }
    }

    /// <summary>
    /// This class represents the elimination technique where a value is eliminated because it already exists in the same column.
    /// </summary>
    public class ColumnEliminationTechnique : EliminationTechnique
    {
        private Square mSquare;
        private int mOrder;

        public ColumnEliminationTechnique(Square pSquare)
        {
            mSquare = pSquare;
        }

        /// <summary>
        /// The rank of this technique is 3 because it a basic solving rule but is still supercede by the box found technique.
        /// </summary>
        public int Rank
        {
            get { return 3; }
        }

        /// <summary>
        /// Property to access the order in which this technique was used to eliminate a value
        /// </summary>
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public void apply(TextBox[,] pTextBoxes)
        {
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                pTextBoxes[i, mSquare.Column].BackColor = Color.LightSalmon;
            }

            pTextBoxes[mSquare.Row, mSquare.Column].Text = Convert.ToString(mSquare.Value);
        }
    }

  

    /// <summary>
    /// This class represents the technique used to eliminate values where the possible squares for a value in a row/column/box, 
    /// completly overlap a another row/column/box (overlapee), meaning that value for the overlapee can go nowhere but in those
    /// squares
    /// </summary>
    public class PossibleValueOverlapTechnique : EliminationTechnique
    {
        private Entity mOverlappee;
        private List<Square> mSquares;
        private byte mValue;
        private int mOrder;

        public PossibleValueOverlapTechnique(Entity pOverlappee, List<Square> pSquares, byte pValue)
        {
            mOverlappee = pOverlappee;
            mSquares = pSquares;
            mValue = pValue;
        }

        /// <summary>
        /// The rank of this technique is 4 because it is a level more advanced than the basic solving rules
        /// </summary>
        public int Rank
        {
            get { return 4; }
        }

        /// <summary>
        /// Property to access the order in which this technique was used to eliminate a value
        /// </summary>
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public void apply(TextBox[,] pTextBoxes)
        {
            foreach(Square square in mOverlappee.Squares)
            {
                pTextBoxes[square.Row, square.Column].BackColor = Color.LightSalmon;
            }

            foreach (Square square in mSquares)
            { 
                TextBox textBox = pTextBoxes[square.Row, square.Column];
                textBox.Font = new Font(textBox.Font.FontFamily, 18, FontStyle.Italic);
                textBox.Text = Convert.ToString(mValue);
            }
        }
    }


    /// <summary>
    /// This class represents the technique used to eliminate values where 2 or 3 values must go in the same 2 or 3 squares within 
    /// a row/column/box meaning that now other values can possibly go in those squares.
    /// </summary>
    public class PossibleValueClosureTechnique : EliminationTechnique
    {
        private List<byte> mValues;
        private List<Square> mSquares;
        private Entity mEntity;
        private int mOrder;

        public PossibleValueClosureTechnique(List<byte> pValues, List<Square> pSquares, Entity pEntity)
        {
            mValues = pValues;
            mSquares = pSquares;
            mEntity = pEntity;
        }

        /// <summary>
        /// The rank of this technique is 4 because it is a level more advanced than the basic solving rules
        /// </summary>
        public int Rank
        {
            get { return 4; }
        }

        /// <summary>
        /// Property to access the order in which this technique was used to eliminate a value
        /// </summary>
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public void apply(TextBox[,] pTextBoxes)
        {
            foreach (Square square in mEntity.Squares)
            {
                pTextBoxes[square.Row, square.Column].BackColor = Color.LightSalmon;
            }

            foreach (Square square in mSquares)
            {
                TextBox textBox = pTextBoxes[square.Row, square.Column];
                textBox.Font = new Font(textBox.Font.FontFamily, 12, FontStyle.Italic);

                string closureString = "";
                foreach(byte value in mValues)
                {
                    if(square.EliminatedTechnique(value, mOrder).Any())
                    {
                        closureString = closureString + Convert.ToString(value) + "/";
                    }
                }

                textBox.Text = closureString.Trim('/');
            }
        }
    }

    /// <summary>
    /// This class represents the technique used to eliminate values if two row/column/box have only two possible squares 
    /// for a value and those possible squares are in the same two columns or the same two rows, then those two rows or
    /// columns cannot have that value anywhere else except in those two squares
    /// </summary>
    public class PossibleValueRowColumnShadow : EliminationTechnique
    {
        private int mOrder;
        protected List<Square> mSquares;
        protected byte mValue;
        protected Entity mEntityA;
        protected Entity mEntityB;

        public PossibleValueRowColumnShadow(Entity pEntityA, Entity pEntityB, byte pValue, List<Square> pSquares)
        {
            mEntityA = pEntityA;
            mEntityB = pEntityB;
            mValue = pValue;
            mSquares = pSquares;
        }

        /// <summary>
        /// This technique is rank 5 because it is more difficult than the rank 4 techniques
        /// </summary>
        public int Rank
        {
            get { return 5; }
        }

        /// <summary>
        /// Property to access the order in which this technique was used to eliminate a value
        /// </summary>
        public int Order
        {
            get { return mOrder; }
            set { mOrder = value; }
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public virtual void apply(TextBox[,] pTextBoxes)
        {
            foreach (Square square in mEntityA.Squares)
            {
                pTextBoxes[square.Row, square.Column].BackColor = Color.LightSalmon;
            }

            foreach (Square square in mEntityB.Squares)
            {
                pTextBoxes[square.Row, square.Column].BackColor = Color.LightSalmon;
            }

            foreach (Square square in mSquares)
            {
                TextBox textBox = pTextBoxes[square.Row, square.Column];
                textBox.Font = new Font(textBox.Font.FontFamily, 18, FontStyle.Italic);
                textBox.Text = Convert.ToString(mValue);
            }
        }
    }

    /// <summary>
    /// This class represents a PossibleValueRowColumnShadow technique where values are eliminated by row shadow
    /// </summary>
    public class PossibleValueRowShadow : PossibleValueRowColumnShadow
    {
        private int mRowA;
        private int mRowB;

        public PossibleValueRowShadow(Entity pEntityA, Entity pEntityB, byte pValue, List<Square> pSquares, int pRowA, int pRowB)
            : base(pEntityA, pEntityB, pValue, pSquares)
        {
            mRowA = pRowA;
            mRowB = pRowB;
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public override void apply(TextBox[,] pTextBoxes)
        {
            base.apply(pTextBoxes);
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                pTextBoxes[mRowA, i].BackColor = Color.LightSalmon;
                pTextBoxes[mRowB, i].BackColor = Color.LightSalmon;
            }
        }
    }

    /// <summary>
    /// This class represents a PossibleValueRowColumnShadow technique where values are eliminated by column shadow
    /// </summary>
    public class PossibleValueColumnShadow : PossibleValueRowColumnShadow
    {
        private int mColumnA;
        private int mColumnB;

        public PossibleValueColumnShadow(Entity pEntityA, Entity pEntityB, byte pValue, List<Square> pSquares, int pColumnA, int pColumnB)
            : base(pEntityA, pEntityB, pValue, pSquares)
        {
            mColumnA = pColumnA;
            mColumnB = pColumnB;
        }

        /// <summary>
        /// Displays this techniques on given set of text boxes
        /// </summary>
        /// <param name="pTextBoxes">The text boxes used to display this technique on</param>
        public override void apply(TextBox[,] pTextBoxes)
        {
            base.apply(pTextBoxes);
            for (int i = 0; i < Constants.BOARD_SIZE; i++)
            {
                pTextBoxes[i, mColumnA].BackColor = Color.LightSalmon;
                pTextBoxes[i, mColumnB].BackColor = Color.LightSalmon;
            }
        }
    }
}
