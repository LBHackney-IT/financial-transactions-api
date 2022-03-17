using System;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class TransactionLimitedModel
    {
        /// <summary>
        /// The guid of a record
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The guid of a tenancy/property
        /// </summary>
        public Guid TargetId { get; set; }

        /// <summary>
        /// Amount of Transaction
        /// </summary>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Total paid amount
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        public decimal ChargedAmount { get; set; }

        /// <summary>
        /// Total balance amount
        /// </summary>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Housing Benefit Contribution
        /// </summary>
        public decimal HousingBenefitAmount { get; set; }
    }
}
