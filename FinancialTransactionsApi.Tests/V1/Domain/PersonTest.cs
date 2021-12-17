using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Domain
{
    public class PersonTest
    {
        [Fact]
        public void PersonHasPropertiesSet()
        {
            var person = new Person()
            {
                Id = new Guid("9b014c26-88be-466e-a589-0f402c6b94c1"),
                FullName = "Kian Hyaward"
            };

            person.Id.Should().Be(new Guid("9b014c26-88be-466e-a589-0f402c6b94c1"));
            person.FullName.Should().BeEquivalentTo("Kian Hyaward");
        }
    }
}
