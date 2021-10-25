using System;

namespace TransactionsApi.V1.Boundary.Response
{
    public class TransactionResponseObject
    {
        /// <summary>
        /// 793dd4ca-d7c4-4110-a8ff-c58eac4b90a7
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 793dd4ca-d7c4-4110-a8ff-c58eac4b90f8
        /// </summary>
        public Guid TargetId { get; set; }
        /// <summary>
        /// 1
        /// </summary>
        public short PeriodNo { get; set; }
        /// <summary>
        /// 2020
        /// </summary>
        public short FinancialYear { get; set; }
        /// <summary>
        /// 2
        /// </summary>
        public short FinancialMonth { get; set; }
        /// <summary>
        /// WON
        /// </summary>
        public string TransactionType { get; set; }
        /// <summary>
        /// 2020-05-05 11:13:14:125
        /// </summary>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// 125.23
        /// </summary>
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// 2345
        /// </summary>
        public string PaymentReference { get; set; }
        /// <summary>
        /// 56.78
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 87.53
        /// </summary>
        public decimal ChargedAmount { get; set; }
        /// <summary>
        /// 1025
        /// </summary>
        public decimal BalanceAmount { get; set; }
        /// <summary>
        /// 25.56
        /// </summary>
        public decimal HousingBenefitAmount { get; set; }
        
    }
}
