using FinancialTransactionsApi.V1.Boundary.Response;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(int id);
    }
}
