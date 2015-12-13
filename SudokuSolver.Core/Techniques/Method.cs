using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core.Techniques
{
    using Model;
    using Solver;

    /// <summary>
    /// The concrete implementation of <see cref="IMethod"/> There are three types of methods:
    /// 1). Given Value -  A value was part of the initial sudoku.
    /// 2). Entity Value - A value was found by eliminating all other possible locations in a Box, Row, or Column. A value can
    ///     be found using more than one method of this type (e.g. A value might be found in the same square by a Box and Row 
    ///     at the same time).
    /// 3). Only Possible Value - A value was found by eliminating all other possible values for a particular square.
    /// </summary>
    internal class Method : IMethod
    {
        /// <summary>
        /// The name of the method.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The description of the method.
        /// </summary>
        public string Desc { get; private set; }

        /// <summary>
        /// The indexes of the sqaures in the method. For the Entity method this is the squares in the Box, Row, or Column. For
        /// the other methods this is the square where the value was found.
        /// </summary>
        public IEnumerable<int> Indexes { get; private set; }

        /// <summary>
        /// The minimum set of techniques used to generate the method
        /// </summary>
        public IEnumerable<ITechnique> Techniques { get; private set; }

        /// <summary>
        ///  The rank of the method. The rank will increase based on the complexity of the method. 
        /// </summary>
        public int Rank { get; private set; }


        private Method()
        {
            // Private constructor ensures that only the method can instantiate concrete instances of itself
        }

        /// <summary>
        /// Creates a method to represent the case where the value was part of the initial sudoku
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square where the value is.</param>
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

        /// <summary>
        /// Creates a method to represent the case where the value was found within a box.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pSquares">The squares of the box.</param>
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

        /// <summary>
        /// Creates a method to represent the case where the value was found within a row.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pSquares">The squares of the row.</param>
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

        /// <summary>
        /// Creates a method to represent the case where the value was found within a column.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pSquares">The squares of the column.</param>
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

        /// <summary>
        /// Creates a method to represent the case where there was only one possible value in a square.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pSquare">The square</param>
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

        /// <summary>
        /// Calculate the minimum set of techniques needed to find a value in an entity.
        /// </summary>
        /// <param name="pValue">The value</param>
        /// <param name="pSquares">The squares of the entity.</param>
        /// <returns>The minimum set of techniques.</returns>
        private static IEnumerable<ITechnique> EvaluateEntityTechniques(char pValue, IEnumerable<Square> pSquares)
        {
            List<ITechnique> techniques = new List<ITechnique>();
            var allSquares = pSquares.ToList();
            
            // Consider techniques for each rank of technique included in the minimum set
            var rank = pSquares.Max(s => s.Rank(pValue));
            for (int r = rank; r >= 0; r--)
            {
                // Get all the square that are elminated using technqiues of this rank
                var rankSquares = allSquares.Where(s => s.Rank(pValue) == r).ToList();

                // Get the set of techniques that are not optional (i.e. when a square has only one technique for a value)
                var techniquesApplied = rankSquares.Select(s => s.Techniques(pValue)).
                       Where(t => t.Count() == 1).SelectMany(t => t).Distinct().ToList();

                // Calculate the set of squares remaining for this rank after applying the non-optional techniques
                allSquares.RemoveAll(s => s.Techniques(pValue, false).Intersect(techniquesApplied).Any());
                rankSquares.RemoveAll(s => s.Techniques(pValue).Intersect(techniquesApplied).Any());

                // Get the techniques for the remaining squares and order them by occurence
                var techniquesRemaining = rankSquares.SelectMany(s => s.Techniques(pValue)).GroupBy(t => t).
                    OrderByDescending(g => g.Count()).Select(g => g.Key);

                // Apply the remaining techniques until all sqaures have a technique
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

        /// <summary>
        /// Calculate the minimum set of techniques needed to make a value the only possible value in a square
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pSquare">The square.</param>
        /// <returns>The minimum set of techniques.</returns>
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
