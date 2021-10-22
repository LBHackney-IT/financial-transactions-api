using FinancialTransactionsApi.Tests.V1.E2ETests.Fixture;
using FinancialTransactionsApi.Tests.V1.E2ETests.Steps.Base;
using FinancialTransactionsApi.V1.Boundary.Response;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Steps
{
    public class GetTransactionSteps : BaseSteps
    {
        public GetTransactionSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/transactions/search", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/transactions/search?searchText=payment", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingByPaymentReferenceAndAddress(string paymentRef, string address)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/search?searchText={paymentRef}%20{address}", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/transactions/search?searchText={TransactionFixture.Alphabet.Last()}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }







        public void ThenTheLastRequestShouldBe200()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        public void ThenTheLastRequestShouldBe404()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTransactionListResponse>>(resultBody, _jsonOptions);

            result.Results.Transactions.Count.Should().Be(pageSize);
        }


    }
}