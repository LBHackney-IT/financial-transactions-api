using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FluentAssertions;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Boundary.Request
{
    public class SuspenseTransactionsSearchRequestTests
    {
        [Fact]
        public void SuspenseTransactionsSearchRequest_Creating_HasProperties()
        {
            SuspenseTransactionsSearchRequest request =
                new SuspenseTransactionsSearchRequest();

            request.GetType().GetProperties().Length.Should().Be(3);
        }
    }
}
