using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Core
{
    public static class Constants
    {
        public const int SUDOKU_SIZE = 81;
        public const int ENTITY_SIZE = 9;
        public const int BOX_SIZE = 3;

        public static IEnumerable<char> ALL_VALUES
        {
            get
            {
                yield return '1';
                yield return '2';
                yield return '3';
                yield return '4';
                yield return '5';
                yield return '6';
                yield return '7';
                yield return '8';
                yield return '9';
            }
        }
    }
}
