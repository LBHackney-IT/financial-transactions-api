using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.Sns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class AddBatchUseCase : IAddBatchUseCase
    {
        private readonly ISnsGateway _snsGateway;
        private readonly ISnsFactory _snsFactory;

        public AddBatchUseCase(ISnsGateway snsGateway, ISnsFactory snsFactory)
        {
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

            var sentSnsEventsCount = 0;

            foreach (var item in transactionList)
            {
                await PublishSnsMessage(item).ConfigureAwait(false);
                sentSnsEventsCount++;
            }

            if (sentSnsEventsCount == transactionList.Count)
            {
                return transactionList.Count;
            }
            else
            {
                return 0;
            }
        }

        private async Task PublishSnsMessage(Transaction item)
        {
            var transactionSnsMessage = _snsFactory.Create(item);
            var transactionTopicArn = Environment.GetEnvironmentVariable("TRANSACTION_SNS_ARN");
            await _snsGateway.Publish(transactionSnsMessage, transactionTopicArn, EventConstants.MESSAGEGROUPID).ConfigureAwait(false);
        }
    }
}
