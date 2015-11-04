using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Core.Model
{
    public interface IModel
    {
        string Error { get; }

        bool Invalid { get; }

        void StartGame(char[] pSudokuValues, Action pOnSolved);

        void ClearGame();

        IEnumerable<IFoundValue> InitialValues();

        IEnumerable<IFoundValue> RemainingValues();

        IFoundValue NextValue();

        IFoundValue PrevValue();
    }
}
