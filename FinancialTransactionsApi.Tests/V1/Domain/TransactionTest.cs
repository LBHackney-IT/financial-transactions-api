using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Domain
{
    public class TransactionTest
    {
        [Fact]
        public void TransactionHasPropertiesSet()
        {
            var transaction = new Transaction()
            {
                Id = new Guid("9b014c26-88be-466e-a589-0f402c6b94c1"),
                TargetId = new Guid("58daf21a-e2d5-475f-87f4-1c0c7f1ffb10"),
                TargetType = TargetType.Tenure,
                TransactionSource = "DD",
                TransactionDate = new DateTime(2021, 8, 1),
                BalanceAmount = 1245.12M,
                ChargedAmount = 456.14M,
                FinancialMonth = 8,
                FinancialYear = 2021,
                Address = "Address",
                Fund = "fund",
                HousingBenefitAmount = 214.11M,
                BankAccountNumber = "12345678",
                PaidAmount = 12356.17M,
                PeriodNo = 35,
                TransactionAmount = 1238.12M,
                TransactionType = TransactionType.ArrangementInterest,
                CreatedAt = new DateTime(2021, 8, 1),
                LastUpdatedAt = new DateTime(2021, 8, 1),
                CreatedBy = "Admin",
                LastUpdatedBy = "Admin"
            };

            transaction.Id.Should().Be(new Guid("9b014c26-88be-466e-a589-0f402c6b94c1"));
            transaction.TargetId.Should().Be(new Guid("58daf21a-e2d5-475f-87f4-1c0c7f1ffb10"));
            transaction.TargetType.Should().Be(TargetType.Tenure);
            transaction.TransactionSource.Should().BeEquivalentTo("DD");
            transaction.TransactionDate.Should().Be(new DateTime(2021, 8, 1));
            transaction.BalanceAmount.Should().Be(1245.12M);
            transaction.ChargedAmount.Should().Be(456.14M);
            transaction.FinancialMonth.Should().Be(8);
            transaction.FinancialYear.Should().Be(2021);
            transaction.Address.Should().BeEquivalentTo("Address");
            transaction.Fund.Should().BeEquivalentTo("fund");
            transaction.HousingBenefitAmount.Should().Be(214.11M);
            transaction.BankAccountNumber.Should().Be("12345678");
            transaction.IsSuspense.Should().BeFalse();
            transaction.PaidAmount.Should().Be(12356.17M);
            transaction.PeriodNo.Should().Be(35);
            transaction.TransactionAmount.Should().Be(1238.12M);
            transaction.TransactionType.Should().Be(TransactionType.ArrangementInterest);
            transaction.CreatedBy.Should().Be("Admin");
            transaction.CreatedAt.Should().Be(new DateTime(2021, 8, 1));
            transaction.LastUpdatedBy.Should().Be("Admin");
            transaction.LastUpdatedAt.Should().Be(new DateTime(2021, 8, 1));
        }
    }
}
