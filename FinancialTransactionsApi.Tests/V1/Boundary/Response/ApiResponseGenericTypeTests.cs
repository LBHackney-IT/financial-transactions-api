using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Helpers;
using FluentAssertions;
using Hackney.Core.DynamoDb;
using System.Collections.Generic;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Boundary.Response
{
    public class ApiResponseGenericTypeTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ApiResponseGenericType_ReturnTheSameObjectType()
        {
            var transactionsList = _fixture.Build<Transaction>().Create();

            var responseWrapperMockObject = new ResponseWrapper<Transaction>(transactionsList);

            ApiResponse<ResponseWrapper<Transaction>> request = new ApiResponse<ResponseWrapper<Transaction>>(responseWrapperMockObject);

            request.Results.Should().BeEquivalentTo(responseWrapperMockObject);
        }

        [Fact]
        public void ApiResponseGenericType_ReturnTheSameCollectionOfObjectTypes()
        {
            var transactionsList = _fixture.Build<Transaction>().CreateMany();

            var responseWrapperMockObject = new ResponseWrapper<IEnumerable<Transaction>>(transactionsList);

            ApiResponse<ResponseWrapper<IEnumerable<Transaction>>> request = new ApiResponse<ResponseWrapper<IEnumerable<Transaction>>>(responseWrapperMockObject);

            request.Results.Should().BeEquivalentTo(responseWrapperMockObject);
        }

        [Fact]
        public void ApiResponseGenericType_ReturnNull()
        {
            ApiResponse<ResponseWrapper<IEnumerable<Transaction>>> request = new ApiResponse<ResponseWrapper<IEnumerable<Transaction>>>();

            request.Total.Should().Be(default(long));

            request.Results.Should().BeNull();
        }

        [Fact]
        public void ApiResponseGenericType_ReturnEmtpyCollection()
        {
            var transactionsList = _fixture.Build<Transaction>().CreateMany(0);

            var responseWrapperMockObject = new ResponseWrapper<IEnumerable<Transaction>>(transactionsList);

            ApiResponse<ResponseWrapper<IEnumerable<Transaction>>> request = new ApiResponse<ResponseWrapper<IEnumerable<Transaction>>>(responseWrapperMockObject);

            request.Results.Value.Should().BeEmpty();
        }
    }
}
