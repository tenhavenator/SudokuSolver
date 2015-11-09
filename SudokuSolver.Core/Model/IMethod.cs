using System.Collections.Generic;

namespace SudokuSolver.Core.Model
{
    /// <summary>
    /// Interface that represents a method used to find a value in the sudoku. There are three types of methods:
    /// 1). Given Value -  A value was part of the initial sudoku.
    /// 2). Entity Value - A value was found by eliminating all other possible locations in a Box, Row, or Column. A value can
    ///     be found using more than one method of this type (e.g. A value might be found in the same square by a Box and Row 
    ///     at the same time).
    /// 3). Only Possible Value - A value was found by eliminating all other possible values for a particular square.
    /// </summary>
    public interface IMethod
    {
        /// <summary>
        /// The name of the method.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the method.
        /// </summary>
        string Desc { get; }

        /// <summary>
        /// The indexes of the sqaures in the method. For the Entity method this is the squares in the Box, Row, or Column. For
        /// the other methods this is the square where the value was found.
        /// </summary>
        IEnumerable<int> Indexes { get; }

        /// <summary>
        /// The minimum set of techniques used to generate the method
        /// </summary>
        IEnumerable<ITechnique> Techniques { get; }
    }
}
