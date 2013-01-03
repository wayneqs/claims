using System.Collections.Generic;

namespace Towers.Claims
{
    public class OriginBlock
    {
        private readonly int _originYear;
        private readonly ErrorCollector _collector;
        private readonly Dictionary<int, double> _blockPayments = new Dictionary<int, double>();

        public OriginBlock(int originYear, ErrorCollector collector)
        {
            _originYear = originYear;
            _collector = collector;
        }

        public double this[int developmentYear]
        {
            get
            {
                double result;
                _blockPayments.TryGetValue(developmentYear, out result);
                return result;
            }
            set
            {
                if (MaxDevelopmentYear == 0 || MaxDevelopmentYear < developmentYear)
                    MaxDevelopmentYear = developmentYear;

                if( developmentYear < _originYear )
                {
                    _collector.Add("Development year ({0}) cannot be before the origin year ({1})", developmentYear, _originYear);
                    return;
                }
                _blockPayments[developmentYear] = value;
            }
        }

        public int MaxDevelopmentYear { get; private set; }
    }
}