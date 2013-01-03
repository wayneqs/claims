using System.Collections.Generic;
using System.IO;
using System.Text;
using Moq;
using Towers.Claims.FeedProcessing;
using Xunit;

namespace Towers.Claims.Facts.FeedProcessing
{
    public class ColumnReaderFacts
    {
        private readonly ColumnReader _reader;
        private readonly List<string> _calls;

        public ColumnReaderFacts()
        {
            _calls = new List<string>();

            var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes("line1\nline2"));
            var mParser = new Mock<IDelimiterSeparatedFieldParser>();

            mParser.Setup(parser => parser.Parse(It.IsAny<string>())).Returns(new string[0]).Callback<string>(_calls.Add);

            _reader = new ColumnReader(new StreamReader(memoryStream), mParser.Object);
        }

        [Fact]
        public void ShouldReadLineAndDelegateParsing()
        {
            // read two lines
            _reader.Read();
            _reader.Read();

            Assert.Equal(2, _calls.Count);
            Assert.Equal("line1", _calls[0]);
            Assert.Equal("line2", _calls[1]);
        }

        [Fact]
        public void LineCountShouldBeZeroBeforeReading()
        {
            Assert.Equal(0, _reader.CurrentLineNumber);
        }

        [Fact]
        public void ShouldIncrementCurrentLineNumberAfterEachRead()
        {
            _reader.Read();
            Assert.Equal(1, _reader.CurrentLineNumber);
        }

        [Fact]
        public void ShouldReportNoFurtherLinesToBeRead()
        {
            Assert.NotNull(_reader.Read());
            Assert.NotNull(_reader.Read());

            Assert.Null(_reader.Read());
        }
    }
}
