using FinancialTransactionsApi.V1.Domain;
using System;
using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class TransactionResponses
    {
        /// <summary>
        /// List of transactions
        /// </summary>
        public IEnumerable<TransactionResponse> TransactionsList { get; set; }
        /// <summary>
        /// Total record count of the transactions
        /// </summary>
        /// <example>
        ///     150
        /// </example>
        public int Total { get; set; }
    }
}
