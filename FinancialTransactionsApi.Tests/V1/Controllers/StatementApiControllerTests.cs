using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Controllers;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xunit;
using FinancialTransactionsApi.V1.Boundary.Request;
using Hackney.Core.DynamoDb;
using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using System.Linq;

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
            var transactionsList = _fixture.Build<Transaction>().CreateMany(5);

            var responseMock = new PagedResult<Transaction>(transactionsList);

            _getByTargetIdsUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionByTargetIdsQuery>())).ReturnsAsync(responseMock);

            var result = await _controller.GetByTargetId(It.IsAny<TransactionByTargetIdsQuery>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var transaction = okResult?.Value as PagedResult<Transaction>;

            transaction.Should().NotBeNull();

            transaction.Results.Should().BeEquivalentTo(transactionsList);
        }

        [Fact]
        public async Task GetByTargetId_UseCaseReturnTransactionByTargetId_ShouldReturns404()
        {
            IEnumerable<Transaction> transactionsList = Enumerable.Empty<Transaction>();

            var responseMock = new PagedResult<Transaction>(transactionsList);

            _getByTargetIdsUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionByTargetIdsQuery>())).ReturnsAsync(responseMock);

            var result = await _controller.GetByTargetId(It.IsAny<TransactionByTargetIdsQuery>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var transaction = okResult?.Value as PagedResult<Transaction>;

            transaction.Should().NotBeNull();

            transaction.Results.Should().BeEmpty();
        }
    }
}
