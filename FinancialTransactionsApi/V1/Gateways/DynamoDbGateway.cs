using Amazon.DynamoDBv2.DataModel;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;

namespace TransactionsApi.V1.Gateways
{
    public class DynamoDbGateway : ITransactionGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }

        public async Task AddRangeAsync(List<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                await AddAsync(transaction).ConfigureAwait(false);
            }
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(Guid targetid, string transactionType, DateTime? startDate, DateTime? endDate)
        {
            var scanConditions = new List<ScanCondition>();
            scanConditions.Add(new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.GreaterThan, Guid.Parse("00000000-0000-0000-0000-000000000000")));

            if (transactionType != null)
                scanConditions.Add(new ScanCondition("TransactionType", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, transactionType));
            if (targetid != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                scanConditions.Add(new ScanCondition("TargetId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, targetid));
            if (startDate.HasValue && endDate.HasValue)
            {
                scanConditions.Add(new ScanCondition("TransactionDate", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Between, startDate, endDate));
            }
            
            List<TransactionDbEntity> data =
                await _dynamoDbContext
                .ScanAsync<TransactionDbEntity>(scanConditions)
                .GetRemainingAsync()
                .ConfigureAwait(false);

            return data.Select(p => p.ToDomain()).ToList();
        }
        public async Task<List<Transaction>> GetAllTransactionsSummaryAsync(Guid targetid, DateTime? startDate, DateTime? endDate)
        {
            var scanConditions = new List<ScanCondition>();
            
            if (targetid != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                scanConditions.Add(new ScanCondition("TargetId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, targetid));
            if (startDate.HasValue && endDate.HasValue)
            {
                scanConditions.Add(new ScanCondition("TransactionDate", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Between, startDate, endDate));
            }
            
            List<TransactionDbEntity> data =
                await _dynamoDbContext
                .ScanAsync<TransactionDbEntity>(scanConditions)
                .GetRemainingAsync()
                .ConfigureAwait(false);

            return data.Select(p => p.ToDomain()).OrderByDescending(x => x.TransactionDate).ToList();
        }
        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var data =  await _dynamoDbContext.LoadAsync<TransactionDbEntity>(id).ConfigureAwait(false);
            return data?.ToDomain();
        } 
    }
}
