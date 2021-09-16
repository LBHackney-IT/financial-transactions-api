using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
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

        public async Task<int> ExecuteAsync(IEnumerable<AddTransactionRequest> transactions)
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

        private async Task<bool> AddWeeklyCharge(AddTransactionRequest transaction)
        {
            if (!transaction.IsSuspense)
            {
                var result = transaction.HaveAllFieldsInAddWeeklyChargeModel();
                if (!result)
                {
                    throw new ArgumentException("Transaction model dont have all information in fields!");
                }
            }

            var transactionDomain = transaction.ToDomain();

            transactionDomain.FinancialMonth = (short) transaction.TransactionDate.Month;

            transactionDomain.FinancialYear = (short) transaction.TransactionDate.Year;

            transactionDomain.Id = Guid.NewGuid();

            await _gateway.AddAsync(transactionDomain).ConfigureAwait(false);

            return true;
        }
    }
}
