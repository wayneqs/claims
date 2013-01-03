using Xunit;

namespace Towers.Claims.Facts
{
    public class OriginBlockFacts
    {
        [Fact]
        public void ShouldRecordMaximumDevelopmentYear()
        {
            var block = new OriginBlock(1990, new ErrorCollector());
            block[1994] = 3;
            block[1992] = 3;
            Assert.Equal(1994, block.MaxDevelopmentYear);
        }

        [Fact]
        public void ShouldNotRecordPaymentsIfDevelopmentYearIsBeforeOriginYear()
        {
            var collector = new ErrorCollector();
            var block = new OriginBlock(1990, collector);
            block[1989] = 3;
            Assert.Equal(0, block[1989]);
        }

        [Fact]
        public void ShouldRecordErrorIfDevelopmentYearIsBeforeOriginYear()
        {
            var collector = new ErrorCollector();
            Assert.False(collector.ContainsErrors);

            var block = new OriginBlock(1990, collector);
            block[1989] = 3;

            Assert.True(collector.ContainsErrors);
        }

    }
}
