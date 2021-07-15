using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Gateways;
using TransactionsApi.V1.UseCase;
using Xunit;

namespace TransactionsApi.Tests.V1.UseCase
{
    public class GetByIdUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();
        public GetByIdUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _classUnderTest = new GetByIdUseCase(_mockGateway.Object);
        }


        [Fact]
        public async Task GetTransactionByIdAsyncReturnsSuccessResponse()
        {
            // Arrange
            var query = Guid.NewGuid();
            var transaction = _fixture.Create<Transaction>();
            _mockGateway.Setup(x => x.GetTransactionByIdAsync(query)).ReturnsAsync(transaction);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(transaction.ToResponse());
        }
        
    }
}
