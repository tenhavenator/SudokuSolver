using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SudokuSolver
{
    public enum State { ACTIVE, IDLE, LOADING };

    public partial class SudokuMainView : Form
    {
        private SudokuGridLayoutPanel mSudokuGridLayoutPanel;
        private State mCurrentState;

        public event EventHandler ClearSignal;
        public event EventHandler FinishedSignal;
        public event EventHandler NextSignal;
        public event EventHandler PrevSignal;
        public event EventHandler SolveSignal;
        public event EventHandler UnsolveSignal;

        public event EventHandler<CellEventArgs> ClickSignal;

        private EventHandler mMouseEnterTextBox;
        private EventHandler mMouseLeaveTextBox;
        private MouseEventHandler mMouseDownTextBox;

        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public SudokuMainView()
        {
            InitializeComponent();
            mSudokuGridLayoutPanel = new SudokuGridLayoutPanel();
            mSudokuGridLayoutPanel.Anchor = AnchorStyles.None;
            mSudokuGridLayoutPanel.Location = new Point(38, 50);
            mPanelSudokuMain.Controls.Add(mSudokuGridLayoutPanel);

            mButtonClear.Click += new EventHandler(OnClearButtonClick);
            mButtonFinished.Click += new EventHandler(OnFinishedButtonClick);
            mButtonNext.Click += new EventHandler(OnNextButtonClick);
            mButtonBack.Click += new EventHandler(OnPrevButtonClick);
            mButtonSolve.Click += new EventHandler(OnSolveButtonClick);
            mButtonUnsolve.Click += new EventHandler(OnUnsolveButtonClick);

            mMouseEnterTextBox = new EventHandler(OnMouseEnterTextBox);
            mMouseLeaveTextBox = new EventHandler(OnMouseLeaveTextBox);
            mMouseDownTextBox = new MouseEventHandler(OnMouseDownTextBox);

            mCurrentState = State.IDLE;
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

        private void OnNextButtonClick(object pSender, EventArgs pArgs)
        {
            if (NextSignal != null)
            {
                NextSignal(pSender, pArgs);
            }
            
        }

        private void OnPrevButtonClick(object pSender, EventArgs pArgs)
        {
            if (PrevSignal != null)
            {
                PrevSignal(pSender, pArgs);
            }
        }

        private void OnSolveButtonClick(object pSender, EventArgs pArgs)
        {
            if (SolveSignal != null)
            {
                SolveSignal(pSender, pArgs);
            }
        }

        private void OnUnsolveButtonClick(object pSender, EventArgs pArgs)
        {
            if (UnsolveSignal != null)
            {
                UnsolveSignal(pSender, pArgs);
            }
        }

        private void OnMouseEnterTextBox(object pSender, EventArgs _pArgs)
        {
            TextBox textBox = pSender as TextBox;
            if (!textBox.Text.Equals(string.Empty))
            {
                textBox.BackColor = Color.LightGreen;
            }
        }

        private void OnMouseLeaveTextBox(object pSender, EventArgs _pArgs)
        {
            TextBox textBox = pSender as TextBox;
            textBox.BackColor = Color.White;
        }

        private void OnMouseDownTextBox(object pSender, MouseEventArgs _pArgs)
        {
            TextBox textBox = pSender as TextBox;
            HideCaret(textBox.Handle);
        }

        /*private void OnClickTextBox(object pSender, EventArgs _pArgs)
        {
            TextBox textBox = pSender as TextBox;
            if (!textBox.Text.Equals(string.Empty) && ClickSignal != null)
            {
                int index = Array.IndexOf(mSudokuGridLayoutPanel.TextBoxes, textBox);
                ClickSignal(pSender, new ClickEventArgs() { Index = index });
            }
        }*/

        public void ChangeControlState(State pNextState)
        {
            if (mCurrentState == State.IDLE && pNextState == State.LOADING)
            { 
                mButtonClear.Enabled = false;
                mButtonFinished.Enabled = false;
                UseWaitCursor = true;

                mSudokuGridLayoutPanel.ForEachTextBox(t => 
                {
                    t.ReadOnly = true;
                    t.Cursor = Cursors.Arrow;
                    t.MouseDown += mMouseDownTextBox;
                });
            }
            else if (mCurrentState == State.IDLE && pNextState == State.IDLE)
            {
                mSudokuGridLayoutPanel.ForEachTextBox(t =>
                {
                    t.Text = string.Empty;
                });
            }
            else if (mCurrentState == State.LOADING && pNextState == State.IDLE)
            {
                mButtonClear.Enabled = true;
                mButtonFinished.Enabled = true;
                UseWaitCursor = false;

                mSudokuGridLayoutPanel.ForEachTextBox(t =>
                {
                    t.ReadOnly = false;
                    t.Cursor = Cursors.IBeam;
                });
            }
            else if (mCurrentState == State.LOADING && pNextState == State.ACTIVE)
            {
                mButtonClear.Enabled = true;
                mButtonFinished.Visible = false;
                mLabelFinished.Visible = false;
                mLabelSolveDetails.Visible = true;
                mButtonSolve.Visible = true;
                mButtonUnsolve.Visible = true;
                mButtonNext.Visible = true;
                mButtonBack.Visible = true;
                UseWaitCursor = false;

                mSudokuGridLayoutPanel.ForEachTextBox(t =>
                {
                    if (t.Text.Equals(string.Empty))
                    {
                        t.BackColor = Color.White;
                        t.MouseEnter += mMouseEnterTextBox;
                        t.MouseLeave += mMouseLeaveTextBox;
                    }
                });
            }
            else if (mCurrentState == State.ACTIVE && pNextState == State.IDLE)
            {
                mButtonFinished.Visible = true;
                mButtonFinished.Enabled = true;
                mLabelFinished.Visible = true;

                mLabelSolveDetails.Visible = false;
                mButtonSolve.Visible = false;
                mButtonUnsolve.Visible = false;
                mButtonNext.Visible = false;
                mButtonBack.Visible = false;

                mSudokuGridLayoutPanel.ForEachTextBox(t =>
                {
                    t.ReadOnly = false;
                    t.Cursor = Cursors.IBeam;
                    t.Text = string.Empty;
                    t.BackColor = Color.Empty;
                    t.MouseEnter -= mMouseEnterTextBox;
                    t.MouseEnter -= mMouseLeaveTextBox;
                    t.MouseDown -= mMouseDownTextBox;
                });
            }
            else
            {
                return;
            }

            mCurrentState = pNextState;
        }

        public char this[int pIndex]
        {
            get { return '\0'; }
        }

        public char GetTextBoxText(int pIndex)
        {
            string text = mSudokuGridLayoutPanel.TextBoxes[pIndex].Text;
            return text.Length == 0 ? '\0' : text[0];
        }

        public void SetTextBoxText(int pIndex, char pText)
        {
            mSudokuGridLayoutPanel.TextBoxes[pIndex].Text = pText.ToString();
        }

        public void ShowView()
        {
            Application.EnableVisualStyles();
            Application.Run(this);
        }

    }

    


    public class CellEventArgs : EventArgs
    {
        public int Index { get; set; }
    }
}
