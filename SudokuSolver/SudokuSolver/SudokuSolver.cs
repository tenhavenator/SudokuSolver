/// <summary>
/// This class contains the static functions for solving sudokus and generating hints.
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace SudokuSolver
{

    public class SudokuSolver
    {

        /// <summary>
        /// Solves a given sudoku. TODO make this actually solve a sudoku.
        /// </summary>
        public static SolveResult solve(byte[] pSudokuValues, BackgroundWorker pBackgroundSudokuSolver)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (pBackgroundSudokuSolver.CancellationPending)
                {
                    return new SolveResult(SolveResult.CANCELLED);
                }

                Thread.Sleep(100);
                pBackgroundSudokuSolver.ReportProgress(i);
                
            }
            return new SolveResult(SolveResult.SUCCESS);
        }
    }
}
