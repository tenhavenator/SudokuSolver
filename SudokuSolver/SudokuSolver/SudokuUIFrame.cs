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

        private TextBox[] sudokuGrid;
        //private ProgressFrame progressDialog;

        public SudokuUIFrame()
        {
            InitializeComponent();
            InitializeTextBoxes();
           //InitializeBackgroundSudokuSolver();

            this.buttonCustom.Click += new EventHandler(this.buttonCustom_Click);
            //this.buttonFinished.Click += new EventHandler(this.buttonFinished_Click);
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
            sudokuGrid = new TextBox[SIZE];
            sudokuGrid[0] = textBox00;
            sudokuGrid[1] = textBox10;
            sudokuGrid[2] = textBox20;
            sudokuGrid[3] = textBox30;
            sudokuGrid[4] = textBox40;
            sudokuGrid[5] = textBox50;
            sudokuGrid[6] = textBox60;
            sudokuGrid[7] = textBox70;
            sudokuGrid[8] = textBox80;

            sudokuGrid[9] = textBox01;
            sudokuGrid[10] = textBox11;
            sudokuGrid[11] = textBox21;
            sudokuGrid[12] = textBox31;
            sudokuGrid[13] = textBox41;
            sudokuGrid[14] = textBox51;
            sudokuGrid[15] = textBox61;
            sudokuGrid[16] = textBox71;
            sudokuGrid[17] = textBox81;

            sudokuGrid[18] = textBox02;
            sudokuGrid[19] = textBox12;
            sudokuGrid[20] = textBox22;
            sudokuGrid[21] = textBox32;
            sudokuGrid[22] = textBox42;
            sudokuGrid[23] = textBox52;
            sudokuGrid[24] = textBox62;
            sudokuGrid[25] = textBox72;
            sudokuGrid[26] = textBox82;

            sudokuGrid[27] = textBox03;
            sudokuGrid[28] = textBox13;
            sudokuGrid[29] = textBox23;
            sudokuGrid[30] = textBox33;
            sudokuGrid[31] = textBox43;
            sudokuGrid[32] = textBox53;
            sudokuGrid[33] = textBox63;
            sudokuGrid[34] = textBox73;
            sudokuGrid[35] = textBox83;

            sudokuGrid[36] = textBox04;
            sudokuGrid[37] = textBox14;
            sudokuGrid[38] = textBox24;
            sudokuGrid[39] = textBox34;
            sudokuGrid[40] = textBox44;
            sudokuGrid[41] = textBox54;
            sudokuGrid[42] = textBox64;
            sudokuGrid[43] = textBox74;
            sudokuGrid[44] = textBox84;

            sudokuGrid[45] = textBox05;
            sudokuGrid[46] = textBox15;
            sudokuGrid[47] = textBox25;
            sudokuGrid[48] = textBox35;
            sudokuGrid[49] = textBox45;
            sudokuGrid[50] = textBox55;
            sudokuGrid[51] = textBox65;
            sudokuGrid[52] = textBox75;
            sudokuGrid[53] = textBox85;

            sudokuGrid[54] = textBox06;
            sudokuGrid[55] = textBox16;
            sudokuGrid[56] = textBox26;
            sudokuGrid[57] = textBox36;
            sudokuGrid[58] = textBox46;
            sudokuGrid[59] = textBox56;
            sudokuGrid[60] = textBox66;
            sudokuGrid[61] = textBox76;
            sudokuGrid[62] = textBox86;

            sudokuGrid[63] = textBox07;
            sudokuGrid[64] = textBox17;
            sudokuGrid[65] = textBox27;
            sudokuGrid[66] = textBox37;
            sudokuGrid[67] = textBox47;
            sudokuGrid[68] = textBox57;
            sudokuGrid[69] = textBox67;
            sudokuGrid[70] = textBox77;
            sudokuGrid[71] = textBox87;

            sudokuGrid[72] = textBox08;
            sudokuGrid[73] = textBox18;
            sudokuGrid[74] = textBox28;
            sudokuGrid[75] = textBox38;
            sudokuGrid[76] = textBox48;
            sudokuGrid[77] = textBox58;
            sudokuGrid[78] = textBox68;
            sudokuGrid[79] = textBox78;
            sudokuGrid[80] = textBox88;

            foreach (TextBox textBox in sudokuGrid)
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

        /*private void InitializeBackgroundSudokuSolver()
        {
            backgroundSudokuSolver.DoWork += new DoWorkEventHandler(backgroundSudokuSolver_Solve);
            backgroundSudokuSolver.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundSudokuSolver_Finished);
            backgroundSudokuSolver.ProgressChanged += new ProgressChangedEventHandler(backgroundSudokuSolver_Progress);
        }

        private void backgroundSudokuSolver_Solve(object sender, DoWorkEventArgs e)
        {
            e.Result = SudokuSolver.solve((byte[])e.Argument, backgroundSudokuSolver, e);
        }

        private void backgroundSudokuSolver_Finished(object sender, RunWorkerCompletedEventArgs e)
        {
            progressDialog.Close();

            if (e.Error != null)
            { 
                
            
            
            }
        }

        private void backgroundSudokuSolver_Progress(object sender, ProgressChangedEventArgs e)
        {
            progressDialog.Progress = e.ProgressPercentage;
        }*/
        
        /// <summary>
        /// The action taken when the "Custom" button is pressed on the menu screen. Brings the panel containing the empty sudoku grid to the foreground.
        /// </summary>
        public void buttonCustom_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
        }

        /// <summary>
        /// The action taken when the "Exit" button is pressed from the menu screen. Exits the program.
        /// </summary>
        public void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

       /* public void buttonFinished_Click(object sender, EventArgs e)
        {
            byte[] sudokuValues = new byte[SIZE];

            for(int i = 0; i < SIZE; i++)
            {
                if (sudokuGrid[i].Text.Equals(""))
                {
                    sudokuValues[i] = 0;
                }
                else
                {
                    sudokuGrid[i].ReadOnly = true;
                    sudokuValues[i] = Convert.ToByte(sudokuGrid[i].Text);
                }
            }

            //startGame(sudokuValues);
        }*/

       /* private void startGame(byte[] pSudokuValues)
        {
            progressDialog = new ProgressFrame();
            backgroundSudokuSolver.RunWorkerAsync();

            if (progressDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                backgroundSudokuSolver.CancelAsync();
            }

        }*/


    }
}
