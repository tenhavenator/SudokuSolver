using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UI
{
    public interface IMainView
    {
        void Show();

        event EventHandler ClearSignal;
        event EventHandler FinishedSignal;
        event EventHandler NextSignal;
        event EventHandler PrevSignal;
        event EventHandler SolveSignal;
        event EventHandler UnsolveSignal;

        IGridView GridView { get; }

        bool ClearControlsEnabled { set; }
        bool SetupControlsEnabled { set; }
        bool LoadingControlsEnabled { set; }
        bool GameControlsEnabled { set; }
    }
}
