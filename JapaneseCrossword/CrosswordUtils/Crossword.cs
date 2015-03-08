using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils;

namespace JapaneseCrossword.CrosswordUtils
{
    public class Crossword
    {
        private CrosswordTemplate crosswordTemplate;
        public Crossword(CrosswordTemplate crosswordTemplate)
        {
            this.crosswordTemplate = crosswordTemplate;
        }

        public SolutionStatus Solve()
        {
            return SolutionStatus.Solved;
        }
    }
}
