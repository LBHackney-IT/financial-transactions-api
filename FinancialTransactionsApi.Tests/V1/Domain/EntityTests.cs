using System;
using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Domain
{
    public class EntityTests
    {
        [Fact]
        public void EntitiesHaveAnId()
        {
            var entity = new Transaction();
            entity.Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000000"));
        }

        [Fact]
        public void EntitiesHaveATransactionDate()
        {
            var entity = new Transaction();
            var date = new DateTime(2019, 02, 21);
            entity.TransactionDate = date;

            entity.TransactionDate.Should().BeSameDateAs(date);
        }
    }
}
