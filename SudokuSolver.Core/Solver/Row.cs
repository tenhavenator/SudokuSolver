using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;

namespace SudokuSolver.Core.Solver
{
    internal class Row : Entity
    {
        public Row(IEnumerable<Square> pGrid, int pIndex) : base(pGrid, pIndex) { }

        protected override IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex)
        {
            var startIndex = pEntityIndex * Constants.ENTITY_SIZE;
            return pGrid.Where((s, i) => i >= startIndex && i < startIndex + Constants.ENTITY_SIZE);
        }

        protected override ITechnique CreateTechnique(char pValue, int pIndex)
        {
            return Technique.CreateRowTechnique(pValue, pIndex, mSquares.Select(s => s.Index));
        }

        protected override Method CreateMethod(char pValue)
        {
            return Method.CreateRowMethod(pValue, mSquares.Except(mValueSquares[pValue]));
        }
    }
}
