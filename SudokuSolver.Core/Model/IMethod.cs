using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Core.Model
{
    public interface IMethod
    {
        string Name { get; }

        string Desc { get; }

        IEnumerable<int> Indexes { get; }

        IEnumerable<ITechnique> Techniques { get; }
    }
}
