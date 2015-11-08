using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;

namespace SudokuSolver.Core.Solver
{
    internal class SudokuSolver
    {
        private int mInitialCount;
        private List<Entity> mEntities;
        private List<Square> mSquares;
        private List<IFoundValue> mFoundValues;

        private SudokuSolver(char[] pInitialValues)
        {
            mFoundValues = new List<IFoundValue>();
            mEntities = new List<Entity>();
            mSquares = new List<Square>();

            for (int i = 0; i < Constants.SUDOKU_SIZE; i++)
            {
                mSquares.Add(new Square() { Index = i, Filled = false });
            }

            for (int i = 0; i < Constants.ENTITY_SIZE; i++)
            {
                mEntities.Add(new Box(mSquares, i));
                mEntities.Add(new Row(mSquares, i));
                mEntities.Add(new Col(mSquares, i));
            }

            for (int i = 0; i < Constants.SUDOKU_SIZE; i++)
            {
                char value = pInitialValues[i];
                if (value != '\0')
                {
                    InsertValue(new FoundValue()
                    {
                        Value = value,
                        Index = i,
                        Methods = new IMethod[] { Method.CreateGivenValue(value, i) }
                    });
                }
            }

            mInitialCount = mFoundValues.Count();
        }

        public static SolveResult Solve(char[] pInitialValues)
        {
            return new SudokuSolver(pInitialValues).Solve();
        }

        private IEnumerable<int> InsertValue(IFoundValue pFoundValue)
        {
            if (mFoundValues.Any(v => v.Index == pFoundValue.Index))
            {
                return Enumerable.Empty<int>();
            }

            mFoundValues.Add(pFoundValue);

            Square square = mSquares[pFoundValue.Index];
            square.InsertValue(pFoundValue.Value);

            return square.Entities.SelectMany(e => e.Squares.Select(s => s.Index));
        }

        private IEnumerable<FoundValue> CheckValues()
        {
            List<FoundValue> foundValues = new List<FoundValue>();
            foreach (var square in mSquares.Where(s => !s.Filled))
            {
                if (square.HasFoundValue())
                {
                    foundValues.Add(square.GetFoundValue());
                }
            }

            return foundValues;
        }

        private SolveResult Solve()
        {
            try
            {
                DateTime start = DateTime.Now;
                while (true)
                {
                    int foundValuesCount = mFoundValues.Count;

                    if (foundValuesCount == Constants.SUDOKU_SIZE)
                    {
                        return new SolveResult { FoundValues = mFoundValues, InitialCount = mInitialCount };
                    }

                    IEnumerable<FoundValue> foundValues = CheckValues();
                    if (foundValues.Any())
                    {
                        InsertValue(foundValues.OrderByDescending(v => v.Rank).First());
                        continue;                    
                    }
                    

                    // Apply advanced techs an check again


                    return new SolveResult();
                   
                }
            }
            catch (Exception e)
            {
                return new SolveResult { Error = e.Message };
            }
        }
    }
}
