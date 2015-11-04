using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;

namespace SudokuSolver.Core.Techniques
{
    internal class FoundValue : IFoundValue
    {
        public char Value { get; set; }

        public int Index { get; set; }

        public IEnumerable<IMethod> Methods { get; set; }

        public int Rank { get; set; }
    }
}
