using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Exceptions;
using SalesVentilationEquipment.Server.Models;
using SalesVentilationEquipment.Server.Repositories;
using SalesVentilationEquipment.Server.Requests;
using SalesVentilationEquipment.Server.Responses;

namespace SalesVentilationEquipment.Server.Services
{
    public class ProductService
    {
        #region Fields
        private readonly Repository<Product> _repository;
        private readonly ILogger<Product> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public ProductService(ApplicationDbContext context, ILogger<Product> logger)
        {
            _logger = logger;
            _repository = new Repository<Product>(context, logger);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a response as a list of products depending on the conditions.
        /// </summary>
        /// <returns>List of products</returns>
        public async Task<Response<IEnumerable<ProductResponse>>> Get()
        {
            Repository<Product> query = _repository;
            
            try
            {
                IEnumerable<Product> elements = await query
                    .GetAsync();

                return new Response<IEnumerable<ProductResponse>>
                {
                    Data = await Task.WhenAll(elements.Select(element => SetResponse(element)))
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Response<IEnumerable<ProductResponse>>();
            }
        }

        /// <summary>
        /// Adds a product.
        /// </summary>
        /// <param name="data">Product data</param>
        /// <returns>Query result</returns>
        public async Task<Response<ProductResponse>> Add(ProductRequest data)
        {
            Product hasElement = await _repository.GetByIdAsync(data.Id);
            ThrowExceptionIfNotNull(hasElement, "Product");
            Product createdElement = await _repository.AddAsync(new Product
            {
                Id = data.Id,
                Name = data.Name,
                Price = data.Price,
                Description = data.Description,
                Feature = data.Feature
            });

            return new Response<ProductResponse>
            {
                Code = 201,
                Data = await SetResponse(createdElement),
                Message = "Created"
            };
        }

        /// <summary>
        /// Returns the product by its ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<ProductResponse>> GetById(dynamic id)
        {
            Product element = await _repository
                .Filter(new Dictionary<string, Func<Product, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Product");

            return new Response<ProductResponse>
            {
                Data = await SetResponse(element),
            };
        }

        /// <summary>
        /// Updates a product by its ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="data">Product data</param>
        /// <returns>Query result</returns>
        public async Task<Response<ProductResponse>> Update(dynamic id, ProductRequest data)
        {
            Product element = await _repository.GetByIdAsync(id);
            ThrowExceptionIfNull(element, "Product");
            element = await _repository
                .Filter(new Dictionary<string, Func<Product, bool>>
                    {
                        { "Id", s => s.Id == id },
                    }
                )
                .GetFirstAsync();
            ThrowExceptionIfNull(element, "Product");
            element.Name = data.Name;
            element.Price = data.Price;
            element.Description = data.Description;
            element.Feature = data.Feature;

            Product updatedElement = await _repository.UpdateAsync(element);

            return new Response<ProductResponse>
            {
                Code = 200,
                Data = await SetResponse(updatedElement),
                Message = "Updated"
            };
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Query result</returns>
        public async Task<Response<OkResponse>> Delete(dynamic id)
        {
            Product deletedElement = await _repository.RemoveAsync(id);
            ThrowExceptionIfNull(deletedElement, "Product");

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
        /// <param name="element">Product object</param>
        /// <returns>Product response object</returns>
        private async Task<ProductResponse> SetResponse(Product element)
        {
            return await Task.FromResult(new ProductResponse
            {
                Id = element.Id,
                Name = element.Name,
                Price = element.Price,
                Description = element.Description,
                Feature = element.Feature
            });
        }
        #endregion
    }
}
