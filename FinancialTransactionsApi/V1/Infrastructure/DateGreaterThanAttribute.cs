using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {

        private string DateToCompareFieldName { get; set; }

        public DateGreaterThanAttribute(string dateToCompareFieldName)
        {
            DateToCompareFieldName = dateToCompareFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? laterDate = (DateTime?) value;

            DateTime? earlierDate = (DateTime?) validationContext.ObjectType.GetProperty(DateToCompareFieldName).GetValue(validationContext.ObjectInstance, null);
            if (earlierDate != null || laterDate != null)
            {
                if (laterDate > earlierDate)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(string.Format("{0} must be greater!", DateToCompareFieldName));
                }
            }

            return ValidationResult.Success;
        }

    }
}

