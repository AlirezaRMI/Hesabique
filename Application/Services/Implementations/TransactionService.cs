using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enum.Transeation;
using Domain.IRepository;
using Domain.ViewModel.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class TransactionService(
    IBaseRepository<Transaction?> repository,
    IMapper mapper) : ITransactionService
{
    public async Task<AddTransactionResult> CreateAsync(
        AddTransactionViewModel addTransactionViewModel, string? userId)
    {
        if (userId is null) return AddTransactionResult.Error;

        var now = DateTime.Now;

        var transaction = mapper.Map<Transaction>(addTransactionViewModel);
        transaction.UserId = userId;
        transaction.Status = TransactionStatus.Pending;
        transaction.CreateDate = DateOnly.FromDateTime(now);
        transaction.CreatTime = TimeOnly.FromDateTime(now);

        await repository.AddAsync(transaction);
        return AddTransactionResult.Success;
    }
    
    public async Task<MineTransaction> UpdateAsync(EditeTransactionViewModel editeTransactionViewModel, string? userId)
    {
        if (editeTransactionViewModel.Id is null) return MineTransaction.Unknown;

        var transaction = await repository.GetByIdAsync(editeTransactionViewModel.Id);
        if (transaction is null) return MineTransaction.Unknown;

        mapper.Map(editeTransactionViewModel, transaction);

        await repository.UpdateAsync(transaction);
        return MineTransaction.Success;
    }

    public async Task<TransactionViewModel?> FindAsync(string id)
    {
        var transaction = await repository.GetByIdAsync(id);
        if (transaction == null)
            return null;    
            
        
        return transaction is null ? null : mapper.Map<TransactionViewModel>(transaction);
    }

    public async Task<PaginatedList<TransactionViewModel>> ListAsync(
        string userId, int page = 1, int pageSize = 20, TransactionStatus? status = null)
    {
        var query = repository.GetQueryable()
            .Where(t => t.UserId == userId);

        if (status is not null) query = query.Where(t => t.Status == status);

        var total = await query.CountAsync();

        var items = await query.OrderByDescending(t => t.CreateDate)
            .ThenByDescending(t => t.CreatTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => mapper.Map<TransactionViewModel>(t))
            .ToListAsync();

        return new PaginatedList<TransactionViewModel>(items, total, page, pageSize);
    }

    public async Task<MineTransaction> ConfirmAsync(string id)
    {
        var transaction = await repository.GetByIdAsync(id);
        if (transaction is null) return MineTransaction.Unknown;

        transaction.IsConfirmed = true;
        transaction.Status = TransactionStatus.Confirmed;

        await repository.UpdateAsync(transaction);
        return MineTransaction.Success;
    }

    public async Task<MineTransaction> DeleteAsync(string id)
    {
        var transaction = await repository.GetByIdAsync(id);
        if (transaction is null) return MineTransaction.Unknown;

        await repository.DeleteAsync(transaction);
        return MineTransaction.Success;
    }

    public async Task<int> GetUserTransactionsCountAsync(string userId)
    {
        return await repository.GetUserTransactionsCountAsync(userId);
    }

    public async Task<List<TransactionViewModel>> GetUserTransactionsPagingAsync(string userId, int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;

        var transactions = await repository.GetUserTransactionsPagingAsync(userId, skip, pageSize);

        return transactions
            .Select(t => new TransactionViewModel(t))
            .ToList();
    }
}