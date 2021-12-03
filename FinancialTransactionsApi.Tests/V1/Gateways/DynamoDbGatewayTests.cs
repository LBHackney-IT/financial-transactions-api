using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
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
        private readonly DynamoDbGateway _gateway;
        public DynamoDbGatewayTests()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _gateway = new DynamoDbGateway(_dynamoDb.Object);
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

            _dynamoDb.Setup(x => x.LoadAsync<TransactionDbEntity>(It.IsAny<Guid>(), It.IsAny<Guid>(),
                default))
                .ReturnsAsync(expectedResult);

            var result = await _gateway.GetTransactionByIdAsync(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);

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

            await _gateway.AddBatchAsync(entities).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<TransactionDbEntity>(), default), Times.Exactly(3));
        }


    }
}
