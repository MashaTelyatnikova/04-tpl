using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils;

namespace JapaneseCrossword.CrosswordUtils
{
    public class CrosswordSolver : ICrosswordSolver
    {
        private CrosswordTemplate crosswordTemplate;
        public CrosswordSolver(CrosswordTemplate crosswordTemplate)
        {
            this.crosswordTemplate = crosswordTemplate;
        }

        public CrosswordSolution Solve()
        {
            return null;
        }
    }
}
