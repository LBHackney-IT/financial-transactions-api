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
using FinancialTransactionsApi.V1.Helpers.GeneralModels;

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
                Results = transactions
            };

            var transactionRequest = new GetActiveTransactionsRequest() { PageNumber = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow, PeriodEndDate = DateTime.UtcNow };

            _mockGateway.Setup(x => x.GetAllActive(transactionRequest)).ReturnsAsync(obj);

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
                Results = transactions
            };

            var transactionRequest = new GetActiveTransactionsRequest() { PageNumber = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow, PeriodEndDate = DateTime.UtcNow };

            _mockGateway.Setup(_ => _.GetAllActive(transactionRequest)).ReturnsAsync(obj);

            var response = await _getAllActiveTransactionsUseCase.ExecuteAsync(transactionRequest).ConfigureAwait(false);

            var expectedResponse = obj.ToResponse();

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
