using System.Linq.Expressions;

namespace Data.Interfaces;

// Interface av BaseRepository. Interface't är generiskt vilket innebär att
// så länge det är en klass (where TEntity : class) som implementerar
// interfacet så kan metoderna implementeras.
public interface IBaseRepository<TEntity> where TEntity : class
{
    // Task representerar en asynkron operation som kan returnera ett värde.

    // Se BaseRepository för implementingarna
    Task<bool> AddAsync(TEntity entity);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression);

    Task<T?> GetAsync<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, T>> selector);

    Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, T>> selector);

    Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<TEntity, T>> selector);

    Task<bool> RemoveAsync(TEntity entity);

    Task<bool> UpdateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false,
                                Expression<Func<TEntity, object>>? sortBy = null,
                                Expression<Func<TEntity, bool>>? filterBy = null,
                                params Expression<Func<TEntity, object>>[] includes);
}