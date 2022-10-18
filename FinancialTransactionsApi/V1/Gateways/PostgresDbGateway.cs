
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using Hackney.Core.DynamoDb;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class PostgresDbGateway : IPostgresDbGateway
    {
        private readonly DatabaseContext _databaseContext;
        public PostgresDbGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<TransactionPostgres> GetTransactionByIdAsync(Guid targetId, Guid id)
        {
            var data = await _databaseContext.TransactionEntities.AsNoTracking().Where(t => t.Id == id || t.TargetId == targetId).FirstOrDefaultAsync().ConfigureAwait(false);
            return data?.ToDomain();
        }
    }
}