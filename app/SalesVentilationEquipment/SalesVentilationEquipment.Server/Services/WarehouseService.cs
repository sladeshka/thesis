using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;

namespace SalesVentilationEquipment.Server.Services
{
    public class WarehouseService
    {
        #region Fields
        private readonly Repository<Warehouse> _repository;
        private readonly ILogger<Warehouse> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public WarehouseService(ApplicationDbContext context, ILogger<Warehouse> logger)
        {
            _logger = logger;
            _repository = new Repository<Warehouse>(context, logger);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a response as a list of carts depending on the conditions.
        /// </summary>
        /// <returns>List of carts</returns>
        public async Task<Response<IEnumerable<WarehouseResponse>>> Get()
        {
            Repository<Warehouse> query = _repository;
            
            try
            {
                IEnumerable<Warehouse> elements = await query
                    .GetAsync();

                return new Response<IEnumerable<WarehouseResponse>>
                {
                    Data = await Task.WhenAll(elements.Select(element => SetResponse(element)))
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Response<IEnumerable<WarehouseResponse>>();
            }
        }

        /// <summary>
        /// Adds a cart.
        /// </summary>
        /// <param name="data">Warehouse data</param>
        /// <returns>Query result</returns>
        public async Task<Response<WarehouseResponse>> Add(WarehouseRequest data)
        {
            Warehouse hasElement = await _repository.GetByIdAsync(data.Id);
            ThrowExceptionIfNotNull(hasElement, "Warehouse");
            Warehouse createdElement = await _repository.AddAsync(new Warehouse
            {
                Id = data.Id,
                Name = data.Name,
                Address = data.Address,
            });

            return new Response<WarehouseResponse>
            {
                Code = 201,
                Data = await SetResponse(createdElement),
                Message = "Created"
            };
        }

        /// <summary>
        /// Returns the cart by its ID.
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<WarehouseResponse>> GetById(dynamic id)
        {
            Warehouse element = await _repository
                .Filter(new Dictionary<string, Func<Warehouse, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Warehouse");

            return new Response<WarehouseResponse>
            {
                Data = await SetResponse(element),
            };
        }

        /// <summary>
        /// Updates a cart by its ID.
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <param name="data">Warehouse data</param>
        /// <returns>Query result</returns>
        public async Task<Response<WarehouseResponse>> Update(dynamic id, WarehouseRequest data)
        {
            Warehouse element = await _repository.GetByIdAsync(id);
            ThrowExceptionIfNull(element, "Warehouse");
            element = await _repository
                .Filter(new Dictionary<string, Func<Warehouse, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Warehouse");
            element.Name = data.Name;
            element.Address = data.Address;

            Warehouse updatedElement = await _repository.UpdateAsync(element);

            return new Response<WarehouseResponse>
            {
                Code = 200,
                Data = await SetResponse(updatedElement),
                Message = "Updated"
            };
        }

        /// <summary>
        /// Deletes a cart by its ID.
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<OkResponse>> Delete(dynamic id)
        {
            Warehouse deletedElement = await _repository.RemoveAsync(id);
            ThrowExceptionIfNull(deletedElement, "Warehouse");

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
        /// <param name="element">Warehouse object</param>
        /// <returns>Warehouse response object</returns>
        private async Task<WarehouseResponse> SetResponse(Warehouse element)
        {
            return await Task.FromResult(new WarehouseResponse
            {
                Id = element.Id,
                Name = element.Name,
                Address = element.Address,
            });
        }
        #endregion
    }
}
