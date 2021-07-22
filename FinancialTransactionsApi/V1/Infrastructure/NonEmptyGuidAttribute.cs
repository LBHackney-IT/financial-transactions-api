using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class NonEmptyGuidAttribute : ValidationAttribute
    {
        private readonly string _fieldName;

        public NonEmptyGuidAttribute(string fieldName = null)
        {
            _fieldName = fieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return (value is Guid guid) && Guid.Empty == guid ?
                new ValidationResult($"The field {_fieldName ?? validationContext.MemberName} cannot be empty or default.") : null;
        }
    }
}
