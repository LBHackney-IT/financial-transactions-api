using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace FinancialTransactionsApi.Tests.V1.E2ETests.Fixture
{

    public class TransactionFixture //: BaseFixture
    {
        public List<Transaction> Transactions { get; private set; }
        private readonly AutoFixture.Fixture _fixture = new AutoFixture.Fixture();
        public AddTransactionRequest TransactionRequestObject { get; private set; } = new AddTransactionRequest();

        public TransactionFixture()
        {

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
                BankAccountNumber = transaction.BankAccountNumber,
                PaidAmount = transaction.PaidAmount,
                PeriodNo = transaction.PeriodNo,
                TargetId = transaction.TargetId,
                TransactionAmount = transaction.TransactionAmount,
                TransactionSource = transaction.TransactionSource,
                TransactionType = transaction.TransactionType
            };
        }
        public void GivenAnInvalidNewTransactionRequest()
        {
            TransactionRequestObject = new AddTransactionRequest();
        }

        private Transaction ConstructTransaction()
        {
            var entity = _fixture.Create<Transaction>();

            entity.TransactionDate = new DateTime(2021, 8, 1);
            entity.PeriodNo = 35;
            entity.Fund = null;
            entity.SuspenseResolutionInfo = null;
            entity.BankAccountNumber = "12345678";

            return entity;
        }

    }
}
