using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using System;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class TransactionQuery : BaseSearchQuery
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
        /// Type of transactioin tenancy/property
        /// </summary>
        /// <example>
        /// Rent
        /// </example>
        public TransactionType? TransactionType { get; set; }

        /// <summary>
        /// Selected start date
        /// </summary>
        /// <example>
        /// 2021-07-01
        /// </example>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Selected end date
        /// </summary>
        /// <example>
        /// 2021-08-01
        /// </example>
        public DateTime? EndDate { get; set; }
    }
}
