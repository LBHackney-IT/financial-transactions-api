using System;

namespace FinancialTransactionsApi.V1.Domain
{
    //TODO: rename this class to be the domain object which this API will getting. e.g. Residents or Claimants
    public class Transaction
    {
        public Guid Id {get;set;}
        public Guid TargetId { get; set; }
        public short PeriodNo { get; set; }
        public short FinancialYear { get; set; }
        public short FinancialMonth { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public string PaymentReference { get; set; }
    }
}
