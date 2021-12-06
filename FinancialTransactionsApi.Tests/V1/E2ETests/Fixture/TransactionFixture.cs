using AutoFixture;
using Elasticsearch.Net;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways.Models;
using Nest;
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
        public readonly IElasticClient ElasticSearchClient;
        public AddTransactionRequest TransactionRequestObject { get; private set; } = new AddTransactionRequest();

        private const string Index = "transactions";
        public static readonly string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public TransactionFixture(IElasticClient elasticClient)
        {
            ElasticSearchClient = elasticClient;
            //WaitForEsInstance();
        }

        public void GivenAnTransactionIndexExists()
        {
            ElasticSearchClient.Indices.Delete(Index);

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(Index)).Exists)
            {
                var transactionSettingsDoc = File.ReadAllTextAsync("data/indexes/transactions.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(Index, transactionSettingsDoc)
                    .ConfigureAwait(true);

                var transactions = CreateTransactionsData();
                var awaitable = ElasticSearchClient.IndexManyAsync(transactions, Index).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
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
                PaymentReference = transaction.PaymentReference,
                PeriodNo = transaction.PeriodNo,
                Person = transaction.Person,
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
        private IEnumerable<QueryableTransaction> CreateTransactionsData()
        {
            var listOfTransactions = new List<QueryableTransaction>();
            var random = new Random();

            foreach (var value in Alphabet)
            {
                for (int i = 0; i < 10; i++)
                {
                    var transaction = _fixture.Create<QueryableTransaction>();
                    transaction.Address = value;
                    transaction.PaymentReference = value;

                    listOfTransactions.Add(transaction);
                }
            }

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var transaction = _fixture.Create<QueryableTransaction>();

                var value = Alphabet[random.Next(0, Alphabet.Length)];
                transaction.Address = value;

                listOfTransactions.Add(transaction);
            }

            return listOfTransactions;
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
