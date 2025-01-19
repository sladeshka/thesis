using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;

namespace SalesVentilationEquipment.Server.Services
{
    public class OrderService
    {
        #region Fields
        private readonly Repository<Order> _repository;
        private readonly ILogger<Order> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public OrderService(ApplicationDbContext context, ILogger<Order> logger)
        {
            _logger = logger;
            _repository = new Repository<Order>(context, logger);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a response as a list of orders depending on the conditions.
        /// </summary>
        /// <returns>List of orders</returns>
        public async Task<Response<IEnumerable<OrderResponse>>> Get()
        {
            Repository<Order> query = _repository;
            
            try
            {
                IEnumerable<Order> elements = await query
                    .GetAsync();

                return new Response<IEnumerable<OrderResponse>>
                {
                    Data = await Task.WhenAll(elements.Select(element => SetResponse(element)))
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Response<IEnumerable<OrderResponse>>();
            }
        }

        /// <summary>
        /// Adds a order.
        /// </summary>
        /// <param name="data">Order data</param>
        /// <returns>Query result</returns>
        public async Task<Response<OrderResponse>> Add(OrderRequest data)
        {
            Order hasElement = await _repository.GetByIdAsync(data.Id);
            ThrowExceptionIfNotNull(hasElement, "Order");
            Order createdElement = await _repository.AddAsync(new Order
            {
                Id = data.Id,
                ContractorId = data.ContractorId,
                CartId = data.CartId,
                OrderStatus = data.OrderStatus,
            });

            return new Response<OrderResponse>
            {
                Code = 201,
                Data = await SetResponse(createdElement),
                Message = "Created"
            };
        }

        /// <summary>
        /// Returns the order by its ID.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<OrderResponse>> GetById(dynamic id)
        {
            Order element = await _repository
                .Filter(new Dictionary<string, Func<Order, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Order");

            return new Response<OrderResponse>
            {
                Data = await SetResponse(element),
            };
        }

        /// <summary>
        /// Updates a order by its ID.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="data">Order data</param>
        /// <returns>Query result</returns>
        public async Task<Response<OrderResponse>> Update(dynamic id, OrderRequest data)
        {
            Order element = await _repository.GetByIdAsync(id);
            ThrowExceptionIfNull(element, "Order");
            element = await _repository
                .Filter(new Dictionary<string, Func<Order, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Order");
            element.ContractorId = data.ContractorId;
            element.CartId = data.CartId;
            element.OrderStatus = data.OrderStatus;

            Order updatedElement = await _repository.UpdateAsync(element);

            return new Response<OrderResponse>
            {
                Code = 200,
                Data = await SetResponse(updatedElement),
                Message = "Updated"
            };
        }

        /// <summary>
        /// Deletes a order by its ID.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<OkResponse>> Delete(dynamic id)
        {
            Order deletedElement = await _repository.RemoveAsync(id);
            ThrowExceptionIfNull(deletedElement, "Order");

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
        /// <param name="element">Order object</param>
        /// <returns>Order response object</returns>
        private async Task<OrderResponse> SetResponse(Order element)
        {
            return await Task.FromResult(new OrderResponse
            {
                Id = element.Id,
                ContractorId = element.ContractorId,
                CartId = element.CartId,
                OrderStatus = element.OrderStatus,
            });
        }
        #endregion
    }
}
