using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Infrastructure;
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
        private readonly IGetByTargetIdsUseCase _getByTargetIdsUseCase;

        public StatementController(IGetByTargetIdsUseCase getByTargetIdsUseCase)
        {
            _getByTargetIdsUseCase = getByTargetIdsUseCase;
        }
        /// <summary>
        /// Get transaction by provided id
        /// </summary>
        /// <response code="200">Success. Transaction model was received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Transaction by provided id cannot be found</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="targetIdsQuery">The value by which we are looking for a transaction</param>
        /// <returns>List of transactions</returns>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetByTargetId([FromQuery] TransactionByTargetIdsQuery targetIdsQuery)
        {
            var transactions = await _getByTargetIdsUseCase.ExecuteAsync(targetIdsQuery).ConfigureAwait(false);

            return Ok(transactions);
        }
    }
}
