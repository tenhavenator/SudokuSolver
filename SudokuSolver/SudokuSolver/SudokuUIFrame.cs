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
    /// <summary>
    /// This class contains the actions related to UI interaction on the main screen
    /// </summary>
    public partial class SudokuUIFrame : Form
    {
        private TextBox[,] mSudokuTextBoxGrid;
        private SudokuGame mSudokuGame;

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool HideCaret(IntPtr hWnd);

        public SudokuUIFrame()
        {
            InitializeComponent();
            InitializeTextBoxes();

            this.buttonFinished.Click += new EventHandler(this.buttonFinished_Click);
            this.buttonSolve.Click += new EventHandler(this.buttonSolve_Click);
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);

        }

        #region TextBoxRegion definition

        /// <summary>
        /// Sets up all text boxes in the sudoku grid. Adds all text boxes in the sudoku grid to an array of text
        /// boxes for ease of reference. Adds a listener to each textbox for the various events that will be captured.
        /// </summary>
        private void InitializeTextBoxes()
        {
            mSudokuTextBoxGrid = new TextBox[Constants.BOARD_SIZE, Constants.BOARD_SIZE];
            mSudokuTextBoxGrid[0,0] = textBox00;
            mSudokuTextBoxGrid[0,1] = textBox10;
            mSudokuTextBoxGrid[0,2] = textBox20;
            mSudokuTextBoxGrid[0,3] = textBox30;
            mSudokuTextBoxGrid[0,4] = textBox40;
            mSudokuTextBoxGrid[0,5] = textBox50;
            mSudokuTextBoxGrid[0,6] = textBox60;
            mSudokuTextBoxGrid[0,7] = textBox70;
            mSudokuTextBoxGrid[0,8] = textBox80;

            mSudokuTextBoxGrid[1,0] = textBox01;
            mSudokuTextBoxGrid[1,1] = textBox11;
            mSudokuTextBoxGrid[1,2] = textBox21;
            mSudokuTextBoxGrid[1,3] = textBox31;
            mSudokuTextBoxGrid[1,4] = textBox41;
            mSudokuTextBoxGrid[1,5] = textBox51;
            mSudokuTextBoxGrid[1,6] = textBox61;
            mSudokuTextBoxGrid[1,7] = textBox71;
            mSudokuTextBoxGrid[1,8] = textBox81;

            mSudokuTextBoxGrid[2,0] = textBox02;
            mSudokuTextBoxGrid[2,1] = textBox12;
            mSudokuTextBoxGrid[2,2] = textBox22;
            mSudokuTextBoxGrid[2,3] = textBox32;
            mSudokuTextBoxGrid[2,4] = textBox42;
            mSudokuTextBoxGrid[2,5] = textBox52;
            mSudokuTextBoxGrid[2,6] = textBox62;
            mSudokuTextBoxGrid[2,7] = textBox72;
            mSudokuTextBoxGrid[2,8] = textBox82;

            mSudokuTextBoxGrid[3,0] = textBox03;
            mSudokuTextBoxGrid[3,1] = textBox13;
            mSudokuTextBoxGrid[3,2] = textBox23;
            mSudokuTextBoxGrid[3,3] = textBox33;
            mSudokuTextBoxGrid[3,4] = textBox43;
            mSudokuTextBoxGrid[3,5] = textBox53;
            mSudokuTextBoxGrid[3,6] = textBox63;
            mSudokuTextBoxGrid[3,7] = textBox73;
            mSudokuTextBoxGrid[3,8] = textBox83;

            mSudokuTextBoxGrid[4,0] = textBox04;
            mSudokuTextBoxGrid[4,1] = textBox14;
            mSudokuTextBoxGrid[4,2] = textBox24;
            mSudokuTextBoxGrid[4,3] = textBox34;
            mSudokuTextBoxGrid[4,4] = textBox44;
            mSudokuTextBoxGrid[4,5] = textBox54;
            mSudokuTextBoxGrid[4,6] = textBox64;
            mSudokuTextBoxGrid[4,7] = textBox74;
            mSudokuTextBoxGrid[4,8] = textBox84;

            mSudokuTextBoxGrid[5,0] = textBox05;
            mSudokuTextBoxGrid[5,1] = textBox15;
            mSudokuTextBoxGrid[5,2] = textBox25;
            mSudokuTextBoxGrid[5,3] = textBox35;
            mSudokuTextBoxGrid[5,4] = textBox45;
            mSudokuTextBoxGrid[5,5] = textBox55;
            mSudokuTextBoxGrid[5,6] = textBox65;
            mSudokuTextBoxGrid[5,7] = textBox75;
            mSudokuTextBoxGrid[5,8] = textBox85;

            mSudokuTextBoxGrid[6,0] = textBox06;
            mSudokuTextBoxGrid[6,1] = textBox16;
            mSudokuTextBoxGrid[6,2] = textBox26;
            mSudokuTextBoxGrid[6,3] = textBox36;
            mSudokuTextBoxGrid[6,4] = textBox46;
            mSudokuTextBoxGrid[6,5] = textBox56;
            mSudokuTextBoxGrid[6,6] = textBox66;
            mSudokuTextBoxGrid[6,7] = textBox76;
            mSudokuTextBoxGrid[6,8] = textBox86;

            mSudokuTextBoxGrid[7,0] = textBox07;
            mSudokuTextBoxGrid[7,1] = textBox17;
            mSudokuTextBoxGrid[7,2] = textBox27;
            mSudokuTextBoxGrid[7,3] = textBox37;
            mSudokuTextBoxGrid[7,4] = textBox47;
            mSudokuTextBoxGrid[7,5] = textBox57;
            mSudokuTextBoxGrid[7,6] = textBox67;
            mSudokuTextBoxGrid[7,7] = textBox77;
            mSudokuTextBoxGrid[7,8] = textBox87;

            mSudokuTextBoxGrid[8,0] = textBox08;
            mSudokuTextBoxGrid[8,1] = textBox18;
            mSudokuTextBoxGrid[8,2] = textBox28;
            mSudokuTextBoxGrid[8,3] = textBox38;
            mSudokuTextBoxGrid[8,4] = textBox48;
            mSudokuTextBoxGrid[8,5] = textBox58;
            mSudokuTextBoxGrid[8,6] = textBox68;
            mSudokuTextBoxGrid[8,7] = textBox78;
            mSudokuTextBoxGrid[8,8] = textBox88;

            for (int row = 0; row < Constants.BOARD_SIZE; row++)
            {
                for (int column = 0; column < Constants.BOARD_SIZE; column++)
                {
                    // New variables need to be declared because the variables in the for loop will be part of the 
                    // closures defined below and will be changed with each loop iteration
                    int rowClosure = row;
                    int columnClosure = column;

                    // Add event handlers
                    mSudokuTextBoxGrid[row, column].TextChanged += new EventHandler(this.textBox_Changed);
                    mSudokuTextBoxGrid[row, column].MouseDown += new MouseEventHandler(this.textBox_MouseDown);

                    mSudokuTextBoxGrid[row, column].MouseMove += (sender, eventArgs) =>
                    {
                        textBox_MouseOver(sender, rowClosure, columnClosure);
                    };
                    
                    mSudokuTextBoxGrid[row, column].MouseLeave += (sender, eventArgs) =>
                    {
                        textBox_MouseLeave(sender, rowClosure, columnClosure);
                    };
                }
            }
        }

        /// <summary>
        /// Handler for text box "mouse down" event (when the mouse is over the text box and the mouse button has been pressed). 
        /// Hides the flashing cursor if a sudoku has already been entered.
        /// </summary>
        /// <param name="pSender">Textbox that was clicked</param>
        private void  textBox_MouseDown(object pSender, EventArgs _pArgs)
        {
            if (mSudokuGame != null)
            {
                TextBox textBox = pSender as TextBox;
                HideCaret(textBox.Handle);
            }
        }

        /// <summary>
        /// Handler for the text box "text changed" event (when the text in the box is changed). Filters out any invalid input
        /// entered into the sudoku grid. Invalid input is anything that is not an integer between 1 and 9
        /// </summary>
        /// <param name="pSender"> Textbox that was changed</param>
        private void textBox_Changed(object pSender, EventArgs _pArgs)
        {
            TextBox textBox = pSender as TextBox;
            try
            {
                byte value = Convert.ToByte(textBox.Text);
                if (value < 1 || value > 9)
                {
                    throw new FormatException();
                }
            }
            catch (FormatException fe)
            {
                textBox.Text = "";
            }
        }

        /// <summary>
        /// Handler for the text box "mouse over" event (when the mouse moves over a textbox). If there is current sudoku and the
        /// textbox has a value, then it is colored green to indicate it is clickable. Otherwise it is colored red.
        /// </summary>
        /// <param name="pSender"> The textbox that the mouse moved over</param>
        /// <param name="pRow">The row of the textbox</param>
        /// <param name="pColumn">The column of the textbox</param>
        private void textBox_MouseOver(object pSender, int pRow, int pColumn)
        {
            if (mSudokuGame != null)
            {
                TextBox textBox = pSender as TextBox;
                if (mSudokuGame.EnteredValues[pRow, pColumn] != 0)
                {
                    textBox.BackColor = Color.LightGreen;
                }
                else
                {
                    textBox.BackColor = Color.LightSalmon;
                }

                textBox.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Handler of for the text box "mouse leave" event (when the mouse moves off of a textbox). Changes the color of 
        /// the text box back to its original color
        /// </summary>
        /// <param name="pSender">The text box that the mouse has left </param>
        /// <param name="pRow">The row of the textbox</param>
        /// <param name="pColumn">The column of the textbox</param>
        private void textBox_MouseLeave(object pSender, int pRow, int pColumn)
        {
            if (mSudokuGame != null)
            {
                TextBox textBox = pSender as TextBox;
                if (mSudokuGame.GivenValues[pRow, pColumn] == 0)
                {
                    textBox.BackColor = Color.White;
                }
                else 
                {
                    textBox.BackColor = Color.LightGray;
                }
            }
        }

        #endregion 
        
        /// <summary>
        /// The handler for the solve button click event. Displays all values of the solved sudoku.
        /// </summary>
        private void buttonSolve_Click(object sender, EventArgs e)
        {
            DialogResult result =  MessageBox.Show("Are you sure you want to solve the sudoku?", "Solve Sudoku", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                for (int row = 0; row < Constants.BOARD_SIZE; row++)
                {
                    for (int column = 0; column < Constants.BOARD_SIZE; column++)
                    {
                        mSudokuTextBoxGrid[row, column].Text = Convert.ToString(mSudokuGame.SolvedValues[row, column]);
                    }
                }
            }
        }

        /// <summary>
        /// The handler for the finished button click event. Grabs the values from the text box grid and sends them to the solver.
        /// If the values were a valid sudoku then the text box grid is set to read only, the solving buttons are displayed, and 
        /// the current sudoku game is set.
        /// </summary>
        public void buttonFinished_Click(object sender, EventArgs e)
        {
            byte[,] sudokuValues = new byte[Constants.BOARD_SIZE, Constants.BOARD_SIZE];

            // Grab the values from the text box
            for (int row = 0; row < Constants.BOARD_SIZE; row++)
            {
                for (int column = 0; column < Constants.BOARD_SIZE; column++)
                {
                
                    if (mSudokuTextBoxGrid[row, column].Text.Equals(""))
                    {
                        sudokuValues[row, column] = 0;
                    }
                    else
                    {
                        sudokuValues[row, column] = Convert.ToByte(mSudokuTextBoxGrid[row, column].Text);
                    }
                }
            }

            // Display the progress bar while solving the sudoku
            BackgroundSolveWorkerFrame frame = new BackgroundSolveWorkerFrame(sudokuValues);
            SolveResult result = frame.executeSolveWorkerDialog(this);

            switch (result.ResultType)
            { 
                case SolveResult.SUCCESS:
                    labelFinished.Visible = false;
                    buttonFinished.Visible = false;

                    labelSolveDetails.Visible = true;
                    buttonSolve.Visible = true;
                    buttonNext.Visible = true;
                    buttonBack.Visible = true;

                    mSudokuGame = new SudokuGame(sudokuValues, result.SolveResultValues);

                    for (int row = 0; row < Constants.BOARD_SIZE; row++)
                    {
                        for (int column = 0; column < Constants.BOARD_SIZE; column++)
                        {
                            mSudokuTextBoxGrid[row, column].ReadOnly = true;
                            if (mSudokuGame.GivenValues[row, column] != 0)
                            {
                                mSudokuTextBoxGrid[row, column].BackColor = Color.LightGray;
                            }
                            else
                            {
                                mSudokuTextBoxGrid[row, column].BackColor = Color.White;
                            }
                        }
                    }

                    break;

                case SolveResult.INVALID:
                    MessageBox.Show("The enter values are not a valid Sudoku", "Invalid Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case SolveResult.ERROR:
                    MessageBox.Show("There was an error while trying to process the Sudoku: " + result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case SolveResult.CANCELLED:
                    break;

                default:
                    // Should never happen 
                    break;
            }
        }

        /// <summary>
        /// Handler for the clear button click event. Hides the solving buttons, clears all text boxes and sets them to mutable, 
        /// nullifies the current sudoku game.
        /// </summary>
        public void buttonClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the board?", "Clear Board", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                for (int row = 0; row < Constants.BOARD_SIZE; row++)
                {
                    for (int column = 0; column < Constants.BOARD_SIZE; column++)
                    {
                        mSudokuTextBoxGrid[row, column].ReadOnly = false;
                        mSudokuTextBoxGrid[row, column].Clear();
                    }
                }

                if (mSudokuGame != null)
                {
                    labelFinished.Visible = true;
                    buttonFinished.Visible = true;

                    labelSolveDetails.Visible = false;
                    buttonSolve.Visible = false;
                    buttonNext.Visible = false;
                    buttonBack.Visible = false;

                    mSudokuGame = null;
                }
            }
        }
    }
}
