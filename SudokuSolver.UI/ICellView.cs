using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SudokuSolver.UI
{
    public interface ICellView
    {
        char Value { get; set; }
        bool Enabled { set; }
        Color Background { set; }
    }
}
