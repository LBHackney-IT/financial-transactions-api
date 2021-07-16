using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class TransactionQuery
    {
        /// <summary>
        /// The guid of a tenancy/property
        /// </summary>
        [Required]
        public Guid TargetId { get; set; }
        /// <summary>
        /// Type of transactioin tenancy/property
        /// </summary>
        public string TransactionType { get; set; }
        /// <summary>
        /// Selected Start Date Range like 2021-07-01
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Selected Start Date Range like 2021-07-01
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
