using System.Collections.Generic;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public class TaskSolution
    {
        public List<CrosswordCell> Cells { get; private set; }
        public int LineNumber { get; private set; }
        public CrosswordLineType LineType { get; private set; }

        public TaskSolution(List<CrosswordCell> cells, int lineNumber, CrosswordLineType lineType)
        {
            Cells = cells;
            LineNumber = lineNumber;
            LineType = lineType;
        }

    }
}
