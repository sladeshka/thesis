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
    [Route("api/v1/products")]
    public class ProductsController : Controller
    {
        #region Fields

        private readonly ProductService _service;

        #endregion

        #region Constructor

        public ProductsController(ProductService service)
        {
            _service = service;
        }

        #endregion

        #region Methods

        // GET api/v1/products
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<Product>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<ProductResponse>>> Get()
        {
            return new ObjectResult(await _service.Get())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // POST api/v1/products
        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(Response<ProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<ProductResponse>>> Post([FromBody] ProductRequest request)
        {

            return new ObjectResult(await _service.Add(request))
            {
                StatusCode = StatusCodes.Status201Created
            };

        }

        // GET api/v1/products/{product}
        [HttpGet("{product}")]
        [ProducesResponseType(typeof(Response<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<ProductResponse>>> GetById(Guid product)
        {
            return new ObjectResult(await _service.GetById(product))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // PUT api/v1/products/{productId}
        [HttpPut("{product}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<ProductResponse>>> Put(Guid product, [FromBody] ProductRequest request)
        {
            return new ObjectResult(await _service.Update(product, request))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // DELETE api/v1/products/{productId}
        [HttpDelete("{product}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<ProductResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Response<ProductResponse>>> Delete(Guid product)
        {
            return new ObjectResult(await _service.Delete(product))
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        #endregion
    }
}
