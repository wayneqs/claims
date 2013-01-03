using Moq;
using Towers.Claims.FeedDefinitions;
using Towers.Claims.FeedProcessing;
using Xunit;

namespace Towers.Claims.Facts
{
    public class TriangleBuilderFacts
    {
        public class WhenInitialisedWithReader
        {
            private readonly Mock<IReader<TriangleFeedFullDataExtract>> _mReader;

            private readonly TriangleBuilder _sut;
            public WhenInitialisedWithReader()
            {
                var records = new[]
                                     {
                                         new TriangleFeedFullDataExtract {ProductName = "Comp", OriginYear = 1990, DevelopmentYear = 1990, Value = 110},
                                         new TriangleFeedFullDataExtract {ProductName = "Comp", OriginYear = 1990, DevelopmentYear = 1991, Value = 220},
                                         new TriangleFeedFullDataExtract {ProductName = "Non-Comp", OriginYear = 1990, DevelopmentYear = 1990, Value = 45.2},
                                         new TriangleFeedFullDataExtract {ProductName = "Non-Comp", OriginYear = 1990, DevelopmentYear = 1991, Value = 90}
                                     };

                _mReader = new Mock<IReader<TriangleFeedFullDataExtract>>();
                int readerNext = 0;
                _mReader.Setup(reader => reader.Read()).Returns(() => readerNext < records.Length ? records[readerNext] : null).Callback(() => readerNext++);
                _mReader.Setup(reader => reader.Peek()).Returns(() => readerNext < records.Length ? records[readerNext] : null);
                _sut = BuildSUT();
            }

            [Fact]
            public void ShouldBuildTriangles()
            {
                Assert.NotNull(_sut.BuildNext());
                Assert.NotNull(_sut.BuildNext());
                Assert.Null(_sut.BuildNext());
            }

            [Fact]
            public void ShouldReadTriangleRecords()
            {
                var triangle1 = _sut.BuildNext();
                Assert.Equal("Comp", triangle1.ProductName);
                Assert.Equal(110, triangle1[1990][1990]);
                Assert.Equal(220, triangle1[1990][1991]);

                var triangle2 = _sut.BuildNext();
                Assert.Equal("Non-Comp", triangle2.ProductName);
                Assert.Equal(45.2, triangle2[1990][1990]);
                Assert.Equal(90, triangle2[1990][1991]);
            }

            private TriangleBuilder BuildSUT()
            {
                // return SUT
                return new TriangleBuilder(_mReader.Object, new ErrorCollector());
            }
        }
    }
}
