using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using FinancialTransactionsApi.Tests.V1.Helper;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Gateways
{
    public class DynamoDbGatewayTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDb;
        private readonly DynamoDbGateway _gateway;
        private const string Pk = "#lbhtransaction";


        public DynamoDbGatewayTests()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _amazonDynamoDb = new Mock<IAmazonDynamoDB>();
            _gateway = new DynamoDbGateway(_dynamoDb.Object, _amazonDynamoDb.Object);
        }

        [Fact]
        public async Task GetById_EntityDoesntExists_ReturnsNull()
        {
            _dynamoDb.Setup(x => x.LoadAsync<TransactionDbEntity>(Pk, It.IsAny<Guid>(), default))
                .ReturnsAsync((TransactionDbEntity) null);

            var result = await _gateway.GetTransactionByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_EntityExists_ReturnsEntity()
        {
            var expectedResult = new TransactionDbEntity()
            {
                Pk = Pk,
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

            _dynamoDb.Setup(x => x.LoadAsync<TransactionDbEntity>(Pk,
                It.IsAny<Guid>(),
                default))
                .ReturnsAsync(expectedResult);

            var result = await _gateway.GetTransactionByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(expectedResult, opt =>
                opt.Excluding(a => a.Pk));
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
            var transactionQuery = new TransactionQuery
            {
                TargetId = Guid.NewGuid(),
                TransactionType = TransactionType.Rent,
                StartDate = DateTime.Now.AddDays(40),
                EndDate = DateTime.Now.AddDays(30)
            };
            _amazonDynamoDb.Setup(p => p.QueryAsync(It.IsAny<QueryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueryResponse());

            var responseResult = await _gateway.GetPagedTransactionsAsync(transactionQuery).ConfigureAwait(false);

            responseResult.Total.Should().Be(0);
            responseResult.Transactions.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ByTransactionQueryReturnsListWithEntities_ReturnsAllCorrect()
        {
            QueryResponse response = FakeDataHelper.MockQueryResponse<Transaction>(2);
            var expectedResponse = response.ToTransactions();

            _amazonDynamoDb.Setup(p => p.QueryAsync(It.IsAny<QueryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var transactionQuery = new TransactionQuery
            {
                TargetId = expectedResponse.First().TargetId,
                TransactionType = expectedResponse.First().TransactionType,
                StartDate = expectedResponse.First().TransactionDate.AddDays(-2),
                EndDate = expectedResponse.First().TransactionDate.AddDays(2),
                Page = 1,
                PageSize = 2
            };
            var responseResult = await _gateway.GetPagedTransactionsAsync(transactionQuery).ConfigureAwait(false);

            responseResult.Transactions.Should().BeEquivalentTo(expectedResponse);
        }

        [Theory]

        [InlineData(null, 1, 1, 0)]
        [InlineData("a", 1, 1, 1)]
        [InlineData("1", 2, 4, 20)]
        public async Task GetAllSuspenseValidInputReturnsData(string text, int page, int pageSize, int count)
        {
            var responseTransaction = FakeDataHelper.MockQueryResponse<Transaction>(count);

            var rawExpectedResult = responseTransaction.ToTransactions();

            if (text != null)
            {
                rawExpectedResult = rawExpectedResult.Where(p =>
                    p.Person.FullName.ToLower().Contains(text) ||
                    p.PaymentReference.ToLower().Contains(text) ||
                    p.TransactionDate.ToString("F").Contains(text) ||
                    p.BankAccountNumber.Contains(text) ||
                    p.Fund.ToLower().Contains(text) ||
                    p.BalanceAmount.ToString("F").Contains(text)).ToList();
            }

            var expectedResult = rawExpectedResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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
            result.Total.Should().Be(rawExpectedResult.Count);
            result.Transactions.Should().BeEquivalentTo(expectedResult);
            result.Transactions.Should().HaveCountLessOrEqualTo(pageSize);
        }
    }
}
