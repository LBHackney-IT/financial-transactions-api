using System.Text.Json.Serialization;

namespace FinancialTransactionsApi.V1.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        Rent,
        Charge
    }
}
