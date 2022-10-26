using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
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
    public class GetSuspenseAccountUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetAllUseCase _getAllUseCase;
        private readonly Fixture _fixture;

        public GetSuspenseAccountUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _getAllUseCase = new GetAllUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAll_GatewayReturnsList_ReturnsList()
        {
            var transactions = _fixture.CreateMany<Transaction>();
            var paginationMeta = new PaginationMetaData(1, 1, 1, 1, 1);
            var obj = new PaginatedResponse<Transaction>(transactions, paginationMeta);
            var transactionQuery = new TransactionQuery()
            {
                TargetId = Guid.NewGuid(),
                TransactionType = TransactionType.ArrangementInterest,
                EndDate = DateTime.Now,
                StartDate = DateTime.UtcNow,
                PageSize = 1,
                Page = 1
            };

            _mockGateway.Setup(_ => _.GetPagedTransactionsAsync(transactionQuery)).ReturnsAsync(obj);
            var response = await _getAllUseCase.ExecuteAsync(transactionQuery).ConfigureAwait(false);

            var expectedResponse = transactions.ToResponse();

            response.Results.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
