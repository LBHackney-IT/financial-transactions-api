using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Factories
{
    public class TransactionFactoryTest
    {
        [Fact]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var dbEntity = new TransactionDbEntity()
            {
                Id = Guid.NewGuid(),
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                FinancialMonth = 2,
                FinancialYear = 2022,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                },
                SuspenseResolutionInfo = new SuspenseResolutionInfo
                {
                    ResolutionDate = new DateTime(2021, 8, 1),
                    Note = "Some note"
                },
                CreatedBy = "Admin",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "Admin"
            };

            var domain = dbEntity.ToDomain();

            domain.Should().BeEquivalentTo(dbEntity);
        }

        [Fact]
        public void CanMapADomainEntityToADatabaseObject()
        {
            var domain = new Transaction()
            {
                Id = Guid.NewGuid(),
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                FinancialMonth = 2,
                FinancialYear = 2022,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                },
                SuspenseResolutionInfo = new SuspenseResolutionInfo()
                {
                    ResolutionDate = new DateTime(2021, 8, 1),
                    IsConfirmed = true,
                    IsApproved = true,
                    Note = "Some note"
                },
                CreatedBy = "Admin",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "Admin"
            };

            var dbEntity = domain.ToDatabase();

            dbEntity.Should().BeEquivalentTo(domain);
        }

        [Fact]
        public void CanMapAddRequestEntityToADomainObject()
        {
            var request = new AddTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                }
            };

            var domain = request.ToDomain();

            domain.Should().BeEquivalentTo(request);
        }

        [Fact]
        public void CanMapUpdateRequestEntityToADomainObject()
        {
            var request = new UpdateTransactionRequest()
            {
                TargetId = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Address = "Address",
                BalanceAmount = 145.23M,
                ChargedAmount = 134.12M,
                Fund = "HSGSUN",
                HousingBenefitAmount = 123.12M,
                BankAccountNumber = "12345678",
                IsSuspense = true,
                PaidAmount = 123.22M,
                PaymentReference = "123451",
                PeriodNo = 2,
                TransactionAmount = 126.83M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Charge,
                Person = new Person()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Kain Hyawrd"
                },
                SuspenseResolutionInfo = new SuspenseResolutionInfo()
                {
                    ResolutionDate = new DateTime(2021, 8, 1),
                    IsConfirmed = true,
                    IsApproved = true,
                    Note = "Some note"
                }
            };

            var domain = request.ToDomain();

            domain.Should().BeEquivalentTo(request);
        }

        [Fact]
        public void CanMapAddRequestEntityCollectionToADomainObjectCollection()
        {
            var requestCollection = new List<AddTransactionRequest>
            {
                new AddTransactionRequest()
                {
                    TargetId = Guid.NewGuid(),
                    TransactionDate = DateTime.UtcNow,
                    Address = "Address",
                    BalanceAmount = 145.23M,
                    ChargedAmount = 134.12M,
                    Fund = "HSGSUN",
                    HousingBenefitAmount = 123.12M,
                    BankAccountNumber = "12345678",
                    IsSuspense = true,
                    PaidAmount = 123.22M,
                    PaymentReference = "123451",
                    PeriodNo = 2,
                    TransactionAmount = 126.83M,
                    TransactionSource = "DD",
                    TransactionType = TransactionType.Charge,
                    Person = new Person()
                    {
                        Id = Guid.NewGuid(),
                        FullName = "Kain Hyawrd"
                    }
                },
                new AddTransactionRequest()
                {
                    TargetId = Guid.NewGuid(),
                    TransactionDate = DateTime.UtcNow,
                    Address = "Address2",
                    BalanceAmount = 145.23M,
                    ChargedAmount = 134.12M,
                    Fund = "HSGSUN2",
                    HousingBenefitAmount = 123.12M,
                    BankAccountNumber = "12345658",
                    IsSuspense = true,
                    PaidAmount = 123.22M,
                    PaymentReference = "123451",
                    PeriodNo = 2,
                    TransactionAmount = 126.83M,
                    TransactionSource = "DD",
                    TransactionType = TransactionType.Charge,
                    Person = new Person()
                    {
                        Id = Guid.NewGuid(),
                        FullName = "Andrew Hyawrd"
                    }
                },
            };

            var domainCollection = requestCollection.ToDomain();

            domainCollection.Should().BeEquivalentTo(requestCollection);
        }
    }
}
