using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SalesVentilationEquipment.Server.Data;
using SalesVentilationEquipment.Server.Models;

namespace SalesVentilationEquipment.Server.Repositories
{
    /// <summary>
    /// Universal class for working with data in a database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> where T : BaseModel
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        private List<Expression<Func<T, object>>> _includes;
        private Dictionary<string, Func<T, bool>> _filters;
        private Func<IQueryable<T>, IOrderedQueryable<T>>? _orderBy;
        private int? _limit;
        private readonly ILogger<T> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">DB context</param>
        /// <param name="logger">Logging class</param>
        public Repository(
            ApplicationDbContext context,
            ILogger<T> logger)
        {
            _context = context;
            _includes = new List<Expression<Func<T, object>>>();
            _filters = new Dictionary<string, Func<T, bool>>();
            _logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the database context
        /// </summary>
        /// <returns></returns>
        public ApplicationDbContext GetContext()
        {
            return _context;
        }

        /// <summary>
        /// Clears selected settings
        /// </summary>
        /// <returns>Self</returns>
        public Repository<T> Clean()
        {
            _filters = new Dictionary<string, Func<T, bool>>();
            _includes = new List<Expression<Func<T, object>>>();
            _orderBy = null;
            _limit = null;

            return this;
        }

        /// <summary>
        /// Related Objects
        /// </summary>
        /// <param name="includes">Object name</param>
        /// <returns>Self</returns>
        public Repository<T> Include(params Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
            {
                _includes.Add(include);
            }

            return this;
        }

        /// <summary>
        /// Filter by value
        /// </summary>
        /// <param name="filters">Value</param>
        /// <returns>Self</returns>
        public Repository<T> Filter(Dictionary<string, Func<T, bool>> filters)
        {
            foreach (var filter in filters)
            {
                _filters[filter.Key] = filter.Value;
            }

            return this;
        }

        /// <summary>
        /// Sorts by field
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">Field</param>
        /// <param name="ascending">Sort direction</param>
        /// <returns>Self</returns>
        public Repository<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector, bool ascending = true)
        {
            _orderBy = ascending
                ? (query => query.OrderBy(keySelector))
                : (query => query.OrderByDescending(keySelector));

            return this;
        }

        /// <summary>
        /// Limit on the number of items returned
        /// </summary>
        /// <param name="count">Limit</param>
        /// <returns>Self</returns>
        public Repository<T> Limit(int count)
        {
            _limit = count;
            return this;
        }

        /// <summary>
        /// Returns a list of objects
        /// </summary>
        /// <returns>List of objects</returns>
        public async Task<IEnumerable<T>> GetAsync()
        {
            try
            {
                var query = _context.Set<T>().AsQueryable();

                foreach (var include in _includes)
                {
                    query = query.Include(include);
                }

                foreach (var filterFunc in _filters)
                {
                    query = query.Where(filterFunc.Value).AsQueryable();
                }

                if (_orderBy != null)
                {
                    query = _orderBy(query);
                }

                if (_limit.HasValue)
                {
                    query = query.Take(_limit.Value);
                }

                return await Task.FromResult(query.ToList());
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, e.Message);
                return Enumerable.Empty<T>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Returns the first object found
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetFirstAsync()
        {
            try
            {
                var query = _context.Set<T>().AsQueryable();

                foreach (var include in _includes)
                {
                    query = query.Include(include);
                }

                foreach (var filterFunc in _filters)
                {
                    query = query.Where(filterFunc.Value).AsQueryable();
                }

                if (_orderBy != null)
                {
                    query = _orderBy(query);
                }

                if (_limit.HasValue)
                {
                    query = query.Take(_limit.Value);
                }

                return await Task.FromResult(query.First());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return default;
            }
        }

        /// <summary>
        /// Returns an object by its ID
        /// </summary>
        /// <param name="id">Object ID</param>
        /// <returns>Object</returns>
        public async Task<T> GetByIdAsync(dynamic id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Adds an object in the database
        /// </summary>
        /// <param name="model">Object</param>
        /// <returns>Object</returns>
        public async Task<T> AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
            await SaveAsync();
            return model;
        }

        /// <summary>
        /// Adds a list of objects in the database
        /// </summary>
        /// <param name="models">List of objects</param>
        /// <returns>List of objects</returns>
        public async Task<IEnumerable<T>> AddListAsync(IEnumerable<T> models)
        {
            try
            {
                _context.ChangeTracker.Clear();
                var copyModels = new List<T>(models);
                await _context.Set<T>().AddRangeAsync(copyModels);
                await SaveAsync();
                return copyModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Updates an object in the database
        /// </summary>
        /// <param name="model">Object</param>
        /// <returns>Object</returns>
        public async Task<T> UpdateAsync(T model)
        {
            _context.Set<T>().Update(model);
            await SaveAsync();
            return model;
        }

        /// <summary>
        /// Updates a list of objects in the database
        /// </summary>
        /// <param name="models">List of objects</param>
        /// <returns>List of objects</returns>
        public async Task<IEnumerable<T>> UpdateListAsync(IEnumerable<T> models)
        {
            try
            {
                _context.ChangeTracker.Clear();
                _context.Set<T>().UpdateRange(models);
                await SaveAsync();
                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Adds or updates a list of objects in the database
        /// </summary>
        /// <param name="models">List of objects</param>
        /// <returns>List of objects</returns>
        public async Task<IEnumerable<T>> AddOrUpdateListAsync(IEnumerable<T> models)
        {
            try
            {
                var existingElements = await GetAsync();
                var listForUpdates = existingElements.ToDictionary(GetKey<T>, element => element);
                Dictionary<Guid, T> listForAdded = new();
                _context.ChangeTracker.Clear();

                foreach (var model in models)
                {
                    var key = GetKey<T>(model);
                    if (listForUpdates.TryGetValue(key, out var existingElement))
                    {
                        listForUpdates[key] = model;
                    }
                    else
                    {
                        listForAdded[key] = model;
                    }
                }
                await UpdateListAsync(listForUpdates.Values.ToList());
                await AddListAsync(listForAdded.Values.ToList());

                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Removes an object by id from the database
        /// </summary>
        /// <param name="id">Object ID</param>
        /// <returns>Object</returns>
        public async Task<T?> RemoveAsync(dynamic id)
        {
            var model = await GetByIdAsync(id);

            if (model != null)
            {
                _context.Set<T>().Remove(model);
                await SaveAsync();
            }

            return model;
        }

        /// <summary>
        /// Removes a list of objects from the database
        /// </summary>
        /// <param name="models">List of objects</param>
        /// <returns>List of objects</returns>
        public async Task<IEnumerable<T>> RemoveListAsync(IEnumerable<T> models)
        {
            try
            {
                _context.ChangeTracker.Clear();
                _context.Set<T>().RemoveRange(models);
                await SaveAsync();
                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        /// <summary>
        /// Primary Key Determination Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">Object type</param>
        /// <returns>Key</returns>
        private Guid GetKey<T>(T entity)
        {
            var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            return (Guid)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }
        #endregion
    }
}
