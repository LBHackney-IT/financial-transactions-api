using AutoFixture;
using Elasticsearch.Net;
using FinancialTransactionsApi.V1.Gateways.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Fixture
{

    public class TransactionFixture : BaseFixture
    {
        // public List<QueryableTransaction> Transactions { get; private set; }
        private const string Index = "transactions";
        public static readonly string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public TransactionFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            WaitForEsInstance();
        }

        public void GivenAnTransactionIndexExists()
        {
            ElasticSearchClient.Indices.Delete(Index);

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(Index)).Exists)
            {
                var transactionSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/transactionIndex.json").Result;
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

        private static IEnumerable<QueryableTransaction> CreateTransactionsData()
        {
            var listOfTransactions = new List<QueryableTransaction>();
            var fixture = new AutoFixture.Fixture();
            var random = new Random();

            foreach (var value in Alphabet)
            {
                for (int i = 0; i < 10; i++)
                {
                    var transaction = fixture.Create<QueryableTransaction>();
                    transaction.Address = value;
                    transaction.PaymentReference = value;

                    listOfTransactions.Add(transaction);
                }
            }

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var transaction = fixture.Create<QueryableTransaction>();

                var value = Alphabet[random.Next(0, Alphabet.Length)];
                transaction.Address = value;

                listOfTransactions.Add(transaction);
            }

            return listOfTransactions;
        }
    }
}
