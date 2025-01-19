using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;

namespace SalesVentilationEquipment.Server.Services
{
    public class CartService
    {
        #region Fields
        private readonly Repository<Cart> _repository;
        private readonly ILogger<Cart> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public CartService(ApplicationDbContext context, ILogger<Cart> logger)
        {
            _logger = logger;
            _repository = new Repository<Cart>(context, logger);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a response as a list of carts depending on the conditions.
        /// </summary>
        /// <returns>List of carts</returns>
        public async Task<Response<IEnumerable<CartResponse>>> Get()
        {
            Repository<Cart> query = _repository;
            
            try
            {
                IEnumerable<Cart> elements = await query
                    .GetAsync();

                return new Response<IEnumerable<CartResponse>>
                {
                    Data = await Task.WhenAll(elements.Select(element => SetResponse(element)))
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Response<IEnumerable<CartResponse>>();
            }
        }

        /// <summary>
        /// Adds a cart.
        /// </summary>
        /// <param name="data">Cart data</param>
        /// <returns>Query result</returns>
        public async Task<Response<CartResponse>> Add(CartRequest data)
        {
            Cart hasElement = await _repository.GetByIdAsync(data.Id);
            ThrowExceptionIfNotNull(hasElement, "Cart");
            Cart createdElement = await _repository.AddAsync(new Cart
            {
                Id = data.Id,
                ContractorId = data.ContractorId,
                TotalSum = 0,
                Discount = 0
            });

            return new Response<CartResponse>
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
        public async Task<Response<CartResponse>> GetById(dynamic id)
        {
            Cart element = await _repository
                .Filter(new Dictionary<string, Func<Cart, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Cart");

            return new Response<CartResponse>
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
        public async Task<Response<CartResponse>> Update(dynamic id, CartRequest data)
        {
            Cart element = await _repository.GetByIdAsync(id);
            ThrowExceptionIfNull(element, "Cart");
            element = await _repository
                .Filter(new Dictionary<string, Func<Cart, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Cart");

            Cart updatedElement = await _repository.UpdateAsync(element);

            return new Response<CartResponse>
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
            Cart deletedElement = await _repository.RemoveAsync(id);
            ThrowExceptionIfNull(deletedElement, "Cart");

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
        private async Task<CartResponse> SetResponse(Cart element)
        {
            return await Task.FromResult(new CartResponse
            {
                Id = element.Id,
                TotalSum = element.TotalSum,
                Discount = element.Discount
            });
        }
        #endregion
    }
}
