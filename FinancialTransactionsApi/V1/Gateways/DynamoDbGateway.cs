using Amazon.DynamoDBv2.DataModel;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class DynamoDbGateway : ITransactionGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly DynamoDbContextWrapper _wrapper;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, DynamoDbContextWrapper wrapper)
        {
            _dynamoDbContext = dynamoDbContext;
            _wrapper = wrapper;
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

        public async Task<List<Transaction>> GetAllTransactionsAsync(Guid targetId, TransactionType? transactionType, DateTime? startDate, DateTime? endDate)
        {
            List<ScanCondition> scanConditions = new List<ScanCondition>
            {
                new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.GreaterThan, Guid.Parse("00000000-0000-0000-0000-000000000000"))
            };

            if (transactionType != null)
            {
                scanConditions.Add(new ScanCondition("TransactionType", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, transactionType));
            }

            if (targetId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                scanConditions.Add(new ScanCondition("TargetId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, targetId));
            }

            if (startDate.HasValue)
            {
                if (endDate == null)
                {
                    endDate = DateTime.Now;
                }

                scanConditions.Add(new ScanCondition("TransactionDate", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Between, startDate.Value, endDate.Value));
            }

            var data = await _wrapper
              .ScanAsync(_dynamoDbContext, scanConditions)
              .ConfigureAwait(false);

            return data.Select(p => p.ToDomain()).ToList();
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var data = await _wrapper.LoadAsync(_dynamoDbContext, id).ConfigureAwait(false);

            return data?.ToDomain();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }
    }
}
