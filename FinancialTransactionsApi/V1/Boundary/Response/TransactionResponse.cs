using FinancialTransactionsApi.V1.Domain;
using System;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class TransactionResponse
    {
        /// <summary>
        /// The guid of a record
        /// </summary>
        /// <example>
        /// 2f378d65-38d3-4fb4-877b-afeee666209e
        /// </example>
        public Guid Id { get; set; }

        /// <summary>
        /// The guid of a tenancy/property
        /// </summary>
        /// <example>
        /// 94b02545-0233-4640-98dd-b2900423c0a5
        /// </example>
        public Guid TargetId { get; set; }

        /// <summary>
        /// Week number for Rent and Period number for LeaseHolders
        /// </summary>
        /// <example>
        /// 2
        /// </example>
        public short PeriodNo { get; set; }

        /// <summary>
        /// Financial year of transaction
        /// </summary>
        /// <example>
        /// 2022
        /// </example>
        public short FinancialYear { get; set; }

        /// <summary>
        /// Financial Month of transaction
        /// </summary>
        /// <example>
        /// 1
        /// </example>
        public short FinancialMonth { get; set; }

        /// <summary>
        /// Transaction Information
        /// </summary>
        /// <example>
        /// DD
        /// </example>
        public string TransactionSource { get; set; }

        /// <summary>
        /// Type of transaction
        /// </summary>
        /// <example>
        /// Rent
        /// </example>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Date of transaction
        /// </summary>
        /// <example>
        /// 2021-04-27T23:00:00.000Z
        /// </example>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Amount of Transaction
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Same as Rent Account Number
        /// </summary>
        /// <example>
        /// 216704
        /// </example>
        public string PaymentReference { get; set; }

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
        public SuspenseInfo SuspenseInfo { get; set; }

        /// <summary>
        /// Total paid amount
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        /// <example>
        /// 87.53
        /// </example>
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
