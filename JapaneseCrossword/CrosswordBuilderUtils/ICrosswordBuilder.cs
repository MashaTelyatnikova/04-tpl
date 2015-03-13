namespace JapaneseCrossword.CrosswordBuilderUtils
{
    public interface ICrosswordBuilder
    {
        Crossword BuildFromFile(string file);
    }
}
