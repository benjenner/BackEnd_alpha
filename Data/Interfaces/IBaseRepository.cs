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

    Task<bool> RemoveAsync(TEntity entity);

    Task<bool> UpdateAsync(TEntity entity);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);

    //Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy,
                                           params Expression<Func<TEntity, object>>[] includes);

    //Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false,
    //                            Expression<Func<TEntity, object>>? sortBy = null,
    //                            Expression<Func<TEntity, bool>>? filterBy = null,
    //                            params Expression<Func<TEntity, object>>[] includes);

    Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false,
                                      Expression<Func<TEntity, object>>? sortBy = null,
                                      Expression<Func<TEntity, bool>>? filterBy = null,
                                      params Expression<Func<TEntity, object>>[] includes);
}