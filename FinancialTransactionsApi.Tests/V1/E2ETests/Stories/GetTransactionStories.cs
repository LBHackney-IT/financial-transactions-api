using FinancialTransactionsApi.Tests.V1.E2ETests.Fixture;
using FinancialTransactionsApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Stories
{

    [Story(
       AsA = "Service",
       IWant = "The Transaction Search Endpoint to return results",
       SoThat = "it is possible to search for assets")]
    [Collection("DynamoDb collection")]
    public class GetTransactionStories
    {
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly TransactionFixture _transactionFixture;
        private readonly GetTransactionSteps _steps;

        public GetTransactionStories(DynamoDbIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _steps = new GetTransactionSteps(_dbFixture.Client);
            _transactionFixture = new TransactionFixture(_dbFixture.ElasticSearchClient, _dbFixture.Client);
        }


        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _transactionFixture.GivenAnTransactionIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _transactionFixture.GivenAnTransactionIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(5))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceRequestDoesNotContainSearchStringReturnsOkResult()
        {
            this.Given(g => _transactionFixture.GivenAnTransactionIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceRequestByPaymentReferenceAndAddressReturnsOkResult()
        {
            this.Given(g => _transactionFixture.GivenAnTransactionIndexExists())
                .When(w => _steps.WhenSearchingByPaymentReferenceAndAddress("payment", "address"))
                .Then(t => _steps.ThenTheLastRequestShouldBe404())
                .BDDfy();
        }
    }
}
