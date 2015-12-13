using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UI
{
    public interface IGridView
    {
        ICellView this[int pIndex] { get; }
        void ForEachCell(Action<ICellView> pAction);
        bool LoadingControlsEnabled { set; }
    }
}
