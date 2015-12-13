using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SudokuSolver.Core.Model;
using SudokuSolver.UI;
using System.Threading;

namespace SudokuSolver.Presenter
{
    public class MainPresenter
    {
        private const int DELAY = 2500;
        private const int SUDOKU_SIZE = 81;

        private IMainView mMainView;
        private IModel mModel;
 
        public MainPresenter()
        {
            mMainView = MainView.Create();
            mModel = Model.CreateModel();

            mMainView.ClearSignal += (_pSender, _pArgs) => OnClearSignal();
            mMainView.FinishedSignal += (_pSender, _pArgs) => OnFinishedSignal();
            mMainView.NextSignal += (_pSender, _pArgs) => OnNextSignal();
            mMainView.PrevSignal += (_pSender, _pArgs) => OnPrevSignal();
            mMainView.SolveSignal += (_pSender, _pArgs) => OnSolveSignal();
            mMainView.UnsolveSignal += (_pSender, _pArgs) => OnUnsolveSignal();

            mMainView.Show();
        }

        private void OnClearSignal()
        {
            mModel.ClearGame();
            mMainView.GameControlsEnabled = false;
            mMainView.GridView.ForEachCell(c =>
            {
                c.Enabled = true;
                c.Value = '\0';

            });
        }

        private void OnFinishedSignal()
        {
            mMainView.LoadingControlsEnabled = true;

            char[] sudokuValues = new char[SUDOKU_SIZE];

            for (int i = 0; i < SUDOKU_SIZE; i++)
            {
                sudokuValues[i] = mMainView.GridView[i].Value;
            }

            DateTime start = DateTime.Now;
            mModel.StartGame(sudokuValues, () =>
            {
                int elapsed = (int)(DateTime.Now - start).TotalMilliseconds;
                if (elapsed < DELAY)
                {
                    Thread.Sleep(DELAY - elapsed);
                }

                mMainView.LoadingControlsEnabled = false;

                if (mModel.Error != null)
                {
                    mMainView.LoadingControlsEnabled = false;
                    mMainView.GameControlsEnabled = false;


                   // MessageBox.Show("There was an error while trying to process the Sudoku: " + mModel.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (mModel.Invalid)
                {
                    mMainView.LoadingControlsEnabled = false;
                    mMainView.GameControlsEnabled = false;

                  //  MessageBox.Show("The enter values are not a valid Sudoku", "Invalid Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    mMainView.LoadingControlsEnabled = false;
                    mMainView.SetupControlsEnabled = false;
                }
            });
        }

        private void OnSolveSignal()
        {
            var values = mModel.RemainingValues();
            foreach (var value in values)
            {
                mMainView.GridView[value.Index].Value = value.Value;
            }
        }

        private void OnUnsolveSignal()
        {
            var values = mModel.InitialValues();
            foreach (var value in values)
            {
                mMainView.GridView[value.Index].Value = '\0';
            }
        }

        private void OnNextSignal()
        {
            IFoundValue value = mModel.NextValue();
            mMainView.GridView[value.Index].Value = value.Value;
        }

        private void OnPrevSignal()
        {
            IFoundValue value = mModel.PrevValue();
            mMainView.GridView[value.Index].Value = '\0';
        }

        [STAThread]
        public static void Main()
        {
            new MainPresenter();
        }
    }
}
