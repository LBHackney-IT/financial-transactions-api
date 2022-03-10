using FinancialTransactionsApi.V1.Domain;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface ISuspenseAccountApprovalUseCase
    {
        public Task<bool> ExecuteAsync(Transaction transaction);
    }
}
