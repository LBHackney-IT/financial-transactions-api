using Xunit.Sdk;

namespace FinancialTransactionsApi.Tests.V1.Helper
{
    public static class AssertExtensions
    {
        /// <summary>
        /// This method used for throw exception in xUnit tests.
        /// When running all tests correctly, this method should not be called.
        /// </summary>
        public static void Fail()
        {
            throw new XunitException("Exception must be thrown");
        }
    }
}
