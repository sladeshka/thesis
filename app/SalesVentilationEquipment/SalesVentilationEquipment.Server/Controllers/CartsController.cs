using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;
using SalesVentilationEquipment.Server.Services;

namespace SalesVentilationEquipment.Server.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/v1/carts")]
    public class CartsController : Controller
    {
        #region Fields

        private readonly CartService _cartService;
        private readonly ProductInCartService _productInCartService;

        #endregion

        #region Constructor

        public CartsController(CartService cartService, ProductInCartService productInCartService)
        {
            _cartService = cartService;
            _productInCartService = productInCartService;
        }

        #endregion

        #region Methods

        // GET api/v1/carts
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<Cart>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<CartResponse>>> GetCart()
        {
            return new ObjectResult(await _cartService.Get())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // POST api/v1/carts
        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(Response<CartResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<CartResponse>>> AddCart([FromBody] CartRequest request)
        {

            return new ObjectResult(await _cartService.Add(request))
            {
                StatusCode = StatusCodes.Status201Created
            };

        }

        // GET api/v1/carts/{cartId}
        [HttpGet("{cart}")]
        [ProducesResponseType(typeof(Response<CartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<CartResponse>>> GetCartById(Guid cart)
        {
            return new ObjectResult(await _cartService.GetById(cart))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // PUT api/v1/carts/{cartId}
        [HttpPut("{cart}")]
        //[Authorize]
        [ProducesResponseType(typeof(Response<CartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<CartResponse>>> PutCart(Guid cart, [FromBody] CartRequest request)
        {
            return new ObjectResult(await _cartService.Update(cart, request))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // DELETE api/v1/carts/{cartId}
        [HttpDelete("{cart}")]
        //[Authorize]
        [ProducesResponseType(typeof(Response<CartResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Response<CartResponse>>> DeleteCart(Guid cart)
        {
            return new ObjectResult(await _cartService.Delete(cart))
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        // GET api/v1/carts/{cartId}/products
        [HttpGet("{cart}/products")]
        [ProducesResponseType(typeof(Response<IEnumerable<Product>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<ProductResponse>>> GetProducts(Guid cart)
        {
            return new ObjectResult(await _productInCartService.Get())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // POST api/v1/carts/{cartId}/products
        [HttpPost("{cart}/products")]
        //[Authorize]
        [ProducesResponseType(typeof(Response<CartResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<CartResponse>>> AddProduct(Guid cart, [FromBody] ProductInCartRequest request)
        {

            return new ObjectResult(await _productInCartService.Add(cart, request))
            {
                StatusCode = StatusCodes.Status201Created
            };
        }


        // GET api/v1/carts/{cartId}/products/{productId}
        [HttpGet("{cart}/products/{product}")]
        [ProducesResponseType(typeof(Response<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<ProductResponse>>> GetProductById(Guid cart, Guid product)
        {
            return new ObjectResult(await _productInCartService.GetById(product))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // PUT api/v1/carts/{cartId}/products/{productId}
        [HttpPut("{cart}/products/{product}")]
        //[Authorize]
        [ProducesResponseType(typeof(Response<CartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse<UnprocessableEntityException>),
            StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Response<CartResponse>>> PutProductInCart(Guid cart, Guid product, [FromBody] ProductInCartRequest request)
        {
            return new ObjectResult(await _productInCartService.Update(cart, product, request))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        // DELETE api/v1/carts/{cartId}/product/{productId}
        [HttpDelete("{cart}/products/{product}")]
        //[Authorize]
        [ProducesResponseType(typeof(Response<CartResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse<ForbiddenException>), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Response<CartResponse>>> DeleteProduct(Guid cart, Guid product)
        {
            return new ObjectResult(await _productInCartService.Delete(product))
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }
        #endregion
    }
}
