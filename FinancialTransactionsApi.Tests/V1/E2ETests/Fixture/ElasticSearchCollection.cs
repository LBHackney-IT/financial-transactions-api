using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Fixture
{
    [CollectionDefinition("ElasticSearch collection", DisableParallelization = true)]
    public class ElasticSearchCollection : ICollectionFixture<DynamoDbMockWebApplicationFactory<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
