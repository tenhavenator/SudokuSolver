using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;

namespace SudokuSolver.Core.Solver
{
    internal class Box : Entity
    {
        public Box(IEnumerable<Square> pGrid, int pIndex) : base(pGrid, pIndex) { }

        protected override IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex)
        {
            var boxRow = pEntityIndex / Constants.BOX_SIZE;
            var boxCol = pEntityIndex % Constants.BOX_SIZE;
            var boxRowIndexes = Constants.BOX_SIZE * Constants.ENTITY_SIZE;

            return pGrid.Where((s, i) => (i / (boxRowIndexes)) == boxRow
                && (i / Constants.BOX_SIZE) % Constants.BOX_SIZE == boxCol);
        }

        protected override ITechnique CreateTechnique(char pValue, int pIndex)
        {
            return Technique.CreateBoxTechnique(pValue, pIndex, mSquares.Select(s => s.Index));
        }

        protected override Method CreateMethod(char pValue)
        {
            return Method.CreateBoxMethod(pValue, mSquares.Except(mValueSquares[pValue]));
        }
    }
}
