using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class GreatAndEqualThan : ValidationAttribute
    {
        private readonly decimal _minValue;

        public GreatAndEqualThan(string minValue)
        {
            if (decimal.TryParse(minValue, out var digit))
                _minValue = digit;
            throw new Exception("Invalid input number");
        }
        public override bool IsValid(object value)
        {
            return (decimal) value >= _minValue;
        }
    }
}
