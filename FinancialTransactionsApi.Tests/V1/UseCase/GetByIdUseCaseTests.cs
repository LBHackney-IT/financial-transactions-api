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
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Helpers;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetByIdUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly GetByIdUseCase _getByIdUseCase;
        private readonly Fixture _fixture = new Fixture();

        public GetByIdUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _getByIdUseCase = new GetByIdUseCase(_mockGateway.Object);
        }


        [Fact]
        public async Task GetById_GatewayReturnTransaction_ReturnTransaction()
        {
            var id = Guid.NewGuid();

            var transaction = _fixture.Create<Transaction>();

            _mockGateway.Setup(x => x.GetTransactionByIdAsync(id)).ReturnsAsync(transaction);
            var response = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);

            response.Should().BeEquivalentTo(transaction.ToResponseWrapper());
        }

        [Fact]
        public async Task GetById_GatewayReturnNull_ReturnNull()
        {
            var id = Guid.NewGuid();
            var targetId = Guid.NewGuid();
            var queryParam = new TransactionByIdQueryParameter { Id = id };
            _mockGateway.Setup(x => x.GetTransactionByIdAsync(id)).ReturnsAsync((Transaction) null);

            var response = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);

            response.Should().BeNull();
        }
    }
}
