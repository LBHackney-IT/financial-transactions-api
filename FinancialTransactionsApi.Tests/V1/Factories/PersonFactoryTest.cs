using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Factories
{
    public class PersonFactoryTest
    {
        [Fact]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var dbEntity = new PersonDbEntity()
            {
                Id = Guid.NewGuid(),
                FullName = "Kyan Hyarward"
            };

            var domain = dbEntity.ToDomain();

            domain.Should().BeEquivalentTo(dbEntity);
        }

        [Fact]
        public void CanMapADomainEntityToADatabaseObject()
        {
            var domain = new Person()
            {
                Id = Guid.NewGuid(),
                FullName = "Kyan Hyarward"
            };

            var dbEntity = domain.ToDatabase();

            dbEntity.Should().BeEquivalentTo(domain);
        }

        [Fact]
        public void CanMapADomainEntityToADomainEntity()
        {
            var domain = new Person()
            {
                Id = Guid.NewGuid(),
                FullName = "Kyan Hyarward"
            };

            var domainClone = domain.ToDomain();

            domainClone.Should().BeEquivalentTo(domain);
        }
    }
}
