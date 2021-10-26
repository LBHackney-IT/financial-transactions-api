using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Factories
{
    public interface ISnsFactory
    {
        TransactionSns Create(Transaction transaction);

        TransactionSns Update(Transaction transaction);
    }
}
