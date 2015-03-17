using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public class MultiThreadedCrosswordSolver : CrosswordSolver
    {
        protected override bool TryUpdateLines()
        {
            var tasks = GetTasks(CrosswordLineType.Row).Concat(GetTasks(CrosswordLineType.Column)).ToArray();

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(tasks);

            var taskSolutions = tasks.Select(task => task.Result).ToList();

            if (taskSolutions.Any(taskSolution => taskSolution.Cells == null))
                return false;
            
            foreach (var solution in taskSolutions.Where(solution => solution.LineType == CrosswordLineType.Row))
            {
                LinesForUpdatingAtType[solution.LineType][solution.LineNumber] = false;
                UpdateLine(solution.LineType, solution.LineNumber, solution.Cells);
            }

            foreach (var solution in taskSolutions.Where(solution => solution.LineType == CrosswordLineType.Column))
            {
                LinesForUpdatingAtType[solution.LineType][solution.LineNumber] = false;
                UpdateLine(solution.LineType, solution.LineNumber, solution.Cells);
            }

            return true;
        }

        private IEnumerable<Task<TaskSolution>> GetTasks(CrosswordLineType type)
        {
            var tasks = new List<Task<TaskSolution>>();
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
                        return new TaskSolution(updater.GetUpdatedCells(), localNumber, type);
                    }));

                }
            }

            return tasks;
        } 
    }
}
