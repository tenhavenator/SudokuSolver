using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace SudokuSolver
{
    public class SudokuController
    {
        private const int SUDOKU_SIZE = 81;

        private SudokuMainView mView;


        public SudokuController()
        {
            mView =  new SudokuMainView();

            mView.ClearSignal += (_pSender, _pArgs) => OnClearSignal();
            mView.FinishedSignal += (_pSender, _pArgs) => OnFinishedSignal();

            mView.ShowView();
        }

        private void OnClearSignal()
        {
            // Do other stuff with the model

            mView.ChangeControlState(ControlState.IDLE);
        }

        private void OnFinishedSignal()
        {
            mView.ChangeControlState(ControlState.LOADING);

            byte[] sudokuValues = new byte[SUDOKU_SIZE];
            for (int i = 0; i < SUDOKU_SIZE; i++)
            {
                string text = mView.GetTextBoxText(i);
                if (text.Equals(string.Empty))
                {
                    sudokuValues[i] = 0;
                }
                else
                {
                    sudokuValues[i] = Convert.ToByte(text);
                }
            }

            SudokuModel.StartGame(sudokuValues, OnSolveComplete);
        }

        private void OnSolveComplete(SolveResult pResult)
        {
            switch (pResult.ResultType)
            {
                case SolveResult.SUCCESS:
                    mView.ChangeControlState(ControlState.ACTIVE);
                    break;

                case SolveResult.INVALID:
                    mView.ChangeControlState(ControlState.IDLE);
                    MessageBox.Show("The enter values are not a valid Sudoku", "Invalid Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case SolveResult.ERROR:
                    mView.ChangeControlState(ControlState.IDLE);
                    MessageBox.Show("There was an error while trying to process the Sudoku: " + pResult.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    // Should never happen 
                    break;
            }
        }

        /// <summary>
        /// Handler for the text box "mouse click" event. Displays the details of how the value was found if there is a 
        /// current game.
        /// </summary>
        /// <param name="pRow">The row of the clicked square</param>
        /// <param name="pColumn">The column of the clicked square</param>
        private void textBox_MouseClick(int pRow, int pColumn)
        {
            /*if (mSudokuGame != null)
            {
                if (mSudokuGame.GivenValues[pRow, pColumn] == 0 && mSudokuGame.EnteredValues[pRow, pColumn] != 0)
                {
                    DetailsFrame detailsFrame = new DetailsFrame();
                    mSudokuGame.SolvedValues[pRow, pColumn].Technique.showDetails(detailsFrame);
                    detailsFrame.ShowDialog();
                }
            }*/
        }

        /// <summary>
        /// Handler for text box "mouse down" event (when the mouse is over the text box and the mouse button has been pressed). 
        /// Hides the flashing cursor if a sudoku has already been entered.
        /// </summary>
        /// <param name="pSender">Textbox that was clicked</param>
        private void textBox_MouseDown(object pSender, EventArgs _pArgs)
        {
            /*  if (mSudokuGame != null)
              {
                  TextBox textBox = pSender as TextBox;
                  HideCaret(textBox.Handle);
              }*/
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
            /* TextBox textBox = pSender as TextBox;
             if (mSudokuGame != null)
             {
                 if (mSudokuGame.GivenValues[pRow, pColumn] == 0 && mSudokuGame.EnteredValues[pRow, pColumn] != 0)
                 {
                     textBox.BackColor = Color.LightGreen;
                 }

                 textBox.Cursor = Cursors.Arrow;
             }
             else
             {
                 textBox.Cursor = Cursors.IBeam;
             }*/
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
            /* if (mSudokuGame != null)
             {
                 TextBox textBox = pSender as TextBox;
                 if (mSudokuGame.GivenValues[pRow, pColumn] == 0)
                 {
                     textBox.BackColor = Color.White;
                 }
             }*/
        }


        [STAThread]
        public static void Main()
        {
            new SudokuController();
        }
    }
}
