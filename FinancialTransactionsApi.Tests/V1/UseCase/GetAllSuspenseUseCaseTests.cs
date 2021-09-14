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
            /*List<Transaction> transactions = _fixture.CreateMany<Transaction>(5).ToList();*/
            TransactionList transactionList = _fixture.Build<TransactionList>()
                .With(p => p.Transactions, _fixture.CreateMany<Transaction>(5).ToList())
                .With(p => p.Total, 5).Create();

            _gateway.Setup(x => x.GetAllSuspenseAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(transactionList);

            _allUseCaseTests = new GetAllSuspenseUseCase(_gateway.Object);

            var result =
                await _allUseCaseTests.ExecuteAsync(new SuspenseTransactionsSearchRequest() { Page = 0, PageSize = 12 })
                    .ConfigureAwait(false);

            result.Should().BeOfType(typeof(TransactionResponses));
            result.Should().NotBeNull();
            result.TransactionsList.Should().BeEquivalentTo(transactionList.Transactions);
            result.Total.Should().Be(5);
        }

        [Fact]
        public async Task GetAllSuspense_GatewayReturnsEmptyList_ReturnsEmptyList()
        {
            TransactionList transactionsList = new TransactionList
            {
                Transactions = new List<Transaction>(0),
                Total = 0
            };

            _gateway.SetupSequence(x => x.GetAllSuspenseAsync(It.IsAny<SuspenseTransactionsSearchRequest>()))
                .ReturnsAsync(transactionsList)
                .ReturnsAsync(transactionsList);

            _allUseCaseTests = new GetAllSuspenseUseCase(_gateway.Object);

            var result =
                await _allUseCaseTests.ExecuteAsync(new SuspenseTransactionsSearchRequest() { Page = 0, PageSize = 12 })
                    .ConfigureAwait(false);

            result.Should().BeOfType(typeof(TransactionResponses));
            result.Should().NotBeNull();
            result.TransactionsList.Should().BeEquivalentTo(transactionsList.Transactions);
            result.Total.Should().Be(0);
        }
    }
}
