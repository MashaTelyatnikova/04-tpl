using System.Collections.Generic;
using JapaneseCrossword.CrosswordUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils
{
    public class SingleThreadedCrosswordSolver : CrosswordSolver
    {
        protected override bool UpdateLines(List<bool> linesForUpdating, CrosswordLineType type)
        {
            var linesUpdated = false;
            for (var lineNumber = 0; lineNumber < linesForUpdating.Count; ++lineNumber)
            {
                if (linesForUpdating[lineNumber])
                {
                    linesUpdated = true;
                    linesForUpdating[lineNumber] = false;
                    UpdateLine(Crossword.GetLine(lineNumber, type));
                }
            }

            return linesUpdated;
        }
    }
}
