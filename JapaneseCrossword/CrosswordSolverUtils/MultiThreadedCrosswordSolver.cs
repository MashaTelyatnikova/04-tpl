using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public class MultiThreadedCrosswordSolver : CrosswordSolver
    {
        protected override bool UpdateLines(CrosswordLineType type)
        {
            var tasks = new List<Task<Tuple<int, List<CrosswordCell>>>>();
            var linesForUpdating = LinesForUpdatingAtType[type];

            for (var number = 0; number < linesForUpdating.Count; ++number)
            {
                if (linesForUpdating[number])
                {
                    linesForUpdating[number] = false;
                    var localNumber = number;
                    tasks.Add(Task.Run(() =>
                    {
                        var rowCells = GetLineCells(type, localNumber);
                        var line = GetCrosswordLine(type, localNumber);
                        var updater = new CrosswordLineCellsUpdater(rowCells, line);
                        return Tuple.Create(localNumber, updater.GetUpdatedCells());
                    }));
                  
                }
            }

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(tasks.ToArray());

            var updatesResults = tasks.Select(task => task.Result).ToList();

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
