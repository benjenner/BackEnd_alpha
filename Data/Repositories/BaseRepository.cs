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

    public virtual async Task<T?> GetAsync<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, T>> selector)
    {
        return await _dbSet.Where(expression).Select(selector).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, T>> selector)
    {
        return await _dbSet.Where(expression).Select(selector).ToListAsync();
    }

    // Här kan en selector sättas in.
    public virtual async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<TEntity, T>> selector)
    {
        return await _dbSet.Select(selector).ToListAsync();
    }

    // Standard-metod för att hämta samtliga objekt i ett dbSet
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return entities;
    }

    // Overload metod av GetAllAsync som används när man vill ha ett filtervillkor som returnerar true för dom som ska inluderas
    // och false för dom som ska uteslutas.
    // Exempel på användning i kod : Hämta alla clients som har Stockholm som location
    // var stockholmClients = await repository.GetAllAsync(client => client.Location == "Stockholm");

    // Metoden tar ett lambda-expression som argument. Expression definieras som en funktion (func) som tar en TEntity (en klass) som input
    // och ett logiskt villkor (bool) som output. Output'en bestämmer om entiteten ska returneras i where-selektionen.
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entities = await _dbSet.Where(expression).ToListAsync();
        return entities;
    }

    // Metod som returnerar EN entitet. Detta genom att skriva TEntity och inte IEnumerable samt inkludera
    // FirstOrDefaultAsync i LINQ-uttrycket.
    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(expression);
        return entity;
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

    // Enumarable är en readonly-lista för att säkerställa att listan hålls intakt.

    // Sortering och filtrering hanteras i databasen vilket underlättar för applikationen
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false,
                                Expression<Func<TEntity, object>>? sortBy = null,
                                Expression<Func<TEntity, bool>>? filterBy = null,
                                params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        // Filter som möjliggör att vi hämtar alla som är av en viss status (t.ex COMPLETED)
        if (filterBy != null)
        {
            query = query.Where(filterBy);
        }

        // included inkluderar alla olika tabeller som ska vara med ( ex .Include(x. x.User)
        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // sortBy hanterar sorteringen av listan, ASC eller DESC och fält (ex OrderBy(x => x.Created))
        if (sortBy != null)
        {
            query = orderByDescending ? query.OrderByDescending(sortBy) : query.OrderBy(sortBy);
        }

        return await query.ToListAsync();
    }
}