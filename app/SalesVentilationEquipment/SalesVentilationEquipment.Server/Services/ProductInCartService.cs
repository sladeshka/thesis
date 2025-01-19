using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;

namespace SalesVentilationEquipment.Server.Services
{
    public class ProductInCartService
    {
        #region Fields
        private readonly Repository<ProductInCart> _repository;
        private readonly Repository<Product> _productRepository;
        private readonly ILogger<ProductInCart> _logger;
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public ProductInCartService(ApplicationDbContext context, ILogger<ProductInCart> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _repository = new Repository<ProductInCart>(context, _loggerFactory.CreateLogger<ProductInCart>());
            _productRepository = new Repository<Product>(context, _loggerFactory.CreateLogger<Product>());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a response as a list of carts depending on the conditions.
        /// </summary>
        /// <returns>List of carts</returns>
        public async Task<Response<IEnumerable<ProductInCartResponse>>> Get()
        {
            Repository<ProductInCart> query = _repository;

            try
            {
                IEnumerable<ProductInCart> elements = await query
                    .GetAsync();
                return new Response<IEnumerable<ProductInCartResponse>>
                {
                    Data = await Task.WhenAll(elements.Select(element => SetResponse(element)))
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Response<IEnumerable<ProductInCartResponse>>();
            }
        }

        /// <summary>
        /// Adds a cart.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="data">Cart data</param>
        /// <returns>Query result</returns>
        public async Task<Response<ProductInCartResponse>> Add(Guid cartId, ProductInCartRequest data)
        {
            ProductInCart hasElement = await _repository.GetByIdAsync(data.Id);
            ThrowExceptionIfNotNull(hasElement, "ProductInCart");

            Product product = await _productRepository.GetByIdAsync(data.ProductId);

            ProductInCart createdElement = await _repository.AddAsync(new ProductInCart
            {
                Id = data.Id,
                CartId = cartId,
                ProductId = data.ProductId,
                Quantity = data.Quantity,
                UnitPrice = product.Price
            });

            return new Response<ProductInCartResponse>
            {
                Code = 201,
                Data = await SetResponse(createdElement),
                Message = "Created"
            };
        }

        /// <summary>
        /// Returns the cart by its ID.
        /// </summary>
        /// <param name="id">Cart ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<ProductInCartResponse>> GetById(dynamic id)
        {
            ProductInCart element = await _repository
                .Filter(new Dictionary<string, Func<ProductInCart, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "ProductInCart");

            return new Response<ProductInCartResponse>
            {
                Data = await SetResponse(element),
            };
        }

        /// <summary>
        /// Updates a cart by its ID.
        /// </summary>
        /// <param name="id">Cart ID</param>
        /// <param name="data">Cart data</param>
        /// <returns>Query result</returns>
        public async Task<Response<ProductInCartResponse>> Update(Guid cartId, Guid id, ProductInCartRequest data)
        {
            ProductInCart element = await _repository.GetByIdAsync(id);
            ThrowExceptionIfNull(element, "ProductInCart");
            element = await _repository
                .Filter(new Dictionary<string, Func<ProductInCart, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "ProductInCart");
            element.CartId = cartId;
            element.ProductId = data.ProductId;
            element.Quantity = data.Quantity;

            ProductInCart updatedElement = await _repository.UpdateAsync(element);

            return new Response<ProductInCartResponse>
            {
                Code = 200,
                Data = await SetResponse(updatedElement),
                Message = "Updated"
            };
        }

        /// <summary>
        /// Deletes a cart by its ID.
        /// </summary>
        /// <param name="id">Cart ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<OkResponse>> Delete(dynamic id)
        {
            ProductInCart deletedElement = await _repository.RemoveAsync(id);
            ThrowExceptionIfNull(deletedElement, "ProductInCart");

            return new Response<OkResponse>
            {
                Code = 204,
                Data = { },
                Message = "Deleted",
            };
        }

        /// <summary>
        /// Checks whether an object exists and if it is NULL throws an exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="fieldName">Object name</param>
        /// <exception cref="UnprocessableEntityException">Throws a 422 exception in response</exception>
        private void ThrowExceptionIfNull(dynamic value, string fieldName)
        {
            if (value == null)
            {
                throw new UnprocessableEntityException(new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        Field = fieldName.ToLower(),
                        Message = "Not found"
                    }
                });
            }
        }

        /// <summary>
        /// Checks whether the object exists and if it is not NULL throws an exception.
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="fieldName">Object name</param>
        /// <exception cref="UnprocessableEntityException">Throws a 422 exception in response</exception>
        private void ThrowExceptionIfNotNull(dynamic value, string fieldName)
        {
            if (value != null)
            {
                throw new UnprocessableEntityException(new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        Field = fieldName.ToLower(),
                        Message = "Already exists"
                    }
                });
            }
        }

        /// <summary>
        /// Generates a response object.
        /// </summary>
        /// <param name="element">Cart object</param>
        /// <returns>Cart response object</returns>
        private async Task<ProductInCartResponse> SetResponse(ProductInCart element)
        {
            return await Task.FromResult(new ProductInCartResponse
            {
                Id = element.Id,
                CartId = element.CartId,
                ProductId = element.ProductId,
                Quantity = element.Quantity,
                UnitPrice = element.UnitPrice
            });
        }
        #endregion
    }
}
