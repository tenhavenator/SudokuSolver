using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;

namespace SudokuSolver.Core.Solver
{
    internal class Col : Entity
    {
        public Col(IEnumerable<Square> pGrid, int pIndex) : base(pGrid, pIndex) { }

        protected override IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex)
        {
            return pGrid.Where((s, i) => i % Constants.ENTITY_SIZE == pEntityIndex);
        }

        protected override ITechnique CreateTechnique(char pValue, int pIndex)
        {
            return Technique.CreateBoxTechnique(pValue, pIndex, mSquares.Select(s => s.Index));
        }

        protected override Method CreateMethod(char pValue)
        {
            return Method.CreateColMethod(pValue, mSquares.Except(mValueSquares[pValue]));
        }
    }
}
