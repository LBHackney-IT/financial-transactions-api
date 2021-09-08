using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
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
            List<TransactionResponse> responses = _fixture.Create<List<TransactionResponse>>();

            _getAllSuspenseTransactions.Setup(p => p.ExecuteAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(responses);

            var resultAllSuspense = await _sutApiController.GetAllSuspense(It.IsAny<SuspenseTransactionsSearchRequest>()).ConfigureAwait(false);

            resultAllSuspense.Should().NotBeNull();
            Assert.IsType<OkObjectResult>(resultAllSuspense);
            ((OkObjectResult) resultAllSuspense).Value.Should().BeEquivalentTo(responses);
        }

        [Fact]
        public async Task GetAllSuspenseWithValidRequestReturnEmpty()
        {
            List<TransactionResponse> responses = new List<TransactionResponse>(); 

            _getAllSuspenseTransactions.Setup(p => p.ExecuteAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(responses);

            var resultAllSuspense = await _sutApiController.GetAllSuspense(It.IsAny<SuspenseTransactionsSearchRequest>()).ConfigureAwait(false);

            resultAllSuspense.Should().NotBeNull();
            Assert.IsType<OkObjectResult>(resultAllSuspense);
            ((OkObjectResult) resultAllSuspense).Value.Should().BeEquivalentTo(responses);
        }
    }
}
