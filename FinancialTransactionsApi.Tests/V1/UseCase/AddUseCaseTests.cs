using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using FluentAssertions;
using Hackney.Core.Sns;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class AddUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly Mock<ISnsGateway> _mockSnsGateway;
        private readonly Mock<ISnsFactory> _mockSnsFactory;
        private readonly AddUseCase _addUseCase;
        private readonly Fixture _fixture = new Fixture();

        public AddUseCaseTests()
        {
            _mockSnsFactory = new Mock<ISnsFactory>();
            _mockSnsGateway = new Mock<ISnsGateway>();
            _mockGateway = new Mock<ITransactionGateway>();
            _addUseCase = new AddUseCase(_mockGateway.Object, _mockSnsGateway.Object, _mockSnsFactory.Object);
        }

        [Fact]
        public async Task Add_WithSuspenseModel_ReturnTransaction()
        {
            var entity = new Transaction()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                PaidAmount = 123.22M,
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionType = TransactionType.ArrangementInterest,
                CreatedBy = "admin"
            };


            _mockGateway.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _addUseCase.ExecuteAsync(entity)
                .ConfigureAwait(false);

            var expectedResponse = entity.ToResponse();

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth)
                                                                         .Excluding(x => x.CreatedAt)
                                                                         .Excluding(x => x.LastUpdatedAt)
                                                                         .Excluding(x => x.LastUpdatedBy));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);

            var curDate = DateTime.UtcNow;
            response.LastUpdatedBy.Should().Be(expectedResponse.CreatedBy);
            response.CreatedAt.Should().BeCloseTo(curDate, 1000);
            response.LastUpdatedAt.Should().BeCloseTo(curDate, 1000);

            _mockGateway.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task Add_WithNotSuspenseModel_ReturnTransaction()
        {
            var entity = new Transaction()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                PaidAmount = 123.22M,
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.ArrangementInterest,
                CreatedBy = "admin"
            };

            _mockGateway.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _addUseCase.ExecuteAsync(entity)
                .ConfigureAwait(false);

            var expectedResponse = entity.ToResponse();

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth)
                                                                         .Excluding(x => x.CreatedAt)
                                                                         .Excluding(x => x.LastUpdatedAt)
                                                                         .Excluding(x => x.LastUpdatedBy));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);

            _mockGateway.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }


        [Fact]
        public async Task CreateTransactionPublishes()
        {
            // Arrange
            var transaction = _fixture.Create<TransactionResponse>();
            var request = new Transaction
            {
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 154.12M,
                ChargedAmount = 123.78M,
                BankAccountNumber = "12345678",
                PaidAmount = 125.62M,
                PeriodNo = 31,
                TargetId = new Guid("9e067bac-56ed-4802-a83f-b1e32f09177e"),
                TransactionAmount = 186.90M,
                TransactionSource = "DD",
                TransactionType = TransactionType.ArrangementInterest,
                CreatedBy = "Admin"
            };

            _mockGateway.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            // Act
            _ = await _addUseCase.ExecuteAsync(request).ConfigureAwait(false);

            // Assert
            _mockSnsFactory.Verify(x => x.Create(It.IsAny<Transaction>()));
            _mockSnsGateway.Verify(x => x.Publish(It.IsAny<TransactionSns>(), It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
