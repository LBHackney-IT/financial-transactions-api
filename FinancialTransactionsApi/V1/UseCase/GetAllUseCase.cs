using FinancialTransactionsApi.V1.Boundary.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using FinancialTransactionsApi.V1.Boundary.Request;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetAllUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<TransactionResponse>> ExecuteAsync(TransactionQuery query)
        {
            var transactions =
                await _gateway.GetAllTransactionsAsync(query.TargetId, query.TransactionType, query.StartDate, query.EndDate)
                    .ConfigureAwait(false);

            return transactions.ToResponse();
        }
    }
}
