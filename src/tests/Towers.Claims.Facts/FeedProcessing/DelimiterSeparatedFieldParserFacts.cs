using Towers.Claims.FeedProcessing;
using Xunit;

namespace Towers.Claims.Facts.FeedProcessing
{
    public class DelimiterSeparatedFieldParserFacts
    {
        [Fact]
        public void ItShouldParseEmpty()
        {
            var parser = new DelimiterSeparatedFieldParser(',');
            Assert.Empty(parser.Parse(""));
        }

        [Fact]
        public void ItShouldParseWhiteSpace()
        {
            var parser = new DelimiterSeparatedFieldParser(',');
            Assert.Empty(parser.Parse("    \t     "));
        }

        [Fact]
        public void ItShouldParseNull()
        {
            var parser = new DelimiterSeparatedFieldParser(',');
            Assert.Empty(parser.Parse(null));
        }

        public class WhenInputDoesNotContainQuotes
        {
            [Fact]
            public void ItShouldParse()
            {
                var expectedValues = new[] {"hello-world", "Garr", "345"};

                var parser = new DelimiterSeparatedFieldParser(',');

                var actualValues = parser.Parse("hello-world,Garr,345");

                for (int i = 0; i < expectedValues.Length; i++)
                {
                    Assert.Equal(expectedValues[i], actualValues[i]);
                }
            }

            [Fact]
            public void ItShouldCleanEmptySpace()
            {
                var expectedValues = new[] { "hello-world", "Garr" };

                var parser = new DelimiterSeparatedFieldParser(',');

                var actualValues = parser.Parse("       hello-world     ,       Garr       ");

                for (int i = 0; i < expectedValues.Length; i++)
                {
                    Assert.Equal(expectedValues[i], actualValues[i]);
                }
            }
        }

        public class WhenInputContainsQuotes
        {
            [Fact]
            public void ItShouldParse()
            {
                var expectedValues = new[] {"hello, world", "Garr", "345","123"};

                var parser = new DelimiterSeparatedFieldParser(',');

                var actualValues = parser.Parse("\"hello, world\",\"Garr\",345,123");

                for (int i = 0; i < expectedValues.Length; i++)
                {
                    Assert.Equal(expectedValues[i], actualValues[i]);
                }
            }


            [Fact]
            public void ItShouldCleanEmptySpace()
            {
                var expectedValues = new[] { "hello, world", "Garr" };

                var parser = new DelimiterSeparatedFieldParser(',');

                var actualValues = parser.Parse("       \"hello, world     \",       Garr       ");

                for (int i = 0; i < expectedValues.Length; i++)
                {
                    Assert.Equal(expectedValues[i], actualValues[i]);
                }
            }

            public class WhenQuotesAreUnterminated
            {
                [Fact]
                public void ItShouldDumpIntoOneFieldWhenQuoteIsTheBeginningOfTheLine()
                {
                    var expectedValues = new[] { "hello, world,123" };

                    var parser = new DelimiterSeparatedFieldParser(',');

                    var actualValues = parser.Parse("\"hello, world,123");

                    for (int i = 0; i < expectedValues.Length; i++)
                    {
                        Assert.Equal(expectedValues[i], actualValues[i]);
                    }
                }

                [Fact]
                public void ItShouldDumpIntoLastFieldWhenQuoteIsInTheMiddleOfTheLine()
                {
                    var expectedValues = new[] { "hello-world", "123\",14" };

                    var parser = new DelimiterSeparatedFieldParser(',');

                    var actualValues = parser.Parse("hello-world,123\",14");

                    for (int i = 0; i < expectedValues.Length; i++)
                    {
                        Assert.Equal(expectedValues[i], actualValues[i]);
                    }
                }
            }
        }
    }
}