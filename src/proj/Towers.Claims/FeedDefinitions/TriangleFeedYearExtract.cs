using Towers.Claims.FeedProcessing;
using Towers.Claims.Mapping;

namespace Towers.Claims.FeedDefinitions
{
    [SkipLines(1)]
    public class TriangleFeedYearExtract
    {
        [FieldIndex(2)]
        public int DevelopmentYear;
        
        [FieldIndex(1)]
        public int OriginYear;
    }
}