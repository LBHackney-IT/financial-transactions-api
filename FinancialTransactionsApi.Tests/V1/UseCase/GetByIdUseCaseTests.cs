using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using Xunit;
using FinancialTransactionsApi.V1.Boundary.Request;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetByIdUseCaseTests
    {
        private readonly Mock<IPostgresDbGateway> _mockGateway;
        private readonly GetByIdUseCase _getByIdUseCase;
        private readonly Fixture _fixture = new Fixture();

        public GetByIdUseCaseTests()
        {
            _mockGateway = new Mock<IPostgresDbGateway>();
            _getByIdUseCase = new GetByIdUseCase(_mockGateway.Object);
        }


        [Fact]
        public async Task GetById_GatewayReturnTransaction_ReturnTransaction()
        {
            var id = Guid.NewGuid();
            var targetId = Guid.NewGuid();

            var transaction = _fixture.Create<Transaction>();

            _mockGateway.Setup(x => x.GetTransactionByIdAsync(targetId, id)).ReturnsAsync(transaction);
            var response = await _getByIdUseCase.ExecuteAsync(id, targetId).ConfigureAwait(false);

            response.Should().BeEquivalentTo(transaction.ToResponse());
        }

        [Fact]
        public async Task GetById_GatewayReturnNull_ReturnNull()
        {
            var id = Guid.NewGuid();
            var targetId = Guid.NewGuid();
            var queryParam = new TransactionByIdQueryParameter { Id = id, TargetId = targetId };
            _mockGateway.Setup(x => x.GetTransactionByIdAsync(targetId, id)).ReturnsAsync((Transaction) null);

            var response = await _getByIdUseCase.ExecuteAsync(id, targetId).ConfigureAwait(false);

            response.Should().BeNull();
        }
    }
}
