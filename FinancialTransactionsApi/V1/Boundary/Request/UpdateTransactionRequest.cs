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
        /// Type of transaction [Charge, Rent]
        /// </summary>
        /// <example>
        /// Rent
        /// </example>
        [AllowedValues(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }

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
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
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
        /// Is this account need to be in suspense
        /// </summary>
        /// <example>
        /// true
        /// </example>
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
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }

        /// <summary>
        /// Total paid amount
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        /// <example>
        /// 87.53
        /// </example>
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal ChargedAmount { get; set; }

        /// <summary>
        /// Total balance amount
        /// </summary>
        /// <example>
        /// 1025.00
        /// </example>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Housing Benefit Contribution
        /// </summary>
        /// <example>
        /// 25.56
        /// </example>
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
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
        public Person Person { get; set; }

        /// <summary>
        /// ToDO: No information about this field
        /// </summary>
        /// <example>
        /// HSGSUN
        /// </example>
        public string Fund { get; set; }
    }
}
