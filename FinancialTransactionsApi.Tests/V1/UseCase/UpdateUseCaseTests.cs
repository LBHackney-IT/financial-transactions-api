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
            var entity = new UpdateTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
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
                }
            };

            _mockGateway.Setup(x => x.UpdateAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _updateUseCase.ExecuteAsync(entity, Guid.NewGuid())
                .ConfigureAwait(false);

            var expectedResponse = entity.ToDomain().ToResponse();

            _mockGateway.Verify(_ => _.UpdateAsync(It.IsAny<Transaction>()), Times.Once);

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);
        }

        [Fact]
        public async Task Update_WithNotSuspenseModel_WorkedOnce()
        {
            var entity = new UpdateTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
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
                    IsResolve = true,
                    Note = "Note"
                }
            };

            _mockGateway.Setup(x => x.UpdateAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _updateUseCase.ExecuteAsync(entity, Guid.NewGuid())
                .ConfigureAwait(false);

            var expectedResponse = entity.ToDomain().ToResponse();

            _mockGateway.Verify(_ => _.UpdateAsync(It.IsAny<Transaction>()), Times.Once);

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);
        }

        [Fact]
        public async Task Update_WithInvalidSuspenseInformation_ThrowException()
        {
            var entity = new UpdateTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                HousingBenefitAmount = 123.12M,
                IsSuspense = false,
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                }
            };

            try
            {
                await _updateUseCase.ExecuteAsync(entity, Guid.NewGuid()).ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType(typeof(ArgumentException));
                ex.Message.Should().BeEquivalentTo("Transaction model dont have all information in fields!");
            }
        }
    }
}
