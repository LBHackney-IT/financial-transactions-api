using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Controllers;
using FinancialTransactionsApi.V1.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Infrastructure
{

    public class CorrelationMiddlewareTest
    {
        private CorrelationMiddleware _sut;
        public CorrelationMiddlewareTest()
        {
            _sut = new CorrelationMiddleware(null);
        }
       

        [Fact]
        public async Task DoesNotReplaceCorrelationIdIfOneExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var headerValue = "123";

            httpContext.HttpContext.Request.Headers.Add(Constants.CorrelationId, headerValue);

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[Constants.CorrelationId].Should().BeEquivalentTo(headerValue);
            httpContext.HttpContext.Response.Headers[Constants.CorrelationId].Should().BeEquivalentTo(headerValue);
        }

        [Fact]
        public async Task AddsCorrelationIdIfOneDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[Constants.CorrelationId].Should().HaveCountGreaterThan(0);
            httpContext.HttpContext.Response.Headers[Constants.CorrelationId].Should().HaveCountGreaterThan(0);
        }
    }
}
