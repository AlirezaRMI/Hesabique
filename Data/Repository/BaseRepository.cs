using System.Linq.Expressions;
using Data.Context;
using Domain.Entities.Common;
using Domain.IRepository;
using Domain.ViewModel.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class BaseRepository<T>(AccountingContext context) : IBaseRepository<T>
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

    public Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, IOrderedQueryable<T> order = null, Expression<Func<T, object>>[]? includes = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, IOrderedQueryable<T> order = null, string? includes = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, IOrderedQueryable<T> order)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task <T?> GetByIdAsync(string id)
    {
       return await context.Set<T>().FindAsync(id);
    }

    public Task<T?> GetByIdAsync(string id, Expression<Func<T, object>>[]? includes = null)
    {
        throw new NotImplementedException();
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

    public async Task<List<T>> GetUserTransactionsPagingAsync(string userId, int skip, int take)
    {
        return await context.Set<T>()
            .Where(t=>EF.Property<string>(t, "UserId") == userId)
            .OrderByDescending(t => t.CreateDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetUserTransactionsCountAsync(string userId)
    {
        return await context.Set<T>()
       .CountAsync(t => EF.Property<string>(t, "UserId") == userId);
    }
}