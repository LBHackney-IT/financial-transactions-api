using System;
using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace FinancialTransactionsApi.Tests.V1.Domain
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void EntitiesHaveAnId()
        {
            var entity = new Transaction();
            entity.Id.Should().BeGreaterOrEqualTo(0);
        }

        [Test]
        public void EntitiesHaveACreatedAt()
        {
            var entity = new Transaction();
            var date = new DateTime(2019, 02, 21);
            entity.CreatedAt = date;

            entity.CreatedAt.Should().BeSameDateAs(date);
        }
    }
}
