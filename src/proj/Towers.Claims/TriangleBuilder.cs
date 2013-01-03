using System;
using System.Collections.Generic;
using Towers.Claims.FeedDefinitions;
using Towers.Claims.FeedProcessing;

namespace Towers.Claims
{
    public class TriangleBuilder
    {
        private readonly IReader<TriangleFeedFullDataExtract> _reader;
        private readonly ErrorCollector _collector;

        public TriangleBuilder(IReader<TriangleFeedFullDataExtract> reader, ErrorCollector collector)
        {
            _reader = reader;
            _collector = collector;
        }

        public ClaimTriangle BuildNext()
        {
            var buffer = new List<TriangleFeedFullDataExtract>();
            while (true)
            {
                var record = _reader.Read();

                // explaining lamdas; to help document what is happening
                Func<bool> onLastTriangle = () => _reader.Peek() == null;
                Func<bool> newTriangleFound = () => _reader.Peek().ProductName != record.ProductName;
                Func<bool> movedPastLastTriangle = () => _reader.Peek() == null && record == null;

                if (movedPastLastTriangle()) return null;

                buffer.Add(record);

                if (onLastTriangle() || newTriangleFound())
                {
                    return ConvertIt(record.ProductName, buffer);
                }
            }
        }

        private ClaimTriangle ConvertIt(string productName, IEnumerable<TriangleFeedFullDataExtract> triangleRecords)
        {
            var triangle = new ClaimTriangle(productName, _collector);
            foreach (var paymentRecord in triangleRecords)
            {
                triangle[paymentRecord.OriginYear][paymentRecord.DevelopmentYear] = paymentRecord.Value;
            }
            return triangle;
        }
    }

}
