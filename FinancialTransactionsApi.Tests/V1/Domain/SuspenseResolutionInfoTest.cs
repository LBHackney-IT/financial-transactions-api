using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Domain
{
    public class SuspenseResolutionInfoTest
    {
        [Fact]
        public void SuspenseResolutionInfoHasPropertiesSet()
        {
            var suspense = new SuspenseResolutionInfo()
            {
                ResolutionDate = new DateTime(2021, 8, 1),
                IsResolve = true,
                Note = "Some note"
            };

            suspense.ResolutionDate.Should().Be(new DateTime(2021, 8, 1));
            suspense.IsResolve.Should().BeTrue();
            suspense.Note.Should().BeEquivalentTo("Some note");
        }
    }
}
