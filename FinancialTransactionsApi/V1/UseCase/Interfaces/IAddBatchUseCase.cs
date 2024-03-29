using FinancialTransactionsApi.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IAddBatchUseCase
    {
        public Task<int> ExecuteAsync(IEnumerable<Transaction> transactions);
    }
}
