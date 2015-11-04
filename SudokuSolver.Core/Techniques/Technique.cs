using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Core.Model;

namespace SudokuSolver.Core.Techniques
{
    internal class Technique : ITechnique
    {
        public const int BASIC_TECH_RANK = 2;

        public int Rank { get; private set; }

        public string Name { get; private set;  }

        public string Desc { get; private set; }

        public IEnumerable<int> Indexes { get; private set; }

        public IDictionary<char, IEnumerable<int>> ValueMap { get; private set; }

        private Technique()
        { 
        
        }

        public static ITechnique CreateOccupiedTechnique(char pValue, int pIndex)
        {
            return new Technique()
            {
                Rank = 0,
                Name = "Occupied Technique",
                Desc = "Nothing can go in this square. There is already a " + pValue + " here.",
                Indexes = new List<int>() { pIndex },
                ValueMap = new Dictionary<char, IEnumerable<int>>() { { pValue, new List<int>() { pIndex } } },
            };
        }

        public static ITechnique CreateBoxTechnique(char pValue, int pIndex, IEnumerable<int> pIndexes)
        {
            return new Technique()
            {
                Rank = 1,
                Name = "Box Elimination Technique",
                Desc = "There is already a " + pValue + " in this box.",
                Indexes = pIndexes,
                ValueMap = new Dictionary<char, IEnumerable<int>>() { { pValue, new List<int>() { pIndex } } },
            };
        }

        public static ITechnique CreateRowTechnique(char pValue, int pIndex, IEnumerable<int> pIndexes)
        {
            return new Technique()
            {
                Rank = 2,
                Name = "Row Elimination Technique",
                Desc = "There is already a " + pValue + " in this row.",
                Indexes = pIndexes,
                ValueMap = new Dictionary<char, IEnumerable<int>>() { { pValue, new List<int>() { pIndex } } },
            };
        }

        public static ITechnique CreateColTechnique(char pValue, int pIndex, IEnumerable<int> pIndexes)
        {
            return new Technique()
            {
                Rank = 2,
                Name = "Column Elimination Technique",
                Desc = "There is already a " + pValue + " in this column.",
                Indexes = pIndexes,
                ValueMap = new Dictionary<char, IEnumerable<int>>() { { pValue, new List<int>() { pIndex } } },
            };
        }
    }
}
