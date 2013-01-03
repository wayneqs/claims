using System.Collections.Generic;
using System.Linq;

namespace Towers.Claims.FeedProcessing
{
    /// <summary>
    /// Simple parser for delimited lines
    /// </summary>
    public class DelimiterSeparatedFieldParser : IDelimiterSeparatedFieldParser
    {
        private readonly char _delimiter;

        public DelimiterSeparatedFieldParser(char delimiter)
        {
            _delimiter = delimiter;
        }

        public string[] Parse(string line)
        {
            if( string.IsNullOrWhiteSpace(line) ) return new string[0];

            if( line.Any( c => c == '"') )
            {
                // line contains quotes so we need to handle them with care...

                var fields = new List<string>();
                bool openQuote = false;
                int startPosition = 0;
                for (int position = 0; position < line.Length; position++)
                {
                    openQuote ^= line[position] == '"';
                    if (openQuote )
                    {
                        continue;
                    }
                    var isLastChar = position == line.Length - 1;
                    if(line[position] == _delimiter || isLastChar)
                    {
                        // read up to but not including the position so the delimiter isn't read in; but on the last char we need to allow for the lack of delimiter
                        fields.Add(line.Substring(startPosition, isLastChar ? position+1 - startPosition : position - startPosition).Trim('"', ' ', '\t'));
                        startPosition = position + 1; // to hop past the delimiter
                    }
                }

                // if we have an unterminated string and we have already parsed some fields then just dump the rest of the line into a field
                if( fields.Any() && openQuote )
                {
                    fields.Add(line.Substring(startPosition));
                }

                // no fields parsed and we have an open quote, just dump the whole line into a field
                if( !fields.Any() && openQuote )
                {
                    fields.Add(line.Trim('"', ' ', '\t'));
                }

                return fields.ToArray();
            }

            // no quotes just split and trim...
            return line.Split(_delimiter).Select( f => f.Trim()).ToArray();
        }
    }
}
