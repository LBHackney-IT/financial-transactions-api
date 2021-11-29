using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/statements")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class StatementController : BaseController
    {

        private readonly IExportSelectedItemUseCase _exportSelectedItemUseCase;
        private readonly IExportStatementUseCase _exportStatementUseCase;

        public StatementController(IExportStatementUseCase exportStatementUseCase,
                                   IExportSelectedItemUseCase exportSelectedItemUseCase)
        {
            _exportStatementUseCase = exportStatementUseCase;
            _exportSelectedItemUseCase = exportSelectedItemUseCase;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("export")]
        public async Task<IActionResult> ExportStatementReportAsync([FromBody] ExportTransactionQuery query)
        {
            var result = await _exportStatementUseCase.ExecuteAsync(query).ConfigureAwait(false);
            if (result == null)
                return NotFound($"No records found for the following ID: {query.TargetId}");
            if (query?.FileType == "pdf")
            {
                return File(result, "application/pdf", $"{query.StatementType}_{DateTime.UtcNow.Ticks}.{query.FileType}");
            }
            return File(result, "text/csv", $"{query.StatementType}_{DateTime.UtcNow.Ticks}.{query.FileType}");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("selection/export")]
        public async Task<IActionResult> ExportSelectedItemAsync([FromBody] TransactionExportRequest request)
        {
            var result = await _exportSelectedItemUseCase.ExecuteAsync(request).ConfigureAwait(false);
            if (result == null)
                return NotFound("No record found");
            return File(result, "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
        }
    }
}
