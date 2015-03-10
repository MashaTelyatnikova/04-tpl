using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JapaneseCrossword.CrosswordUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils
{
    public class MultiThreadedCrosswordSolver : CrosswordSolver
    {
        protected override bool UpdateLines(List<bool> linesForUpdating, CrosswordLineType type)
        {
            try
            {
                var tasks = new List<Task>();
                var linesUpdated = false;

                for (var lineNumber = 0; lineNumber < linesForUpdating.Count; ++lineNumber)
                {
                    if (linesForUpdating[lineNumber])
                    {
                        linesUpdated = true;
                        linesForUpdating[lineNumber] = false;
                        var number = lineNumber;
                        tasks.Add(Task.Run(() => UpdateLine(Crossword.GetLine(number, type))));
                    }
                }

                Task.WaitAll(tasks.ToArray());
                return linesUpdated;
            }
            catch (AggregateException)
            {
                throw new ArgumentException("Incorrect Crossword");
            }
        }
    }
}
