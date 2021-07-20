using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class NonEmptyGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (value is Guid guid) && Guid.Empty == guid ?
                new ValidationResult($"The field {validationContext.MemberName} cannot be empty or default.") : null;
        }
    }
}
