using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using Hackney.Core.DynamoDb;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetByTargetIdsUseCase
    {
        Task<PagedResult<Transaction>> ExecuteAsync(TransactionByTargetIdsQuery targetIdsQuery);
    }
}
