using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Gateways.Sns.Interfaces
{
    public interface ISnsGateway
    {
        Task Publish(TransactionsSns transactionsSns);
    }
}
