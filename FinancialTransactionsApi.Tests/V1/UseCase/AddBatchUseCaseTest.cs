using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.Sns;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using FinancialTransactionsApi.Tests.V1.Domain;
using AutoFixture;
using System.Linq;
using Amazon.XRay.Recorder.Core.Internal.Entities;
using System.Collections.Generic;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class AddBatchUseCaseTest
    {
        private readonly Mock<ISnsGateway> _mockSnsGateway;
        private readonly Mock<ISnsFactory> _mockSnsFactory;
        private readonly AddBatchUseCase _addBatchUseCase;
        private readonly Fixture _fixture;

        public AddBatchUseCaseTest()
        {
            _fixture = new Fixture();
            _mockSnsFactory = new Mock<ISnsFactory>();
            _mockSnsGateway = new Mock<ISnsGateway>();
            _addBatchUseCase = new AddBatchUseCase(_mockSnsGateway.Object, _mockSnsFactory.Object);
        }

        [Fact]
        public async Task AddBatchUseCase_ReturnNumber()
        {
            var transactionList = _fixture.CreateMany<Transaction>(5).ToList();

            transactionList.ForEach(item => { item.IsSuspense = true; });

            int response = await _addBatchUseCase.ExecuteAsync(transactionList).ConfigureAwait(false);

            Assert.NotEqual(0, response);
        }
    }
}
