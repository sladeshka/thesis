using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;

namespace SalesVentilationEquipment.Server.Services
{
    public class ContractorService
    {
        #region Fields
        private readonly Repository<Contractor> _repository;
        private readonly ILogger<Contractor> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public ContractorService(ApplicationDbContext context, ILogger<Contractor> logger)
        {
            _logger = logger;
            _repository = new Repository<Contractor>(context, logger);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a response as a list of contractors depending on the conditions.
        /// </summary>
        /// <returns>List of contractors</returns>
        public async Task<Response<IEnumerable<ContractorResponse>>> Get()
        {
            Repository<Contractor> query = _repository;
            
            try
            {
                IEnumerable<Contractor> elements = await query
                    .GetAsync();

                return new Response<IEnumerable<ContractorResponse>>
                {
                    Data = await Task.WhenAll(elements.Select(element => SetResponse(element)))
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Response<IEnumerable<ContractorResponse>>();
            }
        }

        /// <summary>
        /// Adds a contractor.
        /// </summary>
        /// <param name="data">Contractor data</param>
        /// <returns>Query result</returns>
        public async Task<Response<ContractorResponse>> Add(ContractorRequest data)
        {
            Contractor hasElement = await _repository.GetByIdAsync(data.Id);
            ThrowExceptionIfNotNull(hasElement, "Contractor");
            Contractor createdElement = await _repository.AddAsync(new Contractor
            {
                Id = data.Id,
                Name = data.Name,
                ContactInfo = data.ContactInfo,
            });

            return new Response<ContractorResponse>
            {
                Code = 201,
                Data = await SetResponse(createdElement),
                Message = "Created"
            };
        }

        /// <summary>
        /// Returns the contractor by its ID.
        /// </summary>
        /// <param name="id">Contractor ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<ContractorResponse>> GetById(dynamic id)
        {
            Contractor element = await _repository
                .Filter(new Dictionary<string, Func<Contractor, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Contractor");

            return new Response<ContractorResponse>
            {
                Data = await SetResponse(element),
            };
        }

        /// <summary>
        /// Updates a contractor by its ID.
        /// </summary>
        /// <param name="id">Contractor ID</param>
        /// <param name="data">Contractor data</param>
        /// <returns>Query result</returns>
        public async Task<Response<ContractorResponse>> Update(dynamic id, ContractorRequest data)
        {
            Contractor element = await _repository.GetByIdAsync(id);
            ThrowExceptionIfNull(element, "Contractor");
            element = await _repository
                .Filter(new Dictionary<string, Func<Contractor, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Contractor");
            element.Name = data.Name;
            element.ContactInfo = data.ContactInfo;

            Contractor updatedElement = await _repository.UpdateAsync(element);

            return new Response<ContractorResponse>
            {
                Code = 200,
                Data = await SetResponse(updatedElement),
                Message = "Updated"
            };
        }

        /// <summary>
        /// Deletes a contractor by its ID.
        /// </summary>
        /// <param name="id">Contractor ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<OkResponse>> Delete(dynamic id)
        {
            Contractor deletedElement = await _repository.RemoveAsync(id);
            ThrowExceptionIfNull(deletedElement, "Contractor");

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
        /// <param name="element">Contractor object</param>
        /// <returns>Contractor response object</returns>
        private async Task<ContractorResponse> SetResponse(Contractor element)
        {
            return await Task.FromResult(new ContractorResponse
            {
                Id = element.Id,
                Name = element.Name,
                ContactInfo = element.ContactInfo,
            });
        }
        #endregion
    }
}
