using FinancialTransactionsApi.V1.Boundary.Request;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Infrastructure;

namespace TransactionsApi.V1.Factories
{
    public static class TransactionFactory
    {
        public static Transaction ToDomain(this TransactionDbEntity transactionDbEntity)
        {
            return new Transaction
            {
                FinancialMonth = transactionDbEntity.FinancialMonth,
                FinancialYear = transactionDbEntity.FinancialYear,
                Id = transactionDbEntity.Id,
                PaymentReference = transactionDbEntity.PaymentReference,
                PeriodNo = transactionDbEntity.PeriodNo,
                TargetId = transactionDbEntity.TargetId,
                TransactionAmount = transactionDbEntity.TransactionAmount,
                TransactionDate = transactionDbEntity.TransactionDate,
                TransactionType = transactionDbEntity.TransactionType,
                PaidAmount = transactionDbEntity.PaidAmount,
                ChargedAmount = transactionDbEntity.ChargedAmount,
                BalanceAmount = transactionDbEntity.BalanceAmount,
                HousingBenefitAmount= transactionDbEntity.HousingBenefitAmount
            };
        }

        public static TransactionDbEntity ToDatabase(this Transaction transaction)
        {
            return new TransactionDbEntity
            {
                FinancialMonth = transaction.FinancialMonth,
                FinancialYear = transaction.FinancialYear,
                TransactionType = transaction.TransactionType,
                TransactionDate = transaction.TransactionDate,
                TransactionAmount = transaction.TransactionAmount,
                TargetId = transaction.TargetId,
                PeriodNo = transaction.PeriodNo,
                PaymentReference = transaction.PaymentReference,
                Id = transaction.Id,
                PaidAmount = transaction.PaidAmount,
                ChargedAmount = transaction.ChargedAmount,
                BalanceAmount = transaction.BalanceAmount,
                HousingBenefitAmount = transaction.HousingBenefitAmount
            };
        }
        public static Transaction ToTransactionDomain(this TransactionRequest transactionRequest)
        {
            return transactionRequest == null ? null : new Transaction
            {
                TargetId = transactionRequest.TargetId,
                BalanceAmount = transactionRequest.BalanceAmount,
                ChargedAmount = transactionRequest.ChargedAmount,
                FinancialMonth = transactionRequest.FinancialMonth,
                FinancialYear = transactionRequest.FinancialYear,
                HousingBenefitAmount = transactionRequest.HousingBenefitAmount,
                PaidAmount = transactionRequest.PaidAmount,
                PaymentReference = transactionRequest.PaymentReference,
                PeriodNo = transactionRequest.PeriodNo,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDate = transactionRequest.TransactionDate,
                TransactionType = transactionRequest.TransactionType
            };
        }

    }
}
