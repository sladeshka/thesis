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
    [Route("api/v1/stores")]
    public class StoresController : Controller
    {
        #region Fields

        private readonly StoreService _service;

        #endregion

        #region Constructor

        public StoresController(StoreService service)
        {
            _service = service;
        }

        #endregion

        #region Methods

        // GET api/v1/stores
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<Store>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<StoreResponse>>> Get()
        {
            return new ObjectResult(await _service.Get())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // POST api/v1/stores
        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(Response<StoreResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<StoreResponse>>> Post([FromBody] StoreRequest request)
        {

            return new ObjectResult(await _service.Add(request))
            {
                StatusCode = StatusCodes.Status201Created
            };

        }

        // GET api/v1/stores/{store}
        [HttpGet("{store}")]
        [ProducesResponseType(typeof(Response<StoreResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<StoreResponse>>> GetById(Guid store)
        {
            return new ObjectResult(await _service.GetById(store))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // PUT api/v1/stores/{storeId}
        [HttpPut("{store}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<StoreResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<StoreResponse>>> Put(Guid store, [FromBody] StoreRequest request)
        {
            return new ObjectResult(await _service.Update(store, request))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // DELETE api/v1/stores/{storeId}
        [HttpDelete("{store}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<StoreResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Response<StoreResponse>>> Delete(Guid store)
        {
            return new ObjectResult(await _service.Delete(store))
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        #endregion
    }
}
