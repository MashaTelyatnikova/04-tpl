using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils.Interfaces
{
    public interface ICrosswordSolver
    {
        CrosswordSolution Solve(Crossword crossword);
    }
}