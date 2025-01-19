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
    [Route("api/v1/orders")]
    public class OrdersController : Controller
    {
        #region Fields

        private readonly OrderService _service;

        #endregion

        #region Constructor

        public OrdersController(OrderService service)
        {
            _service = service;
        }

        #endregion

        #region Methods

        // GET api/v1/orders
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<Order>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<OrderResponse>>> Get()
        {
            return new ObjectResult(await _service.Get())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // POST api/v1/orders
        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(Response<OrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<OrderResponse>>> Post([FromBody] OrderRequest request)
        {

            return new ObjectResult(await _service.Add(request))
            {
                StatusCode = StatusCodes.Status201Created
            };

        }

        // GET api/v1/orders/{order}
        [HttpGet("{order}")]
        [ProducesResponseType(typeof(Response<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<OrderResponse>>> GetById(Guid order)
        {
            return new ObjectResult(await _service.GetById(order))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // PUT api/v1/orders/{orderId}
        [HttpPut("{order}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<OrderResponse>>> Put(Guid order, [FromBody] OrderRequest request)
        {
            return new ObjectResult(await _service.Update(order, request))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // DELETE api/v1/orders/{orderId}
        [HttpDelete("{order}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<OrderResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Response<OrderResponse>>> Delete(Guid order)
        {
            return new ObjectResult(await _service.Delete(order))
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        #endregion
    }
}
