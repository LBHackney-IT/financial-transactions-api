using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Gateways;
using TransactionsApi.V1.UseCase;
using Xunit;

namespace TransactionsApi.Tests.V1.UseCase
{
    public class GetAllUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetAllUseCase _classUnderTest;
        private readonly Fixture _fixture;
        public GetAllUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _classUnderTest = new GetAllUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }


        [Fact]
        public async Task GetAllTransactionsAsync()
        {
            var transactions = _fixture.CreateMany<Transaction>().ToList();
            var targetId = Guid.NewGuid();
            var id = Guid.NewGuid().ToString();
            var date = DateTime.Now;
            _mockGateway.Setup(x => x.GetAllTransactionsAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(transactions);
            // Act
            var response = await _classUnderTest.ExecuteAsync(targetId, id, date, date).ConfigureAwait(false);
            var expectedResponse = new TransactionResponseObjectList { ResponseObjects = transactions.ToResponse() };
            // Assert
            response.Should().BeEquivalentTo(expectedResponse);
        }

        //TODO: Add extra tests here for extra functionality added to the use case
    }
}
