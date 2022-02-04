using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Boundary
{
    public abstract class BaseTransactionModel
    {
        /// <summary>
        /// The guid of a tenancy/property
        /// If targetId be empty the transaction is suspense
        /// </summary>
        /// <example>
        /// 94b02545-0233-4640-98dd-b2900423c0a5
        /// </example>
        public Guid TargetId { get; set; }
        /// <summary>
        ///     The target of provided id by the target_id
        /// </summary>
        /// <example>
        ///     Asset
        /// </example>
        public TargetType TargetType { get; set; }
        /// <summary>
        /// Week number for Rent and Period number for LeaseHolders
        /// </summary>
        /// <example>
        /// 2
        /// </example>
        [Range(1, 53)]
        public short PeriodNo { get; set; }

        /// <summary>
        /// Transaction Information
        /// </summary>
        /// <example>
        /// DD
        /// </example>
        public string TransactionSource { get; set; }



        /// <summary>
        /// Date of transaction
        /// </summary>
        /// <example>
        /// 2021-04-27T23:00:00.000Z
        /// </example>
        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Amount of Transaction
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        [GreatAndEqualThan("0.0")]
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Same as Rent Account Number
        /// </summary>
        /// <example>
        /// 216704
        /// </example>
        public string PaymentReference { get; set; }

        /// <summary>
        /// Bank account number
        /// </summary>
        /// <example>
        /// ******78
        /// </example>
        [StringLength(8, MinimumLength = 8, ErrorMessage = "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        public string BankAccountNumber { get; set; }

        /// <summary>
        ///     Provided by the banking system and includes the bank branch and credit card number
        /// </summary>
        /// <example>
        ///     1234-9875-6548-1235
        /// </example>
        [AllowNull]
        public string SortCode { get; set; }

        /// <summary>
        /// Is this account need to be in suspense
        /// </summary>
        /// <example>
        /// true
        /// </example>
        public bool IsSuspense => TargetId == Guid.Empty;

        /// <summary>
        /// Total paid amount
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        [GreatAndEqualThan("0.0")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        /// <example>
        /// 87.53
        /// </example>
        [GreatAndEqualThan("0.0")]
        public decimal ChargedAmount { get; set; }

        /// <summary>
        /// Total balance amount
        /// </summary>
        /// <example>
        /// 1025.00
        /// </example>
        [GreatAndEqualThan("0.0")]
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Housing Benefit Contribution
        /// </summary>
        /// <example>
        /// 25.56
        /// </example>
        [GreatAndEqualThan("0.0")]
        public decimal HousingBenefitAmount { get; set; }

        /// <summary>
        /// Address of property
        /// </summary>
        /// <example>
        /// Apartment 22, 18 G road, SW11
        /// </example>
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
        public Sender Person { get; set; }

        /// <summary>
        /// ToDO: No information about this field
        /// </summary>
        /// <example>
        ///     HSGSUN
        /// </example>
        public string Fund { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// 2020
        /// </example>
        public short FinancialYear { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     10
        /// </example>
        public short FinancialMonth { get; set; }
        /// <example>
        ///     Admin
        /// </example>
        public string LastUpdatedBy { get; set; }

        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime LastUpdatedAt { get; set; }

        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime CreatedAt { get; set; }

        /// <example>
        ///     Admin
        /// </example>
        public string CreatedBy { get; set; }
    }
}
