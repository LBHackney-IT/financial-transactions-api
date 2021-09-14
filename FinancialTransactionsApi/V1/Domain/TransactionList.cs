using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Domain
{
    public class TransactionList
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public decimal Total { get; set; }
    }
}
