using JapaneseCrossword.CrosswordUtils;

namespace JapaneseCrossword
{
    public interface ICrosswordSolver
    {
        SolutionStatus Solve(string inputFilePath, string outputFilePath);
    }
}