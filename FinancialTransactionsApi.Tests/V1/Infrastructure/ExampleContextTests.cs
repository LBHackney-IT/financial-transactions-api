using FluentAssertions;
using System.Linq;
using TransactionsApi.Tests.V1.Helper;
using Xunit;

namespace TransactionsApi.Tests.V1.Infrastructure
{
    public class DatabaseContextTest : DatabaseTests
    {
        [Fact]
        public void CanGetADatabaseEntity()
        {
            var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntity();

            DatabaseContext.Add(databaseEntity);
            DatabaseContext.SaveChanges();

            var result = DatabaseContext.TransactionEntities.ToList().FirstOrDefault();
            result.Should().Equals(databaseEntity);
        }
    }
}
