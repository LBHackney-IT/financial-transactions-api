using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Helpers;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetByTargetIdUseCase
    {
        Task<ResponseWrapper<IEnumerable<TransactionResponse>>> ExecuteAsync(string targetType, Guid targetId, DateTime? startDate, DateTime? endDate);
    }
}
