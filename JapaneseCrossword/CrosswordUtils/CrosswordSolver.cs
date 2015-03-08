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

        public SolutionStatus Solve()
        {
            return SolutionStatus.Solved;
        }
    }
}
