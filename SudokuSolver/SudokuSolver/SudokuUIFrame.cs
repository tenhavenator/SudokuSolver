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
        private readonly static int SIZE = 81;

        private TextBox[] mSudokuTextBoxGrid;
        private SudokuGame mSudokuGame;

        public SudokuUIFrame()
        {
            InitializeComponent();
            InitializeTextBoxes();

            this.buttonCustom.Click += new EventHandler(this.buttonCustom_Click);
            this.buttonFinished.Click += new EventHandler(this.buttonFinished_Click);
            this.buttonExit.Click += new EventHandler(this.buttonExit_Click);
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
            mSudokuTextBoxGrid = new TextBox[SIZE];
            mSudokuTextBoxGrid[0] = textBox00;
            mSudokuTextBoxGrid[1] = textBox10;
            mSudokuTextBoxGrid[2] = textBox20;
            mSudokuTextBoxGrid[3] = textBox30;
            mSudokuTextBoxGrid[4] = textBox40;
            mSudokuTextBoxGrid[5] = textBox50;
            mSudokuTextBoxGrid[6] = textBox60;
            mSudokuTextBoxGrid[7] = textBox70;
            mSudokuTextBoxGrid[8] = textBox80;

            mSudokuTextBoxGrid[9] = textBox01;
            mSudokuTextBoxGrid[10] = textBox11;
            mSudokuTextBoxGrid[11] = textBox21;
            mSudokuTextBoxGrid[12] = textBox31;
            mSudokuTextBoxGrid[13] = textBox41;
            mSudokuTextBoxGrid[14] = textBox51;
            mSudokuTextBoxGrid[15] = textBox61;
            mSudokuTextBoxGrid[16] = textBox71;
            mSudokuTextBoxGrid[17] = textBox81;

            mSudokuTextBoxGrid[18] = textBox02;
            mSudokuTextBoxGrid[19] = textBox12;
            mSudokuTextBoxGrid[20] = textBox22;
            mSudokuTextBoxGrid[21] = textBox32;
            mSudokuTextBoxGrid[22] = textBox42;
            mSudokuTextBoxGrid[23] = textBox52;
            mSudokuTextBoxGrid[24] = textBox62;
            mSudokuTextBoxGrid[25] = textBox72;
            mSudokuTextBoxGrid[26] = textBox82;

            mSudokuTextBoxGrid[27] = textBox03;
            mSudokuTextBoxGrid[28] = textBox13;
            mSudokuTextBoxGrid[29] = textBox23;
            mSudokuTextBoxGrid[30] = textBox33;
            mSudokuTextBoxGrid[31] = textBox43;
            mSudokuTextBoxGrid[32] = textBox53;
            mSudokuTextBoxGrid[33] = textBox63;
            mSudokuTextBoxGrid[34] = textBox73;
            mSudokuTextBoxGrid[35] = textBox83;

            mSudokuTextBoxGrid[36] = textBox04;
            mSudokuTextBoxGrid[37] = textBox14;
            mSudokuTextBoxGrid[38] = textBox24;
            mSudokuTextBoxGrid[39] = textBox34;
            mSudokuTextBoxGrid[40] = textBox44;
            mSudokuTextBoxGrid[41] = textBox54;
            mSudokuTextBoxGrid[42] = textBox64;
            mSudokuTextBoxGrid[43] = textBox74;
            mSudokuTextBoxGrid[44] = textBox84;

            mSudokuTextBoxGrid[45] = textBox05;
            mSudokuTextBoxGrid[46] = textBox15;
            mSudokuTextBoxGrid[47] = textBox25;
            mSudokuTextBoxGrid[48] = textBox35;
            mSudokuTextBoxGrid[49] = textBox45;
            mSudokuTextBoxGrid[50] = textBox55;
            mSudokuTextBoxGrid[51] = textBox65;
            mSudokuTextBoxGrid[52] = textBox75;
            mSudokuTextBoxGrid[53] = textBox85;

            mSudokuTextBoxGrid[54] = textBox06;
            mSudokuTextBoxGrid[55] = textBox16;
            mSudokuTextBoxGrid[56] = textBox26;
            mSudokuTextBoxGrid[57] = textBox36;
            mSudokuTextBoxGrid[58] = textBox46;
            mSudokuTextBoxGrid[59] = textBox56;
            mSudokuTextBoxGrid[60] = textBox66;
            mSudokuTextBoxGrid[61] = textBox76;
            mSudokuTextBoxGrid[62] = textBox86;

            mSudokuTextBoxGrid[63] = textBox07;
            mSudokuTextBoxGrid[64] = textBox17;
            mSudokuTextBoxGrid[65] = textBox27;
            mSudokuTextBoxGrid[66] = textBox37;
            mSudokuTextBoxGrid[67] = textBox47;
            mSudokuTextBoxGrid[68] = textBox57;
            mSudokuTextBoxGrid[69] = textBox67;
            mSudokuTextBoxGrid[70] = textBox77;
            mSudokuTextBoxGrid[71] = textBox87;

            mSudokuTextBoxGrid[72] = textBox08;
            mSudokuTextBoxGrid[73] = textBox18;
            mSudokuTextBoxGrid[74] = textBox28;
            mSudokuTextBoxGrid[75] = textBox38;
            mSudokuTextBoxGrid[76] = textBox48;
            mSudokuTextBoxGrid[77] = textBox58;
            mSudokuTextBoxGrid[78] = textBox68;
            mSudokuTextBoxGrid[79] = textBox78;
            mSudokuTextBoxGrid[80] = textBox88;

            foreach (TextBox textBox in mSudokuTextBoxGrid)
            {
                textBox.TextChanged += new EventHandler(this.textBox_Changed);
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
        public void textBox_Changed(object pSender, EventArgs _pArgs)
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

        #endregion 
        
        /// <summary>
        /// The action taken when the "Custom" button is pressed on the menu screen. Brings the panel containing the empty sudoku grid to the foreground.
        /// </summary>
        public void buttonCustom_Click(object sender, EventArgs e)
        {
            labelFinished.Visible = true;
            buttonFinished.Visible = true;

            panel2.BringToFront();
        }

        /// <summary>
        /// The action taken when the "Exit" button is pressed from the menu screen. Exits the program.
        /// </summary>
        public void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// The action taken when the "Finished" button is pressed from the custom game screen.
        /// </summary>
        public void buttonFinished_Click(object sender, EventArgs e)
        {
            byte[] sudokuValues = new byte[SIZE];

            for(int i = 0; i < SIZE; i++)
            {
                if (mSudokuTextBoxGrid[i].Text.Equals(""))
                {
                    sudokuValues[i] = 0;
                }
                else
                {
                    sudokuValues[i] = Convert.ToByte(mSudokuTextBoxGrid[i].Text);
                }
            }

            // Display the progress bar while solving the sudoku
            BackgroundSolveWorkerFrame frame = new BackgroundSolveWorkerFrame(sudokuValues);
            SolveResult result = frame.executeSolveWorkerDialog(this);

            switch (result.getResultType())
            { 
                case SolveResult.SUCCESS:
                    labelFinished.Visible = false;
                    buttonFinished.Visible = false;
                    startGame(sudokuValues, result.getSolvedSudoku());
                    break;

                case SolveResult.INVALID:
                    MessageBox.Show("The enter values are not a valid Sudoku", "Invalid Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case SolveResult.ERROR:
                    MessageBox.Show("There was an error while trying to process the Sudoku: " + result.getErrorMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case SolveResult.CANCELLED:
                    break;

                default:
                    // Should never happen 
                    break;
            }
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        /// <param name="pSudokuGivenValues">The given values for the game</param>
        /// <param name="pSudokuSolvedValues">The solution to the game</param>
        private void startGame(byte[] pSudokuGivenValues, byte[] pSudokuSolvedValues)
        {
            mSudokuGame = new SudokuGame(pSudokuGivenValues, pSudokuSolvedValues);

            for (int i = 0; i < SIZE; i++)
            {
                if (pSudokuGivenValues[i] != 0)
                {
                    mSudokuTextBoxGrid[i].ReadOnly = true;
                }
            }
        }
    }
}
