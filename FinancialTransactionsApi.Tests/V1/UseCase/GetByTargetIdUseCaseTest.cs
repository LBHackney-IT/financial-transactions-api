using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using Hackney.Core.DynamoDb;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using FinancialTransactionsApi.V1.Factories;
using System.Linq;

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

            _mockGateway.Setup(_ => _.GetByTargetId("Tenure", Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow)).ReturnsAsync(responseMock);

            var response = await _getByTargetIdUseCase.ExecuteAsync("Tenure", Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow).ConfigureAwait(false);

            var expectedResponse = responseMock.ToResponseWrapper();

            expectedResponse.Value.Should().BeEquivalentTo(expectedResponse.Value);
        }
    }
}
