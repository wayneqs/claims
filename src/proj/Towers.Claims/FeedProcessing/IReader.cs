namespace Towers.Claims.FeedProcessing
{
    public interface IReader<out T>
    {
        T Read();
        T Peek();
    }
}
