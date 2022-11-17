using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Controllers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FinancialTransactionsApi.V1.Boundary.Request;
using FluentAssertions;
using System.Linq;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;

namespace FinancialTransactionsApi.Tests.V1.Controllers
{
    public class StatementApiControllerTests
    {
        private readonly StatementController _controller;
        private readonly Mock<IGetByTargetIdsUseCase> _getByTargetIdsUseCase;
        private readonly Fixture _fixture = new Fixture();

        public StatementApiControllerTests()
        {
            _getByTargetIdsUseCase = new Mock<IGetByTargetIdsUseCase>();

            _controller = new StatementController(_getByTargetIdsUseCase.Object);
        }

        [Fact]
        public async Task GetByTargetId_UseCaseReturnTransactionByTargetId_ShouldReturns200()
        {
            var transactionsList = _fixture.Build<TransactionResponse>().CreateMany(5);

            var responseMock = new PaginatedResponse<TransactionResponse>() { Results = transactionsList };

            _getByTargetIdsUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionByTargetIdsQuery>())).ReturnsAsync(responseMock);

            var result = await _controller.GetByTargetId(It.IsAny<TransactionByTargetIdsQuery>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var transaction = okResult?.Value as PaginatedResponse<TransactionResponse>;

            transaction.Results.Should().NotBeNull();

            transaction.Results.Should().BeEquivalentTo(transactionsList);
        }

        [Fact]
        public async Task GetByTargetId_UseCaseReturnTransactionByTargetId_ShouldReturns404()
        {
            IEnumerable<TransactionResponse> transactionsList = Enumerable.Empty<TransactionResponse>();

            var responseMock = new PaginatedResponse<TransactionResponse>() { Results = transactionsList };

            _getByTargetIdsUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionByTargetIdsQuery>())).ReturnsAsync(responseMock);

            var result = await _controller.GetByTargetId(It.IsAny<TransactionByTargetIdsQuery>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var transaction = okResult?.Value as PaginatedResponse<TransactionResponse>;

            transaction.Results.Should().NotBeNull();

            transaction.Results.Should().BeEmpty();
        }
    }
}
