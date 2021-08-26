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
    public class AddUseCaseTests
    {
        private readonly Mock<ITransactionGateway> _mockGateway;
        private readonly AddUseCase _addUseCase;

        public AddUseCaseTests()
        {
            _mockGateway = new Mock<ITransactionGateway>();
            _addUseCase = new AddUseCase(_mockGateway.Object);
        }

        [Fact]
        public async Task Add_WithSuspenseModel_ReturnTransaction()
        {
            var entity = new AddTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
                PaidAmount = 123.22M,
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionType = TransactionType.Charge,
                Person = new Person()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                }
            };

            _mockGateway.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _addUseCase.ExecuteAsync(entity)
                .ConfigureAwait(false);

            var expectedResponse = entity.ToDomain().ToResponse();

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);

            _mockGateway.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task Add_WithNotSuspenseModel_ReturnTransaction()
        {
            var entity = new AddTransactionRequest()
            {
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
                }
            };

            _mockGateway.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            var response = await _addUseCase.ExecuteAsync(entity)
                .ConfigureAwait(false);

            var expectedResponse = entity.ToDomain().ToResponse();

            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id)
                                                                         .Excluding(x => x.FinancialYear)
                                                                         .Excluding(x => x.FinancialMonth));

            response.FinancialMonth.Should().Be(8);
            response.FinancialYear.Should().Be(2021);

            _mockGateway.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task Add_WithInvalidSuspenseInformation_ThrowException()
        {
            var entity = new AddTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
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
                var response = await _addUseCase.ExecuteAsync(entity).ConfigureAwait(false);
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
