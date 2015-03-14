using System;
using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public class SingleThreadedCrosswordSolver : CrosswordSolver
    {
        protected override bool UpdateLines(CrosswordLineType type)
        {
            var linesForUpdating = LinesForUpdatingAtType[type];

            for (var i = 0; i < linesForUpdating.Count; ++i)
            {
                if (linesForUpdating[i])
                {
                    linesForUpdating[i] = false;

                    var rowCells = GetLineCells(type, i);
                    var line = GetCrosswordLine(type, i);
                    var updater = new CrosswordLineCellsUpdater(rowCells, line);
                    var updatedCells = updater.GetUpdatedCells();
                    
                    if (updatedCells == null)
                        return false;
                    
                    UpdateLine(type, i, updatedCells);
                }
            }
            return true;
        }
    }
}
