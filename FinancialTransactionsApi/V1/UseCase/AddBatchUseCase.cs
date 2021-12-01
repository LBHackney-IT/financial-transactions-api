using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class AddBatchUseCase : IAddBatchUseCase
    {
        private readonly ITransactionGateway _gateway;

        public AddBatchUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<int> ExecuteAsync(IEnumerable<Transaction> transactions)
        {
            int processingCount = 0;
            foreach (var transaction in transactions)
            {
                var response = await AddWeeklyCharge(transaction).ConfigureAwait(false);
                if (response)
                    processingCount++;
            }
            return processingCount;
        }

        private async Task<bool> AddWeeklyCharge(Transaction transaction)
        {
            transaction.FinancialMonth = (short) transaction.TransactionDate.Month;

            transaction.FinancialYear = (short) transaction.TransactionDate.Year;

            transaction.Id = Guid.NewGuid();
            DateTime currentDate = DateTime.UtcNow;

            transaction.CreatedAt = currentDate;
            transaction.LastUpdatedAt = currentDate;
            transaction.LastUpdatedBy = transaction.CreatedBy;

            await _gateway.AddAsync(transaction).ConfigureAwait(false);

            return true;
        }
    }
}
