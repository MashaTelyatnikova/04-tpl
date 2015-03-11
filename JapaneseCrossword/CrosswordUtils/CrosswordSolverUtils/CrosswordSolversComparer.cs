using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilderUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils.Interfaces;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils
{
    public static class CrosswordSolversComparer
    {
        private const string TestsDirectory = @"Tests";
        private static readonly Font ChartFont = new Font(new FontFamily("Arial"), 10);
        private const Docking TitlesPlace = Docking.Bottom;
    
        public static Chart Compare(ICrosswordSolver [] crosswordSolvers, Color[] colors)
        {
            var series = crosswordSolvers.Select((solver, i) => GetSeriesAtSolver(solver, colors[i])).ToList();
            
            var resultChart = new Chart();
            resultChart.ChartAreas.Add(new ChartArea());
            resultChart.Dock = DockStyle.Fill;
            
            for (var i = 0; i < crosswordSolvers.Length; ++i)
            {
                resultChart.Titles.Add(new Title(crosswordSolvers[i].Name, TitlesPlace, ChartFont, colors[i]));

                resultChart.Series.Add(series[i]);
            }
            
            return resultChart;
        }

        private static Series GetSeriesAtSolver(ICrosswordSolver solver, Color color)
        {
            var crosswordBuilder = new CrosswordBuilder();
            var crosswords = Directory.GetFiles(TestsDirectory).Select(file => crosswordBuilder.BuildFromFile(file)).ToList();
            var resultSeries = new Series {ChartType = SeriesChartType.FastLine, Color = color};
            
            GC.Collect();
            
            foreach (var crossword in crosswords)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                
                solver.Solve(crossword);
                
                stopwatch.Stop();
                
                resultSeries.Points.Add(new DataPoint(crossword.Width*crossword.Height, stopwatch.ElapsedMilliseconds));
            }

            return resultSeries;
        }
    }
}
