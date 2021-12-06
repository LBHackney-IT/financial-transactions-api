using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure;
using Hackney.Core.Sns;
using Nest;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class AddBatchUseCase : IAddBatchUseCase
    {
        private readonly ITransactionGateway _gateway;
        private readonly ISnsGateway _snsGateway;
        private readonly ISnsFactory _snsFactory;

        public AddBatchUseCase(ITransactionGateway gateway, ISnsGateway snsGateway, ISnsFactory snsFactory)
        {
            _gateway = gateway;
            _snsGateway = snsGateway;
            _snsFactory = snsFactory;
        }

        public async Task<int> ExecuteAsync(IEnumerable<Transaction> transactions)
        {
            var transactionList = new List<Transaction>();

            transactions.ToList().ForEach(item =>
            {
                if (!item.IsSuspense)
                {
                    var result = item.HaveAllFieldsInBatchProcessingModel();
                    if (!result)
                    {
                        throw new ArgumentException("Transaction model don't have all information in fields!");
                    }
                }

                item.FinancialMonth = (short) item.TransactionDate.Month;

                item.FinancialYear = (short) item.TransactionDate.Year;

                item.Id = Guid.NewGuid();

                transactionList.Add(item);
            });

            var response = await _gateway.AddBatchAsync(transactionList).ConfigureAwait(false);
            var processingCount = 0;

            foreach (var item in transactionList)
            {
                await PublishSnsMessage(item).ConfigureAwait(false);
                processingCount++;
            }
           
            if (response && (processingCount == transactionList.Count))
                    return transactionList.Count;
            else
                    return 0;
        }

        private async Task PublishSnsMessage(Transaction item)
        {
            var transactionSnsMessage = _snsFactory.Create(item);
            var transactionTopicArn = Environment.GetEnvironmentVariable("TRANSACTION_SNS_ARN");
            await _snsGateway.Publish(transactionSnsMessage, transactionTopicArn, EventConstants.MESSAGEGROUPID).ConfigureAwait(false);
        }
    }
}
