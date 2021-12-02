using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using System;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class ExportTransactionQuery
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
        /// Type of statement quaterly/yearly
        /// </summary>
        /// <example>
        /// Quaterly
        /// </example>
        public StatementType StatementType { get; set; }
        /// <summary>
        /// Type of file (csv or pdf)
        /// </summary>
        /// <example>
        /// csv
        /// </example>
        public string FileType { get; set; }

    }

}
