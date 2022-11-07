using FinancialTransactionsApi.V1.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<TransactionEntity> Transactions { get; set; }
    }
}
