using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;
using SudokuSolver.Core.Solver;

namespace SudokuSolver.Core.Techniques
{
    internal class Method : IMethod
    {
        public string Name { get; private set; }

        public string Desc { get; private set; }

        public IEnumerable<int> Indexes { get; private set; }

        public IEnumerable<ITechnique> Techniques { get; private set; }

        public int Rank { get; private set; }

        public static Method CreateGivenValue(char pValue, int pIndex)
        {
            return new Method()
            {
                Name = "Given Value",
                Desc = "This " + pValue + " was part of the initial Sudoku.",
                Indexes = new int[] { pIndex },
                Techniques = Enumerable.Empty<ITechnique>(),
                Rank = 0
            };
        }

        public static Method CreateBoxMethod(char pValue, IEnumerable<Square> pSquares)
        {
            var techniques = EvaluateEntityTechniques(pValue, pSquares);
            var maxTechRank = techniques.Max(t => t.Rank);
            var rank = maxTechRank + maxTechRank <= Technique.BASIC_TECH_RANK ? 1 : 2;

            return new Method()
            {
                Name = "Box Found Value",
                Desc = "This is the only place where " + pValue + " can go in this box.",
                Indexes = pSquares.Select(e => e.Index),
                Techniques = techniques,
                Rank = rank 
            };
        }

        public static Method CreateRowMethod(char pValue, IEnumerable<Square> pSquares)
        {
            return new Method()
            {
                Name = "Row Found Value",
                Desc = "This is the only place where " + pValue + " can go in this row.",
                Indexes = pSquares.Select(e => e.Index),
                Techniques = EvaluateEntityTechniques(pValue, pSquares)
            };

        }

        public static Method CreateColMethod(char pValue, IEnumerable<Square> pSquares)
        {
            return new Method()
            {
                Name = "Column Found Value",
                Desc = "This is the only place where " + pValue + " can go in this column.",
                Indexes = pSquares.Select(e => e.Index),
                Techniques = EvaluateEntityTechniques(pValue, pSquares)
            };
        }

        public static Method CreateOnlyValue(char pValue, Square pSquare)
        {
            var techniques = EvaluateSquareTechniques(pValue, pSquare);
            var rank = Math.Max(Technique.BASIC_TECH_RANK, techniques.Max(t => t.Rank)) * 2;

            return new Method()
            {
                Name = "Only Possible Value",
                Desc = pValue + " is the only possible value that can go in this square.",
                Indexes = Enumerable.Empty<int>(),
                Techniques = techniques,
                Rank = rank
            };
        }

        private static IEnumerable<ITechnique> EvaluateEntityTechniques(char pValue, IEnumerable<Square> pSquares)
        {
            List<ITechnique> techniques = new List<ITechnique>();
            var allSquares = pSquares.ToList();
            var rank = pSquares.Max(s => s.Rank(pValue));

            for (int r = rank; r >= 0; r--)
            {
                var rankSquares = allSquares.Where(s => s.Rank(pValue) == r).ToList();
                var techniquesApplied = rankSquares.Select(s => s.Techniques(pValue)).
                       Where(t => t.Count() == 1).SelectMany(t => t).Distinct().ToList();

                allSquares.RemoveAll(s => s.Techniques(pValue, false).Intersect(techniquesApplied).Any());
                rankSquares.RemoveAll(s => s.Techniques(pValue).Intersect(techniquesApplied).Any());

                var techniquesRemaining = rankSquares.SelectMany(s => s.Techniques(pValue)).GroupBy(t => t).
                    OrderByDescending(g => g.Count()).Select(g => g.Key);

                foreach (ITechnique tech in techniquesRemaining)
                {
                    techniquesApplied.Add(tech);
                    rankSquares.RemoveAll(s => s.Techniques(pValue).Contains(tech));
                    allSquares.RemoveAll(s => s.Techniques(pValue, false).Contains(tech));

                    if (!rankSquares.Any())
                    {
                        break;
                    }
                }

                techniques.AddRange(techniquesApplied);
            }

            return techniques;
        }

        private static IEnumerable<ITechnique> EvaluateSquareTechniques(char pValue, Square pSquare)
        {
            List<ITechnique> techniques = new List<ITechnique>();

            foreach(char value in Constants.ALL_VALUES)
            {
                if(value != pValue)
                {
                    techniques.Add(pSquare.Techniques(value).First());
                }
            }

            return techniques;
        }
    }
}
