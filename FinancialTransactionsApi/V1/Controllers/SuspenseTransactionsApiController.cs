using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.UseCase.Interfaces;

namespace FinancialTransactionsApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/suspense-transactions")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class SuspenseTransactionsApiController : BaseController
    {
        private readonly IGetAllSuspenseUseCase _getAllSuspenseUseCase;

        public SuspenseTransactionsApiController(IGetAllSuspenseUseCase getAllSuspenseUseCase)
        {
            _getAllSuspenseUseCase = getAllSuspenseUseCase;
        }
        /// <summary>
        /// Gets a collection of suspense transactions for a tenancy/property
        /// </summary>
        /// <param name="query">Pagination information</param>
        /// <response code="200">Success. Suspense transaction models were received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAllSuspense([FromQuery] SuspenseTransactionsSearchRequest query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, GetErrorMessage(ModelState)));
            }

            var transactions = await _getAllSuspenseUseCase.ExecuteAsync(query).ConfigureAwait(false);

            return Ok(transactions);
        }

        [HttpGet]
        [Route("{id}")]
        public string Test([FromRoute] string id)
        {
            return id;
        }
    }
}
