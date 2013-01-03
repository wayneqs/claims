namespace Towers.Claims.FeedProcessing
{
    public interface IColumnFileReader
    {
        string[] Read();
        int CurrentLineNumber { get; }
    }
}