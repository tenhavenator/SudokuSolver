using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

namespace SudokuSolver
{
    public static class SudokuModel
    {
        public static void StartGame(byte[] pSudokuValues, Action<SolveResult> pOnSolveComplete)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (pSender, pArgs) => backgroundWorkerDo(pArgs);
            backgroundWorker.RunWorkerCompleted += (pSender, pArgs) => backgroundWorkerFinished(pArgs, pOnSolveComplete);
            backgroundWorker.RunWorkerAsync();

        }

        private static void backgroundWorkerDo(DoWorkEventArgs e)
        {
            Thread.Sleep(5000);
            e.Result = SolveResult.createErrorResult("There is nothing here yet.");
        }

        private static void backgroundWorkerFinished(RunWorkerCompletedEventArgs e, Action<SolveResult> pOnSolveComplete)
        {
            pOnSolveComplete(e.Result as SolveResult);
        }
    }
}
