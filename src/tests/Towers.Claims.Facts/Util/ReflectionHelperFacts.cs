using Towers.Claims.FeedDefinitions;
using Towers.Claims.FeedProcessing;
using Towers.Claims.Util;
using Xunit;

namespace Towers.Claims.Facts.Util
{
    public class ReflectionHelperFacts
    {
        [Fact]
        public void ShouldGrabAttributeFromType()
        {
            var attribute = ReflectionHelper.GetAttribute<SkipLinesAttribute>(typeof(TriangleFeedYearExtract));
            Assert.NotNull(attribute);
        }

        [Fact]
        public void ShouldReportIfAttributeNotOnType()
        {
            var attribute = ReflectionHelper.GetAttribute<SkipLinesAttribute>(GetType());
            Assert.Null(attribute);
        }
    }
}