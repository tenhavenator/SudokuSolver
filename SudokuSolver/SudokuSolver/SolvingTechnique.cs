/// <summary>
/// This file contains the classes used to represent solving techniques
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
    /// Interface to represent a technique used to find a value on the board
    /// </summary>
    public interface SolvingTechnique
    {
        /// <summary>
        /// Sets up a DetailsFrame to display the details of the solving technique
        /// </summary>
        /// <param name="pDetailsFrame">The frame to set up</param>
        void showDetails(DetailsFrame pDetailsFrame);
    }

    /// <summary>
    /// The class used to represent the technique used when a value is given. This class is just a place holder
    /// </summary>
    public class GivenValueTechnique : SolvingTechnique
    {
        public void showDetails(DetailsFrame pDetailsFrame)
        { }
    }

    /// <summary>
    /// The class to represent a value found by box, row, column elimination
    /// </summary>
    public class EntityFoundValueTechnique : SolvingTechnique
    { 
        private Entity mEntity;
        private Square mSquare;

        public EntityFoundValueTechnique(Entity pEntity, Square pSquare)
        {
            mEntity = pEntity;
            mSquare = pSquare;
        }

        /// <summary>
        /// Sets up a DetailsFrame to display the details of a value found by row, column, box elimination
        /// </summary>
        /// <param name="pDetailsFrame">The frame to set up</param>
        public void showDetails(DetailsFrame pDetailsFrame)
        {
            // Apply the elimination technqiue for each square in the entity
            foreach (Square square in mEntity.Squares.Where(s => s != mSquare))
            {
                square.EliminatedTechnique(mSquare.Value, mSquare.Order).ForEach(et => et.apply(pDetailsFrame.TextBoxGrid));
            }

            // Color each square in the entity
            foreach (Square square in mEntity.Squares.Where(s => s != mSquare))
            {
                pDetailsFrame.TextBoxGrid[square.Row, square.Column].BackColor = Color.Salmon;
            }

            // Insert the found value into the text box
            TextBox textBox = pDetailsFrame.TextBoxGrid[mSquare.Row, mSquare.Column];
            textBox.Font = new Font(textBox.Font.FontFamily, 26, FontStyle.Regular);
            textBox.Text = Convert.ToString(mSquare.Value);
            textBox.BackColor = Color.LightGreen;

            pDetailsFrame.DetailsLabel = "This is the only place where " + Convert.ToString(mSquare.Value) + " can go in this " + mEntity.EntityType + ".";
        }
    }

    /// <summary>
    /// The class to represent the technique used when there is only one value that can go in a square
    /// </summary>
    public class OnlyPossibleValueTechnique : SolvingTechnique
    {
        private Square mSquare;

        public OnlyPossibleValueTechnique(Square pSquare)
        {
            mSquare = pSquare;
        }

        /// <summary>
        /// Sets up a DetailsFrame to display the details of a value found by exhaustive elimination
        /// </summary>
        /// <param name="pDetailsFrame">The frame to set up</param>
        public void showDetails(DetailsFrame pDetailsFrame)
        {
            // Apply the elimination techniques for all other values in the square
            foreach (byte value in mSquare.EliminatedValues.Where(v => v != mSquare.Value))
            {
                mSquare.EliminatedTechnique(value, mSquare.Order).ForEach(et => et.apply(pDetailsFrame.TextBoxGrid));
            }

            pDetailsFrame.TextBoxGrid[mSquare.Row, mSquare.Column].Text = Convert.ToString(mSquare.Value);
            pDetailsFrame.TextBoxGrid[mSquare.Row, mSquare.Column].BackColor = Color.LightGreen;

            pDetailsFrame.DetailsLabel = Convert.ToString(mSquare.Value) + " is the only value that can go in this square.";
        }
    }
}
