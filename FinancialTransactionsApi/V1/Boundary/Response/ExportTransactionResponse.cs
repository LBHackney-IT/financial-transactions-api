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
        public decimal TransactionAmount { get; set; }
        public string PaymentReference { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ChargedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal HousingBenefitAmount { get; set; }

    }
}
