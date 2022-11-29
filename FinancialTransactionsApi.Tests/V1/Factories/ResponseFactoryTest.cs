using AutoFixture;
using FluentAssertions;
using System.Collections.Generic;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using Xunit;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;

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
            domain.Should().BeEquivalentTo(response, options =>
            {
                options.Excluding(info => info.TransactionType);
                options.Excluding(info => info.PaymentReference);
                return options;
            });

        }

        [Fact]
        public void CanMapDomainTransactionResponseObjectListToAResponsesList()
        {
            var list = _fixture.CreateMany<Transaction>(10);
            var responseNotes = list.ToResponse();

            responseNotes.Should().BeEquivalentTo(list, opt => opt.Excluding(x => x.TransactionType));
        }

        [Fact]
        public void CanMapNullDomainTransactionResponseObjectListToAnEmptyResponsesList()
        {
            List<Transaction> list = null;
            var responseNotes = list.ToResponse();

            responseNotes.Should().BeEmpty();
        }

        [Fact]
        public void CorrectlyMapsPaginatedTransactionsFromDomainToResponse()
        {
            var itemCount = 10;
            var tDomain = _fixture.CreateMany<Transaction>(10);
            var tResponse = tDomain.ToResponse();
            var paginatedResult = new Paginated<Transaction>
            {
                Results = tDomain,
                CurrentPage = 5,
                PageSize = 10,
                TotalResultCount = 49
            };

            var mappingResult = paginatedResult.ToResponse();

            mappingResult.Should().NotBeNull();
            mappingResult.Results.Should().NotBeNull();
            mappingResult.Metadata.Should().NotBeNull();
            mappingResult.Metadata.Pagination.Should().NotBeNull();

            mappingResult.Results.Should().HaveCount(itemCount);
            mappingResult.Results.Should().BeEquivalentTo(tResponse);

            mappingResult.Metadata.Pagination.ResultCount.Should().Be(paginatedResult.ResultCount);
            mappingResult.Metadata.Pagination.CurrentPage.Should().Be(paginatedResult.CurrentPage);
            mappingResult.Metadata.Pagination.PageSize.Should().Be(paginatedResult.PageSize);
            mappingResult.Metadata.Pagination.TotalCount.Should().Be(paginatedResult.TotalResultCount);
            mappingResult.Metadata.Pagination.PageCount.Should().Be(paginatedResult.PageCount);
        }
    }
}
