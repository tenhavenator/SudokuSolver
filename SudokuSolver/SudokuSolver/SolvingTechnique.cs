/// <summary>
/// This file contains the calles used to represent solving techniques
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{

    public interface SolvingTechnique
    {}

    public class SquareOccupiedTechnique : SolvingTechnique
    {}

    public class GivenValueTechnique : SolvingTechnique
    {}

    public class EntityFoundValueTechnique : SolvingTechnique
    {}

    public class BoxFoundValueTechnique : EntityFoundValueTechnique
    {}

    public class RowFoundValueTechnique : EntityFoundValueTechnique
    {}

    public class ColumnFoundValueTechnique : EntityFoundValueTechnique
    {}

    public class OnlyPossibleValueTechnique : SolvingTechnique
    {}

    public class PossibleValueOverlapTechnique : SolvingTechnique
    {}

    public class PossibleValueClosureTechnique : SolvingTechnique
    {}

    public abstract class PossibleValueRowColumnShadow : SolvingTechnique
    {}

    public class PossibleValueRowShadow : PossibleValueRowColumnShadow
    { }

    public class PossibleValueColumnShadow : PossibleValueRowColumnShadow
    {}
}
