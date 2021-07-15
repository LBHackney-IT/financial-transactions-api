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
                HousingBenefitAmount= transactionDbEntity.HousingBenefitAmount,
                AssetFullAddress = transactionDbEntity.AssetFullAddress
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
                HousingBenefitAmount = transaction.HousingBenefitAmount,
                AssetFullAddress = transaction.AssetFullAddress
            };
        }


    }
}
