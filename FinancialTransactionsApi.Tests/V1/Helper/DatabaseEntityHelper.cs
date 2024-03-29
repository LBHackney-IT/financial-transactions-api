using AutoFixture;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.Tests.V1.Helper
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
                PeriodNo = entity.PeriodNo,
                FinancialMonth = entity.FinancialMonth,
                FinancialYear = entity.FinancialYear,
                TransactionAmount = entity.TransactionAmount,
                PaidAmount = entity.PaidAmount,
                ChargedAmount = entity.ChargedAmount,
                BalanceAmount = entity.BalanceAmount,
                HousingBenefitAmount = entity.HousingBenefitAmount,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                LastUpdatedAt = entity.LastUpdatedAt,
                LastUpdatedBy = entity.LastUpdatedBy

            };
        }
    }
}
