using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class GetAllSuspenseUseCaseTests
    {
        private GetAllSuspenseUseCase _allUseCaseTests;
        private readonly Mock<ITransactionGateway> _gateway = new Mock<ITransactionGateway>();
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task GetAllSuspense_GatewayReturnsList_ReturnsList()
        {
            List<Transaction> transactions = _fixture.CreateMany<Transaction>(5).ToList();

            _gateway.Setup(x => x.GetAllSuspenseAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(transactions);

            _allUseCaseTests = new GetAllSuspenseUseCase(_gateway.Object);

            var result =
                await _allUseCaseTests.ExecuteAsync(new SuspenseTransactionsSearchRequest() { Page = 0, PageSize = 12 })
                    .ConfigureAwait(false);

            result.Should().BeOfType(typeof(List<TransactionResponse>));
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(transactions);
        }

        [Fact]
        public async Task GetAllSuspense_GatewayReturnsEmptyList_ReturnsEmptyList()
        {
            List<Transaction> transactions = new List<Transaction>(0);

            _gateway.Setup(x => x.GetAllSuspenseAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(transactions);

            _allUseCaseTests = new GetAllSuspenseUseCase(_gateway.Object);

            var result =
                await _allUseCaseTests.ExecuteAsync(new SuspenseTransactionsSearchRequest() { Page = 0, PageSize = 12 })
                    .ConfigureAwait(false);

            result.Should().BeOfType(typeof(List<TransactionResponse>));
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(transactions);
        }
    }
}
