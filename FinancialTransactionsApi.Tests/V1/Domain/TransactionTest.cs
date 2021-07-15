using FinancialTransactionsApi.Tests.V1.Helper;
using FluentAssertions;
using TransactionsApi.V1.Domain;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Domain
{
    public class TransactionTest
    {
        [Fact]
        public void TransactionHasPropertiesSet()
        {
            Transaction transaction = Constants.ConstructTransactionFromConstants();

            transaction.Id.Should().Be(Constants.ID);
            transaction.TargetId.Should().Be(Constants.TARGETID);
            transaction.PeriodNo.Should().Be(Constants.PERIODNO);
            transaction.FinancialMonth.Should().Be(Constants.MONTH);
            transaction.FinancialYear.Should().Be(Constants.YEAR);
            transaction.TransactionAmount.Should().Be(Constants.AMOUNT);
            transaction.TransactionType.Should().Be(Constants.TYPE);
            transaction.TransactionDate.Should().Be(Constants.DATE);
            transaction.PaymentReference.Should().Be(Constants.REFERENCE);
            transaction.ChargedAmount.Should().Be(Constants.AMOUNT);
            transaction.PaidAmount.Should().Be(Constants.AMOUNT);
            transaction.BalanceAmount.Should().Be(Constants.AMOUNT);
            transaction.HousingBenefitAmount.Should().Be(Constants.AMOUNT);

        }
    }
}
