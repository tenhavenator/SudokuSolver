using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SudokuSolver
{
    public enum ControlState { ACTIVE, IDLE, LOADING };

    /// <summary>
    /// This class contains the actions related to UI interaction on the main screen
    /// </summary>
    public partial class SudokuMainView : Form
    {
        private const int SUDOKU_SIZE = 81;

        private SudokuGridLayoutPanel mSudokuGridLayoutPanel;
        private ControlState mCurrentState;

        // User input events
        public event EventHandler ClearSignal;
        public event EventHandler FinishedSignal;

        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        public SudokuMainView()
        {
            InitializeComponent();
            mSudokuGridLayoutPanel = new SudokuGridLayoutPanel();
            mSudokuGridLayoutPanel.Anchor = AnchorStyles.None;
            mSudokuGridLayoutPanel.Location = new Point(38, 50);
            this.mPanelSudokuMain.Controls.Add(mSudokuGridLayoutPanel);

            // Setup button handlers
            this.mButtonClear.Click += new EventHandler(OnClearButtonClick);
            this.mButtonFinished.Click += new EventHandler(OnFinishedButtonClick);

            mCurrentState = ControlState.IDLE;

            for (int i = 0; i < SUDOKU_SIZE; i++)
            {
                TextBox textBox = mSudokuGridLayoutPanel.TextBoxes[i];
                textBox.MouseEnter += (_pSender, _pArgs) =>
                {
                    if (textBox.ReadOnly && textBox.BackColor == Color.White && !textBox.Text.Equals(string.Empty))
                    {
                        textBox.BackColor = Color.LightGreen;
                    }
                };

                textBox.MouseLeave += (_pSender, _pArgs) =>
                { 
                    if(textBox.BackColor == Color.LightGreen)
                    {
                        textBox.BackColor = Color.White;
                    }
                };

                textBox.MouseDown += (_pSender, _pArgs) =>
                {
                    if (textBox.ReadOnly)
                    {
                        HideCaret(textBox.Handle);
                    }
                };
            }
        }

        private void OnClearButtonClick(object pSender, EventArgs pArgs)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the board?", "Clear Board", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK && ClearSignal != null)
            {
                ClearSignal(pSender, pArgs);
            }
        }

        private void OnFinishedButtonClick(object pSender, EventArgs pArgs)
        {
            if (FinishedSignal != null)
            {
                FinishedSignal(pSender, pArgs);
            }
        }

        public void ChangeControlState(ControlState pState)
        {
            if (pState == ControlState.LOADING && mCurrentState == ControlState.IDLE)
            { 
                this.mButtonClear.Enabled = false;
                this.mButtonFinished.Enabled = false;
                this.UseWaitCursor = true;

                for (int i = 0; i < SUDOKU_SIZE; i++)
                {
                    mSudokuGridLayoutPanel.TextBoxes[i].ReadOnly = true;
                    mSudokuGridLayoutPanel.TextBoxes[i].Cursor = Cursors.Arrow;
                }

            }
            else if (pState == ControlState.IDLE && mCurrentState == ControlState.IDLE)
            {
                for (int i = 0; i < SUDOKU_SIZE; i++)
                {
                    mSudokuGridLayoutPanel.TextBoxes[i].Text = string.Empty;
                }
            }
            else if (pState == ControlState.IDLE && mCurrentState == ControlState.LOADING)
            {
                this.mButtonClear.Enabled = true;
                this.mButtonFinished.Enabled = true;
                this.UseWaitCursor = false;

                for (int i = 0; i < SUDOKU_SIZE; i++)
                {
                    mSudokuGridLayoutPanel.TextBoxes[i].ReadOnly = false;
                    mSudokuGridLayoutPanel.TextBoxes[i].Cursor = Cursors.IBeam;
                }
            }
            else if (pState == ControlState.ACTIVE && mCurrentState == ControlState.LOADING)
            {
                this.mButtonClear.Enabled = true;

                this.mButtonFinished.Visible = false;
                this.mLabelFinished.Visible = false;

                this.mLabelSolveDetails.Visible = true;
                this.mButtonSolve.Visible = true;
                this.mButtonNext.Visible = true;
                this.mButtonBack.Visible = true;

                this.UseWaitCursor = false;

                for (int i = 0; i < SUDOKU_SIZE; i++)
                {
                    if (mSudokuGridLayoutPanel.TextBoxes[i].Text.Equals(string.Empty))
                    {
                        mSudokuGridLayoutPanel.TextBoxes[i].BackColor = Color.White;
                    }
                }
            }
            else if (pState == ControlState.IDLE && mCurrentState == ControlState.ACTIVE)
            {
                this.mButtonFinished.Visible = true;
                this.mButtonFinished.Enabled = true;
                this.mLabelFinished.Visible = true;

                this.mLabelSolveDetails.Visible = false;
                this.mButtonSolve.Visible = false;
                this.mButtonNext.Visible = false;
                this.mButtonBack.Visible = false;

                for (int i = 0; i < SUDOKU_SIZE; i++)
                {
                    mSudokuGridLayoutPanel.TextBoxes[i].ReadOnly = false;
                    mSudokuGridLayoutPanel.TextBoxes[i].Cursor = Cursors.IBeam;
                    mSudokuGridLayoutPanel.TextBoxes[i].Text = string.Empty;
                    mSudokuGridLayoutPanel.TextBoxes[i].BackColor = Color.Empty;
                }
            }
            else
            {
                return;
            }

            mCurrentState = pState;
        }

        public string GetTextBoxText(int pIndex)
        {
            return mSudokuGridLayoutPanel.TextBoxes[pIndex].Text;
        }

        public void SetTextBoxText(int pIndex, string pText)
        {
            mSudokuGridLayoutPanel.TextBoxes[pIndex].Text = pText;
        }

        public void ShowView()
        {
            Application.EnableVisualStyles();
            Application.Run(this);
        }
    }
}
