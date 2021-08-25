using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    /// <summary>
    /// Validate the string for digital only value
    /// </summary>
    public class OnlyDigitalStringFormat : ValidationAttribute
    {
        private readonly string _fieldName;

        public OnlyDigitalStringFormat(string fieldName = null)
        {
            _fieldName = fieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var data = (string) value;

            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }
            else
            {
                if (Regex.IsMatch(data, @"\D"))
                {
                    return new ValidationResult($"The field {validationContext.MemberName} should contain only digital characters.");
                }

                return ValidationResult.Success;
            }
        }
    }
}
