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
    /// This is the class that displays a progress bar while using a back ground worker to solve a sudoku
    /// </summary>
    public partial class BackgroundSolveWorkerFrame : Form
    {
        private SolveResult mSolveResult;
        private byte[,] mSudokuValues;

        public BackgroundSolveWorkerFrame(byte[,] pSudokuValues)
        {
            InitializeComponent();

            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorkerDo);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerFinished);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerProgress);
            mSudokuValues = pSudokuValues;
        }

        /// <summary>
        /// Handler for the background worker "do work" event (the background worker is told to be begin its task). 
        /// Passes the sudoku to the solver and captures the result 
        /// </summary>
        private void backgroundWorkerDo(object sender, DoWorkEventArgs e)
        {
            mSolveResult = SudokuSolver.solve(mSudokuValues, backgroundWorker);
        }

        /// <summary>
        /// Handler for the background worker "finished" event (the background worker has finished its task).
        /// Closes the progress bar diaglog
        /// </summary>
        private void backgroundWorkerFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handler for the background worker "progress updated" event (the progress of the background worker's task has changed).
        /// Updates the progress bar on the dialog.
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
        /// Handler for the cancel button clicked event. Notifies the background worker when the cancel button is pressed.
        /// </summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// Execute the background worker and return the result.
        /// </summary>
        public SolveResult executeSolveWorkerDialog(Form pParent)
        {
            backgroundWorker.RunWorkerAsync();
            this.ShowDialog(pParent);
            return mSolveResult;
        }
    }
}
