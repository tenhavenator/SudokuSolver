using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SudokuSolver.UI
{
    public partial class MainView : Form, IMainView
    {
        private GridView mGridView;

        public event EventHandler ClearSignal;
        public event EventHandler FinishedSignal;
        public event EventHandler NextSignal;
        public event EventHandler PrevSignal;
        public event EventHandler SolveSignal;
        public event EventHandler UnsolveSignal;

        private EventHandler mMouseEnterTextBox;
        private EventHandler mMouseLeaveTextBox;
        private MouseEventHandler mMouseDownTextBox;

        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public MainView()
        {
            InitializeComponent();
            mGridView = new GridView();
            mGridView.Anchor = AnchorStyles.None;
            mGridView.Location = new Point(38, 50);
            mPanelSudokuMain.Controls.Add(mGridView);

            mButtonClear.Click += new EventHandler(OnClearButtonClick);
            mButtonFinished.Click += new EventHandler(OnFinishedButtonClick);
            mButtonNext.Click += new EventHandler(OnNextButtonClick);
            mButtonBack.Click += new EventHandler(OnPrevButtonClick);
            mButtonSolve.Click += new EventHandler(OnSolveButtonClick);
            mButtonUnsolve.Click += new EventHandler(OnUnsolveButtonClick);

        }

        public static IMainView Create()
        { 
            return new MainView();
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

        public IGridView GridView
        {
            get { return mGridView;  }
        }


        public bool ClearControlsEnabled
        {
            set 
            {
                mButtonClear.Enabled = value;
            }
        }

        public bool SetupControlsEnabled
        {
            set 
            {
                mButtonFinished.Enabled = value;
                mLabelFinished.Visible = value;
            }
        }

        public bool LoadingControlsEnabled
        {
            set
            {
                UseWaitCursor = value;
                ClearControlsEnabled = !value;
                SetupControlsEnabled = !value;
                GameControlsEnabled = !value;
                mGridView.LoadingControlsEnabled = value;
            }
        }

        public bool GameControlsEnabled
        {
            set
            {
                mLabelSolveDetails.Visible = value;
                mButtonSolve.Visible = value;
                mButtonUnsolve.Visible = value;
                mButtonNext.Visible = value;
                mButtonBack.Visible = value;  
            }
        }

        public void Show()
        {
            Application.EnableVisualStyles();
            Application.Run(this);
        }
    }
}
