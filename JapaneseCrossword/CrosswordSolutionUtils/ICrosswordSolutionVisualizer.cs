namespace JapaneseCrossword.CrosswordSolutionUtils
{
    public interface ICrosswordSolutionVisualizer<out T>
    {
        T Visualize (CrosswordSolution crosswordSolution);
    }
}
