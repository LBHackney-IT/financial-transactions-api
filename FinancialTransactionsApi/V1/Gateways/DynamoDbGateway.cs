using Amazon.DynamoDBv2.DataModel;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using FinancialTransactionsApi.V1.Gateways;

namespace TransactionsApi.V1.Gateways
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

        public async Task<List<Transaction>> GetAllTransactionsAsync(Guid targetid, string transactionType, DateTime? startDate, DateTime? endDate)
        {
            //string DATEFORMAT = "yyyy-MM-ddTHH\\:mm\\:ss.fffffffZ";
            List<ScanCondition> scanConditions = new List<ScanCondition>
            {
                new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.GreaterThan, Guid.Parse("00000000-0000-0000-0000-000000000000"))
            };

            if (transactionType != null)
                scanConditions.Add(new ScanCondition("TransactionType", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, transactionType));
            if (targetid != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                scanConditions.Add(new ScanCondition("TargetId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, targetid));
           
            if (startDate.HasValue)
            {
                if (endDate == null)
                    endDate = DateTime.Now;
                scanConditions.Add(new ScanCondition("TransactionDate", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Between, startDate.Value, endDate.Value));
            }

                

            //List<TransactionDbEntity> data =
            //    await _dynamoDbContext
            //    .ScanAsync<TransactionDbEntity>(scanConditions)
            //    .GetRemainingAsync()
            //    .ConfigureAwait(false);

            List<TransactionDbEntity> data =
              await _wrapper
              .ScanAsync(_dynamoDbContext, scanConditions)
              .ConfigureAwait(false);

            return data.Select(p => p.ToDomain()).ToList();
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var data =  await _dynamoDbContext.LoadAsync<TransactionDbEntity>(id).ConfigureAwait(false);
            return data?.ToDomain();
        } 
    }
}
