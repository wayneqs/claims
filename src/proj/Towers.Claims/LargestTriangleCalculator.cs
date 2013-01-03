using System;
using Towers.Claims.FeedDefinitions;

namespace Towers.Claims
{
    public class LargestTriangleCalculator
    {
        private readonly Func<TriangleFeedYearExtract> _readerFunc;
        private int _originYear = -1;
        private int _developmentYear;

        public LargestTriangleCalculator(Func<TriangleFeedYearExtract> readerFunc)
        {
            _readerFunc = readerFunc;
        }

        public TriangleDimensions Calculate()
        {
            TriangleFeedYearExtract extract;
            while ((extract = _readerFunc()) != null)
            {
                if (_originYear == -1 || extract.OriginYear < _originYear)
                    _originYear = extract.OriginYear;
                if (_developmentYear < extract.DevelopmentYear)
                    _developmentYear = extract.DevelopmentYear;
            }
            // return the dimensions; include the origin year in the development year
            return new TriangleDimensions { OriginYear = _originYear, DevelopmentYears = _developmentYear - (_originYear - 1) };
        }
    }
}
