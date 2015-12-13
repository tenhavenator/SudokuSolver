using System.Collections.Generic;

namespace SudokuSolver.Core.Techniques
{
    using Model;

    /// <summary>
    /// The concrete implementation of <see cref="ITechnique"/> The basic techniques derived from the rules of sudoku are:
    /// 1). Occupiued - A value cannot go in a square because there is already a value in the square
    /// 2). Box - A value cannot go in a square because a square is the same box already has the value.
    /// 3). Row - A value cannot go in a square because a square is the same row already has the value.
    /// 4). Column - A value cannot go in a square because a square is the same column already has the value.
    /// 
    /// More advanced techniques will be needed to solve harder sudokus
    /// </summary>
    internal class Technique : ITechnique
    {
        /// <summary>
        /// The highest rank of technique derived from basic sudoku rules.
        /// </summary>
        public const int BASIC_TECH_RANK = 2;

        /// <summary>
        /// The rank of the technique. The rank will increase based on the complexity of the technique and will be used to 
        /// compute the minimum set of technqiues needed to find a value.
        /// </summary>
        public int Rank { get; private set; }

        /// <summary>
        /// The name of the technique
        /// </summary>
        public string Name { get; private set;  }

        /// <summary>
        /// The description of the technique
        /// </summary>
        public string Desc { get; private set; }

        /// <summary>
        /// The indexes of the squares in the technique (e.g. For a box technique this would be the indexes of the squares in 
        /// the box).
        /// </summary>
        public IEnumerable<int> Indexes { get; private set; }

        /// <summary>
        /// The values in the technique and indexes of the squares where they go (e.g. For a box technique there would be one 
        /// value in one square). This mapping is mostly used to represent techniques that are more complex than the basic 
        /// ones.
        /// </summary>
        public IDictionary<char, IEnumerable<int>> ValueMap { get; private set; }

        private Technique()
        {
            // Private constructor ensures that only the technique can instantiate concrete instances of itself
        }

        /// <summary>
        /// Create a technique to represent the case where a value cannot go in a square because there is already
        /// a value in the square.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square.</param>
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

        /// <summary>
        /// Create a technique to represent the case where a value cannot go in a square because a square is the same box
        /// already has the value.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square.</param>
        /// <param name="pIndexes">The indexes of the squares in the box.</param>
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

        /// <summary>
        /// Create a technique to represent the case where a value cannot go in a square because a square is the same row
        /// already has the value.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square.</param>
        /// <param name="pIndexes">The indexes of the squares in the row.</param>
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

        /// <summary>
        /// Create a technique to represent the case where a value cannot go in a square because a square is the same column
        /// already has the value.
        /// </summary>
        /// <param name="pValue">The value.</param>
        /// <param name="pIndex">The index of the square.</param>
        /// <param name="pIndexes">The indexes of the squares in the column.</param>
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
