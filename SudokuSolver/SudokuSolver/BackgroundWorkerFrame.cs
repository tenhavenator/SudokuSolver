/// <summary>
/// These are the classes for using background workers with progress bars
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
    /// <summary>
    /// This is the generic class for using a background worker with a progress bar
    /// </summary>
    public abstract partial class BackgroundWorkerFrame : Form
    {
        protected byte[,] mSudokuValues;

        public BackgroundWorkerFrame(byte[,] pSudokuValues)
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            mSudokuValues = pSudokuValues;
        }

        /// <summary>
        /// Initializes the background worker
        /// </summary>
        private void InitializeBackgroundWorker()
        {
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorkerDo);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerFinished);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerProgress);
        }

        /// <summary>
        /// Defines what the background worker should do.
        /// </summary>
        protected abstract void backgroundWorkerDo(object sender, DoWorkEventArgs e);

        /// <summary>
        /// Closes the dialog when the background worker is finished
        /// </summary>
        protected void backgroundWorkerFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Updates the progress bar on the dialog when background worker progress changes
        /// </summary>
        private void backgroundWorkerProgress(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;

            if (progress > 100)
            {
                progress = 100;
            }

            if (progress < 0)
            {
                progress = 0;
            }

            progressBar.Value = progress;
        }

        /// <summary>
        /// Notifies the background worker when the cancel button is pressed
        /// </summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// Starts the background worker and shows the progress dialog.
        /// </summary>
        protected void executeWorkerDialog(Form pParent) {

            backgroundWorker.RunWorkerAsync();
            this.ShowDialog(pParent);
        }
    }

    /// <summary>
    /// This is the class for using a background worker to solve a sudoku
    /// </summary>
    public class BackgroundSolveWorkerFrame : BackgroundWorkerFrame
    {
        private SolveResult mSolveResult;

        public BackgroundSolveWorkerFrame(byte[,] pSudokuValues) : base(pSudokuValues)
        {
            label1.Text = "Loading and checking sudoku...";
        }

        /// <summary>
        /// Defines this background worker to solve a sudoku
        /// </summary>
        protected override void backgroundWorkerDo(object sender, DoWorkEventArgs e)
        {
            mSolveResult = SudokuSolver.solve(mSudokuValues, backgroundWorker);
        }

        /// <summary>
        /// Execute the background worker and return the result.
        /// </summary>
        public SolveResult executeSolveWorkerDialog(Form pParent)
        {
            executeWorkerDialog(pParent);
            return mSolveResult;        
        }
    }
}
