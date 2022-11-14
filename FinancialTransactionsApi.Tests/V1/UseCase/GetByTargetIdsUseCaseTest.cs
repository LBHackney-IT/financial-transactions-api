using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetByTargetIdsUseCaseTest
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetByTargetIdsUseCase _getByTargetIdsUseCase;
        private readonly Fixture _fixture;

        public GetByTargetIdsUseCaseTest()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _getByTargetIdsUseCase = new GetByTargetIdsUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetByTargetIds_GatewayReturnTransactionResponse_ReturnTransactionResponse()
        {
            var transactionsList = _fixture.Build<Transaction>().CreateMany(5);

            var responseMock = new Paginated<Transaction>() { Results = transactionsList };

            _mockGateway.Setup(x => x.GetPagedTransactionsByTargetIdsAsync(It.IsAny<TransactionByTargetIdsQuery>())).ReturnsAsync(responseMock);

            var response = await _getByTargetIdsUseCase.ExecuteAsync(It.IsAny<TransactionByTargetIdsQuery>()).ConfigureAwait(false);

            response.Should().BeEquivalentTo(responseMock.ToResponse());
        }

        [Fact]
        public async Task GetByTargetIds_GatewayReturnEmptyTransactionResponse_ReturnEmptyTransactionResponse()
        {
            var transactionsList = Enumerable.Empty<Transaction>();

            var responseMock = new Paginated<Transaction>() { Results = transactionsList };

            _mockGateway.Setup(x => x.GetPagedTransactionsByTargetIdsAsync(It.IsAny<TransactionByTargetIdsQuery>())).ReturnsAsync(responseMock);

            var response = await _getByTargetIdsUseCase.ExecuteAsync(It.IsAny<TransactionByTargetIdsQuery>()).ConfigureAwait(false);

            var expectedResponse = responseMock.ToResponse();

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
