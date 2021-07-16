using System;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class TransactionRequest
    {
        public Guid TargetId { get; set; }
        public short PeriodNo { get; set; }
        public short FinancialYear { get; set; }
        public short FinancialMonth { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public string PaymentReference { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal ChargedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal HousingBenefitAmount { get; set; }
    }
}
