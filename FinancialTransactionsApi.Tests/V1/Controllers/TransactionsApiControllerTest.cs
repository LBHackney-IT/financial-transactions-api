using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Controllers;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.UseCase.Interfaces;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Controllers
{
    public class TransactionsApiControllerTest
    {
        private readonly TransactionsApiController _classUnderTest;
        private readonly Mock<IGetByIdUseCase> _getByIdUseCase;
        private readonly Mock<IGetAllUseCase> _getAllUseCase;
        private readonly Mock<IAddUseCase> _addUseCase;
        private readonly Fixture _fixture = new Fixture();
        public TransactionsApiControllerTest()
        {
            _getByIdUseCase = new Mock<IGetByIdUseCase>();
            _getAllUseCase = new Mock<IGetAllUseCase>();
            _addUseCase = new Mock<IAddUseCase>();
            _classUnderTest = new TransactionsApiController(_getAllUseCase.Object, _getByIdUseCase.Object, _addUseCase.Object);
        }



        [Fact]
        public async Task GetTransactionWithNoIdReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _getByIdUseCase.Setup(x => x.ExecuteAsync(id)).ReturnsAsync((TransactionResponseObject) null);

            var response = await _classUnderTest.GetById(id).ConfigureAwait(false);
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            //(response as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task GetTransactionWithValidIdReturnsOKResponse()
        {

            var transactionResponse = _fixture.Create<TransactionResponseObject>();
            _getByIdUseCase.Setup(x => x.ExecuteAsync(transactionResponse.Id)).ReturnsAsync(transactionResponse);

            var response = await _classUnderTest.GetById(transactionResponse.Id).ConfigureAwait(false);
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().Be(transactionResponse);
        }

        [Fact]
        public async Task GetTransactionWithValidTargetIdAndTransactionTypeNotFound()
        {
            var targerId = Guid.NewGuid();
            var transType = _fixture.Create<string>();
            var date = DateTime.Now;
            _getAllUseCase.Setup(x => x.ExecuteAsync(targerId, transType, date, date)).ReturnsAsync((TransactionResponseObjectList) null);
            var query = new TransactionQuery { TargetId = targerId, TransactionType = transType, StartDate = date, EndDate = date };
            var response = await _classUnderTest.GetAll(query).ConfigureAwait(false);
            response.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        [Fact]
        public async Task GetTransactionWithValidTargetIdAndTransactionTypeReturnsOKResponse()
        {

            var transactionsObj = _fixture.Build<TransactionResponseObject>()
                            .With(x => x.TargetId, Guid.NewGuid())
                            .With(x => x.TransactionType, "Sample")
                            .With(x => x.TransactionDate, DateTime.Now)
                            .CreateMany(5);
            var search = transactionsObj.FirstOrDefault();
            var transactionsResponse = new TransactionResponseObjectList { ResponseObjects = transactionsObj.ToList() };
            _getAllUseCase.Setup(x => x.ExecuteAsync(search.TargetId, search.TransactionType, search.TransactionDate, search.TransactionDate)).ReturnsAsync(transactionsResponse);
            var query = new TransactionQuery { TargetId = search.TargetId, TransactionType = search.TransactionType, StartDate = search.TransactionDate, EndDate = search.TransactionDate };
            var response = await _classUnderTest.GetAll(query).ConfigureAwait(false);
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().Be(transactionsResponse);
        }

        [Fact]
        public async Task PostNewTransactionSuccessfulSaves()
        {

            var transactionsObj = _fixture.Create<TransactionRequest>();
            var returnObj = transactionsObj.ToTransactionDomain().ToResponse();
            _addUseCase.Setup(x => x.ExecuteAsync(transactionsObj)).ReturnsAsync(returnObj);

            var response = await _classUnderTest.Add(transactionsObj).ConfigureAwait(false);
            response.Should().BeOfType(typeof(CreatedAtActionResult));
            (response as CreatedAtActionResult).ActionName.Should().Be("GetById");
            var id = (response as CreatedAtActionResult).Value;
            id.Should().NotBeNull();
        }
    }
}
