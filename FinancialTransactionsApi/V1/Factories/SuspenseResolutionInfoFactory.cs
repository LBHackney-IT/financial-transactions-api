using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure.Entities;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class SuspenseResolutionInfoFactory
    {
        public static SuspenseResolutionInfoDbEntity ToDatabase(this SuspenseResolutionInfo suspense)
        {
            return suspense == null ? null : new SuspenseResolutionInfoDbEntity()
            {
                ResolutionDate = suspense.ResolutionDate,
                IsResolve = suspense.IsResolve,
                Note = suspense.Note
            };
        }

        public static SuspenseResolutionInfo ToDomain(this SuspenseResolutionInfoDbEntity suspenseDbEntity)
        {
            return suspenseDbEntity == null ? null : new SuspenseResolutionInfo()
            {
                ResolutionDate = suspenseDbEntity.ResolutionDate,
                IsResolve = suspenseDbEntity.IsResolve,
                Note = suspenseDbEntity.Note
            };
        }
        public static SuspenseResolutionInfo ToDomain(this SuspenseResolutionInfo suspense)
        {
            return suspense == null ? null : new SuspenseResolutionInfo()
            {
                ResolutionDate = suspense.ResolutionDate,
                IsResolve = suspense.IsResolve,
                Note = suspense.Note
            };
        }
    }
}
