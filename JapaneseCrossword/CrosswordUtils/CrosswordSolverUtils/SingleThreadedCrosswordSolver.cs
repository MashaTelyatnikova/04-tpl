using System.Collections.Generic;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils
{
    public class SingleThreadedCrosswordSolver : CrosswordSolver
    {
        protected override bool UpdateLines(List<bool> linesForUpdating, LineType type)
        {
            var linesUpdated = false;
            for (var lineNumber = 0; lineNumber < linesForUpdating.Count; ++lineNumber)
            {
                if (linesForUpdating[lineNumber])
                {
                    linesUpdated = true;
                    linesForUpdating[lineNumber] = false;
                    UpdateLine(lineNumber, type);
                }
            }

            return linesUpdated;
        }
    }
}
