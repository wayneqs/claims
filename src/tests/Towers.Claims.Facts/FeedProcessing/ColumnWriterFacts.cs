using System;
using System.IO;
using Towers.Claims.FeedProcessing;
using Xunit;

namespace Towers.Claims.Facts.FeedProcessing
{
    public class ColumnWriterFacts
    {
        private readonly StreamReader _reader;
        private MemoryStream _memoryStream;

        public ColumnWriterFacts()
        {
            var sut = BuildSUT();
            sut.Write();
            sut.Write();
            sut.Write();

            // reset the memory stream because its data pointer is on the last byte written.
            _memoryStream.Position = 0;
            // now we can read it back again and check what was written to it
            _reader = new StreamReader(_memoryStream);
        }
        
        [Fact]
        public void WritingShouldPullDataFromTheReaderAndPushToOutputStream()
        {
            Assert.Equal("header", _reader.ReadLine());
            Assert.Equal("3", _reader.ReadLine());
            Assert.Equal("2", _reader.ReadLine());
            Assert.Equal("1", _reader.ReadLine());
        }

        private ColumnWriter<DataSource> BuildSUT()
        {
            var items = new[]
                            {
                                new DataSource{ Value = 3 }, new DataSource{ Value = 2}, new DataSource{ Value = 1}
                            };
            // and we need to fake a reader...
            int next = 0;
            Func<DataSource> readerFunc = () => next < items.Length ? items[next++] : null;
            Func<DataSource, string[]> converter = ds => new[] {ds.Value.ToString()};

            _memoryStream = new MemoryStream();

            // stream writer will push the read and transformed data into the memory stream
            var writer = new StreamWriter(_memoryStream);

            // return SUT
            return new ColumnWriter<DataSource>(readerFunc, converter, "header", writer, '|');
        }
    }

    public class DataSource
    {
        public int Value;
    }
}
