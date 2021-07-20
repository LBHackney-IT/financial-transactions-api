using AutoFixture;
using FluentAssertions;
using System.Collections.Generic;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Factories
{

    public class ResponseFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();
        [Fact]
        public void CanMapANullTransactionResponseObjectToAResponseObject()
        {
            Transaction domain = null;
            var response = domain.ToResponse();

            response.Should().BeNull();
        }

        [Fact]
        public void CanMapATransactionResponseObjectToAResponseObject()
        {
            var domain = _fixture.Create<Transaction>();
            var response = domain.ToResponse();
            domain.Should().BeEquivalentTo(response);

        }

        [Fact]
        public void CanMapDomainTransactionResponseObjectListToAResponsesList()
        {
            var list = _fixture.CreateMany<Transaction>(10);
            var responseNotes = list.ToResponse();

            responseNotes.Should().BeEquivalentTo(list);
        }

        [Fact]
        public void CanMapNullDomainTransactionResponseObjectListToAnEmptyResponsesList()
        {
            List<Transaction> list = null;
            var responseNotes = list.ToResponse();

            responseNotes.Should().BeEmpty();
        }
    }
}
