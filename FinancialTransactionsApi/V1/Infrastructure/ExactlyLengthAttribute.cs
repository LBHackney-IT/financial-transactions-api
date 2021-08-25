using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    /// <summary>
    /// Validate the string for the exactly length
    /// </summary>
    public class ExactlyLengthAttribute : ValidationAttribute
    {
        private readonly int _length;

        public ExactlyLengthAttribute(int length)
        {
            _length = length;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var data = value as string;

            if (data == null)
            {
                return null;
            }

            if (data.Length != _length)
            {
                return new ValidationResult($"The field {validationContext.MemberName} must be a string with a length exactly equals to {_length}.");
            }

            return ValidationResult.Success;
        }
    }
}
