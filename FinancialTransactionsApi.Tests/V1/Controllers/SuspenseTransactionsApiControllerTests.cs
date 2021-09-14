using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Controllers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Controllers
{
    public class SuspenseTransactionsApiControllerTests
    {
        private readonly SuspenseTransactionsApiController _sutApiController;
        private readonly Mock<IGetAllSuspenseUseCase> _getAllSuspenseTransactions;
        private readonly Fixture _fixture = new Fixture();
        public SuspenseTransactionsApiControllerTests()
        {
            _getAllSuspenseTransactions = new Mock<IGetAllSuspenseUseCase>();
            _sutApiController = new SuspenseTransactionsApiController(_getAllSuspenseTransactions.Object);
        }

        [Fact]
        public async Task GetAllSuspenseWithValidRequestReturnData()
        {
            TransactionResponses responses = _fixture.Create<TransactionResponses>();

            _getAllSuspenseTransactions.Setup(p => p.ExecuteAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(responses);

            var resultAllSuspense = await _sutApiController.GetAllSuspense(It.IsAny<string>(), It.IsAny<SuspenseTransactionsSearchRequest>()).ConfigureAwait(false);

            resultAllSuspense.Should().NotBeNull();
            Assert.IsType<OkObjectResult>(resultAllSuspense);
            ((OkObjectResult) resultAllSuspense).Value.Should().BeEquivalentTo(responses);
        }

        [Fact]
        public async Task GetAllSuspenseWithValidRequestReturnEmpty()
        {
            TransactionResponses responses = new TransactionResponses();

            _getAllSuspenseTransactions.Setup(p => p.ExecuteAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(responses);

            var resultAllSuspense = await _sutApiController.GetAllSuspense(It.IsAny<string>(), It.IsAny<SuspenseTransactionsSearchRequest>()).ConfigureAwait(false);

            resultAllSuspense.Should().NotBeNull();
            Assert.IsType<OkObjectResult>(resultAllSuspense);
            ((OkObjectResult) resultAllSuspense).Value.Should().BeEquivalentTo(responses);
        }

        [Fact]
        public async Task GetAllSuspenseWithInValidPageNumberRaiseBadRequestException()
        {
            List<TransactionResponse> responses = new List<TransactionResponse>();
            _sutApiController.ModelState.AddModelError("page", "The page number must be great and equal than 1");

            var result = await _sutApiController.GetAllSuspense(It.IsAny<string>(), new SuspenseTransactionsSearchRequest
            {
                Page = _fixture.Create<int>(),
                PageSize = _fixture.Create<int>(),
                SearchText = It.IsAny<string>()
            }).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            _getAllSuspenseTransactions.Verify(p => p.ExecuteAsync(It.IsAny<SuspenseTransactionsSearchRequest>()), Times.Never);
        }

        [Fact]
        public async Task GetAllSuspenseWithInValidPageSizeRaiseBadRequestException()
        {
            List<TransactionResponse> responses = new List<TransactionResponse>();
            _sutApiController.ModelState.AddModelError("pageSize", "The page size must be great and equal than 1");

            var result = await _sutApiController.GetAllSuspense(It.IsAny<string>(), new SuspenseTransactionsSearchRequest
            {
                Page = _fixture.Create<int>(),
                PageSize = _fixture.Create<int>(),
                SearchText = It.IsAny<string>()
            }).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
            _getAllSuspenseTransactions.Verify(p => p.ExecuteAsync(It.IsAny<SuspenseTransactionsSearchRequest>()), Times.Never);
        }
    }
}
