using Microsoft.EntityFrameworkCore;

namespace TransactionsApi.V1.Infrastructure
{

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TransactionDbEntity> TransactionEntities { get; set; }
    }
}
