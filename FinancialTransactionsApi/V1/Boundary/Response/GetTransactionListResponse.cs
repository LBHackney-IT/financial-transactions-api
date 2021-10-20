using System;
using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class GetTransactionListResponse
    {
        private long _total;

        public List<TransactionResponse> Transactions { get; set; }

        public GetTransactionListResponse()
        {
            Transactions = new List<TransactionResponse>();
        }

        public void SetTotal(long total)
        {
            _total = total;
        }

        public long Total()
        {
            return _total;
        }


    }
}
