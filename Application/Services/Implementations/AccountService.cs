using Application.Services.Interfaces;
using Data.Context;
using Domain.Entities.Ledger;
using Domain.Enumes.BaseEnum;
using Domain.Enumes.Ledger;
using Domain.IRepository;
using Domain.ViewModel.Ledger;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class AccountService(
    IBaseRepository<Account> repository,
    IBaseRepository<JournalLine> lineRepo) : IAccountService
{
    public async Task<IEnumerable<AccountTreeNodeViewModel>> GetTreeAsync()
    {
        var accounts = await repository.GetAllAsync();

        var dict = accounts.ToDictionary(a => a.Id);

        foreach (var item in accounts)
        {
            if (item.ParentId is null) continue;
            if (dict.TryGetValue(item.ParentId, out var parent))
                parent.Children.Add(item);
        }

        return accounts.Where(a => a.ParentId is null)
            .Select(ToTreeNodeVm)
            .ToList();
    }

    public async Task<AccountViewModel> GetByIdAsync(string accountId)
    {
        var account = await repository.GetByIdAsync(accountId)
                      ?? throw new KeyNotFoundException("Account not found.");

        return ToViewModel(account);
    }

    public async Task<long> GetBalanceAsync(string accountId, DateTime? to = null)
    {
        var query = lineRepo.GetQueryable()
            .Where(l => l.AccountId == accountId);

        if (to is not null)
            query = query.Where(l => l.JournalEntry.Date <= to);

        long debit = await query.SumAsync(l => (long)l.Debit);
        long credit = await query.SumAsync(l => (long)l.Credit);

        return debit - credit;
    }


    public async Task<OperationResult> AddAsync(AddAccountViewModel addAccountViewModel)
    {
        bool codeExists = await repository.GetQueryable().AnyAsync(a => a.Code == addAccountViewModel.ToString());
        if (codeExists) return OperationResult.ValidationError;

        // var entity = new Account
        // {
        //     Id = Guid.NewGuid().ToString("N"),
        //     Code = addAccountViewModel.Code,
        //     Name = addAccountViewModel.Name,
        //     Type = addAccountViewModel.Type,
        //     ParentId = addAccountViewModel.ParentId
        // };

        // await repository.AddAsync();
        return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateAsync(EditAccountViewModel editAccountViewModel)
    {
        var account = await repository.GetByIdAsync(editAccountViewModel.ToString());
        if (account is null) return OperationResult.NotFound;

        if (editAccountViewModel.ToString() != account.Code)
        {
            bool duplicate = await repository.GetQueryable().AnyAsync(a => a.Code == editAccountViewModel.ToString());
            if (duplicate) return OperationResult.ValidationError;
        }

        account.Code = editAccountViewModel.ToString();
        account.Name = editAccountViewModel.ToString();
        account.ParentId = editAccountViewModel.ToString();
        account.Type = AccountType.Asset;

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


    private static AccountViewModel ToViewModel(Account a) =>
        new(a.Id, a.Code, a.Name, a.Type.ToString());

    private static AccountTreeNodeViewModel ToTreeNodeVm(Account a) =>
        new(
            a.Id,
            a.Code,
            a.Name,
            a.Type.ToString(),
            a.Children.Select(ToTreeNodeVm).ToList()
        );
}