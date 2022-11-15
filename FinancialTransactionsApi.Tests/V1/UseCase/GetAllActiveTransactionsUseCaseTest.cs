using Moq;
using System;
using Xunit;
using AutoFixture;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using FinancialTransactionsApi.V1.Factories;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetAllActiveTransactionsUseCaseTest
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetAllActiveTransactionsUseCase _getAllActiveTransactionsUseCase;
        private readonly Fixture _fixture;

        public GetAllActiveTransactionsUseCaseTest()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _getAllActiveTransactionsUseCase = new GetAllActiveTransactionsUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllActiveTransactions_GatewayReturnsList()
        {
            var transactions = _fixture.CreateMany<Transaction>(10);

            var obj = new Paginated<Transaction>()
            {
                Results = transactions,
                CurrentPage = 1,
                PageSize = 11,
                TotalResultCount = transactions.Count(),
            };

            var transactionRequest = new GetActiveTransactionsRequest() { PageNumber = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow, PeriodEndDate = DateTime.UtcNow };

            _mockGateway.Setup(x => x.GetAllActive(transactionRequest)).ReturnsAsync(transactions);

            var response = await _getAllActiveTransactionsUseCase.ExecuteAsync(transactionRequest).ConfigureAwait(false);

            var expectedResponse = obj.ToResponse();

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetAllActiveTransactions_GatewayReturnsEmptyList()
        {
            var transactions = Enumerable.Empty<Transaction>();

            var obj = new Paginated<Transaction>()
            {
                Results = transactions,
                CurrentPage= 1,
                PageSize = 11,
                TotalResultCount= 0,
            };

            var transactionRequest = new GetActiveTransactionsRequest() { PageNumber = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow, PeriodEndDate = DateTime.UtcNow };

            _mockGateway.Setup(_ => _.GetAllActive(transactionRequest)).ReturnsAsync(transactions);

            var response = await _getAllActiveTransactionsUseCase.ExecuteAsync(transactionRequest).ConfigureAwait(false);

            var expectedResponse = obj.ToResponse();

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
