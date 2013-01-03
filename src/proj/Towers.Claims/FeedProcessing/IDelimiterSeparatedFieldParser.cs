namespace Towers.Claims.FeedProcessing
{
    public interface IDelimiterSeparatedFieldParser
    {
        string[] Parse(string line);
    }
}