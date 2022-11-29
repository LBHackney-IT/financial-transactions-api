using AutoFixture;
using FinancialTransactionsApi.Tests.V1.Helper;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Controllers;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using HSFPagination = Hackney.Shared.Finance.Pagination;
using System.Linq;

namespace FinancialTransactionsApi.Tests.V1.Controllers
{
    public class FinancialTransactionsApiControllerTests
    {
        private readonly FinancialTransactionsApiController _controller;
        private readonly Mock<IGetByIdUseCase> _getByIdUseCase;
        private readonly Mock<IGetAllUseCase> _getAllUseCase;
        private readonly Mock<IAddUseCase> _addUseCase;
        private readonly Mock<IUpdateSuspenseAccountUseCase> _updateUseCase;
        private readonly Mock<IAddBatchUseCase> _addBatchUseCase;
        private readonly Mock<IGetSuspenseAccountUseCase> _suspenseAccountUseCase;
        private readonly Mock<IGetByTargetIdUseCase> _getByTargetIdUseCase;
        private readonly Mock<IGetAllActiveTransactionsUseCase> _getAllActiveTransactionsUseCase;

        private readonly Mock<ISuspenseAccountApprovalUseCase> _suspenseAccountApprovalUseCase;
        private readonly Fixture _fixture = new Fixture();
        private const string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0IiwiaWF0IjoxNjM5NDIyNzE4LCJleHAiOjE5ODY1Nzc5MTgsImF1ZCI6InRlc3QiLCJzdWIiOiJ0ZXN0IiwiZ3JvdXBzIjpbInNvbWUtdmFsaWQtZ29vZ2xlLWdyb3VwIiwic29tZS1vdGhlci12YWxpZC1nb29nbGUtZ3JvdXAiXSwibmFtZSI6InRlc3RpbmcifQ.IcpQ00PGVgksXkR_HFqWOakgbQ_PwW9dTVQu4w77tmU";

        public FinancialTransactionsApiControllerTests()
        {
            _getByIdUseCase = new Mock<IGetByIdUseCase>();
            _getAllUseCase = new Mock<IGetAllUseCase>();
            _addUseCase = new Mock<IAddUseCase>();
            _updateUseCase = new Mock<IUpdateSuspenseAccountUseCase>();
            _addBatchUseCase = new Mock<IAddBatchUseCase>();
            _suspenseAccountUseCase = new Mock<IGetSuspenseAccountUseCase>();
            _getByTargetIdUseCase = new Mock<IGetByTargetIdUseCase>();
            _suspenseAccountApprovalUseCase = new Mock<ISuspenseAccountApprovalUseCase>();
            _getAllActiveTransactionsUseCase = new Mock<IGetAllActiveTransactionsUseCase>();
            _controller = new FinancialTransactionsApiController(
                _getAllUseCase.Object,
                _getByIdUseCase.Object,
                _addUseCase.Object,
                _updateUseCase.Object,
                _addBatchUseCase.Object,
                _suspenseAccountUseCase.Object,
                _getByTargetIdUseCase.Object,
                _getAllActiveTransactionsUseCase.Object);
        }

        [Fact]
        public async Task GetByTargerId_UseCaseReturnTransactionByTargetId_ShouldReturns200()
        {
            var responseMock = new ResponseWrapper<IEnumerable<TransactionResponse>>(_fixture.Build<TransactionResponse>().CreateMany(5));

            _getByTargetIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).ReturnsAsync(responseMock);

            var result = await _controller.GetByTargetId("Tenure", Guid.NewGuid(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult?.Value.Should().BeEquivalentTo(responseMock.Value);
        }

        [Fact]
        public async Task GetByTargetId_UseCaseReturnNullByInvalidTargetId_ShouldReturns404()
        {
            IEnumerable<TransactionResponse> transactionsList = null;

            var responseMock = new ResponseWrapper<IEnumerable<TransactionResponse>>(transactionsList);

            Guid targetId = Guid.NewGuid();

            _getByTargetIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).ReturnsAsync(responseMock);

            var result = await _controller.GetByTargetId("Tenure", targetId, It.IsAny<DateTime?>(), It.IsAny<DateTime?>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();

            notFoundResult?.Value.Should().BeEquivalentTo(targetId);
        }

        [Fact]
        public async Task GetAllActive_UseCaseReturnTransactionByDateAndPageSize_ShouldReturns200()
        {
            var responseMock = new PaginatedResponse<TransactionResponse>() { Results = _fixture.Build<TransactionResponse>().CreateMany(5) };

            _getAllActiveTransactionsUseCase.Setup(x => x.ExecuteAsync(It.IsAny<GetActiveTransactionsRequest>())).ReturnsAsync(responseMock);

            var result = await _controller.GetAllActiveTransactions(It.IsAny<GetActiveTransactionsRequest>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult?.Value.Should().BeEquivalentTo(responseMock);
        }

        [Fact]
        public async Task GetAllActive_UseCaseReturnTransactionByDateAndPageSize_ShouldReturns404()
        {
            var responseMock = new PaginatedResponse<TransactionResponse>() { Results = null };

            _getAllActiveTransactionsUseCase.Setup(x => x.ExecuteAsync(It.IsAny<GetActiveTransactionsRequest>())).ReturnsAsync(responseMock);

            var result = await _controller.GetAllActiveTransactions(It.IsAny<GetActiveTransactionsRequest>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();

            var response = notFoundResult?.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response?.StatusCode.Should().Be((int) HttpStatusCode.NotFound);

            response.Message.Should().BeEquivalentTo("Transaction by provided data cannot be found!");

            response.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task GetById_UseCaseReturnTransactionByValidId_ShouldReturns200()
        {

            var transactionResponse = new ResponseWrapper<TransactionResponse>(_fixture.Create<TransactionResponse>());

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponse);

            var result = await _controller.Get(It.IsAny<Guid>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var transaction = okResult?.Value as TransactionResponse;

            transaction.Should().NotBeNull();

            transaction.Should().BeEquivalentTo(transactionResponse.Value);
        }

        [Fact]
        public async Task GetById_UseCaseReturnNullWithInvalidId_ShouldReturns404()
        {

            TransactionResponse transaction = null;

            var responseMock = new ResponseWrapper<TransactionResponse>(transaction);

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(responseMock);


            var result = await _controller.Get(Guid.NewGuid()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_UseCaseThrownException_ShouldRethrow()
        {
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {

                var result = await _controller.Get(new Guid("6791051d-961d-4e16-9853-6e7e45b01b49"))
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

            var obj1 = new PaginatedResponse<TransactionResponse>() { Results = transactionsList };

            _getAllUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionQuery>())).ReturnsAsync(obj1);

            var query = new TransactionQuery()
            {
                TargetId = Guid.NewGuid()
            };

            var result = await _controller.GetAll("", query).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult?.Value.Should().BeOfType<PaginatedResponse<TransactionResponse>>();

            var responses = okResult?.Value as PaginatedResponse<TransactionResponse>;

            responses?.Results.Should().HaveCount(5);

        }

        [Fact]
        public async Task GetSuspenseAccount_UseCaseReturnList_Returns200_AndHasData()
        {
            var pagedTransactions = new HSFPagination.Paginated<Transaction>();
            pagedTransactions.Results = _fixture.Build<Transaction>().CreateMany(5);

            _suspenseAccountUseCase.Setup(x => x.ExecuteAsync(It.IsAny<SuspenseAccountQuery>())).ReturnsAsync(pagedTransactions);

            var result = await _controller.GetSuspenseAccount(It.IsAny<SuspenseAccountQuery>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var responses = okResult?.Value as HSFPagination.PaginatedResponse<TransactionResponse>;

            responses.Results.Should().NotBeNull();
            responses.Results.Should().HaveCount(5);
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
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.ArrangementInterest
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
                BankAccountNumber = "12345678",
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.ArrangementInterest.GetDescription(),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "testing",
                CreatedBy = "testing"
            };

            _addUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Add("", Token, request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var createResult = result as CreatedAtActionResult;

            createResult.Should().NotBeNull();

            createResult?.StatusCode.Should().Be((int) HttpStatusCode.Created);

            createResult?.RouteValues.Should().NotBeNull();

            createResult?.RouteValues.Should().HaveCount(1);

            createResult?.RouteValues["id"].Should().NotBeNull();

            createResult?.RouteValues["id"].Should().Be(guid);

            var responseObject = createResult.Value as TransactionResponse;

            responseObject.Should().NotBeNull();

            responseObject.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Add_WithNullModel_Returns400()
        {
            var result = await _controller.Add("", Token, null).ConfigureAwait(false);

            result.Should().NotBeNull();

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            var response = badRequestResult?.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

            response.Message.Should().BeEquivalentTo("Transaction model cannot be null!");

            response.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task Add_WithInvalidModel_Returns400()
        {
            try
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
                    PeriodNo = 2,
                    TransactionAmount = 126.83M,
                    TransactionSource = "DD",
                    TransactionType = TransactionType.ArrangementInterest
                };
                var result = await _controller.Add("", Token, request).ConfigureAwait(false);

                result.Should().NotBeNull();

                var badRequestResult = result as BadRequestObjectResult;

                badRequestResult.Should().NotBeNull();

                var response = badRequestResult?.Value as BaseErrorResponse;

                response.Should().NotBeNull();

                response?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

                response.Message.Should().BeEquivalentTo("Transaction model don't have all information in fields!");

                response.Details.Should().BeEquivalentTo(string.Empty);
            }
            catch (Exception ex)
            {
                Assert.False(true, ex.Message + " " + ex.StackTrace + ". " + ex.InnerException?.Message);
            }
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
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.ArrangementInterest
            };
            _addUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var result = await _controller.Add("", Token, request)
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
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.ArrangementInterest
            };

            try
            {
                var result = await _controller.Add("", null, request)
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                _addUseCase.Verify(_ => _.ExecuteAsync(It.IsAny<Transaction>()), Times.Never);
                ex.GetType().Should().Be(typeof(ArgumentNullException));
                ex.Message.Should().Be("Value cannot be null. (Parameter 'token')");
            }
        }

        [Fact]
        public async Task SuspenseAccount_WithValidModel_Returns200()
        {
            var guid = Guid.NewGuid();

            var request = new SuspenseConfirmationRequest()
            {
                TargetId = Guid.NewGuid(),
                Note = "Test"
            };
            var response = new ResponseWrapper<TransactionResponse>(_fixture.Build<TransactionResponse>()
                .With(x => x.TransactionType, TransactionType.ChequePayments.GetDescription())
                .Create());
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(response);

            _updateUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(new TransactionResponse());

            var result = await _controller.SuspenseAccountConfirmation(Token, guid, request).ConfigureAwait(false);

            result.Should().NotBeNull();


            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult?.Value.Should().BeOfType(typeof(TransactionResponse));
        }

        [Fact]
        public async Task SuspenseAccountConfirmation_WithInvalidModel_Returns400()
        {
            var request = new SuspenseConfirmationRequest()
            {
                TargetId = Guid.Empty,
                Note = "Test"
            };
            var result = await _controller.SuspenseAccountConfirmation(Token, Guid.NewGuid(), request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            var response = badRequestResult?.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

            response?.Message.Should().BeEquivalentTo("SuspenseConfirmationRequest model don't have all information in fields!");

            response?.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task SuspenseAccountConfirmation_WithNullModel_Returns400()
        {
            var result = await _controller.SuspenseAccountConfirmation(Token, Guid.NewGuid(), null).ConfigureAwait(false);

            result.Should().NotBeNull();

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            var response = badRequestResult?.Value as BaseErrorResponse;

            response.Should().NotBeNull();

            response?.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

            response?.Message.Should().BeEquivalentTo("SuspenseConfirmationRequest model cannot be null!");

            response?.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task Update_NotFoundEntityWithProvidedId_Returns404()
        {
            ResponseWrapper<TransactionResponse> responseMock = null;
            var request = new SuspenseConfirmationRequest()
            {
                TargetId = Guid.NewGuid(),
                Note = "Test"
            };
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(responseMock);


            var result = await _controller.SuspenseAccountConfirmation(Token, Guid.NewGuid(), request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_UseCaseThrownException_ShouldRethrow()
        {

            var transactionResponse = new ResponseWrapper<TransactionResponse>(_fixture.Create<TransactionResponse>());

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponse);

            var result = await _controller.Get(It.IsAny<Guid>()).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            var transaction = okResult?.Value as TransactionResponse;

            transaction.Should().NotBeNull();

            transaction.Should().BeEquivalentTo(transactionResponse.Value);


            var response = new ResponseWrapper<TransactionResponse>(_fixture.Build<TransactionResponse>()
                .With(x => x.TransactionType, TransactionType.ChequePayments.GetDescription())
                .With(x => x.IsSuspense, true)
                .Create());

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(response);

            _updateUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Transaction>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var request = new SuspenseConfirmationRequest()
                {
                    TargetId = Guid.NewGuid(),
                    Note = "Test"
                };
                var resultMock = await _controller.SuspenseAccountConfirmation(Token, Guid.NewGuid(), request)
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

            TransactionResponse transaction = null;
            var responseMock = new ResponseWrapper<TransactionResponse>(transaction);
            var request = new SuspenseConfirmationRequest()
            {
                TargetId = Guid.NewGuid(),
            };

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
               .ReturnsAsync(responseMock);

            try
            {
                var result = await _controller.SuspenseAccountConfirmation(null, Guid.NewGuid(), request)
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                _updateUseCase.Verify(_ => _.ExecuteAsync(It.IsAny<Transaction>()), Times.Never);
                ex.GetType().Should().Be(typeof(ArgumentNullException));
                ex.Message.Should().Be("Value cannot be null. (Parameter 'token')");
            }

        }

        [Fact]
        public async Task Update_NonSuspenseTransaction_ThrowBadRequest()
        {
            var response = new ResponseWrapper<TransactionResponse>(_fixture.Build<TransactionResponse>()
              .With(x => x.TransactionType, TransactionType.ChequePayments.GetDescription())
              .Create());

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync(response);

            var result = await _controller.SuspenseAccountConfirmation(Token, Guid.NewGuid(), null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();

        }

        [Fact]
        public async Task AddBatch_ThrowBadRequest()
        {
            var result = await _controller.AddBatch(It.IsAny<string>(), Token, null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AddBatch_Returns200()
        {
            var transactionsRequest = _fixture.CreateMany<AddTransactionRequest>(5);

            var transactions = _fixture.CreateMany<Transaction>(5);

            _addBatchUseCase.Setup(x => x.ExecuteAsync(transactions)).ReturnsAsync(transactions.Count());

            var result = await _controller.AddBatch(It.IsAny<string>(), Token, transactionsRequest).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SuspenseAccountApproval_ThrowBadRequest()
        {
            var result = await _controller.SuspenseAccountApproval(Token, It.IsAny<Guid>(), null).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SuspenseAccountApproval_Returns404()
        {
            var transactionsRequest = _fixture.Create<UpdateTransactionRequest>();

            var transactions = _fixture.Create<Transaction>();

            _updateUseCase.Setup(x => x.ExecuteAsync(transactions)).ReturnsAsync(_fixture.Create<TransactionResponse>());

            var result = await _controller.SuspenseAccountApproval(Token, It.IsAny<Guid>(), transactionsRequest).ConfigureAwait(false);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task SuspenseAccountApproval_Returns200()
        {
            var transactionsRequest = _fixture.Create<UpdateTransactionRequest>();

            var transactions = _fixture.Create<Transaction>();

            _updateUseCase.Setup(x => x.ExecuteAsync(transactions)).ReturnsAsync(_fixture.Create<TransactionResponse>());

            var responseMock = new ResponseWrapper<TransactionResponse>(_fixture.Build<TransactionResponse>().Create());

            responseMock.Value.IsSuspense = true;

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync(responseMock);

            var result = await _controller.SuspenseAccountApproval(Token, It.IsAny<Guid>(), transactionsRequest).ConfigureAwait(false);

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
