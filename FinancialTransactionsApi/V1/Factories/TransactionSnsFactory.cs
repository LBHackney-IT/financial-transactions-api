using System;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using Hackney.Core.Sns;
using EventData = Hackney.Core.Sns.EventData;

namespace FinancialTransactionsApi.V1.Factories
{
    public class TransactionSnsFactory : ISnsFactory
    {
        public TransactionSns Create(Transaction transaction)
        {
            return new TransactionSns
            {
                CorrelationId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                EntityId = transaction.Id,
                Id = Guid.NewGuid(),
                EventType = EventConstants.EVENTTYPE,
                Version = EventConstants.VERSION,
                SourceDomain = EventConstants.SOURCEDOMAIN,
                SourceSystem = EventConstants.SOURCESYSTEM,
                User = new User
                {
                    // Name = token.Name,
                    // Email = token.Email
                },
                EventData = new EventData
                {
                    NewData = transaction
                }
            };
        }

        public TransactionSns Update(Transaction transaction)
        {
            return new TransactionSns
            {
                CorrelationId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                EntityId = transaction.Id,
                Id = Guid.NewGuid(),
                EventType = EventConstants.EVENTTYPE,
                Version = EventConstants.VERSION,
                SourceDomain = EventConstants.SOURCEDOMAIN,
                SourceSystem = EventConstants.SOURCESYSTEM,
                User = new User
                {
                    // Name = token.Name,
                    // Email = token.Email
                },
                EventData = new EventData
                {
                    NewData = transaction
                }
            };
        }
    }
}
