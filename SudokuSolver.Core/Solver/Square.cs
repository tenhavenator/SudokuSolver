using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core.Solver
{
    using Model;
    using Techniques;

    /// <summary>
    /// Class that represents a square in the sudoku
    /// </summary>
    internal class Square
    {
        /// <summary>
        /// Class that represents each of the values (1-9) that can go in a square.
        /// </summary>
        private class SquareValue
        {
            /// <summary>
            /// The rank of the techniques used to eliminate this value from the square
            /// </summary>
            public int TechRank { get; set; }

            /// <summary>
            /// The rank of the methods used to find a value for this square.
            /// </summary>
            public int MethodRank { get; set; }

            /// <summary>
            /// The list of techniques used to eliminate this value from the square
            /// </summary>
            public ICollection<ITechnique> Techniques { get; set; }

            /// <summary>
            /// The methods used to find a value for this square.
            /// </summary>
            public ICollection<Method> Methods { get; set; }
        }

        private IDictionary<char, SquareValue> mValues;
        private List<Entity> mEntities;

        /// <summary>
        /// Instantiates a square.
        /// </summary>
        public Square()
        {
            mValues = new Dictionary<char, SquareValue>();
            mEntities = new List<Entity>();

            // Initializes the possible values for this square.
            foreach (char value in Constants.ALL_VALUES)
            {
                mValues.Add(value, new SquareValue() { TechRank = int.MaxValue, 
                    Techniques = new List<ITechnique>(), Methods = new List<Method>() });
            }
        }

        /// <summary>
        /// The index of this square (0-80).
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// If there is a value in this square.
        /// </summary>
        public bool Filled { get; set; }

        /// <summary>
        /// The three entities that this square is part of.
        /// </summary>
        public ICollection<Entity> Entities
        {
            get { return mEntities; }
        }

        /// <summary>
        /// The values that are not possible for this square.
        /// </summary>
        public IEnumerable<char> EliminatedValues
        {
            get
            {
                return mValues.Where(p => p.Value.Techniques.Any()).Select(p => p.Key);
            }
        }

        /// <summary>
        /// The values that are possible for this sqaure.
        /// </summary>
        public IEnumerable<char> PossibleValues
        {
            get
            {
                return mValues.Where(p => !p.Value.Techniques.Any()).Select(p => p.Key);
            }
        }

        /// <summary>
        /// Inserts a value into this square.
        /// </summary>
        /// <param name="pValue">The value to insert.</param>
        public void InsertValue(char pValue)
        {
            if (Filled)
            {
                return;
            }

            Filled = true;

            // Prepare entities for insertion
            mEntities.ForEach(e => e.PreInsertion(pValue));

            // Remove all possible values from this square since it is now occupied
            ITechnique tech = Technique.CreateOccupiedTechnique(pValue, Index);
            foreach (char value in Constants.ALL_VALUES)
            {
                EliminatePossibility(value, tech);
            }

            // Insert the value into the entities
            mEntities.ForEach(e => e.StartInsertion(Index));
            mEntities.ForEach(e => e.EndInsertion());
        }

        /// <summary>
        /// Eliminate a possible value from this square.
        /// </summary>
        /// <param name="pValue">The value to eliminate.</param>
        /// <param name="pTechnique">The technique used to eliminate the value.</param>
        public void EliminatePossibility(char pValue, ITechnique pTechnique)
        {
            // Remove the value and update the technique rank if necessary
            var value = mValues[pValue];
            value.Techniques.Add(pTechnique);
            if (value.TechRank > pTechnique.Rank)
            {
                value.TechRank = pTechnique.Rank;
            }

            // If there is only one value left for the square then generate a method
            if (PossibleValues.Count() == 1)
            {
                var foundValue = PossibleValues.Single();
                AddMethod(foundValue, Method.CreateOnlyValue(foundValue, this));
            }

            mEntities.ForEach(e => e.EliminatePossibility(pValue, this));
        }

        /// <summary>
        /// Adds a method for a value that has been found.
        /// </summary>
        /// <param name="pValue">The value to add the method for.</param>
        /// <param name="pMethod">The method.</param>
        public void AddMethod(char pValue, Method pMethod)
        {
            var value = mValues[pValue];

            if(Filled || pMethod.Rank > value.MethodRank)
            {
                return;
            }

            // Clear the current methods if the one being added has a lower rank
            if (pMethod.Rank > value.MethodRank)
            {
                value.Methods.Clear();
            }

            value.Methods.Add(pMethod);
        }

        /// <summary>
        /// The rank of the techniques used to eliminate a particular value from this square.
        /// </summary>
        /// <param name="pValue">The value to get the technique rank for.</param>
        public int Rank(char pValue)
        {
            return mValues[pValue].TechRank;
        }

        /// <summary>
        /// The techniques used to eliminate a particular value from this square.
        /// </summary>
        /// <param name="pValue">The value to get the techniques for.</param>
        /// <param name="pMinRankOnly">Return only the technqiues of the lowest rank.</param>
        public IEnumerable<ITechnique> Techniques(char pValue, bool pMinRankOnly = true)
        {
            var value = mValues[pValue];
            return value.Techniques.Where(t => !pMinRankOnly || t.Rank == value.TechRank);
        }

        /// <summary>
        /// Whether this square contains a method for finding any value
        /// </summary>
        public bool HasFoundValue()
        {
           return mValues.Where(p => p.Value.Methods.Any()).Any();
        }

        /// <summary>
        /// Gets the value found in this square. 
        /// </summary>
        /// <returns></returns>
        public FoundValue GetFoundValue()
        {
            if (HasFoundValue())
            {
                var value = mValues.First(p => p.Value.Methods.Any());

                return new FoundValue()
                {
                    Index = this.Index,
                    Value = value.Key,
                    Methods = value.Value.Methods,
                    Rank = value.Value.Methods.First().Rank
                };
            }

            return null;
        }
    }
}
