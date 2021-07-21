using AutoFixture;
using FinancialTransactionsApi.Tests.V1.Helper;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Controllers;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Controllers
{
    public class FinancialTransactionsApiControllerTests
    {
        private readonly FinancialTransactionsApiController _controller;
        private readonly Mock<IGetByIdUseCase> _getByIdUseCase;
        private readonly Mock<IGetAllUseCase> _getAllUseCase;
        private readonly Mock<IAddUseCase> _addUseCase;
        private readonly Mock<IUpdateUseCase> _updateUseCase;
        private readonly Fixture _fixture = new Fixture();

        public FinancialTransactionsApiControllerTests()
        {
            _getByIdUseCase = new Mock<IGetByIdUseCase>();
            _getAllUseCase = new Mock<IGetAllUseCase>();
            _addUseCase = new Mock<IAddUseCase>();
            _updateUseCase = new Mock<IUpdateUseCase>();
            _controller = new FinancialTransactionsApiController(_getAllUseCase.Object,
                _getByIdUseCase.Object, _addUseCase.Object, _updateUseCase.Object);
        }

        [Fact]
        public async Task GetById_UseCaseReturnTransactionByValidId_ShouldReturns200()
        {
            var transactionResponse = _fixture.Create<TransactionResponse>();

            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponse);

            var result = await _controller.GetById(transactionResponse.Id).ConfigureAwait(false);

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

            var result = await _controller.GetById(Guid.NewGuid()).ConfigureAwait(false);

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
                var result = await _controller.GetById(new Guid("6791051d-961d-4e16-9853-6e7e45b01b49"))
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
            var obj1 = _fixture.Create<TransactionResponse>();
            var obj2 = _fixture.Create<TransactionResponse>();

            _getAllUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionQuery>()))
                .ReturnsAsync(new List<TransactionResponse>()
                {
                    obj1,
                    obj2
                });

            var query = new TransactionQuery()
            {
                TargetId = Guid.NewGuid()
            };

            var result = await _controller.GetAll(query).ConfigureAwait(false);

            result.Should().NotBeNull();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult.Value.Should().BeOfType<List<TransactionResponse>>();

            var list = okResult.Value as List<TransactionResponse>;

            list.Should().HaveCount(2);

            list[0].Should().BeEquivalentTo(obj1);

            list[1].Should().BeEquivalentTo(obj2);
        }

        [Fact]
        public async Task GetAll_UseCaseThrownException_ShouldRethrow()
        {
            _getAllUseCase.Setup(x => x.ExecuteAsync(It.IsAny<TransactionQuery>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var result = await _controller.GetAll(new TransactionQuery { })
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

            _addUseCase.Setup(x => x.ExecuteAsync(It.IsAny<AddTransactionRequest>()))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Add(request).ConfigureAwait(false);

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
            var result = await _controller.Add(null).ConfigureAwait(false);

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
        public async Task Add_UseCaseThrownException_ShouldRethrow()
        {
            _addUseCase.Setup(x => x.ExecuteAsync(It.IsAny<AddTransactionRequest>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var result = await _controller.Add(new AddTransactionRequest { })
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
                .ReturnsAsync(new TransactionResponse() { Id = guid });

            _updateUseCase.Setup(x => x.ExecuteAsync(It.IsAny<UpdateTransactionRequest>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Update(guid, request).ConfigureAwait(false);

            result.Should().NotBeNull();

            var redirect = result as RedirectToActionResult;

            redirect.Should().NotBeNull();

            redirect.ActionName.Should().BeEquivalentTo("GetById");

            redirect.RouteValues.Should().NotBeNull();

            redirect.RouteValues.Should().HaveCount(1);

            redirect.RouteValues["id"].Should().Be(guid);
        }

        [Fact]
        public async Task Update_WithNullModel_Returns400()
        {
            var result = await _controller.Update(Guid.NewGuid(), null).ConfigureAwait(false);

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
        public async Task Update_NotFoundEntityWithProvidedId_Returns404()
        {
            _getByIdUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TransactionResponse) null);

            var result = await _controller.Update(Guid.NewGuid(), new UpdateTransactionRequest()).ConfigureAwait(false);

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
                .ReturnsAsync(new TransactionResponse { });

            _updateUseCase.Setup(x => x.ExecuteAsync(It.IsAny<UpdateTransactionRequest>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Test exception"));

            try
            {
                var result = await _controller.Update(Guid.NewGuid(), new UpdateTransactionRequest())
                    .ConfigureAwait(false);
                AssertExtensions.Fail();
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(Exception));
                ex.Message.Should().Be("Test exception");
            }
        }
    }
}
