using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class DetailsFrame : Form
    {
        private TextBox[,] mSudokuTextBoxGrid;

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool HideCaret(IntPtr hWnd);

        public DetailsFrame()
        {
            InitializeComponent();
            InitializeTextBoxes();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; 

            this.buttonExit.Click += new EventHandler(this.buttonExit_Click);
        }

        /// <summary>
        /// Sets up all text boxes in the sudoku grid. Adds all text boxes in the sudoku grid to an array of text
        /// boxes for ease of reference. Adds a listener to each textbox for the various events that will be captured.
        /// </summary>
        private void InitializeTextBoxes()
        {
            mSudokuTextBoxGrid = new TextBox[Constants.ENTITY_SIZE, Constants.ENTITY_SIZE];
            mSudokuTextBoxGrid[0, 0] = textBox00;
            mSudokuTextBoxGrid[0, 1] = textBox10;
            mSudokuTextBoxGrid[0, 2] = textBox20;
            mSudokuTextBoxGrid[0, 3] = textBox30;
            mSudokuTextBoxGrid[0, 4] = textBox40;
            mSudokuTextBoxGrid[0, 5] = textBox50;
            mSudokuTextBoxGrid[0, 6] = textBox60;
            mSudokuTextBoxGrid[0, 7] = textBox70;
            mSudokuTextBoxGrid[0, 8] = textBox80;

            mSudokuTextBoxGrid[1, 0] = textBox01;
            mSudokuTextBoxGrid[1, 1] = textBox11;
            mSudokuTextBoxGrid[1, 2] = textBox21;
            mSudokuTextBoxGrid[1, 3] = textBox31;
            mSudokuTextBoxGrid[1, 4] = textBox41;
            mSudokuTextBoxGrid[1, 5] = textBox51;
            mSudokuTextBoxGrid[1, 6] = textBox61;
            mSudokuTextBoxGrid[1, 7] = textBox71;
            mSudokuTextBoxGrid[1, 8] = textBox81;

            mSudokuTextBoxGrid[2, 0] = textBox02;
            mSudokuTextBoxGrid[2, 1] = textBox12;
            mSudokuTextBoxGrid[2, 2] = textBox22;
            mSudokuTextBoxGrid[2, 3] = textBox32;
            mSudokuTextBoxGrid[2, 4] = textBox42;
            mSudokuTextBoxGrid[2, 5] = textBox52;
            mSudokuTextBoxGrid[2, 6] = textBox62;
            mSudokuTextBoxGrid[2, 7] = textBox72;
            mSudokuTextBoxGrid[2, 8] = textBox82;

            mSudokuTextBoxGrid[3, 0] = textBox03;
            mSudokuTextBoxGrid[3, 1] = textBox13;
            mSudokuTextBoxGrid[3, 2] = textBox23;
            mSudokuTextBoxGrid[3, 3] = textBox33;
            mSudokuTextBoxGrid[3, 4] = textBox43;
            mSudokuTextBoxGrid[3, 5] = textBox53;
            mSudokuTextBoxGrid[3, 6] = textBox63;
            mSudokuTextBoxGrid[3, 7] = textBox73;
            mSudokuTextBoxGrid[3, 8] = textBox83;

            mSudokuTextBoxGrid[4, 0] = textBox04;
            mSudokuTextBoxGrid[4, 1] = textBox14;
            mSudokuTextBoxGrid[4, 2] = textBox24;
            mSudokuTextBoxGrid[4, 3] = textBox34;
            mSudokuTextBoxGrid[4, 4] = textBox44;
            mSudokuTextBoxGrid[4, 5] = textBox54;
            mSudokuTextBoxGrid[4, 6] = textBox64;
            mSudokuTextBoxGrid[4, 7] = textBox74;
            mSudokuTextBoxGrid[4, 8] = textBox84;

            mSudokuTextBoxGrid[5, 0] = textBox05;
            mSudokuTextBoxGrid[5, 1] = textBox15;
            mSudokuTextBoxGrid[5, 2] = textBox25;
            mSudokuTextBoxGrid[5, 3] = textBox35;
            mSudokuTextBoxGrid[5, 4] = textBox45;
            mSudokuTextBoxGrid[5, 5] = textBox55;
            mSudokuTextBoxGrid[5, 6] = textBox65;
            mSudokuTextBoxGrid[5, 7] = textBox75;
            mSudokuTextBoxGrid[5, 8] = textBox85;

            mSudokuTextBoxGrid[6, 0] = textBox06;
            mSudokuTextBoxGrid[6, 1] = textBox16;
            mSudokuTextBoxGrid[6, 2] = textBox26;
            mSudokuTextBoxGrid[6, 3] = textBox36;
            mSudokuTextBoxGrid[6, 4] = textBox46;
            mSudokuTextBoxGrid[6, 5] = textBox56;
            mSudokuTextBoxGrid[6, 6] = textBox66;
            mSudokuTextBoxGrid[6, 7] = textBox76;
            mSudokuTextBoxGrid[6, 8] = textBox86;

            mSudokuTextBoxGrid[7, 0] = textBox07;
            mSudokuTextBoxGrid[7, 1] = textBox17;
            mSudokuTextBoxGrid[7, 2] = textBox27;
            mSudokuTextBoxGrid[7, 3] = textBox37;
            mSudokuTextBoxGrid[7, 4] = textBox47;
            mSudokuTextBoxGrid[7, 5] = textBox57;
            mSudokuTextBoxGrid[7, 6] = textBox67;
            mSudokuTextBoxGrid[7, 7] = textBox77;
            mSudokuTextBoxGrid[7, 8] = textBox87;

            mSudokuTextBoxGrid[8, 0] = textBox08;
            mSudokuTextBoxGrid[8, 1] = textBox18;
            mSudokuTextBoxGrid[8, 2] = textBox28;
            mSudokuTextBoxGrid[8, 3] = textBox38;
            mSudokuTextBoxGrid[8, 4] = textBox48;
            mSudokuTextBoxGrid[8, 5] = textBox58;
            mSudokuTextBoxGrid[8, 6] = textBox68;
            mSudokuTextBoxGrid[8, 7] = textBox78;
            mSudokuTextBoxGrid[8, 8] = textBox88;

            for (int row = 0; row < Constants.ENTITY_SIZE; row++)
            {
                for (int column = 0; column < Constants.ENTITY_SIZE; column++)
                {
                    mSudokuTextBoxGrid[row, column].BackColor = Color.Black;
                    mSudokuTextBoxGrid[row, column].ReadOnly = true;
                    mSudokuTextBoxGrid[row, column].Cursor = Cursors.Arrow; 

                    mSudokuTextBoxGrid[row, column].MouseDown += new MouseEventHandler(this.textBox_MouseDown);
                }
            }
        }


        /// <summary>
        /// Handler for text box "mouse down" event (when the mouse is over the text box and the mouse button has been pressed). 
        /// Hides the flashing cursor if a sudoku has already been entered.
        /// </summary>
        /// <param name="pSender">Textbox that was clicked</param>
        private void textBox_MouseDown(object pSender, EventArgs _pArgs)
        {
            TextBox textBox = pSender as TextBox;
            HideCaret(textBox.Handle);
        }

        /// <summary>
        /// The handler for the exit button click event. Closes the details dialog window.
        /// </summary>
        private void buttonExit_Click(object _sender, EventArgs _e)
        {
            this.Close();
        }

        /// <summary>
        /// Property used to access the list of text boxes in this frame
        /// </summary>
        public TextBox[,] TextBoxGrid
        {
            get { return mSudokuTextBoxGrid; }
        }

        /// <summary>
        /// Property used to access the text label at the bottom of this frame
        /// </summary>
        public string DetailsLabel
        {
            get { return labelDetails.Text; }
            set { labelDetails.Text = value;  }
        }

        /*public void sort()
        {
            // Select the techniques that are definately needed to eliminate a square
            var appliedTechniques = (from s in foundValue.Entity.Squares
                                     let techs = s.EliminatedTechnique(foundValue.Value)
                                     where techs.Count == 1
                                     select techs.Single()).ToList();

            // Select the set of squares that do not have a matching elimination technique yet
            var uncoveredSquares = (from s in foundValue.Entity.Squares
                                    let techs = s.EliminatedTechnique(foundValue.Value)
                                    where s != foundValue.Square && !techs.Intersect(appliedTechniques).Any()
                                    select s).ToList();

            // Select the techniques from the uncovered squares and order by occurence
            var remainingTechniques = from s in uncoveredSquares
                                      from tech in s.EliminatedTechnique(foundValue.Value)
                                      group tech by tech into g
                                      orderby g.Count() descending
                                      select g.Key;

            // Apply these techniques until all squares are covered
            foreach (EliminationTechnique tech in remainingTechniques)
            {
                appliedTechniques.Add(tech);
                uncoveredSquares.RemoveAll(s => s.EliminatedTechnique(foundValue.Value).Contains(tech));

                if (!uncoveredSquares.Any())
                {
                    break;
                }
            }
        }*/
    }
}
