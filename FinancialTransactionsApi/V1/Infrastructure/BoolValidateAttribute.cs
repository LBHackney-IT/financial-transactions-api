using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class BoolValidateAttribute : ValidationAttribute
    {
        private readonly bool _permissibleValue;

        public BoolValidateAttribute(bool permissibleValue)
        {
            _permissibleValue = permissibleValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"The field {validationContext.MemberName} cannot be null");
            }

            if (!((bool) value == _permissibleValue))
            {
                return new ValidationResult($"The field {validationContext.MemberName} must be {_permissibleValue}.");
            }

            return ValidationResult.Success;
        }
    }
}
