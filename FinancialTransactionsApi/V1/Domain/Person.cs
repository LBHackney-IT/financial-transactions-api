using FinancialTransactionsApi.V1.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Domain
{
    public class Person
    {
        [NonEmptyGuid("PersonId")]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
