using JapaneseCrossword.CrosswordUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils
{
    public class SingleThreadedCrosswordSolver : CrosswordSolver
    {
        public override string Name
        {
            get { return "Single-Threaded Crossword Solver"; }
        }

        protected override bool UpdateLines(CrosswordLineType type)
        {
            var linesUpdated = false;
            for (var lineNumber = 0; lineNumber < LinesForUpdating[type].Count; ++lineNumber)
            {
                if (LinesForUpdating[type][lineNumber])
                {
                    linesUpdated = true;
                    LinesForUpdating[type][lineNumber] = false;
                    UpdateLine(Crossword.GetLine(lineNumber, type));
                }
            }

            return linesUpdated;
        }
    }
}
