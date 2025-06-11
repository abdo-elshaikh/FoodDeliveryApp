using FoodDeliveryApp.Data;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ILogger<Repository<TEntity>> _logger;

        public Repository(ApplicationDbContext context, ILogger<Repository<TEntity>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity of type {EntityType} with ID {Id}", typeof(TEntity).Name, id);
                throw;
            }
        }

        public virtual async Task<TEntity?> GetByIdAsync(string id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity of type {EntityType} with User ID {Id}", typeof(TEntity).Name, id);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all entities of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet.Where(predicate);
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding entities of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _dbSet.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting entities of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding first entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        public virtual async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding single entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }
    }
}
