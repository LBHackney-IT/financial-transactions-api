using FinancialTransactionsApi.Tests.V1.Helper;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.UseCase
{
    public class UpdateUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly UpdateUseCase _updateUseCase;

        public UpdateUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _updateUseCase = new UpdateUseCase(_mockGateway.Object);
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
                IsSuspense = true,
                PaidAmount = 123.22M,
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                },
                CreatedAt = new DateTime(2021, 8, 1),
                CreatedBy = "admin",
                LastUpdatedBy = "new admin"
            };

            _mockGateway.Setup(x => x.UpdateAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _updateUseCase.ExecuteAsync(entity, Guid.NewGuid())
                .ConfigureAwait(false);

            var expectedResponse = entity.ToResponse();

            _mockGateway.Verify(_ => _.UpdateAsync(It.IsAny<Transaction>()), Times.Once);

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth)
                                                                         .Excluding(x => x.LastUpdatedAt));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);
            response.LastUpdatedAt.Should().BeCloseTo(DateTime.UtcNow, 1000);
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
                IsSuspense = false,
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person()
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

            _mockGateway.Setup(x => x.UpdateAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _updateUseCase.ExecuteAsync(entity, Guid.NewGuid())
                .ConfigureAwait(false);

            var expectedResponse = entity.ToResponse();

            _mockGateway.Verify(_ => _.UpdateAsync(It.IsAny<Transaction>()), Times.Once);

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
