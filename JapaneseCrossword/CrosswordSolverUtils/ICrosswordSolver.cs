using JapaneseCrossword.CrosswordSolutionUtils;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public interface ICrosswordSolver
    {
        CrosswordSolution Solve(Crossword crossword);
    }
}