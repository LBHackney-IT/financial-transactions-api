using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Helper
{
    public class PagingHelperTests
    {
        private readonly PagingHelper _sut;
        private readonly PaginatedResponse<Transaction> _paginatedResponse;
        private readonly Fixture _fixture;

        public PagingHelperTests()
        {
            _fixture = new Fixture();
            _sut = new PagingHelper();
            _paginatedResponse = new PaginatedResponse<Transaction>();
        }

        [Fact]
        public void ShouldReturnZeroIfCurrentPageIsZero()
        {
            // Arrange + Act
            var result = _sut.GetPageOffset((int) new Random(0).NextDouble(), 0);

            // Assert
            result.Should().Be(0);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 20)]
        public void ShouldCalculateTheCorrectOffsetWhenCurrentPageNotZero(int currentPage, int pageSize)
        {
            // Arrange + Act
            var result = _sut.GetPageOffset(pageSize, currentPage);

            // Assert
            result.Should().Be((currentPage - 1) * pageSize);
        }

        [Fact]
        public void PaginatedResponse_ShouldReturnList()
        {
            var obj = _fixture.CreateMany<Transaction>(5);
            _paginatedResponse.Results = obj;
            _paginatedResponse.Metadata = new MetadataModel()
            {
                Pagination = new Pagination()
                {
                    CurrentPage = 1,
                    PageCount = 1,
                    PageSize = obj.Count(),
                    ResultCount = obj.Count(),
                    TotalCount = obj.Count()
                }
            };

            _paginatedResponse.Results.Should().NotBeNullOrEmpty();
            _paginatedResponse.Results.Should().BeEquivalentTo(obj);
            _paginatedResponse.Metadata.Pagination.Should().NotBeNull();
        }
    }
}
