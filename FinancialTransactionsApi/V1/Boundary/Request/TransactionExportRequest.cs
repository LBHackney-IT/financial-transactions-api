using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using System;
using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class TransactionExportRequest
    {

        [NonEmptyGuid]
        public Guid TargetId { get; set; }

        /// <summary>
        /// Type of transactioin tenancy/property
        /// </summary>
        /// <example>
        /// Rent
        /// </example>
        public TransactionType? TransactionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Guid> SelectedItems { get; set; }
    }
}
