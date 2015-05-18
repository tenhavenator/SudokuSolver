/// <summary>
/// This class contains the actions related to UI interaction
/// </summary>

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

        }

        #region TextBoxRegion definition

        /// <summary>
        /// Sets up all text boxes in the sudoku grid
        /// </summary>
        /// <remarks> 
        /// Adds all text boxes in the sudoku grid to an array of text boxes for ease of reference.
        /// Adds a listener to each textbox for a "text changed" event
        /// </remarks>
        /// 
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
                    mSudokuTextBoxGrid[row, column].TextChanged += new EventHandler(this.textBox_Changed);
                    mSudokuTextBoxGrid[row, column].MouseMove += new MouseEventHandler(this.textBox_MouseOver);
                    mSudokuTextBoxGrid[row, column].MouseLeave += new EventHandler(this.textBox_MouseLeave);
                    mSudokuTextBoxGrid[row, column].MouseDown += new MouseEventHandler(this.textBox_MouseDown);
                }
            }
              
            
        }

        private void  textBox_MouseDown(object pSender, EventArgs _pArgs)
        {
            if (mSudokuGame != null)
            {
                TextBox textBox = pSender as TextBox;
                HideCaret(textBox.Handle);
            }
        }

        /// <summary>
        /// Funtction called any time a textbox in the sudoku grid is changed
        /// </summary>
        /// <param name="pSender"> Textbox that changed</param>
        /// <remarks> This function filters out any invalid input entered into the sudoku grid.
        /// Invalid input is anything that is not an integer between 1 and 9
        /// </remarks>
        /// 
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

        private void textBox_MouseOver(object pSender,  EventArgs _pArgs)
        {
            if (mSudokuGame != null)
            {
                TextBox textBox = pSender as TextBox;
                if (textBox.Text != "")
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

        private void textBox_MouseLeave(object pSender, EventArgs _pArgs)
        {
            if (mSudokuGame != null)
            {
                GridTextBox textBox = pSender as GridTextBox;
                if (mSudokuGame.GivenValues[textBox.Row, textBox.Column] == 0)
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
        /// The action taken when the solve is press on the game screen.
        /// </summary>
        public void buttonSolve_Click(object sender, EventArgs e)
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
        /// The action taken when the "Finished" button is pressed from the custom game screen.
        /// </summary>
        public void buttonFinished_Click(object sender, EventArgs e)
        {
            byte[,] sudokuValues = new byte[Constants.BOARD_SIZE, Constants.BOARD_SIZE];


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
                    buttonClear.Visible = true;

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
    }

    public class GridTextBox : TextBox
    {
        private int mRow;
        private int mColumn;

        public GridTextBox(int pRow, int pColumn)
        {
            mRow = pRow;
            mColumn = pColumn;
        }

        public int Row
        {
            get { return mRow; }
        }

        public int Column
        {
            get { return mColumn; }
        }
    
    }
}
