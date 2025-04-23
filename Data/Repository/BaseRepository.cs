using System.Linq.Expressions;
using Data.Context;
using Domain.Entities.Common;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class BaseRepository<T>(HesabiqueContext context,DbSet<T> dbSet) : IBaseRepository<T>
    where T : BaseEntity
{
    public IQueryable<T> GetQueryable()
    {
        return context.Set<T>().AsQueryable();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? order = null,
        Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = dbSet;
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
        IQueryable<T> query = dbSet;

        // اعمال include با استفاده از نام‌ها (رشته‌ای)
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
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task <T?> GetByIdAsync(string id)
    {
       return await context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetByIdAsync(string id, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = dbSet;

        // اعمال includeها
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // پیدا کردن کلید اصلی با EF Core metadata
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
        return await context.Set<T>().Include(includes).SingleOrDefaultAsync(x=>x.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
    
}