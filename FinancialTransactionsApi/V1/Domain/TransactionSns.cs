using Hackney.Core.Sns;
using System;

namespace FinancialTransactionsApi.V1.Domain
{
    public class TransactionSns
    {
        public Guid Id { get; set; }

        public string EventType { get; set; }

        public string SourceDomain { get; set; }

        public string SourceSystem { get; set; }

        public string Version { get; set; }

        public Guid CorrelationId { get; set; }

        public DateTime DateTime { get; set; }

        public User User { get; set; }

        public Guid EntityId { get; set; }

        public Guid EntityTargetId { get; set; }

        public EventData EventData { get; set; } = new EventData();
    }
}
