using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;
using System.Net;

namespace SalesVentilationEquipment.Server.Services
{
    public class StoreService
    {
        #region Fields
        private readonly Repository<Store> _repository;
        private readonly ILogger<Store> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public StoreService(ApplicationDbContext context, ILogger<Store> logger)
        {
            _logger = logger;
            _repository = new Repository<Store>(context, logger);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a response as a list of stores depending on the conditions.
        /// </summary>
        /// <returns>List of stores</returns>
        public async Task<Response<IEnumerable<StoreResponse>>> Get()
        {
            Repository<Store> query = _repository;
            
            try
            {
                IEnumerable<Store> elements = await query
                    .GetAsync();

                return new Response<IEnumerable<StoreResponse>>
                {
                    Data = await Task.WhenAll(elements.Select(element => SetResponse(element)))
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Response<IEnumerable<StoreResponse>>();
            }
        }

        /// <summary>
        /// Adds a store.
        /// </summary>
        /// <param name="data">Store data</param>
        /// <returns>Query result</returns>
        public async Task<Response<StoreResponse>> Add(StoreRequest data)
        {
            Store hasElement = await _repository.GetByIdAsync(data.Id);
            ThrowExceptionIfNotNull(hasElement, "Store");
            Store createdElement = await _repository.AddAsync(new Store
            {
                Id = data.Id,
                Name = data.Name,
                Address = data.Address,

            });

            return new Response<StoreResponse>
            {
                Code = 201,
                Data = await SetResponse(createdElement),
                Message = "Created"
            };
        }

        /// <summary>
        /// Returns the store by its ID.
        /// </summary>
        /// <param name="id">Store ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<StoreResponse>> GetById(dynamic id)
        {
            Store element = await _repository
                .Filter(new Dictionary<string, Func<Store, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Store");

            return new Response<StoreResponse>
            {
                Data = await SetResponse(element),
            };
        }

        /// <summary>
        /// Updates a store by its ID.
        /// </summary>
        /// <param name="id">Store ID</param>
        /// <param name="data">Store data</param>
        /// <returns>Query result</returns>
        public async Task<Response<StoreResponse>> Update(dynamic id, StoreRequest data)
        {
            Store element = await _repository.GetByIdAsync(id);
            ThrowExceptionIfNull(element, "Store");
            element = await _repository
                .Filter(new Dictionary<string, Func<Store, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Store");
            element.Name = data.Name;
            element.Address = data.Address;

            Store updatedElement = await _repository.UpdateAsync(element);

            return new Response<StoreResponse>
            {
                Code = 200,
                Data = await SetResponse(updatedElement),
                Message = "Updated"
            };
        }

        /// <summary>
        /// Deletes a store by its ID.
        /// </summary>
        /// <param name="id">Store ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<OkResponse>> Delete(dynamic id)
        {
            Store deletedElement = await _repository.RemoveAsync(id);
            ThrowExceptionIfNull(deletedElement, "Store");

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
        /// <param name="element">Store object</param>
        /// <returns>Store response object</returns>
        private async Task<StoreResponse> SetResponse(Store element)
        {
            return await Task.FromResult(new StoreResponse
            {
                Id = element.Id,
                Name = element.Name,
                Address= element.Address,
            });
        }
        #endregion
    }
}
