using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SudokuSolver.Core.Model;
using System.Threading;

namespace SudokuSolver
{
    public class SudokuController
    {
        private const int DELAY = 2500;
        private const int SUDOKU_SIZE = 81;

        private SudokuMainView mView;
        private IModel mModel;

        public SudokuController()
        {
            mView =  new SudokuMainView();
            mModel = Model.CreateModel();

            mView.ClearSignal += (_pSender, _pArgs) => OnClearSignal();
            mView.FinishedSignal += (_pSender, _pArgs) => OnFinishedSignal();
            mView.NextSignal += (_pSender, _pArgs) => OnNextSignal();
            mView.PrevSignal += (_pSender, _pArgs) => OnPrevSignal();
            mView.SolveSignal += (_pSender, _pArgs) => OnSolveSignal();
            mView.UnsolveSignal += (_pSender, _pArgs) => OnUnsolveSignal();

            mView.ShowView();
        }

        private void OnClearSignal()
        {
            mModel.ClearGame();
            mView.ChangeControlState(State.IDLE);
        }

        private void OnFinishedSignal()
        {
            mView.ChangeControlState(State.LOADING);

            char[] sudokuValues = new char[SUDOKU_SIZE];

            for (int i = 0; i < SUDOKU_SIZE; i++)
            {
                sudokuValues[i] = mView.GetTextBoxText(i);

            }

            DateTime start = DateTime.Now;
            mModel.StartGame(sudokuValues, () => 
            {
                int elapsed = (int)(DateTime.Now - start).TotalMilliseconds;
                if (elapsed < DELAY)
                {
                    Thread.Sleep(DELAY - elapsed);
                }

                if (mModel.Error != null)
                {
                    mView.ChangeControlState(State.IDLE);
                    MessageBox.Show("There was an error while trying to process the Sudoku: " + mModel.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (mModel.Invalid)
                {

                    mView.ChangeControlState(State.IDLE);
                    MessageBox.Show("The enter values are not a valid Sudoku", "Invalid Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    mView.ChangeControlState(State.ACTIVE);
                }
            });
        }

        private void OnSolveSignal()
        {
            var values = mModel.RemainingValues();
            foreach (var value in values)
            {
                mView.SetTextBoxText(value.Index, value.Value);
            }
        }

        private void OnUnsolveSignal()
        {
            var values = mModel.InitialValues();
            foreach (var value in values)
            {
                mView.SetTextBoxText(value.Index, '\0');
            }
        }

        private void OnNextSignal()
        {
            IFoundValue value = mModel.NextValue();
            mView.SetTextBoxText(value.Index, value.Value);
        }

        private void OnPrevSignal()
        {
            IFoundValue value = mModel.PrevValue();
            mView.SetTextBoxText(value.Index, '\0');
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
