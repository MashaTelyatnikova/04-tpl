using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils.Interfaces
{
    public interface ICrosswordSolver
    {
        CrosswordSolution Solve(CrosswordTemplate crosswordTemplateParam);
    }
}