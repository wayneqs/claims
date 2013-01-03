using System.IO;

namespace Towers.Claims.FeedProcessing
{
    public class ColumnReader : IColumnFileReader
    {
        private readonly StreamReader _reader;
        private readonly IDelimiterSeparatedFieldParser _parser;

        public int CurrentLineNumber { get; private set; }

        public ColumnReader(StreamReader reader, IDelimiterSeparatedFieldParser parser)
        {
            _reader = reader;
            _parser = parser;
        }

        public string[] Read()
        {
            // simplifying assumption here is that we don't have to handle a multi-line column format feed
            var line = _reader.ReadLine();
            CurrentLineNumber++;

            // no data left to read
            if (line == null) return null;

            return _parser.Parse(line);
        }
    }
}