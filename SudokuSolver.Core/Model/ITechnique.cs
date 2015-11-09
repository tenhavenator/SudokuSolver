using System.Collections.Generic;

namespace SudokuSolver.Core.Model
{
    /// <summary>
    /// The interface that represents a technique used to rule out a possible value from a square. The basic techniques derived
    /// from the rules of sudoku are:
    /// 1). Occupiued - A value cannot go in a square because there is already a value in the square
    /// 2). Box - A value cannot go in a square because a square is the same box already has the value.
    /// 3). Row - A value cannot go in a square because a square is the same row already has the value.
    /// 4). Column - A value cannot go in a square because a square is the same column already has the value.
    /// 
    /// More advanced techniques will be needed to solve harder sudokus
    /// </summary>
    public interface ITechnique
    {
        /// <summary>
        /// The rank of the technique. The rank will increase based on the complexity of the technique and will be used to 
        /// compute the minimum set of technqiues needed to find a value.
        /// </summary>
        int Rank { get; }

        /// <summary>
        /// The name of the technique
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the technique
        /// </summary>
        string Desc { get; }

        /// <summary>
        /// The indexes of the squares in the technique (e.g. For a box technique this would be the indexes of the squares in 
        /// the box).
        /// </summary>
        IEnumerable<int> Indexes { get; }

        /// <summary>
        /// The values in the technique and indexes of the squares where they go (e.g. For a box technique there would be one 
        /// value in one square). This mapping is mostly used to represent techniques that are more complex than the basic 
        /// ones.
        /// </summary>
        IDictionary<char, IEnumerable<int>> ValueMap { get; }
    }
}
