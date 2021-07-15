using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoFixture;
using FinancialTransactionsApi.V1.Gateways;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransactionsApi.Tests.V1.Helper;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Gateways;
using TransactionsApi.V1.Infrastructure;
using Xunit;

namespace TransactionsApi.Facts.V1.Gateways
{

    public class DynamoDbGatewayTests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly Mock<DynamoDbContextWrapper> _wrapper;
        private readonly DynamoDbGateway _classUnderTest;
        private readonly List<Action> _cleanup;
        public DynamoDbGatewayTests()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _wrapper = new Mock<DynamoDbContextWrapper>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb.Object, _wrapper.Object);
            _cleanup = new List<Action>();
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var action in _cleanup)
                    action();

                _disposed = true;
            }
        }


        [Fact]
        public async Task PostNewTransactionSuccessfulSaves()
        {
            // Arrange
            var entity = _fixture.Create<Transaction>();
            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);
            await _classUnderTest.AddAsync(entity).ConfigureAwait(false);
            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), default), Times.Once);


        }

        [Fact]
        public async Task PostMultipleNewTransactionSuccessfulSaves()
        {
            // Arrange
            var entities = _fixture.CreateMany<Transaction>(3).ToList();
            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);
            await _classUnderTest.AddRangeAsync(entities).ConfigureAwait(false);
            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), default), Times.Exactly(3));


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
            var dbEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

            _dynamoDb.Setup(x => x.LoadAsync<TransactionDbEntity>(entity.Id, default))
                     .ReturnsAsync(dbEntity);

            var response = await _classUnderTest.GetTransactionByIdAsync(entity.Id).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.LoadAsync<TransactionDbEntity>(entity.Id, default), Times.Once);

            entity.Id.Should().Be(response.Id);
            entity.TransactionDate.Should().BeSameDateAs(response.TransactionDate);
        }

        [Fact]
        public async Task GetEntityByTargetIdAndTransactionTypeReturnsNullIfEntityDoesntExist()
        {

            var databaseEntities = new List<TransactionDbEntity>();
            var targetId = Guid.NewGuid();
            var transType = "Type";
            var startDate = DateTime.Now.AddDays(40);
            var endDate = DateTime.Now.AddDays(30);
            _wrapper.Setup(x => x.ScanAsync(
               It.IsAny<IDynamoDBContext>(),
               It.IsAny<IEnumerable<ScanCondition>>(),
               It.IsAny<DynamoDBOperationConfig>()))
               .ReturnsAsync(new List<TransactionDbEntity>(databaseEntities));
            var response = await _classUnderTest.GetAllTransactionsAsync(targetId, transType, startDate, endDate).ConfigureAwait(false);

            response.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetEntityByTargetIdAndTransactionTypeReturnsTheEntityIfItExists()
        {
           
            var entities = _fixture.Build<Transaction>()
               .With(x => x.TargetId, Guid.NewGuid())
               .With(x => x.TransactionType, "Sample")
               .With(x => x.TransactionDate, DateTime.Now).CreateMany(3).ToList();
            var entity = entities.FirstOrDefault();
            var dbEnty = DatabaseEntityHelper.MapDatabaseEntityFrom(entity);
            var databaseEntities = entities.Select(entity => DatabaseEntityHelper.MapDatabaseEntityFrom(entity));
           
            _wrapper.Setup(x => x.ScanAsync(
                It.IsAny<IDynamoDBContext>(),
                It.IsAny<IEnumerable<ScanCondition>>(),
                It.IsAny<DynamoDBOperationConfig>()))
                .ReturnsAsync(new List<TransactionDbEntity>(databaseEntities));
            var response = await _classUnderTest.GetAllTransactionsAsync(entity.TargetId, entity.TransactionType,entity.TransactionDate, entity.TransactionDate).ConfigureAwait(false);

            response.Should().BeEquivalentTo(entities);
        }
    }
}
