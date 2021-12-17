namespace FinancialTransactionsApi.V1.Infrastructure
{
    public static class EventConstants
    {

        public const string VERSION = "v1";
        public const string EVENTTYPE = "TransactionCreatedEvent";
        public const string SOURCEDOMAIN = "Transactions";
        public const string SOURCESYSTEM = "TransactionAPI";
        public const string MESSAGEGROUPID = "TransactionCreatedMessageGroup";
    }
}
