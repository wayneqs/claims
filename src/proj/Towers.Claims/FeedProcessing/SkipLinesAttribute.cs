using System;

namespace Towers.Claims.FeedProcessing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SkipLinesAttribute : Attribute
    {
        public SkipLinesAttribute(int count)
        {
            Count = count;
        }

        public int Count;
    }
}
