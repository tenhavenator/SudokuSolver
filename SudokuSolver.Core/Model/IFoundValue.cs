using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Core.Model
{
    public interface IFoundValue
    {
        char Value { get; }

        int Index { get; }

        IEnumerable<IMethod> Methods { get; }
    }
}
