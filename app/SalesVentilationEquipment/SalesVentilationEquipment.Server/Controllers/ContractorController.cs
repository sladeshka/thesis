using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;
using SalesVentilationEquipment.Server.Services;

namespace SalesVentilationEquipment.Server.Controllers
{
    [ApiController]
    [Route("api/v1/contractors")]
    public class ContractorsController : Controller
    {
        #region Fields

        private readonly ContractorService _service;

        #endregion

        #region Constructor

        public ContractorsController(ContractorService service)
        {
            _service = service;
        }

        #endregion

        #region Methods

        // GET api/v1/contractors
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<Contractor>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<ContractorResponse>>> Get()
        {
            return new ObjectResult(await _service.Get())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // POST api/v1/contractors
        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(Response<ContractorResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<ContractorResponse>>> Post([FromBody] ContractorRequest request)
        {

            return new ObjectResult(await _service.Add(request))
            {
                StatusCode = StatusCodes.Status201Created
            };

        }

        // GET api/v1/contractors/{contractor}
        [HttpGet("{contractor}")]
        [ProducesResponseType(typeof(Response<ContractorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<ContractorResponse>>> GetById(Guid contractor)
        {
            return new ObjectResult(await _service.GetById(contractor))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // PUT api/v1/contractors/{contractorId}
        [HttpPut("{contractor}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<ContractorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<ContractorResponse>>> Put(Guid contractor, [FromBody] ContractorRequest request)
        {
            return new ObjectResult(await _service.Update(contractor, request))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // DELETE api/v1/contractors/{contractorId}
        [HttpDelete("{contractor}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<ContractorResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Response<ContractorResponse>>> Delete(Guid contractor)
        {
            return new ObjectResult(await _service.Delete(contractor))
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        #endregion
    }
}
