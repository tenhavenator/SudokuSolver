using System.Collections.Generic;

namespace SudokuSolver.Core.Techniques
{
    using Model;

    /// <summary>
    /// The concrete implementation of <see cref="IFoundValue"/>
    /// </summary>
    internal class FoundValue : IFoundValue
    {
        /// <summary>
        /// The value found (1-9).
        /// </summary>
        public char Value { get; set; }

        /// <summary>
        /// The index of the square where the value was found (0-80).
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The methods that can be used to find the value.
        /// </summary>
        public IEnumerable<IMethod> Methods { get; set; }

        /// <summary>
        /// The rank of the methods used to find the value.
        /// </summary>
        public int Rank { get; set; }
    }
}
