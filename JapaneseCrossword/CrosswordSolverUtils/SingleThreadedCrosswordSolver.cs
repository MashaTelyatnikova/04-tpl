using System;
using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public class SingleThreadedCrosswordSolver : MyCrosswordSolver
    {
        protected override bool UpdateLines(LineType type)
        {
            var updatesResults = new List<Tuple<int, List<CrosswordCell>>>();
            var linesForUpdating = LinesForUpdatingAtType[type];

            for (var i = 0; i < linesForUpdating.Count; ++i)
            {
                if (linesForUpdating[i])
                {
                    linesForUpdating[i] = false;

                    var rowCells = GetLineCells(type, i);
                    var line = GetCrosswordLine(type, i);
                    var updater = new CrosswordLineCellsUpdater(rowCells, line);

                    updatesResults.Add(Tuple.Create(i, updater.GetUpdatedCells()));
                }
            }

            if (updatesResults.Any(result => result.Item2 == null))
                return false;

            foreach (var result in updatesResults)
            {
                UpdateLine(type, result.Item1, result.Item2);
            }

            return true;
        }
    }
}
