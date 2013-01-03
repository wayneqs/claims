using Xunit;

namespace Towers.Claims.Facts
{
    public class ClaimTriangleFacts
    {
        readonly ErrorCollector _collector = new ErrorCollector();

        [Fact]
        public void ShouldHandleMissingIncrementalValuesFrontEndOfTriangle()
        {
            var expectedValues = new[] {"ProductA", 
                                            "0",  "0",  "0",   "0", 
                                                  "0",  "0",   "0", 
                                                        "110", "170", 
                                                               "200"};

            var triangle = new ClaimTriangle("ProductA", _collector);
            triangle[1992][1992] = 110;
            triangle[1992][1993] = 170;
            triangle[1993][1993] = 200;

            var actualValues = triangle.Flatten(new TriangleDimensions{OriginYear = 1990, DevelopmentYears = 4});

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], actualValues[i]);
            }
        }

        [Fact]
        public void ShouldHandleMissingIncrementalValuesMidTriangle()
        {
            var expectedValues = new[] {"ProductA", 
                                            "0",  "0",  "0",   "0",
                                                  "0",  "0",   "0", 
                                                        "110", "0", 
                                                               "200"};

            var triangle = new ClaimTriangle("ProductA", _collector);
            triangle[1992][1992] = 110;
            triangle[1993][1993] = 200;

            var actualValues = triangle.Flatten(new TriangleDimensions { OriginYear = 1990, DevelopmentYears = 4 });

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], actualValues[i]);
            }
        }

        [Fact]
        public void ShouldHandleMissingIncrementalValuesTailEndOfTriangle()
        {
            var expectedValues = new[] { "ProductA", 
                                            "0", "0", "0",   "0", 
                                                 "0", "0",   "0", 
                                                      "110", "170", 
                                                             "0" };

            var triangle = new ClaimTriangle("ProductA", _collector);
            triangle[1992][1992] = 110;
            triangle[1992][1993] = 170;

            var actualValues = triangle.Flatten(new TriangleDimensions { OriginYear = 1990, DevelopmentYears = 4 });

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], actualValues[i]);
            }
        }

        [Fact]
        public void ShouldAccumulateFullTriangle()
        {
            var expectedValues = new[] { "ProductA", 
                                            "45.2", "110", "110",  "147", 
                                                    "50",  "125",  "150", 
                                                           "55",   "140", 
                                                                   "100" };

            var triangle = new ClaimTriangle("ProductA", _collector);
            triangle[1990][1990] = 45.2;
            triangle[1990][1991] = 64.8;
            triangle[1990][1993] = 37;
            triangle[1991][1991] = 50;
            triangle[1991][1992] = 75;
            triangle[1991][1993] = 25;
            triangle[1992][1992] = 55;
            triangle[1992][1993] = 85;
            triangle[1993][1993] = 100;

            var actualValues = triangle.Accumulate().Flatten(new TriangleDimensions { OriginYear = 1990, DevelopmentYears = 4 });

            for (int i = 0; i < actualValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], actualValues[i]);
            }
        }

        [Fact]
        public void ShouldAccumulateForSingleOriginBlock()
        {
            var expectedValues = new[] { "ProductA", 
                                            "115", "213", "277", "330", "471", 
                                                   "0",   "0",   "0",   "0", 
                                                          "0",   "0",   "0", 
                                                                 "0",   "0", 
                                                                        "0"};

            var triangle = new ClaimTriangle("ProductA", _collector);
            triangle[1991][1991] = 115;
            triangle[1991][1992] = 98;
            triangle[1991][1993] = 64;
            triangle[1991][1994] = 53;
            triangle[1991][1995] = 141;

            var actualValues = triangle.Accumulate().Flatten(new TriangleDimensions { OriginYear = 1991, DevelopmentYears = 5 });

            for (int i = 0; i < actualValues.Length; i++)
            {
                Assert.Equal(expectedValues[i], actualValues[i]);
            }
        }
    }
}
