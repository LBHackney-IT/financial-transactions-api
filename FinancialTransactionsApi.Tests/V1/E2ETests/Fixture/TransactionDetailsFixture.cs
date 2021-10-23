using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Fixture
{

    public sealed class TransactionDetailsFixture : IDisposable
    {
        private readonly AutoFixture.Fixture _fixture = new AutoFixture.Fixture();
        public readonly IDynamoDBContext DbContext;
        public List<TransactionDbEntity> Transactions { get; private set; } = new List<TransactionDbEntity>();
        public AddTransactionRequest TransactionRequestObject { get; private set; } = new AddTransactionRequest();
        public TransactionResponse TransactionResponse { get; private set; } = new TransactionResponse();
        public Transaction Transaction { get; private set; } = new Transaction();
        public const int Maxresults = 10;
        public Guid Id { get; private set; }
        public Guid TargetId { get; private set; }

        public string InvalidTargetId { get; private set; }

        public TransactionDetailsFixture(IDynamoDBContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (Transactions.Any())
                    foreach (var transaction in Transactions)
                        DbContext.DeleteAsync(transaction).GetAwaiter().GetResult();

                _disposed = true;
            }
        }

        private IEnumerable<TransactionDbEntity> CreateTransactions(int count, bool isActive)
        {
            return _fixture.Build<TransactionDbEntity>()
                .With(x => x.BankAccountNumber, "1234567")
                .With(x => x.TransactionDate, DateTime.UtcNow)
                .With(x => x.IsSuspense, isActive)
                .With(x => x.TransactionType, FinancialTransactionsApi.V1.Domain.TransactionType.Charge)
                .With(x => x.TargetId, Guid.NewGuid()).CreateMany(count);
        }

        public void GivenATransactionAlreadyExist()
        {
            GivenATransactionAlreadyExist(Maxresults);
        }

        public void GivenATransactionAlreadyExist(int count)
        {
            if (Transactions.Any()) return;
            var random = new Random();
            Id = Guid.NewGuid();
            if (count == 1)
            {
                Id = new Guid("9e067bac-56ed-4802-a83f-b1e32f09177e");
            }
            Func<DateTime> funcDt = () => DateTime.UtcNow.AddDays(0 - random.Next(100));
            Transactions.AddRange(_fixture.Build<TransactionDbEntity>()
                .With(x => x.Id, Id)
                .With(x => x.TransactionDate, funcDt)
                .With(x => x.TransactionType, TransactionType.Charge)
                .With(x => x.TargetId, Guid.NewGuid)
                .With(x => x.Address, "Address")
                .With(x => x.BalanceAmount, 154.12M)
                .With(x => x.ChargedAmount, 123.78M)
                .With(x => x.FinancialMonth, 8)
                .With(x => x.FinancialYear, 2021)
                .With(x => x.BankAccountNumber, "12345678")
                .With(x => x.IsSuspense, true)
                .With(x => x.PaidAmount, 125.62M)
                .With(x => x.PeriodNo, 31)
                .With(x => x.TransactionSource, "DD")
                .With(x => x.TransactionAmount, 186.90M)
                .CreateMany(count));
            foreach (var transaction in Transactions)
            {
                transaction.Person = new Person()
                {
                    Id = Guid.NewGuid(),
                    FullName = "Hyan Widro"
                };
                DbContext.SaveAsync(transaction).GetAwaiter().GetResult();
            }
        }

        public void GivenTransactionWithMultiplePagesAlreadyExist()
        {
            GivenATransactionAlreadyExist(35);
        }

        public void GivenATargetIdHasNoActivities()
        {
            TargetId = Guid.NewGuid();
        }

        public void GivenAnInvalidTargetId()
        {
            InvalidTargetId = "12345667890";
        }

        public void GivenAnNewTransactionRequestWithAnBankAccountNumberLength(string bankAccountNumber)
        {
            var transaction = ConstructTransaction();
            TransactionRequestObject = new AddTransactionRequest()
            {

                TransactionDate = transaction.TransactionDate,
                Address = transaction.Address,
                BalanceAmount = transaction.BalanceAmount,
                ChargedAmount = transaction.ChargedAmount,
                Fund = transaction.Fund,
                HousingBenefitAmount = transaction.HousingBenefitAmount,
                IsSuspense = transaction.IsSuspense,
                BankAccountNumber = bankAccountNumber,
                PaidAmount = transaction.PaidAmount,
                PaymentReference = transaction.PaymentReference,
                PeriodNo = transaction.PeriodNo,
                Person = transaction.Person,
                TargetId = transaction.TargetId,
                TransactionAmount = transaction.TransactionAmount,
                TransactionSource = transaction.TransactionSource,
                TransactionType = transaction.TransactionType
            };
        }
        public void GivenANewTransactionRequest()
        {
            var transaction = ConstructTransaction();
            TransactionRequestObject = new AddTransactionRequest()
            {

                TransactionDate = transaction.TransactionDate,
                Address = transaction.Address,
                BalanceAmount = transaction.BalanceAmount,
                ChargedAmount = transaction.ChargedAmount,
                Fund = transaction.Fund,
                HousingBenefitAmount = transaction.HousingBenefitAmount,
                IsSuspense = transaction.IsSuspense,
                BankAccountNumber = transaction.BankAccountNumber,
                PaidAmount = transaction.PaidAmount,
                PaymentReference = transaction.PaymentReference,
                PeriodNo = transaction.PeriodNo,
                Person = transaction.Person,
                TargetId = transaction.TargetId,
                TransactionAmount = transaction.TransactionAmount,
                TransactionSource = transaction.TransactionSource,
                TransactionType = transaction.TransactionType
            };
        }

        public void GivenAUpdateTransactionRequest()
        {
            var transaction = new TransactionDbEntity
            {
                Id = new Guid("6479ffee-b0e8-4c2a-b887-63f2dec086aa"),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 154.12M,
                ChargedAmount = 123.78M,
                FinancialMonth = 8,
                FinancialYear = 2021,
                BankAccountNumber = "12345678",
                IsSuspense = true,
                PaidAmount = 125.62M,
                PeriodNo = 31,
                TargetId = new Guid("9e067bac-56ed-4802-a83f-b1e32f09177e"),
                TransactionAmount = 186.90M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Rent,
                Person = new Person()
                {
                    Id = new Guid("1c046cca-e9a7-403a-8b6f-8abafc4ee126"),
                    FullName = "Hyan Widro"
                }
            };
            DbContext.SaveAsync(transaction).GetAwaiter().GetResult();
            transaction.PaymentReference = "PaymentReference";
            transaction.Fund = "Fund";
            transaction.HousingBenefitAmount = 999.9M;
            transaction.SuspenseResolutionInfo = new SuspenseResolutionInfo()
            {
                IsConfirmed = true,
                IsApproved = true,
                ResolutionDate = new DateTime(2021, 9, 1),
                Note = "Note"
            };
            transaction.IsSuspense = false;
            Transaction = new Transaction
            {
                Id = transaction.Id,
                Address = transaction.Address,
                BalanceAmount = transaction.BalanceAmount,
                ChargedAmount = transaction.ChargedAmount,
                BankAccountNumber = transaction.BankAccountNumber,
                PaidAmount = transaction.PaidAmount,
                PeriodNo = transaction.PeriodNo,
                Person = transaction.Person,
                TargetId = transaction.TargetId,
                TransactionAmount = transaction.TransactionAmount,
                TransactionSource = transaction.TransactionSource,
                FinancialMonth = transaction.FinancialMonth,
                FinancialYear = transaction.FinancialYear,
                TransactionType = transaction.TransactionType,
                PaymentReference = "PaymentReference",
                Fund = "Fund",
                HousingBenefitAmount = 999.9M,
                SuspenseResolutionInfo = new SuspenseResolutionInfo
                {
                    IsConfirmed = true,
                    IsApproved = true,
                    ResolutionDate = new DateTime(2021, 9, 1),
                    Note = "Note"
                },
                IsSuspense = false,
                TransactionDate = new DateTime(2021, 8, 1),
            };


        }
        public void GivenAnInvalidNewTransactionRequest()
        {
            TransactionRequestObject = new AddTransactionRequest
            {
                TransactionAmount = -2000,
                PaidAmount = -2334,
                ChargedAmount = -213,
                HousingBenefitAmount = -1
            };
        }
        private Transaction ConstructTransaction()
        {
            var entity = _fixture.Create<Transaction>();

            entity.TransactionDate = new DateTime(2021, 8, 1);
            entity.PeriodNo = 35;
            entity.Fund = null;
            entity.IsSuspense = true;
            entity.SuspenseResolutionInfo = null;
            entity.BankAccountNumber = "12345678";

            return entity;
        }
    }
}
