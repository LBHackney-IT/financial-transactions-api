using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Infrastructure
{
    public class ValidationAttributeTests
    {
        private GreatAndEqualThanAttribute _greatAndEqualThanAttribute;

        [Theory]
        [InlineData("5.0")]
        public void GreatAndEqualThanAttribute_ReturnTrue(string minValue)
        {
            this._greatAndEqualThanAttribute = new GreatAndEqualThanAttribute(minValue);

            bool result = this._greatAndEqualThanAttribute.IsValid(6.0M);

            Assert.True(result);
        }

        [Theory]
        [InlineData("5.0")]
        public void GreatAndEqualThanAttribute_ReturnFalse(string minValue)
        {
            this._greatAndEqualThanAttribute = new GreatAndEqualThanAttribute(minValue);

            bool result = this._greatAndEqualThanAttribute.IsValid(3.0M);

            Assert.False(result);
        }
    }
}
