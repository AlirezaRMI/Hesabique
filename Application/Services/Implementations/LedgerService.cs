using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Ledger;
using Domain.Enumes.BaseEnum;
using Domain.IRepository;
using Domain.ViewModel.Ledger;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class LedgerService(
    IBaseRepository<Account> accountRepository,
    IBaseRepository<JournalEntry> journalEntryRepository,
    IBaseRepository<JournalLine> lineRepository) : ILedgerService
{
    

    public async Task<IEnumerable<AccountViewModel>> GetAccountsAsync()
    {
        // var list = await accountRepository.GetAllAsync();
        // return list.Select(a => new AccountViewModel
        // (
        //     Id: a.Id,
        //     Code: a.Code,
        //     Name: a.Name,
        //     Type: a.Type.ToString()
        // ));
        return null;
    }

    public async Task<OperationResult> AddAccountAsync(AddAccountViewModel addAccountViewModel)
    {
        bool duplicate = await accountRepository.GetQueryable()
            .AnyAsync(a => a.Code == addAccountViewModel.ToString());
        if (duplicate) return OperationResult.ValidationError;

        // var account = new Account
        // {
        //     Id = Guid.NewGuid().ToString("N"),
        //     Code = addAccountViewModel.Code,
        //     Name = addAccountViewModel.Name,
        //     Type = addAccountViewModel.Type,
        //     ParentId = addAccountViewModel.ParentId
        // };

        // await accountRepository.AddAsync(account);
        return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateAccountAsync(EditAccountViewModel editAccountViewModel)
    {
        // var account = await accountRepository.GetByIdAsync(editAccountViewModel.Id);
        // if (account is null) return OperationResult.NotFound;
        //
        // if (editAccountViewModel.Code != account.Code)
        // {
        //     bool dup = await accountRepository.GetQueryable()
        //         .AnyAsync(a => a.Code == editAccountViewModel.Code);
        //     if (dup) return OperationResult.ValidationError;
        // }
        //
        // account.Code = editAccountViewModel.Code;
        // account.Name = editAccountViewModel.Name;
        // account.Type = editAccountViewModel.Type;
        // account.ParentId = editAccountViewModel.ParentId;
        //
        // await accountRepository.UpdateAsync(account);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteAccountAsync(string accountId)
    {
        var acc = await accountRepository.GetQueryable()
            .Include(a => a.Children)
            .SingleOrDefaultAsync(a => a.Id == accountId);
        if (acc is null) return OperationResult.NotFound;
        if (acc.Children.Any()) return OperationResult.ValidationError;

        bool used = await lineRepository.GetQueryable()
            .AnyAsync(l => l.AccountId == accountId);
        if (used) return OperationResult.ValidationError;

        await accountRepository.DeleteAsync(acc);
        return OperationResult.Success;
    }
    
    public async Task<OperationResult> PostJournalAsync(AddJournalEntryViewModel addJournalEntryViewModel)
    {
        // if (addJournalEntryViewModel.Lines is null || addJournalEntryViewModel.Lines.Count == 0)
        //     return OperationResult.ValidationError;
        //
        // var entry = new JournalEntry
        // {
        //     Id = Guid.NewGuid().ToString("N"),
        //     Date = addJournalEntryViewModel.Date,
        //     Reference = addJournalEntryViewModel.Reference,
        //     Lines = addJournalEntryViewModel.Lines.Select(l => new JournalLine
        //     {
        //         Id = Guid.NewGuid().ToString("N"),
        //         AccountId = l.AccountId,
        //         Debit = l.Debit,
        //         Credit = l.Credit,
        //         Memo = l.Memo
        //     }).ToList()
        // };
        //
        // await journalEntryRepository.AddAsync(entry);
        return OperationResult.Success;
    }

    public async Task<PaginatedList<JournalEntryViewModel>> GetEntriesAsync(
        DateTime? from, DateTime? to, int page = 1, int size = 20)
    {
        var query = journalEntryRepository.GetQueryable().Include(e => e.Lines);

        // if (from is not null) query = query.Where(e => e.Date >= from);
        // if (to is not null) query = query.Where(e => e.Date <= to);
        //
        // int total = await query.CountAsync();
        //
        // var list = await query.OrderByDescending(e => e.Date)
        //     .Skip((page - 1) * size)
        //     .Take(size)
        //     .Select(e => new JournalEntryViewModel
        //     (
        //         Id: e.Id,
        //         Date: e.Date,
        //         Ref: e.Reference,
        //         Lines: e.Lines.Select(l => new JournalLineViewModel
        //         (
        //             AccountId: l.AccountId,
        //             Debit: l.Debit,
        //             Credit: l.Credit,
        //             Memo: l.Memo
        //         )).ToList()
        //     ))
        //     .ToListAsync();
        //
        // return new PaginatedList<JournalEntryViewModel>(list, total, page, size);
        return null;
    }

    public async Task<long> GetAccountBalanceAsync(string accountId, DateTime? to = null)
    {
        var query = lineRepository.GetQueryable()
            .Where(l => l.AccountId == accountId);

        if (to is not null)
            query = query.Where(l => l.JournalEntry.Date <= to);

        long debit = await query.SumAsync(l => (long)l.Debit);
        long credit = await query.SumAsync(l => (long)l.Credit);

        return debit - credit;
    }
}