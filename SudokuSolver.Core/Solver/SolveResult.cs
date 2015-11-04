using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;

namespace SudokuSolver.Core.Solver
{
    internal class SolveResult
    {
        public List<IFoundValue> FoundValues { get; set; }
        public string Error { get; set; }
        public int InitialCount { get; set; }
    }
}
