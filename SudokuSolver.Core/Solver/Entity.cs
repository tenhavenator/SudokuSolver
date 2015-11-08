using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;

namespace SudokuSolver.Core.Solver
{
    internal abstract class Entity
    {
        protected IEnumerable<Square> mSquares;
        protected IDictionary<char, ICollection<Square>> mValueSquares;
        private char? mInserting;

        public Entity(IEnumerable<Square> pGrid, int pIndex)
        {
            mSquares = PickSquares(pGrid, pIndex);
            mValueSquares = new Dictionary<char, ICollection<Square>>();

            foreach (Square square in mSquares)
            {
                square.Entities.Add(this);
            }

            foreach (var value in Constants.ALL_VALUES)
            {
                mValueSquares.Add(value, new List<Square>(mSquares));
            }
        }

        public IEnumerable<Square> Squares
        {
            get { return mSquares; }
        }

        public void PreInsertion(char pValue)
        {
            mInserting = pValue;
        }

        public void StartInsertion(int pIndex)
        {
            var technique = CreateTechnique(mInserting.Value, pIndex);
            mValueSquares[mInserting.Value].Clear();
            foreach (Square square in mSquares)
            {
                square.EliminatePossibility(mInserting.Value, technique);
            }
        }

        public void EndInsertion()
        {
            mInserting = null;
        }

        public void EliminatePossibility(char pValue, Square pSquare)
        {
            var squares = mValueSquares[pValue];
            squares.Remove(pSquare);

            if (squares.Count() == 1 && mInserting != pValue)
            {
                squares.Single().AddMethod(pValue, CreateMethod(pValue));
            }
        }

        protected abstract Method CreateMethod(char pValue);
        protected abstract ITechnique CreateTechnique(char pValue, int pIndex);
        protected abstract IEnumerable<Square> PickSquares(IEnumerable<Square> pGrid, int pEntityIndex);
    }
}
