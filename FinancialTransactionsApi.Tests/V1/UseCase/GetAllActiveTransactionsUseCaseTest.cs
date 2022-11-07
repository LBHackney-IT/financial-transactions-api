using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using Hackney.Core.DynamoDb;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using FinancialTransactionsApi.V1.Factories;
using FluentAssertions;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
            var transactions = _fixture.CreateMany<Transaction>();

            var obj = new PagedResult<Transaction>(transactions);

            var transactionRequest = new GetActiveTransactionsRequest() { Page = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow, PeriodEndDate = DateTime.UtcNow };

            _mockGateway.Setup(_ => _.GetAllActive(transactionRequest)).ReturnsAsync(obj);

            var response = await _getAllActiveTransactionsUseCase.ExecuteAsync(transactionRequest).ConfigureAwait(false);

            var expectedResponse = transactions.ToResponseWrapper();

            response.Value.Should().BeEquivalentTo(expectedResponse.Value);
        }

        [Fact]
        public async Task GetAllActiveTransactions_GatewayReturnsEmptyList()
        {
            var transactions = Enumerable.Empty<Transaction>();

            var obj = new PagedResult<Transaction>(transactions);

            var transactionRequest = new GetActiveTransactionsRequest() { Page = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow, PeriodEndDate = DateTime.UtcNow };

            _mockGateway.Setup(_ => _.GetAllActive(transactionRequest)).ReturnsAsync(obj);

            var response = await _getAllActiveTransactionsUseCase.ExecuteAsync(transactionRequest).ConfigureAwait(false);

            var expectedResponse = transactions.ToResponseWrapper();

            response.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllActiveTransactions_GatewayReturnsNull()
        {
            var transactions = default(IEnumerable<Transaction>);

            var obj = new PagedResult<Transaction>(transactions);

            obj.Results = null;

            var transactionRequest = new GetActiveTransactionsRequest() { Page = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow, PeriodEndDate = DateTime.UtcNow };

            _mockGateway.Setup(_ => _.GetAllActive(transactionRequest)).ReturnsAsync(obj);

            var response = await _getAllActiveTransactionsUseCase.ExecuteAsync(transactionRequest).ConfigureAwait(false);

            response.Should().BeNull();
        }
    }
}
