using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetByTargetIdUseCase
    {
        Task<List<Transaction>> ExecuteAsync(Guid targetId);
    }
}
