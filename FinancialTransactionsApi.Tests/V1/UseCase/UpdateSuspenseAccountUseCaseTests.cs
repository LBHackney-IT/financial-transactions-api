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
    public class UpdateSuspenseAccountUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly UpdateSuspenseAccountUseCase _updateUseCase;
        private readonly Mock<ISnsGateway> _mockSnsGateway;
        private readonly Mock<ISnsFactory> _mockSnsFactory;

        public UpdateSuspenseAccountUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _mockSnsFactory = new Mock<ISnsFactory>();
            _mockSnsGateway = new Mock<ISnsGateway>();
            _updateUseCase = new UpdateSuspenseAccountUseCase(_mockGateway.Object, _mockSnsGateway.Object, _mockSnsFactory.Object);
        }

        [Fact]
        public async Task Update_WithSuspenseModel_WorkedOnce()
        {
            var entity = new Transaction()
            {
                Id = Guid.NewGuid(),
                TargetId = Guid.NewGuid(),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                PaidAmount = 123.22M,
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                FinancialMonth = 8,
                FinancialYear = 2021,
                TransactionType = TransactionType.ArrangementInterest,
                Sender = new Sender()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                },
                CreatedAt = new DateTime(2021, 8, 1),
                CreatedBy = "admin",
                LastUpdatedBy = "new admin"
            };

            _mockGateway.Setup(x => x.UpdateSuspenseAccountAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _updateUseCase.ExecuteAsync(entity)
                .ConfigureAwait(false);

            var expectedResponse = entity.ToResponse();

            _mockGateway.Verify(_ => _.UpdateSuspenseAccountAsync(It.IsAny<Transaction>()), Times.Once);

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth)
                                                                         .Excluding(x => x.LastUpdatedAt));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);
            response.LastUpdatedAt.Should().BeCloseTo(DateTime.UtcNow, 1000);

            _mockSnsFactory.Verify(x => x.Create(It.IsAny<Transaction>()));
            _mockSnsGateway.Verify(x => x.Publish(It.IsAny<TransactionSns>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task Update_WithNotSuspenseModel_WorkedOnce()
        {
            var entity = new Transaction()
            {
                Id = Guid.NewGuid(),
                TargetId = Guid.NewGuid(),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                FinancialMonth = 8,
                FinancialYear = 2021,
                TransactionType = TransactionType.ArrangementInterest,
                Sender = new Sender()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                },
                SuspenseResolutionInfo = new SuspenseResolutionInfo()
                {
                    ResolutionDate = DateTime.UtcNow,
                    IsConfirmed = true,
                    IsApproved = true,
                    Note = "Note"
                },
                CreatedAt = new DateTime(2021, 8, 1),
                CreatedBy = "admin",
                LastUpdatedBy = "new admin"
            };

            _mockGateway.Setup(x => x.UpdateSuspenseAccountAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _updateUseCase.ExecuteAsync(entity)
                .ConfigureAwait(false);

            var expectedResponse = entity.ToResponse();

            _mockGateway.Verify(_ => _.UpdateSuspenseAccountAsync(It.IsAny<Transaction>()), Times.Once);
            _mockSnsFactory.Verify(x => x.Create(It.IsAny<Transaction>()));
            _mockSnsGateway.Verify(x => x.Publish(It.IsAny<TransactionSns>(), It.IsAny<string>(), It.IsAny<string>()));

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth)
                                                                         .Excluding(x => x.LastUpdatedAt));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);
            response.LastUpdatedAt.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }


    }
}
