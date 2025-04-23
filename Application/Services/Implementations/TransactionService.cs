using Application.Helpers;
using Application.Services.Interfaces;
using Data.Context;
using Domain.Entities;
using Domain.Enum.Transeation;
using Domain.IRepository;
using Domain.ViewModel.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class TransactionService(
    IBaseRepository<Transaction> repo,
    HesabiqueContext ctx) : ITransactionService
{
    public async Task<AddTransactionResult> CreateAsync(
        AddTransactionViewModel vm, string? userId)
    {
        if (userId is null) return AddTransactionResult.Error;

        var now = DateTime.Now;

        var trx = new Transaction
        {
            UserId = userId.ToString(),
            Description = vm.Description,
            Price = vm.Price,
            Type = vm.Type,
            Status = TransactionStatus.Pending,
            CreateDate = DateOnly.FromDateTime(now),
            CreatTime = TimeOnly.FromDateTime(now)
        };

        await repo.AddAsync(trx);
        return AddTransactionResult.Success;
    }

    public async Task<MineTransaction> UpdateAsync(EditeTransactionViewModel vm)
    {
        if (vm.Id is null) return MineTransaction.Unknown;

        var trx = await repo.GetByIdAsync(vm.Id);
        if (trx is null) return MineTransaction.Unknown;

        trx.Price = vm.Price;
        trx.Description = vm.Description;
        trx.Status = vm.Status;
        trx.Type = vm.Type;
        trx.CreatTime = vm.CreatTime;

        await repo.UpdateAsync(trx);
        return MineTransaction.Success;
    }

    public async Task<TransactionViewModel?> FindAsync(string id)
    {
        var trx = await repo.GetByIdAsync(id);
        return trx is null ? null : new TransactionViewModel(trx);
    }

    public async Task<PaginatedList<TransactionViewModel>> ListAsync(
        string userId, int page = 1, int pageSize = 20, TransactionStatus? status = null)
    {
        var query = repo.GetQueryable()
            .Where(t => t.UserId == userId.ToString());

        if (status is not null) query = query.Where(t => t.Status == status);

        var total = await query.CountAsync();

        var items = await query.OrderByDescending(t => t.CreateDate)
            .ThenByDescending(t => t.CreatTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TransactionViewModel(t))
            .ToListAsync();

        return new PaginatedList<TransactionViewModel>(items, total, page, pageSize);
    }

    public async Task<MineTransaction> ConfirmAsync(string id)
    {
        var trx = await repo.GetByIdAsync(id);
        if (trx is null) return MineTransaction.Unknown;

        trx.IsConfirmed = true;
        trx.Status = TransactionStatus.Confirmed;
        await repo.UpdateAsync(trx);

        return MineTransaction.Success;
    }

    public async Task<MineTransaction> DeleteAsync(string id)
    {
        var trx = await repo.GetByIdAsync(id);
        if (trx is null) return MineTransaction.Unknown;

        await repo.DeleteAsync(trx);
        return MineTransaction.Success;
    }
}