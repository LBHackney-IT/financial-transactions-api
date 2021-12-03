using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Domain;

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
            var transactionIdList = new List<Guid>();
            var transactionList = new List<Transaction>();

            transactions.ToList().ForEach(item =>
            {
                if (!item.IsSuspense)
                {
                    var result = item.HaveAllFieldsInAddWeeklyChargeModel();
                    if (!result)
                    {
                        throw new ArgumentException("Transaction model don't have all information in fields!");
                    }
                }
                var transactionDomain = item.ToDomain();

                transactionDomain.FinancialMonth = (short) item.TransactionDate.Month;

                transactionDomain.FinancialYear = (short) item.TransactionDate.Year;

                transactionDomain.Id = Guid.NewGuid();
                transactionIdList.Add(transactionDomain.Id);
                transactionList.Add(transactionDomain);
            });

            var response = await _gateway.AddBatchAsync(transactionList).ConfigureAwait(false);
           
                if (response)
                    return transactionList.Count;
                else
                    return 0;
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
