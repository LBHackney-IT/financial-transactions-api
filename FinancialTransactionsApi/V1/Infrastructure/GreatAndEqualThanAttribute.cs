using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class GreatAndEqualThanAttribute : ValidationAttribute
    {
        private readonly decimal? _minValue;

        public GreatAndEqualThanAttribute(string minValue)
        {
            if (decimal.TryParse(minValue, out decimal digit)) _minValue = digit;
        }
        public override bool IsValid(object value)
        {
            return (decimal) value >= _minValue;
        }
    }
}
