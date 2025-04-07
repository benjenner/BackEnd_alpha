using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

// Här tillhandahålls all databasfuntkionalitet (CRUD) som delas av flera olika repositories.

// BaseRepository definieras med en generisk typparameter.
// Detta för att i metoderna kunna skicka in ** flera olika typer av objekt **
public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    // Metoden returnerar true eller false beroende på uttrycket som skickas in
    // Exempel på om man vill se om ett företagsnamn redan finns i databasen
    // var clientExists = await repository.ExistsAsync(client => client.ClientName == "Företag AB");
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        // Any returnerar true om minst en entitet i samlingen matchar villkoret.
        var result = await _dbSet.AnyAsync(expression);
        return result;
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy,
                                         params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(findBy);
        return entity ?? null!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false,
                                      Expression<Func<TEntity, object>>? sortBy = null,
                                      Expression<Func<TEntity, bool>>? filterBy = null,
                                      params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        // Möjliggör att man kan hämta alla som har t.ex en viss status (active)
        if (filterBy != null)
            query = query.Where(filterBy);

        // Inkluderar nav-properties
        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        // Sorteringen av listan, ASC eller DESC och fält (ex. OrderBy(x => x.Created))
        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        return await query.ToListAsync();
    }

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> RemoveAsync(TEntity entity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}