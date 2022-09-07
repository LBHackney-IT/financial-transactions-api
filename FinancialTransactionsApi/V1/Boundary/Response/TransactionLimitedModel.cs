using System;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class TransactionLimitedModel
    {
        /// <summary>
        /// The guid of a record
        /// </summary>
        /// <example>ed17bcbb-c910-d415-9d19-a1e8b3f74553</example>
        public Guid Id { get; set; }

        /// <summary>
        /// The guid of a tenancy/property
        /// </summary>
        /// <example>fadab043-0dd3-3689-bac2-e251c7853ec4</example>
        public Guid TargetId { get; set; }

        /// <summary>
        /// Amount of Transaction
        /// </summary>
        /// <example>150</example>
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Total paid amount
        /// </summary>
        /// <example>150</example>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        /// <example>0</example>
        public decimal ChargedAmount { get; set; }

        /// <summary>
        /// Total balance amount
        /// </summary>
        /// <example>0</example>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Housing Benefit Contribution
        /// </summary>
        /// <example>0</example>
        public decimal HousingBenefitAmount { get; set; }
    }
}
