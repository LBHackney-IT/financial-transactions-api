using FinancialTransactionsApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class AllowedValuesAttribute : ValidationAttribute
    {
        private readonly List<TransactionType> _allowedEnumItems;

        public AllowedValuesAttribute(params TransactionType[] allowedEnumItems)
        {
            _allowedEnumItems = allowedEnumItems.ToList();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"The field {validationContext.MemberName} is required.");
            }

            var valueType = value.GetType();

            if (!valueType.IsEnum || !Enum.IsDefined(typeof(TransactionType), value))
            {
                return new ValidationResult($"The field {validationContext.MemberName} should be a type of TransactionType enum.");
            }

            var isValid = _allowedEnumItems.Contains((TransactionType) value);

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"The field {validationContext.MemberName} should be in a range: [{string.Join(", ", _allowedEnumItems.Select(a => $"{(int) a}({a})"))}].");
            }
        }
    }
}
