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
                IsConfirmed = true,
                IsApproved = false,
                Note = "Some note"
            };

            suspense.ResolutionDate.Should().Be(new DateTime(2021, 8, 1));
            suspense.IsConfirmed.Should().BeTrue();
            suspense.IsApproved.Should().BeFalse();
            suspense.IsResolve.Should().BeFalse();
            suspense.Note.Should().BeEquivalentTo("Some note");
        }
    }
}
