using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;
using SudokuSolver.Core.Techniques;

namespace SudokuSolver.Core.Solver
{
    internal class Square
    {
        private class SquareValue
        {
            public int TechRank { get; set; }
            public int MethodRank { get; set; }
            public ICollection<ITechnique> Techniques { get; set; }
            public ICollection<Method> Methods { get; set; }
        }

        private IDictionary<char, SquareValue> mValues;
        private List<Entity> mEntities;

        public Square()
        {
            mValues = new Dictionary<char, SquareValue>();
            mEntities = new List<Entity>();

            foreach (char value in Constants.ALL_VALUES)
            {
                mValues.Add(value, new SquareValue() { TechRank = int.MaxValue, 
                    Techniques = new List<ITechnique>(), Methods = new List<Method>() });
            }
        }

        public int Index { get; set; }

        public bool Filled { get; set; }

        public ICollection<Entity> Entities
        {
            get { return mEntities; }
        }

        public IEnumerable<char> EliminatedValues
        {
            get
            {
                return mValues.Where(p => p.Value.Techniques.Any()).Select(p => p.Key);
            }
        }

        public IEnumerable<char> PossibleValues
        {
            get
            {
                return mValues.Where(p => !p.Value.Techniques.Any()).Select(p => p.Key);
            }
        }

        public void InsertValue(char pValue)
        {
            Filled = true;

            mEntities.ForEach(e => e.PreInsertion(pValue));

            ITechnique tech = Technique.CreateOccupiedTechnique(pValue, Index);
            foreach (char value in Constants.ALL_VALUES)
            {
                EliminatePossibility(value, tech);
            }

            mEntities.ForEach(e => e.StartInsertion(Index));
            mEntities.ForEach(e => e.EndInsertion());
        }

        public void EliminatePossibility(char pValue, ITechnique pTechnique)
        {
            var value = mValues[pValue];
            value.Techniques.Add(pTechnique);
            if (value.TechRank > pTechnique.Rank)
            {
                value.TechRank = pTechnique.Rank;
            }

            if (PossibleValues.Count() == 1)
            {
                var foundValue = PossibleValues.Single();
                AddMethod(foundValue, Method.CreateOnlyValue(foundValue, this));
            }

            mEntities.ForEach(e => e.EliminatePossibility(pValue, this));
        }

        public void AddMethod(char pValue, Method pMethod)
        {
            var value = mValues[pValue];

            if(Filled || pMethod.Rank > value.MethodRank)
            {
                return;
            }

            if (pMethod.Rank > value.MethodRank)
            {
                value.Methods.Clear();
            }

            value.Methods.Add(pMethod);
        }

        public int Rank(char pValue)
        {
            return mValues[pValue].TechRank;
        }

        public IEnumerable<ITechnique> Techniques(char pValue, bool pMinRankOnly = true)
        {
            var value = mValues[pValue];
            return value.Techniques.Where(t => !pMinRankOnly || t.Rank == value.TechRank);
        }

        public bool HasFoundValue()
        {
           return mValues.Where(p => p.Value.Methods.Any()).Any();
        }

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
