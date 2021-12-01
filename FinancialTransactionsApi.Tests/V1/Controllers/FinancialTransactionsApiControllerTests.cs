using AutoFixture;
using FinancialTransactionsApi.Tests.V1.Helper;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Controllers;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FinancialTransactionsApi.Tests.V1.Controllers
{
    public class FinancialTransactionsApiControllerTests
    {
        private readonly FinancialTransactionsApiController _controller;
        private readonly Mock<IGetByIdUseCase> _getByIdUseCase;
        private readonly Mock<IGetAllUseCase> _getAllUseCase;
        private readonly Mock<IAddUseCase> _addUseCase;
        private readonly Mock<IUpdateUseCase> _updateUseCase;
        private readonly Mock<IAddBatchUseCase> _addBatchUseCase;
        private readonly Mock<IGetTransactionListUseCase> _getTransactionListUseCase;
        private readonly Fixture _fixture = new Fixture();
        private const string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoidGVzdGluZyJ9.jw6U8mE-CxkLQbsCaJMaWXVArXHw0pT_Puo9hCPbN-g";

        public FinancialTransactionsApiControllerTests()
        {
            _getByIdUseCase = new Mock<IGetByIdUseCase>();
            _getAllUseCase = new Mock<IGetAllUseCase>();
            _addUseCase = new Mock<IAddUseCase>();
            _updateUseCase = new Mock<IUpdateUseCase>();
            _addBatchUseCase = new Mock<IAddBatchUseCase>();
            _getTransactionListUseCase = new Mock<IGetTransactionListUseCase>();
            _controller = new FinancialTransactionsApiController(_getAllUseCase.Object,
                _getByIdUseCase.Object, _addUseCase.Object, _updateUseCase.Object, _addBatchUseCase.Object, _getTransactionListUseCase.Object);
        }

        [Fact]
        public async Task GetById_UseCaseReturnTransactionByValidId_ShouldReturns200()
        {
            var transactionResponse = _fixture.Create<TransactionResponse>();

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponse);

            var result = await _controller.Get("", transactionResponse.Id).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var transaction = okResult.Value as TransactionResponse;

            transaction.Should().NotBeNull();

            transaction.Should().BeEquivalentTo(transactionResponse);
        }

        [Fact]
        public async Task GetById_UseCaseReturnNullWithInvalidId_ShouldReturns404()
        {
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TransactionResponse) null);

            var result = await _controller.Get("", Guid.NewGuid()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();

            var response = notFoundResult.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response.StatusCode.Should().Be((int) HttpStatusCode.NotFound);

            response.Message.Should().BeEquivalentTo("No transaction by provided Id cannot be found!");

            response.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task GetById_UseCaseThrownException_ShouldRethrow()
        {
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var result = await _controller.Get("", new Guid("6791051d-961d-4e16-9853-6e7e45b01b49"))
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(Exception));
                ex.Message.Should().Be("Test exception");
            }
        }

        [Fact]
        public async Task GetAll_UseCaseReturnList_Returns200()
        {
            var transactionsList = _fixture.Build<TransactionResponse>().CreateMany(5);

            var obj1 = _fixture.Build<TransactionResponses>()
                .With(s => s.Total, 5)
                .With(s => s.TransactionsList, transactionsList)
                .Create();

            _getAllUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionQuery>()))
                .ReturnsAsync(obj1);

            var query = new TransactionQuery()
            {
                TargetId = Guid.NewGuid()
            };

            var result = await _controller.GetAll("", query).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult?.Value.Should().BeOfType<TransactionResponses>();

            var responses = okResult?.Value as TransactionResponses;

            responses?.TransactionsList.Should().HaveCount(5);
            responses?.Total.Should().Be(5);

        }

        [Fact]
        public async Task GetAll_UseCaseThrownException_ShouldRethrow()
        {
            _getAllUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionQuery>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var result = await _controller.GetAll("", new TransactionQuery { })
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(Exception));
                ex.Message.Should().Be("Test exception");
            }
        }

        [Fact]
        public async Task GetAll_ModelError_ThrowsException()
        {
            _controller.ModelState.AddModelError("TargetId", "The field TargetId cannot be empty or default.");

            var result = await _controller.GetAll(_fixture.Create<Guid>().ToString(), new TransactionQuery())
                .ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();

        }

        [Fact]
        public async Task Add_UseCaseReturnModel_Returns200()
        {
            var guid = Guid.NewGuid();

            var request = new AddTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
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

            var expectedResponse = new TransactionResponse()
            {
                Id = guid,
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                FinancialMonth = 2,
                FinancialYear = 2022,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                IsSuspense = true,
                BankAccountNumber = "12345678",
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
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "testing",
                CreatedBy = "testing"
            };

            _addUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Add("", _token, request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var createResult = result as CreatedAtActionResult;

            createResult.Should().NotBeNull();

            createResult.StatusCode.Should().Be((int) HttpStatusCode.Created);

            createResult.RouteValues.Should().NotBeNull();

            createResult.RouteValues.Should().HaveCount(1);

            createResult.RouteValues["id"].Should().NotBeNull();

            createResult.RouteValues["id"].Should().Be(guid);

            var responseObject = createResult.Value as TransactionResponse;

            responseObject.Should().NotBeNull();

            responseObject.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Add_WithNullModel_Returns400()
        {
            var result = await _controller.Add("", _token, null).ConfigureAwait(false);

            result.Should().NotBeNull();

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            var response = badRequestResult.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

            response.Message.Should().BeEquivalentTo("Transaction model cannot be null!");

            response.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task Add_WithInvalidModel_Returns400()
        {
            var request = new AddTransactionRequest()
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
            var result = await _controller.Add("", _token, request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            var response = badRequestResult.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

            response.Message.Should().BeEquivalentTo("Transaction model dont have all information in fields!");

            response.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task Add_UseCaseThrownException_ShouldRethrow()
        {
            var request = new AddTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
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
            _addUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var result = await _controller.Add("", _token, request)
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(Exception));
                ex.Message.Should().Be("Test exception");
            }
        }

        [Fact]
        public async Task Add_TokenIsNull_ShouldThrowArgumentNullException()
        {
            var request = new AddTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
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
            
            try
            {
                var result = await _controller.Add("", null, request)
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch(Exception ex)
            {
                _addUseCase.Verify(_ => _.ExecuteAsync(It.IsAny<Transaction>()), Times.Never);
                ex.GetType().Should().Be(typeof(ArgumentNullException));
                ex.Message.Should().Be("Value cannot be null. (Parameter 'token')");
            }
        }

        [Fact]
        public async Task Update_WithValidModel_Returns200()
        {
            var guid = Guid.NewGuid();

            var request = new UpdateTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
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
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new TransactionResponse() { Id = guid, IsSuspense = true });

            _updateUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>(), It.IsAny<Guid>()))
                .ReturnsAsync(request.ToDomain().ToResponse());

            var result = await _controller.Update("", _token, guid, request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult?.Value.Should().BeOfType(typeof(TransactionResponse));

            okResult?.Value.Should().BeEquivalentTo(request);
        }

        [Fact]
        public async Task Update_WithInvalidModel_Returns400()
        {
            var request = new UpdateTransactionRequest()
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
            var result = await _controller.Update("", _token, Guid.NewGuid(), request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            var response = badRequestResult?.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

            response?.Message.Should().BeEquivalentTo("Transaction model dont have all information in fields!");

            response?.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task Update_WithNullModel_Returns400()
        {
            var result = await _controller.Update("", _token, Guid.NewGuid(), null).ConfigureAwait(false);

            result.Should().NotBeNull();

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            var response = badRequestResult?.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

            response?.Message.Should().BeEquivalentTo("Transaction model cannot be null!");

            response?.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task Update_NotFoundEntityWithProvidedId_Returns404()
        {
            var request = new UpdateTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
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
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TransactionResponse) null);

            var result = await _controller.Update("", _token, Guid.NewGuid(), request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();

            var response = notFoundResult.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response.StatusCode.Should().Be((int) HttpStatusCode.NotFound);

            response.Message.Should().BeEquivalentTo("No transaction by provided Id cannot be found!");

            response.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task Update_UseCaseThrownException_ShouldRethrow()
        {
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new TransactionResponse { IsSuspense = true });

            _updateUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var request = new UpdateTransactionRequest()
                {
                    TargetId = Guid.NewGuid(),
                    TransactionDate = DateTime.UtcNow,
                    Address = "Address",
                    BalanceAmount = 145.23M,
                    ChargedAmount = 134.12M,
                    Fund = "HSGSUN",
                    HousingBenefitAmount = 123.12M,
                    BankAccountNumber = "12345678",
                    IsSuspense = true,
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
                var result = await _controller.Update("", _token, Guid.NewGuid(), request)
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(Exception));
                ex.Message.Should().Be("Test exception");
            }
        }

        [Fact]
        public async Task Update_TokenIsNull_ShouldThrowArgumentNullException()
        {
            var request = new UpdateTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
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
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new TransactionResponse { IsSuspense = true });

            try
            {
                var result = await _controller.Update("", null, Guid.NewGuid(), request)
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                _updateUseCase.Verify(_ => _.ExecuteAsync(It.IsAny<Transaction>(), It.IsAny<Guid>()), Times.Never);
                ex.GetType().Should().Be(typeof(ArgumentNullException));
                ex.Message.Should().Be("Value cannot be null. (Parameter 'token')");
            }
        }

        [Fact]
        public async Task Update_NonSuspenseTransaction_ThrowBadRequest()
        {
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new TransactionResponse { IsSuspense = false });

            var request = _fixture.Build<UpdateTransactionRequest>()
                .With(s => s.IsSuspense, false).Create();

            var result = await _controller.Update("", _token, Guid.NewGuid(), request)
                .ConfigureAwait(false);

            _updateUseCase.Verify(_ => _.ExecuteAsync(It.IsAny<Transaction>(), It.IsAny<Guid>()), Times.Never);

            result.Should().BeOfType<BadRequestObjectResult>();

        }

        [Fact]
        public async Task GetTransactionListShouldCallGetTransactionListUseCase()
        {
            // given
            var request = new TransactionSearchRequest();
            var response = new GetTransactionListResponse();
            _getTransactionListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _controller.GetTransactionList(request).ConfigureAwait(false);

            // then
            _getTransactionListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }
    }
}
