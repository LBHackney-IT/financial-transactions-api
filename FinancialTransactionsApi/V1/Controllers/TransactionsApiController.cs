using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using System;
using TransactionsApi.V1.Domain;
using System.ComponentModel.DataAnnotations;
using FinancialTransactionsApi.V1.Boundary.Request;

namespace TransactionsApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/transactions")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class TransactionsApiController : BaseController
    {
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly IGetByIdUseCase _getByIdUseCase;
        private readonly IAddUseCase _addUseCase;
        public TransactionsApiController(IGetAllUseCase getAllUseCase, IGetByIdUseCase getByIdUseCase, IAddUseCase addUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUseCase = getByIdUseCase;
            _addUseCase = addUseCase;
        }

        [ProducesResponseType(typeof(TransactionResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            var data = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (data == null)
                return NotFound();
            return Ok(data);


        }
        /// <summary>
        ///  Gets a collection of transactions for a tenancy/property
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(TransactionResponseObjectList), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll([FromQuery] TransactionQuery query)
        {
            try
            {
                var data = await _getAllUseCase.ExecuteAsync(query.TargetId, query.TransactionType, query.StartDate, query.EndDate).ConfigureAwait(false);
                if (data == null)
                    return NotFound();
                return Ok(data);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                throw;
            }
        }

        [ProducesResponseType(typeof(TransactionResponseObjectList), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Transaction transaction)
        {
            transaction.Id = Guid.NewGuid();
            await _addUseCase.ExecuteAsync(transaction).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id });

        }

    }
}
