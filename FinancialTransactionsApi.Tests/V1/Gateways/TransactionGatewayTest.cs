using AutoFixture;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.Tests;
using TransactionsApi.Tests.V1.Helper;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Gateways;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Gateways
{
    public class TransactionGatewayTest : DatabaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TransactionGateway _classUnderTest;

        public TransactionGatewayTest()
        {
            _classUnderTest = new TransactionGateway(DatabaseContext);
        }

        [Fact]
        public async Task GetEntityByIdReturnsNullIfEntityDoesntExist()
        {
            var id = Guid.NewGuid();
            var response = await _classUnderTest.GetTransactionByIdAsync(id).ConfigureAwait(false);

            response.Should().BeNull();
        }

        [Fact]
        public async Task GetEntityByIdReturnsTheEntityIfItExists()
        {
            var entity = _fixture.Create<Transaction>();
            var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

            DatabaseContext.TransactionEntities.Add(databaseEntity);
            DatabaseContext.SaveChanges();

            var response = await _classUnderTest.GetTransactionByIdAsync(databaseEntity.Id).ConfigureAwait(false);

            databaseEntity.Id.Should().Be(response.Id);
            databaseEntity.TransactionDate.Should().BeSameDateAs(response.TransactionDate);
        }
        [Fact]
        public async Task GetEntityTargetIdAndTransactionTypeReturnsNullIfEntityDoesntExist()
        {
            var targetId = Guid.NewGuid();
            var transType = "InvalidString";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;
            var response = await _classUnderTest.GetAllTransactionsAsync(targetId, transType, startDate, endDate).ConfigureAwait(false);

            response.Should().BeEmpty();
        }
        [Fact]
        public async Task GetEntityTargetIdAndTransactionTypeReturnsTheEntityIfItExists()
        {
            var date = DateTime.Now.AddDays(-2);
            var entities = _fixture.Build<Transaction>()
                .With(x => x.TargetId, Guid.NewGuid())
                .With(x => x.TransactionType, "Sample")
                .With(x => x.TransactionDate, date).CreateMany().ToList();
            var entity = entities.FirstOrDefault();

            var databaseEntities = entities.Select(entity => DatabaseEntityHelper.MapDatabaseEntityFrom(entity));

            DatabaseContext.TransactionEntities.AddRange(databaseEntities);
            DatabaseContext.SaveChanges();
            var response = await _classUnderTest.GetAllTransactionsAsync(entity.TargetId, entity.TransactionType, date, date).ConfigureAwait(false);
            response.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task PostNewTransactionSuccessfulSaves()
        {
            // Arrange
            var entity = _fixture.Create<Transaction>();

            await _classUnderTest.AddAsync(entity).ConfigureAwait(false);
            var response = await _classUnderTest.GetTransactionByIdAsync(entity.Id).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(entity);


        }

    }

}


