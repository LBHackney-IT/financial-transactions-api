using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class ExportResponse
    {
        public string FullName { get; set; }

        public string StatementPeriod { get; set; }
        public string BalanceBroughtForward { get; set; }
        public string Balance { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string Footer { get; set; }
        public string SubFooter { get; set; }

        public List<ExportTransactionResponse> Data { get; set; }
    }

    public class ExportTransactionResponse
    {
        public string Date { get; set; }
        public string TransactionDetail { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string Balance { get; set; }
    }
}
