using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Gateways;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class AddUseCase : IAddUseCase
    {
        private readonly ITransactionGateway _gateway;
        public AddUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<TransactionResponseObject> ExecuteAsync(TransactionRequest transaction)
        {
            var transactionDomain = transaction.ToTransactionDomain();
            transactionDomain.Id = Guid.NewGuid();
            await _gateway.AddAsync(transactionDomain).ConfigureAwait(false);
            return transactionDomain.ToResponse();
        }
    }
}
