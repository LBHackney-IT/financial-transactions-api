using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using HSFPagination = Hackney.Shared.Finance.Pagination;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetSuspenseAccountUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetAllUseCase _getAllUseCase;
        private readonly GetSuspenseAccountUseCase _getSuspenseAccountUseCase;
        private readonly Fixture _fixture;

        public GetSuspenseAccountUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _getAllUseCase = new GetAllUseCase(_mockGateway.Object);
            _getSuspenseAccountUseCase = new GetSuspenseAccountUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAll_GatewayReturnsList_ReturnsList()
        {
            var transactions = _fixture.CreateMany<Transaction>();
            var obj = new Paginated<Transaction>() { Results = transactions };
            var transactionQuery = new TransactionQuery()
            {
                TargetId = Guid.NewGuid(),
                TransactionType = TransactionType.ArrangementInterest,
                EndDate = DateTime.Now,
                StartDate = DateTime.UtcNow
            };

            _mockGateway.Setup(_ => _.GetPagedTransactionsAsync(transactionQuery)).ReturnsAsync(transactions);
            var response = await _getAllUseCase.ExecuteAsync(transactionQuery).ConfigureAwait(false);

            var expectedResponse = transactions.ToResponse();

            response.Results.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetSuspenseAccount_GatewayReturnTransactionResponse_ReturnTransactionResponse()
        {

            var paginatedTransaction = new HSFPagination.Paginated<Transaction>();

            paginatedTransaction.Results = _fixture.Build<Transaction>().CreateMany(5);

            _mockGateway.Setup(g => g.GetPagedSuspenseAccountTransactionsAsync(It.IsAny<SuspenseAccountQuery>())).ReturnsAsync(paginatedTransaction);

            var response = await _getSuspenseAccountUseCase.ExecuteAsync(It.IsAny<SuspenseAccountQuery>()).ConfigureAwait(false);

            response.Results.Should().BeEquivalentTo(paginatedTransaction.Results);
        }

        [Fact]
        public async Task GetSuspenseAccount_GatewayReturnEmptyTransactionResponse_ReturnEmptyTransactionResponse()
        {
            var paginatedTransaction = new HSFPagination.Paginated<Transaction>();
            paginatedTransaction.Results = Enumerable.Empty<Transaction>();

            _mockGateway.Setup(x => x.GetPagedSuspenseAccountTransactionsAsync(It.IsAny<SuspenseAccountQuery>())).ReturnsAsync(paginatedTransaction);

            var response = await _getSuspenseAccountUseCase.ExecuteAsync(It.IsAny<SuspenseAccountQuery>()).ConfigureAwait(false);

            response.Results.Should().BeEquivalentTo(paginatedTransaction.Results);
        }
    }
}
