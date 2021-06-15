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
                CreatedAt = entity.CreatedAt,
            };
        }
    }
}
