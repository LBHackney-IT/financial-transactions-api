using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class UpdateTransactionRequest
    {
        /// <summary>
        /// The guid of a tenancy/property
        /// </summary>
        /// <example>
        /// 94b02545-0233-4640-98dd-b2900423c0a5
        /// </example>
        [NonEmptyGuid]
        public Guid TargetId { get; set; }

        /// <summary>
        /// Week number for Rent and Period number for LeaseHolders
        /// </summary>
        /// <example>
        /// 2
        /// </example>
        [Required]
        [Range(0, (double) short.MaxValue)]
        public short PeriodNo { get; set; }

        /// <summary>
        /// Financial year of transaction
        /// </summary>
        /// <example>
        /// 2022
        /// </example>
        [Required]
        [Range(0, (double) short.MaxValue)]
        public short FinancialYear { get; set; }

        /// <summary>
        /// Financial Month of transaction
        /// </summary>
        /// <example>
        /// 1
        /// </example>
        [Required]
        [Range(0, (double) short.MaxValue)]
        public short FinancialMonth { get; set; }

        /// <summary>
        /// Transaction Information
        /// </summary>
        /// <example>
        /// DD
        /// </example>
        [Required]
        public string TransactionSource { get; set; }

        /// <summary>
        /// Type of transaction
        /// </summary>
        /// <example>
        /// Rent
        /// </example>
        [Required]
        [AllowedValues(TransactionType.Charge, TransactionType.Rent)]
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Date of transaction
        /// </summary>
        /// <example>
        /// 2021-04-27T23:00:00.000Z
        /// </example>
        [Required]
        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Amount of Transaction
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        [Required]
        [Range(0, (double) decimal.MaxValue)]
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Same as Rent Account Number
        /// </summary>
        /// <example>
        /// 216704
        /// </example>
        [Required]
        public string PaymentReference { get; set; }

        /// <summary>
        /// Is this account need to be in suspense
        /// </summary>
        /// <example>
        /// true
        /// </example>
        [Required]
        [BoolValidate(false)]
        public bool IsSuspense { get; set; }

        /// <summary>
        /// Information after this recond ceases to be suspense
        /// </summary>
        /// <example>
        /// {
        ///     "ResolutionDate": "2021-04-28T23:00:00.000Z",
        ///     "IsResolve" : true,
        ///     "Note": "Some notes about this recond"
        /// }
        /// </example>
        [Required]
        public SuspenseInfo SuspenseInfo { get; set; }

        /// <summary>
        /// Total paid amount
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        [Required]
        [Range(0, (double) decimal.MaxValue)]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        /// <example>
        /// 87.53
        /// </example>
        [Required]
        [Range(0, (double) decimal.MaxValue)]
        public decimal ChargedAmount { get; set; }

        /// <summary>
        /// Total balance amount
        /// </summary>
        /// <example>
        /// 1025.00
        /// </example>
        [Required]
        [Range((double) decimal.MinValue, (double) decimal.MaxValue)]
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Housing Benefit Contribution
        /// </summary>
        /// <example>
        /// 25.56
        /// </example>
        [Required]
        [Range(0, (double) decimal.MaxValue)]
        public decimal HousingBenefitAmount { get; set; }

        /// <summary>
        /// Address of property
        /// </summary>
        /// <example>
        /// Apartment 22, 18 G road, SW11
        /// </example>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Person, who paid for the transaction
        /// </summary>
        /// <example>
        /// {
        ///     "Id": "6d290de9-75aa-46a9-8bf5-cb8e9bdf4ff0",
        ///     "FullName": "Kian Hayward"
        /// }
        /// </example>
        [Required]
        public Person Person { get; set; }

        /// <summary>
        /// ToDO: No information about this field
        /// </summary>
        /// <example>
        /// HSGSUN
        /// </example>
        [Required]
        public string Fund { get; set; }
    }
}
