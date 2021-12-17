using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class RequiredDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is DateTime))
            {
                return new ValidationResult($"The field {validationContext.MemberName} is required.");
            }

            if ((DateTime) value == DateTime.MinValue)
            {
                return new ValidationResult($"The field {validationContext.MemberName} cannot be default value.");
            }

            return ValidationResult.Success;
        }
    }
}
