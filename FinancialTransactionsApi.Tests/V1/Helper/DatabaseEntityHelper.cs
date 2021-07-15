using AutoFixture;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Infrastructure;

namespace TransactionsApi.Tests.V1.Helper
{
    public static class DatabaseEntityHelper
    {
        public static TransactionDbEntity CreateDatabaseEntity()
        {
            var entity = new Fixture().Create<Transaction>();

            return CreateDatabaseEntityFrom(entity);
        }

        public static TransactionDbEntity CreateDatabaseEntityFrom(Transaction entity)
        {
            return new TransactionDbEntity
            {
                Id = entity.Id,
                TransactionDate = entity.TransactionDate,
            };
        }
        public static TransactionDbEntity MapDatabaseEntityFrom(Transaction entity)
        {
            return new TransactionDbEntity
            {
                Id = entity.Id,
                TargetId = entity.TargetId,
                TransactionType = entity.TransactionType,
                TransactionDate = entity.TransactionDate,
                PaymentReference =entity.PaymentReference,
                PeriodNo = entity.PeriodNo,
                FinancialMonth = entity.FinancialMonth,
                FinancialYear =entity.FinancialYear,
                TransactionAmount = entity.TransactionAmount,
                PaidAmount = entity.PaidAmount,
                ChargedAmount = entity.ChargedAmount,
                BalanceAmount = entity.BalanceAmount,
                HousingBenefitAmount = entity.HousingBenefitAmount

            };
        }
    }
}
