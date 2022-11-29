using System.Linq;
using System.Collections.Generic;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using AutoFixture;
using FluentAssertions;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetByTargetIdUseCaseTest
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetByTargetIdUseCase _getByTargetIdUseCase;
        private readonly Fixture _fixture;

        public GetByTargetIdUseCaseTest()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _getByTargetIdUseCase = new GetByTargetIdUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllActiveTransactions_GatewayReturnsList()
        {
            var responseMock = _fixture.CreateMany<Transaction>(5);

            _mockGateway.Setup(_ => _.GetByTargetId(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(responseMock);

            var response = await _getByTargetIdUseCase.ExecuteAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).ConfigureAwait(false);

            var expectedResponse = responseMock.ToResponseWrapper();

            response.Value.Should().BeEquivalentTo(expectedResponse.Value);
        }

        [Fact]
        public async Task GetAllActiveTransactions_GatewayReturnsEmptyList()
        {
            var responseMock = Enumerable.Empty<Transaction>();

            _mockGateway.Setup(_ => _.GetByTargetId(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(responseMock);

            var response = await _getByTargetIdUseCase.ExecuteAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).ConfigureAwait(false);

            response.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllActiveTransactions_GatewayReturnsNull()
        {
            IEnumerable<Transaction> responseMock = null;

            _mockGateway.Setup(_ => _.GetByTargetId(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(responseMock);

            var response = await _getByTargetIdUseCase.ExecuteAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).ConfigureAwait(false);

            response.Should().BeNull();
        }
    }
}
