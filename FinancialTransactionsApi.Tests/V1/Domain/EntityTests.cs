using System;
using TransactionsApi.V1.Domain;
using FluentAssertions;
using NUnit.Framework;
using Xunit;

namespace TransactionsApi.Tests.V1.Domain
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
