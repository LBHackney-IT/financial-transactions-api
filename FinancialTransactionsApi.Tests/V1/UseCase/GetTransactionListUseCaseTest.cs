using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways.ElasticSearch;
using FinancialTransactionsApi.V1.UseCase;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetTransactionListUseCaseTest
    {
        private readonly Mock<ISearchGateway> _mockGateway;
        private readonly GetTransactionListUseCase _getTransactionListUseCase;
        private readonly Fixture _fixture;

        public GetTransactionListUseCaseTest()
        {
            _mockGateway = new Mock<ISearchGateway>();
            _getTransactionListUseCase = new GetTransactionListUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAll_GatewayReturnsList_ReturnsList()
        {
            var transactions = _fixture.Create<GetTransactionListResponse>();
            transactions.SetTotal(transactions.Transactions.Count);


            var transactionQuery = new TransactionSearchRequest
            {
                SearchText = "testing",
                Page = 0,
                PageSize = 3,

            };

            _mockGateway.Setup(x => x.GetListOfTransactions(It.IsAny<TransactionSearchRequest>())).ReturnsAsync(transactions);

            var response = await _getTransactionListUseCase.ExecuteAsync(transactionQuery).ConfigureAwait(false);



            response.Transactions.Should().BeEquivalentTo(transactions.Transactions);
            response.Total().Should().Be(transactions.Transactions.Count);
        }
    }
}
