using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class ExportResponse
    {
        public string FullName { get; set; }
        public string StatementPeriod { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalCharge { get; set; }
        public decimal TotalHousingBenefit { get; set; }
        public string BankAccountNumber { get; set; }

        public List<ExportTransactionResponse> Data { get; set; }
    }
    public class ExportTransactionResponse
    {
        public string TransactionType { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionAmount { get; set; }
        public string PaymentReference { get; set; }
        public string PaidAmount { get; set; }
        public string ChargedAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string HousingBenefitAmount { get; set; }

    }
}
