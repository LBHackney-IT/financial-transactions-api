using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using System;
using TransactionsApi.V1.Domain;

namespace TransactionsApi.V1.Controllers
{
    [ApiController]
    //TODO: Rename to match the APIs endpoint
    [Route("api/v1/transactions")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    //TODO: rename class to match the API name
    public class TransactionsApiController : BaseController
    {
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly IGetAllSummaryUseCase _getAllSummaryUseCase;
        private readonly IGetByIdUseCase _getByIdUseCase;
        private readonly IAddUseCase _addUseCase;
        public TransactionsApiController(IGetAllUseCase getAllUseCase, IGetByIdUseCase getByIdUseCase,IAddUseCase addUseCase, IGetAllSummaryUseCase getAllSummaryUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUseCase = getByIdUseCase;
            _addUseCase = addUseCase;
            _getAllSummaryUseCase = getAllSummaryUseCase;
        }

        [ProducesResponseType(typeof(TransactionResponseObject),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var data = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [ProducesResponseType(typeof(TransactionResponseObjectList), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll([FromQuery] Guid targetId,[FromQuery]string transactionType, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            var data = await _getAllUseCase.ExecuteAsync(targetId,transactionType, startDate, endDate).ConfigureAwait(false);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [ProducesResponseType(typeof(TransactionResponseObjectList), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("summary")]
        public async Task<IActionResult> GetAllSummary([FromQuery] Guid targetId, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            var data = await _getAllSummaryUseCase.ExecuteAsync(targetId,startDate, endDate).ConfigureAwait(false);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [ProducesResponseType(typeof(TransactionResponseObjectList), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Transaction transaction)
        {
            //var data = await _getByIdUseCase.ExecuteAsync(transaction.Id).ConfigureAwait(false);
            //if (data != null)
            //    return BadRequest("This record already exists");
            transaction.Id =  Guid.NewGuid();
            await _addUseCase.ExecuteAsync(transaction).ConfigureAwait(false);
            return RedirectToAction("GetById", new { id = transaction.Id });
        }

    }
}
