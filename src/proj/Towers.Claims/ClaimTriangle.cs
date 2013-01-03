using System.Collections.Generic;
using System.Globalization;

namespace Towers.Claims
{
    public class ClaimTriangle
    {
        private readonly Dictionary<int, OriginBlock> _policyBlocks = new Dictionary<int, OriginBlock>();
        private int _mostRecentPolicyBlockOrigin = -1;
        private int _oldestPolicyBlockOrigin = -1;

        public ErrorCollector ErrorManager { get; private set; }

        public string ProductName { get; private set; }

        public ClaimTriangle(string productName, ErrorCollector errorManager)
        {
            ErrorManager = errorManager;
            ProductName = productName;
        }
        
        public OriginBlock this[int originYear]
        {
            get
            {
                ErrorManager.Context.Clear();
                ErrorManager.Context.Add(string.Format("Product Name={0}; ", ProductName));

                if( _mostRecentPolicyBlockOrigin < originYear )
                    _mostRecentPolicyBlockOrigin = originYear;
                if( _oldestPolicyBlockOrigin > originYear || _oldestPolicyBlockOrigin == -1 )
                {
                    _oldestPolicyBlockOrigin = originYear;
                }

                if( !_policyBlocks.ContainsKey(originYear) )
                {
                    var blockPayment = new OriginBlock(originYear, ErrorManager);
                    _policyBlocks[originYear] = blockPayment;
                }
                return _policyBlocks[originYear];
            }
        }

        public string[] Flatten(TriangleDimensions dimensions)
        {
            var result = new List<string> { ProductName };

            var lastDevelopmentYear = dimensions.OriginYear + dimensions.DevelopmentYears - 1; // including the origin year
            for (int originYear = dimensions.OriginYear; originYear <= lastDevelopmentYear; originYear++)
            {
                for (int developmentYear = originYear; developmentYear <= lastDevelopmentYear; developmentYear++)
                {
                    result.Add(this[originYear][developmentYear].ToString(CultureInfo.InvariantCulture));
                }
            }
            return result.ToArray();
        }

        public ClaimTriangle Accumulate()
        {
            var triangle = new ClaimTriangle(ProductName, ErrorManager);

            for (int originYear = _oldestPolicyBlockOrigin; originYear <= _mostRecentPolicyBlockOrigin; originYear++)
            {
                var blockPayments = this[originYear];

                double previousValue = 0;
                for (int developmentYear = originYear; developmentYear <= blockPayments.MaxDevelopmentYear; developmentYear++)
                {
                    triangle[originYear][developmentYear] = blockPayments[developmentYear] + previousValue;
                    previousValue = triangle[originYear][developmentYear];
                }
            }
            return triangle;
        }
    }
}
