using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class TransactionResponses
    {
        /// <summary>
        /// List of transactions
        /// </summary>
        public IEnumerable<TransactionResponse> TransactionsList { get; set; }

    }
}
