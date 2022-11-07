using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
//using Xunit.Assert;

namespace FinancialTransactionsApi.Tests.V1.Gateway
{
    public class PostgreDbGatewayTests
    {
        private readonly Mock<DatabaseContext> _mockContext;
        private readonly PostgreDbGateway _postgreDbGateway;
        private readonly Mock<DbSet<TransactionEntity>> _mockSet;
        private readonly Fixture _fixture = new Fixture();

        public PostgreDbGatewayTests()
        {
            _mockContext = new Mock<DatabaseContext>();
            _mockSet = new Mock<DbSet<TransactionEntity>>();
            _postgreDbGateway = new PostgreDbGateway(_mockContext.Object);
        }

        [Fact]
        public async Task GetByTargetId_Gateway_ReturnCollectionOfTransaction_NotEmpty()
        {
            var guidId = Guid.NewGuid();

            var data = _fixture.CreateMany<TransactionEntity>(5).AsQueryable();

            data.ToList().ForEach(item => { item.TargetId = guidId; item.TargetType = TargetType.Tenure.ToString(); });

            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Transactions).Returns(_mockSet.Object);

            var result = await _postgreDbGateway.GetByTargetId("Tenure", guidId, null, null).ConfigureAwait(false);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetByTargetId_Gateway_ReturnCollectionOfTransaction_Empty()
        {
            var data = _fixture.CreateMany<TransactionEntity>(5).AsQueryable();

            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Transactions).Returns(_mockSet.Object);

            var result = await _postgreDbGateway.GetByTargetId(TargetType.Tenure.ToString(), Guid.NewGuid(), It.IsAny<DateTime>(), It.IsAny<DateTime>()).ConfigureAwait(false);

            result.Should().BeEmpty();
        }

        [Fact]
        public void GetById_Gateway_ReturnCollectionOfTransaction_ExpectedException()
        {
            Assert.Throws<NotImplementedException>(delegate { _postgreDbGateway.GetTransactionByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()); });
        }

        [Fact]
        public void GetPaged_Gateway_ReturnCollectionOfTransaction_ExpectedException()
        {
            Assert.Throws<NotImplementedException>(delegate { _postgreDbGateway.GetPagedTransactionsAsync(It.IsAny<TransactionQuery>()); });
        }

        [Fact]
        public void Add_Gateway_Transaction_ExpectedException()
        {
            Assert.Throws<NotImplementedException>(delegate { _postgreDbGateway.AddAsync(It.IsAny<Transaction>()); });
        }

        [Fact]
        public void AddBatch_Gateway_Transaction_ExpectedException()
        {
            Assert.Throws<NotImplementedException>(delegate { _postgreDbGateway.AddBatchAsync(It.IsAny<List<Transaction>>()); });
        }

        [Fact]
        public void UpdateSuspenseAccount_Gateway_Transaction_ExpectedException()
        {
            Assert.Throws<NotImplementedException>(delegate { _postgreDbGateway.UpdateSuspenseAccountAsync(It.IsAny<Transaction>()); });
        }

        [Fact]
        public async Task GetPagedSuspenseAccount_Gateway_Transaction_NotEmpty()
        {

            var data = _fixture.CreateMany<TransactionEntity>(5).AsQueryable();

            data.ToList().ForEach(item => { item.TargetType = TargetType.Tenure.ToString(); item.TransactionDate = DateTime.UtcNow; });

            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Transactions).Returns(_mockSet.Object);

            var request = new SuspenseAccountQuery() { Page = 1, PageSize = 11, SearchText = true };

            var result = await _postgreDbGateway.GetPagedSuspenseAccountTransactionsAsync(request).ConfigureAwait(false);

            result.Results.Should().NotBeEmpty();
        }

        [Fact]

        public async Task GetPagedSuspenseAccount_Gateway_Transaction_Empty()
        {

            var data = _fixture.CreateMany<TransactionEntity>(0).AsQueryable();

            data.ToList().ForEach(item => { item.TargetType = TargetType.Tenure.ToString(); item.TransactionDate = DateTime.UtcNow.AddDays(3); });

            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Transactions).Returns(_mockSet.Object);

            var request = new SuspenseAccountQuery() { Page = 1, PageSize = 11, SearchText = true };

            var result = await _postgreDbGateway.GetPagedSuspenseAccountTransactionsAsync(request).ConfigureAwait(false);

            result.Results.Should().BeEmpty();

        }

        [Fact]
        public async Task GetAllActive_Gateway_ReturnCollectionOfTransaction_NotEmpty()
        {
            var data = _fixture.CreateMany<TransactionEntity>(5).AsQueryable();

            data.ToList().ForEach(item => { item.TargetType = TargetType.Tenure.ToString(); item.TransactionDate = DateTime.UtcNow; });

            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Transactions).Returns(_mockSet.Object);

            var request = new GetActiveTransactionsRequest() { Page = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow.AddDays(-1), PeriodEndDate = DateTime.UtcNow.AddDays(1) };

            var result = await _postgreDbGateway.GetAllActive(request).ConfigureAwait(false);

            result.Results.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAllActive_Gateway_ReturnCollectionOfTransaction_Empty()
        {
            var data = _fixture.CreateMany<TransactionEntity>(5).AsQueryable();

            data.ToList().ForEach(item => { item.TargetType = TargetType.Tenure.ToString(); item.TransactionDate = DateTime.UtcNow.AddDays(3); });

            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<TransactionEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Transactions).Returns(_mockSet.Object);

            var request = new GetActiveTransactionsRequest() { Page = 1, PageSize = 11, PeriodStartDate = DateTime.UtcNow.AddDays(-1), PeriodEndDate = DateTime.UtcNow.AddDays(1) };

            var result = await _postgreDbGateway.GetAllActive(request).ConfigureAwait(false);

            result.Results.Should().BeEmpty();
        }

        [Fact]
        public void GetPagedTransactionsByTargetId_Gateway_Transaction_ExpectedException()
        {
            Assert.Throws<NotImplementedException>(delegate { _postgreDbGateway.GetPagedTransactionsByTargetIdsAsync(It.IsAny<TransactionByTargetIdsQuery>()); });
        }
    }
}
