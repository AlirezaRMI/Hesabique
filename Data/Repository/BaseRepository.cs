using System.Linq.Expressions;
using Data.Context;
using Domain.Entities.Common;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class BaseRepository<T>(HesabiqueContext context) : IBaseRepository<T>
    where T : BaseEntity
{
    protected readonly DbSet<T> DbSet = context.Set<T>();
    
    public IQueryable<T> GetQueryable()
    {
        return DbSet.AsQueryable();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? order = null,
        Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = DbSet;
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        query = query.Where(predicate);
        if (order != null)
        {
            query = order(query);
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, IOrderedQueryable<T> order = null, string? includes = null)
    {
        IQueryable<T> query = DbSet;

        if (!string.IsNullOrWhiteSpace(includes))
        {
            foreach (var include in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include.Trim());
            }
        }
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (order != null)
        {
            return await order.ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, IOrderedQueryable<T> order)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public async Task <T?> GetByIdAsync(string id)
    {
       return await DbSet.FindAsync(id);
    }

    public async Task<T?> GetByIdAsync(string id, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = DbSet;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        var keyProperty = context.Model
            .FindEntityType(typeof(T))?
            .FindPrimaryKey()?
            .Properties
            .FirstOrDefault();

        if (keyProperty == null)
            throw new InvalidOperationException($"Primary key not found for entity {typeof(T).Name}");
        object convertedId;
        try
        {
            convertedId = Convert.ChangeType(id, keyProperty.ClrType);
        }
        catch
        {
            throw new ArgumentException($"Cannot convert id '{id}' to type {keyProperty.ClrType.Name}");
        }
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, keyProperty.Name);
        var equals = Expression.Equal(property, Expression.Constant(convertedId));
        var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        return await query.FirstOrDefaultAsync(lambda);
    }

    public async Task<T?> GetByIdAsync(string id, string? includes = null)
    {
        return await DbSet.Include(includes).SingleOrDefaultAsync(x=>x.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        if (entity is null)
            return false;

        var property = typeof(T).GetProperty("IsDeleted");
        if (property is not null && property.PropertyType == typeof(bool))
        {
            property.SetValue(entity, true);
            DbSet.Update(entity);
        }

        var result = await context.SaveChangesAsync();
        return result > 0;
    }
    
}