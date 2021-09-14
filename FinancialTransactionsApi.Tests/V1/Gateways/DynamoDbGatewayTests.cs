using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using FinancialTransactionsApi.Tests.V1.Helper;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Infrastructure;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Gateways
{
    public class DynamoDbGatewayTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly Mock<DynamoDbContextWrapper> _wrapper;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDb;
        private readonly DynamoDbGateway _gateway;

        public DynamoDbGatewayTests()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _wrapper = new Mock<DynamoDbContextWrapper>();
            _amazonDynamoDb = new Mock<IAmazonDynamoDB>();
            _gateway = new DynamoDbGateway(_dynamoDb.Object, _wrapper.Object, _amazonDynamoDb.Object);
        }

        [Fact]
        public async Task GetById_EntityDoesntExists_ReturnsNull()
        {
            _wrapper.Setup(x => x.LoadAsync(
                It.IsAny<IDynamoDBContext>(),
                It.IsAny<Guid>(),
                It.IsAny<DynamoDBOperationConfig>()))
                .ReturnsAsync((TransactionDbEntity) null);

            var result = await _gateway.GetTransactionByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_EntityExists_ReturnsEntity()
        {
            var expectedResult = new TransactionDbEntity()
            {
                Id = Guid.NewGuid(),
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                FinancialMonth = 2,
                FinancialYear = 2022,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                IsSuspense = true,
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                }
            };

            _wrapper.Setup(x => x.LoadAsync(
                It.IsAny<IDynamoDBContext>(),
                It.IsAny<Guid>(),
                It.IsAny<DynamoDBOperationConfig>()))
                .ReturnsAsync(expectedResult);

            var result = await _gateway.GetTransactionByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task AddAndUpdate_SaveObject_VerifiedOneTimeWorked()
        {
            var entity = _fixture.Create<Transaction>();

            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);

            await _gateway.AddAsync(entity).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), default), Times.Once);
        }

        [Fact]
        public async Task AddAndUpdate_InvalidObject_VerifiedOneTimeWorked()
        {
            Transaction entity = null;

            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);

            await _gateway.AddAsync(entity).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), default), Times.Once);
        }

        [Fact]
        public async Task Add_SaveListOfObjects_VirifiedThreeTimesWorked()
        {
            var entities = _fixture.CreateMany<Transaction>(3).ToList();

            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);

            await _gateway.AddRangeAsync(entities).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), default), Times.Exactly(3));
        }

        [Fact]
        public async Task GetAll_ByTransactionQueryReturnEmptyList_EntitiesDoesntExists()
        {
            var databaseEntities = new List<TransactionDbEntity>();

            var targetId = Guid.NewGuid();
            var transType = TransactionType.Rent;
            var startDate = DateTime.Now.AddDays(40);
            var endDate = DateTime.Now.AddDays(30);

            _wrapper.Setup(x => x.ScanAsync(
               It.IsAny<IDynamoDBContext>(),
               It.IsAny<IEnumerable<ScanCondition>>(),
               It.IsAny<DynamoDBOperationConfig>()))
               .ReturnsAsync(new List<TransactionDbEntity>(databaseEntities));
            var response = await _gateway.GetAllTransactionsAsync(targetId, transType, startDate, endDate).ConfigureAwait(false);

            response.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetAll_ByTransactionQueryReturnsListWithEntities_ReturnsAllCorrect()
        {
            var entities = _fixture.Build<Transaction>()
               .With(x => x.TargetId, Guid.NewGuid())
               .With(x => x.TransactionType, TransactionType.Charge)
               .With(x => x.TransactionDate, DateTime.Now).CreateMany(3).ToList();

            var entity = entities.FirstOrDefault();

            var databaseEntities = entities.Select(entity => entity.ToDatabase());

            _wrapper.Setup(x => x.ScanAsync(
                It.IsAny<IDynamoDBContext>(),
                It.IsAny<IEnumerable<ScanCondition>>(),
                It.IsAny<DynamoDBOperationConfig>()))
                .ReturnsAsync(new List<TransactionDbEntity>(databaseEntities));

            var response = await _gateway.GetAllTransactionsAsync(entity.TargetId, entity.TransactionType, entity.TransactionDate, entity.TransactionDate).ConfigureAwait(false);

            response.Should().BeEquivalentTo(entities);
        }

        [Theory]

        [InlineData(null, 1, 1)]
        [InlineData("a", 1, 1)]
        [InlineData("1", 1, 10)]
        public async Task GetAllSuspenseValidInputRetursData(string text, int page, int pageSize)
        {
            var transactions = _fixture.Build<Transaction>()
                .With(s => s.IsSuspense, true).CreateMany(10);

            var responseTransaction = FakeDataHelper.MockQueryResponse<Transaction>(10);

            var expectedResult = responseTransaction.ToTransactions();

            if (text != null)
            {
                expectedResult = expectedResult.Where(p =>
                    p.Person.FullName.ToLower().Contains(text) ||
                    p.PaymentReference.ToLower().Contains(text) ||
                    p.TransactionDate.ToString("F").Contains(text) ||
                    p.BankAccountNumber.Contains(text) ||
                    p.Fund.ToLower().Contains(text) ||
                    p.BalanceAmount.ToString("F").Contains(text)).ToList();
            }

            expectedResult = expectedResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            _amazonDynamoDb.Setup(s => s.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync(responseTransaction);

            var result = await _gateway.GetAllSuspenseAsync(
                new SuspenseTransactionsSearchRequest
                {
                    SearchText = text,
                    Page = page,
                    PageSize = pageSize
                }).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
