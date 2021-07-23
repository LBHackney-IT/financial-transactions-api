using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure.Entities;

namespace FinancialTransactionsApi.V1.Factories
{
    public static class PersonFactory
    {
        public static PersonDbEntity ToDatabase(this Person person)
        {
            return person == null ? null : new PersonDbEntity()
            {
                Id = person.Id,
                FullName = person.FullName
            };
        }

        public static Person ToDomain(this PersonDbEntity personDbEntity)
        {
            return personDbEntity == null ? null : new Person()
            {
                Id = personDbEntity.Id,
                FullName = personDbEntity.FullName
            };
        }

        public static Person ToDomain(this Person person)
        {
            return person == null ? null : new Person()
            {
                Id = person.Id,
                FullName = person.FullName
            };
        }
    }
}
