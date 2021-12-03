using Amazon.DynamoDBv2.Model;
using FinancialTransactionsApi.Tests.V1.Helper;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Infrastructure
{
    public class QueryResponseExtensionTests
    {
        [Fact]
        public void ToTransactionsValidDataReturnsListOfTransactions()
        {
            var queryResponse = FakeDataHelper.MockQueryResponse<Transaction>(5);

            var transactions = queryResponse.ToTransactions();

            transactions.Should().NotBeNull();
            transactions.Count.Should().Be(5);
            int index = 0;
            foreach (Transaction transaction in transactions)
            {
                transaction.Person.Should().NotBeNull();
                transaction.SuspenseResolutionInfo.Should().NotBeNull();

                queryResponse.Items[index]["person"].M["id"].S.Should().BeEquivalentTo(transaction.Person.Id.ToString());
                queryResponse.Items[index]["person"].M["fullName"].S.Should().BeEquivalentTo(transaction.Person.FullName);

                queryResponse.Items[index]["suspense_resolution_info"].M["isConfirmed"].BOOL.
                    Should().Be(transaction.SuspenseResolutionInfo.IsConfirmed);
                queryResponse.Items[index]["suspense_resolution_info"].M["isApproved"].BOOL.
                    Should().Be(transaction.SuspenseResolutionInfo.IsApproved);
                queryResponse.Items[index]["suspense_resolution_info"].M["resolutionDate"].S.
                    Should().BeEquivalentTo(transaction.SuspenseResolutionInfo.ResolutionDate?.ToString("F"));
                queryResponse.Items[index]["suspense_resolution_info"].M["note"].S.
                    Should().BeEquivalentTo(transaction.SuspenseResolutionInfo.Note.ToString());

                queryResponse.Items[index]["address"].S.Should().BeEquivalentTo(transaction.Address);
                queryResponse.Items[index]["bank_account_number"].S.Should().BeEquivalentTo(transaction.BankAccountNumber);
                queryResponse.Items[index]["fund"].S.Should().BeEquivalentTo(transaction.Fund);
                queryResponse.Items[index]["payment_reference"].S.Should().BeEquivalentTo(transaction.PaymentReference);
                queryResponse.Items[index]["transaction_source"].S.Should().BeEquivalentTo(transaction.TransactionSource);
                queryResponse.Items[index]["balance_amount"].N.Should().BeEquivalentTo(transaction.BalanceAmount.ToString("F"));
                queryResponse.Items[index]["period_no"].N.Should().BeEquivalentTo(transaction.PeriodNo.ToString());
                queryResponse.Items[index]["charged_amount"].N.Should().BeEquivalentTo(transaction.ChargedAmount.ToString("F"));
                queryResponse.Items[index]["financial_month"].N.Should().BeEquivalentTo(transaction.FinancialMonth.ToString());
                queryResponse.Items[index]["financial_year"].N.Should().BeEquivalentTo(transaction.FinancialYear.ToString());
                queryResponse.Items[index]["housing_benefit_amount"].N.Should().BeEquivalentTo(transaction.HousingBenefitAmount.ToString("F"));
                queryResponse.Items[index]["id"].S.Should().BeEquivalentTo(transaction.Id.ToString());
                queryResponse.Items[index]["is_suspense"].S.Should().BeEquivalentTo(transaction.IsSuspense.ToString());
                queryResponse.Items[index]["paid_amount"].N.Should().BeEquivalentTo(transaction.PaidAmount.ToString("F"));
                queryResponse.Items[index]["target_id"].S.Should().BeEquivalentTo(transaction.TargetId.ToString());
                queryResponse.Items[index]["transaction_amount"].N.Should().BeEquivalentTo(transaction.TransactionAmount.ToString("F"));
                queryResponse.Items[index]["transaction_date"].S.Should().BeEquivalentTo(transaction.TransactionDate.ToString("F"));
                queryResponse.Items[index]["created_at"].S.Should().BeEquivalentTo(transaction.CreatedAt.ToString("F"));
                queryResponse.Items[index]["last_updated_at"].S.Should().BeEquivalentTo(transaction.LastUpdatedAt.ToString("F"));
                queryResponse.Items[index]["created_by"].S.Should().BeEquivalentTo(transaction.CreatedBy);
                queryResponse.Items[index]["last_updated_by"].S.Should().BeEquivalentTo(transaction.LastUpdatedBy);
                queryResponse.Items[index++]["transaction_type"].S.Should().BeEquivalentTo(transaction.TransactionType.ToString());
            }
        }
    }
}
