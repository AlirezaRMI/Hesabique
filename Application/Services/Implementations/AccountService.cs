using Application.Services.Interfaces;
using AutoMapper;
using Data.Context;
using Domain.Entities.Ledger;
using Domain.Enumes.BaseEnum;
using Domain.IRepository;
using Domain.ViewModel.Ledger;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class AccountService(
    IBaseRepository<Account?> repository,
    IBaseRepository<JournalLine?> lineRepo,
    IMapper mapper) : IAccountService
{
    public async Task<IEnumerable<AccountTreeNodeViewModel>> GetTreeAsync()
    {
        var accounts = await repository.GetAllAsync();

        var dictionary = new Dictionary<string, Account?>();
        foreach (var account in accounts)
            if (account.Id != null)
                dictionary.Add(account.Id, account);

        foreach (var item in accounts)
        {
            if (item.ParentId is null) continue;
            if (dictionary.TryGetValue(item.ParentId, out var parent))
                parent.Children.Add(item);
        }

        var roots = accounts.Where(a => a.ParentId is null).ToList();
        return mapper.Map<List<AccountTreeNodeViewModel>>(roots);
    }

    public async Task<AccountViewModel> GetByIdAsync(string accountId)
    {
        var account = await repository.GetByIdAsync(accountId)
                      ?? throw new KeyNotFoundException("حساب کاربری پیدا نشد");

        return mapper.Map<AccountViewModel>(account);
    }

    public async Task<long> GetBalanceAsync(string accountId, DateTime? to = null)
    {
        var query = lineRepo.GetQueryable()
            .Where(l => l.AccountId == accountId);

        if (to is not null)
            query = query.Where(l => l.JournalEntry.CreateDate <= to);

        long debit = await query.SumAsync(l => (long)l.Debit);
        long credit = await query.SumAsync(l => (long)l.Credit);

        return debit - credit;
    }

    public async Task<OperationResult> AddAsync(AddAccountViewModel addAccountViewModel)
    {
        bool codeExists =
            await repository.GetQueryable().AnyAsync(a => a.AccountCode == addAccountViewModel.AccountCode);
        if (codeExists) return OperationResult.ValidationError;

        var entity = mapper.Map<Account>(addAccountViewModel);
        await repository.AddAsync(entity);
        return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateAsync(EditAccountViewModel editAccountViewModel)
    {
        var account = await repository.GetByIdAsync(editAccountViewModel.Id);
        if (account is null) return OperationResult.NotFound;

        if (editAccountViewModel.AccountCode != account.AccountCode)
        {
            bool duplicate = await repository.GetQueryable()
                .AnyAsync(a => a.AccountCode == editAccountViewModel.AccountCode);
            if (duplicate) return OperationResult.ValidationError;
        }

        mapper.Map(editAccountViewModel, account);
        await repository.UpdateAsync(account);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteAsync(string accountId)
    {
        var account = await repository.GetQueryable()
            .Include(a => a.Children)
            .SingleOrDefaultAsync(a => a.Id == accountId);

        if (account is null) return OperationResult.NotFound;
        if (account.Children.Any()) return OperationResult.ValidationError;

        bool used = await lineRepo.GetQueryable()
            .AnyAsync(l => l.AccountId == accountId);
        if (used) return OperationResult.ValidationError;

        await repository.DeleteAsync(account);
        return OperationResult.Success;
    }
}