using System.Collections.Generic;

namespace SudokuSolver.Core.Model
{
    public interface ITechnique
    {
        int Rank { get; }

        string Name { get; }

        string Desc { get; }

        IEnumerable<int> Indexes { get; }

        IDictionary<char, IEnumerable<int>> ValueMap { get; }
    }
}
