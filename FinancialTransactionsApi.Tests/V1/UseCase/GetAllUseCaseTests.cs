using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using FluentAssertions;
using Hackney.Core.DynamoDb;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetAllUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetAllUseCase _getAllUseCase;
        private readonly Fixture _fixture;

        public GetAllUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _getAllUseCase = new GetAllUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAll_GatewayReturnsList_ReturnsList()
        {
            var transactions = _fixture.CreateMany<Transaction>();
            var obj = new PagedResult<Transaction>(transactions);
            var transactionQuery = new TransactionQuery()
            {
                TargetId = Guid.NewGuid(),
                TransactionType = TransactionType.Charge,
                EndDate = DateTime.Now,
                StartDate = DateTime.UtcNow,
                PaginationToken = null
            };

            _mockGateway.Setup(_ => _.GetPagedTransactionsAsync(transactionQuery)).ReturnsAsync(obj);
            var response = await _getAllUseCase.ExecuteAsync(transactionQuery).ConfigureAwait(false);

            var expectedResponse = transactions.ToResponse();

            response.Results.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
