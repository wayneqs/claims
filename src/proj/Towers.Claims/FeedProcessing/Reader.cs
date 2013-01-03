using Towers.Claims.Mapping;
using Towers.Claims.Util;

namespace Towers.Claims.FeedProcessing
{
    public class Reader<T> : IReader<T> where T : class, new()
    {
        private readonly IColumnFileReader _reader;
        private readonly ErrorCollector _collector;
        private int _skipCount;

        public Reader(IColumnFileReader reader, ErrorCollector collector)
        {
            _reader = reader;
            _collector = collector;
            ProcessGenericTypeInformation();

            // skip over the specified number of lines
            // so reading is good to go
            for (int i = 0; i < _skipCount; i++)
            {
                _reader.Read();
            }
        }

        public T Read()
        {
            if( _peekBuffer != null )
            {
                var copy = _peekBuffer;
                _peekBuffer = null;
                return copy;
            }

            using(var tentativeCollector = _collector.TentativelyAdd("Line {0}:", _reader.CurrentLineNumber+1) )
            {
                var actualFields = _reader.Read();

                if (!tentativeCollector.ContainsErrors) 
                    // no errors, so we can roll this tentative collector back
                    tentativeCollector.Rollback();

                return new Mapper<T>(_collector).Map(actualFields);
            }
        }

        private T _peekBuffer;
        public T Peek()
        {
            if( _peekBuffer != null ) return _peekBuffer;
            _peekBuffer = Read();
            return _peekBuffer;
        }

        private void ProcessGenericTypeInformation()
        {
            var type = typeof (T);
            var skipLinesAttribute = ReflectionHelper.GetAttribute<SkipLinesAttribute>(type);
            if (skipLinesAttribute != null)
                _skipCount = skipLinesAttribute.Count;
        }
    }
}
