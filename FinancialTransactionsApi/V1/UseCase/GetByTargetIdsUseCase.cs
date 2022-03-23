using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.DynamoDb;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetByTargetIdsUseCase : IGetByTargetIdsUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetByTargetIdsUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<PagedResult<Transaction>> ExecuteAsync(TransactionByTargetIdsQuery targetIdsQuery)
        {
            return await _gateway.GetPagedTransactionsByTargetIdsAsync(targetIdsQuery).ConfigureAwait(false);
        }
    }
}
