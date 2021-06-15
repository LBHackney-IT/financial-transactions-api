using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class EntityFactory
    {
        public static Transaction ToDomain(this DatabaseEntity databaseEntity)
        {
            //TODO: Map the rest of the fields in the domain object.
            // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings

            return new Transaction
            {
                Id = databaseEntity.Id,
                CreatedAt = databaseEntity.CreatedAt
            };
        }

        public static DatabaseEntity ToDatabase(this Transaction entity)
        {
            //TODO: Map the rest of the fields in the database object.

            return new DatabaseEntity
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
