using AutoFixture;
using FluentAssertions;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Infrastructure;
using Xunit;

namespace TransactionsApi.Tests.V1.Factories
{

    public class EntityFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var databaseEntity = _fixture.Create<TransactionDbEntity>();
            var entity = databaseEntity.ToDomain();

            databaseEntity.Id.Should().Be(entity.Id);
            databaseEntity.TransactionDate.Should().BeSameDateAs(entity.TransactionDate);
        }

        [Fact]
        public void CanMapADomainEntityToADatabaseObject()
        {
            var entity = _fixture.Create<Transaction>();
            var databaseEntity = entity.ToDatabase();

            entity.Id.Should().Be(databaseEntity.Id);
            entity.TransactionDate.Should().BeSameDateAs(databaseEntity.TransactionDate);
        }
    }
}
