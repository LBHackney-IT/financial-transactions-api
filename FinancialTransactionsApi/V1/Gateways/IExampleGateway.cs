using System.Collections.Generic;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Gateways
{
    public interface IExampleGateway
    {
        Transaction GetEntityById(int id);

        List<Transaction> GetAll();
    }
}
