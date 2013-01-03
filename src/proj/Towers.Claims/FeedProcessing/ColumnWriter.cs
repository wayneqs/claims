using System;
using System.IO;
using System.Linq;

namespace Towers.Claims.FeedProcessing
{
    public class ColumnWriter<T> where T : class
    {
        private readonly Func<T> _readerFunc;
        private readonly Func<T, string[]> _converter;
        private readonly string _fileHeader;
        private readonly TextWriter _writer;
        private readonly char _delimiter;

        public ColumnWriter(Func<T> readerFunc, Func<T, string[]> converter, string fileHeader, TextWriter writer, char delimiter)
        {
            _readerFunc = readerFunc;
            _converter = converter;
            _fileHeader = fileHeader;
            _writer = writer;
            _delimiter = delimiter;
        }

        public void Write()
        {
            _writer.WriteLine(_fileHeader);
            T item;
            while((item = _readerFunc()) != null)
            {
                var columns = _converter(item);
                _writer.WriteLine(columns.Aggregate((previous, current) => string.Format("{0}{1} {2}", previous, _delimiter, current)));
                _writer.Flush();
            }
        }
    }
}