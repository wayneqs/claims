using System;
using Towers.Claims.FeedDefinitions;
using Xunit;

namespace Towers.Claims.Facts
{
    public class LargestTriangleCalculatorFacts
    {
        public class WhenInitialisedWithDimensions
        {
            private readonly LargestTriangleCalculator _sut;
            public WhenInitialisedWithDimensions()
            {
                _sut = BuildSUT();
            }

            [Fact]
            public void ShouldCalculateMinimumOriginYear()
            {
                var largest = _sut.Calculate();
                Assert.Equal(1990, largest.OriginYear);
            }

            [Fact]
            public void ShouldCalculateGapBetweenMinimumOriginYearAndMaximumDevelopmentYear()
            {
                var largest = _sut.Calculate();
                Assert.Equal(4, largest.DevelopmentYears);
            }

            private LargestTriangleCalculator BuildSUT()
            {
                var records = new[]
                                     {
                                         new TriangleFeedYearExtract {OriginYear = 1992, DevelopmentYear = 1992},
                                         new TriangleFeedYearExtract {OriginYear = 1992, DevelopmentYear = 1993},
                                         new TriangleFeedYearExtract {OriginYear = 1993, DevelopmentYear = 1993},
                                         new TriangleFeedYearExtract {OriginYear = 1990, DevelopmentYear = 1990},
                                         new TriangleFeedYearExtract {OriginYear = 1990, DevelopmentYear = 1991},
                                         new TriangleFeedYearExtract {OriginYear = 1990, DevelopmentYear = 1993},
                                         new TriangleFeedYearExtract {OriginYear = 1991, DevelopmentYear = 1991},
                                         new TriangleFeedYearExtract {OriginYear = 1991, DevelopmentYear = 1992},
                                         new TriangleFeedYearExtract {OriginYear = 1991, DevelopmentYear = 1993},
                                         new TriangleFeedYearExtract {OriginYear = 1992, DevelopmentYear = 1992},
                                         new TriangleFeedYearExtract {OriginYear = 1992, DevelopmentYear = 1993},
                                         new TriangleFeedYearExtract {OriginYear = 1993, DevelopmentYear = 1993}
                                     };
                // and we need to fake a reader...
                int next = 0;
                Func<TriangleFeedYearExtract> readerFunc = () => next < records.Length ? records[next++] : null;

                // return SUT
                return new LargestTriangleCalculator(readerFunc);
            }
        }
    }
}