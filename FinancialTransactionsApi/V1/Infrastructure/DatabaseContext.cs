using FinancialTransactionsApi.V1.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionsApi.V1.Infrastructure
{

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TransactionDbEntity> TransactionEntities { get; set; }
    }
}
