using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    /// <summary>
    /// Validate the string for digital only value
    /// </summary>
    public class OnlyDigitalStringFormat : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var data = value as string;

            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            if (Regex.IsMatch(data, @"\D"))
            {
                return new ValidationResult($"The field {validationContext.MemberName} should contain only digital characters.");
            }

            return ValidationResult.Success;
        }
    }
}
