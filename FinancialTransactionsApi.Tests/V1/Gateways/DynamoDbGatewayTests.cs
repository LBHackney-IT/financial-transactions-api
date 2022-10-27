using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Gateways
{
    public class DynamoDbGatewayTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly DynamoDbGateway _gateway;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDb;
        private readonly Mock<IConfiguration> _mockConfig;
        public DynamoDbGatewayTests()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _mockConfig = new Mock<IConfiguration>();
            _amazonDynamoDb = new Mock<IAmazonDynamoDB>();
            _gateway = new DynamoDbGateway(_amazonDynamoDb.Object, _dynamoDb.Object, _mockConfig.Object);
        }



        [Fact]
        public async Task GetById_EntityDoesntExists_ReturnsNull()
        {
            _dynamoDb.Setup(x => x.LoadAsync<TransactionDbEntity>(It.IsAny<Guid>(), default))
                .ReturnsAsync((TransactionDbEntity) null);

            var result = await _gateway.GetTransactionByIdAsync(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);

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
                PaidAmount = 123.22M,
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.ArrangementInterest,
                Sender = new Sender
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                },
                CreatedBy = "Admin",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "Admin"
            };

            _dynamoDb.Setup(x => x.LoadAsync<TransactionDbEntity>(It.IsAny<Guid>(), It.IsAny<Guid>(),
                default))
                .ReturnsAsync(expectedResult);

            var result = await _gateway.GetTransactionByIdAsync(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);

            result.Should().NotBeNull();

            result.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(info => info.PaymentReference);
                return options;
            });
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
        public async Task UpdateAndDelete_VerifiedOneTimeWorked()
        {
            var entity = _fixture.Create<Transaction>();

            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);

            _dynamoDb.Setup(x => x.DeleteAsync(It.IsAny<TransactionDbEntity>(), It.IsAny<CancellationToken>()))
              .Returns(Task.CompletedTask);

            await _gateway.UpdateSuspenseAccountAsync(entity).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), default), Times.Once);
            _dynamoDb.Verify(x => x.DeleteAsync<TransactionDbEntity>(Guid.Empty, It.IsAny<Guid>(), default), Times.Once);
        }
    }
}
