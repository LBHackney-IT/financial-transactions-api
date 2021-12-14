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
    [Collection("Aws collection")]
    public class GetTransactionStories
    {
        private readonly AwsIntegrationTests<Startup> _dbFixture;
        private readonly TransactionFixture _transactionFixture;
        private readonly GetTransactionSteps _steps;

        public GetTransactionStories(AwsIntegrationTests<Startup> dbFixture)
        {
            //_factory = factory;
            _dbFixture = dbFixture;
            var elasticClient = dbFixture.ElasticSearchClient;
            _steps = new GetTransactionSteps(dbFixture.Client);
            _transactionFixture = new TransactionFixture(elasticClient);
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
