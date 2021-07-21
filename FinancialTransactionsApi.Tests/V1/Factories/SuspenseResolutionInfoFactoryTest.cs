using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Factories
{
    public class SuspenseResolutionInfoFactoryTest
    {
        [Fact]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var dbEntity = new SuspenseResolutionInfoDbEntity()
            {
                ResolutionDate = new DateTime(2021, 8, 1),
                IsResolve = true,
                Note = "Some note"
            };

            var domain = dbEntity.ToDomain();

            domain.Should().BeEquivalentTo(dbEntity);
        }

        [Fact]
        public void CanMapADomainEntityToADatabaseObject()
        {
            var domain = new SuspenseResolutionInfo()
            {
                ResolutionDate = new DateTime(2021, 8, 1),
                IsResolve = true,
                Note = "Some note"
            };

            var dbEntity = domain.ToDatabase();

            dbEntity.Should().BeEquivalentTo(domain);
        }

        [Fact]
        public void CanMapADomainEntityToADomainObject()
        {
            var domain = new SuspenseResolutionInfo()
            {
                ResolutionDate = new DateTime(2021, 8, 1),
                IsResolve = true,
                Note = "Some note"
            };

            var domainClone = domain.ToDomain();

            domainClone.Should().BeEquivalentTo(domain);
        }
    }
}
