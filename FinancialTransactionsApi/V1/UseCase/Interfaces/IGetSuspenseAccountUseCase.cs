using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using Hackney.Shared.Finance.Pagination;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetSuspenseAccountUseCase
    {
        public Task<Paginated<Transaction>> ExecuteAsync(SuspenseAccountQuery query);
    }
}
