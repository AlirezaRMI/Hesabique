using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
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
    IBaseRepository<JournalLine> lineRepository,
    IMapper mapper) : ILedgerService
{
    public async Task<IEnumerable<AccountViewModel>> GetAccountsAsync()
    {
        var accounts = await accountRepository.GetAllAsync();
        return mapper.Map<IEnumerable<AccountViewModel>>(accounts);
    }

    public async Task<OperationResult> AddAccountAsync(AddAccountViewModel addAccountViewModel)
    {
        bool duplicate = await accountRepository.GetQueryable()
            .AnyAsync(a => a.AccountCode == addAccountViewModel.AccountCode);
        if (duplicate) return OperationResult.ValidationError;

        var account = mapper.Map<Account>(addAccountViewModel);
        await accountRepository.AddAsync(account);
        return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateAccountAsync(EditAccountViewModel editAccountViewModel)
    {
        var account = await accountRepository.GetByIdAsync(editAccountViewModel.Id);
        if (account is null) return OperationResult.NotFound;

        if (editAccountViewModel.AccountCode != account.AccountCode)
        {
            bool dup = await accountRepository.GetQueryable()
                .AnyAsync(a => a.AccountCode == editAccountViewModel.AccountCode);
            if (dup) return OperationResult.ValidationError;
        }

        mapper.Map(editAccountViewModel, account);
        await accountRepository.UpdateAsync(account);
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

    public async Task<OperationResult> PostJournalAsync(AddJournalEntryViewModel model)
    {
        var linesInput = model.Lines.ToList();

        if (!linesInput.Any())
            return OperationResult.ValidationError;

        foreach (var line in linesInput)
        {
            if (line.Debit is null && line.Credit is null)
                return OperationResult.ValidationError;
        }

        var lines = mapper.Map<List<JournalLine>>(linesInput);

        var totalDebit = lines.Sum(l => l.Debit);
        var totalCredit = lines.Sum(l => l.Credit);

        if (totalDebit != totalCredit)
            return OperationResult.ValidationError;

        var entry = new JournalEntry
        {
            CreateDate = model.CreateDate,
            Reference = model.Refrence,
            Lines = lines
        };

        await journalEntryRepository.AddAsync(entry);
        return OperationResult.Success;
    }

    public async Task<PaginatedList<JournalEntryViewModel>> GetEntriesAsync(
        DateTime? from, DateTime? to, int page = 1, int size = 20)
    {
        IQueryable<JournalEntry> query = journalEntryRepository.GetQueryable().Include(e => e.Lines);

        if (from is not null)
            query = query.Where(e => e.CreateDate >= from.Value);

        if (to is not null)
            query = query.Where(e => e.CreateDate <= to.Value);

        int total = await query.CountAsync();

        var entries = await query
            .OrderByDescending(e => e.CreateDate)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        var viewModels = mapper.Map<List<JournalEntryViewModel>>(entries);

        return new PaginatedList<JournalEntryViewModel>(viewModels, total, page, size);
    }

    public async Task<long> GetAccountBalanceAsync(string accountId, DateTime? to = null)
    {
        var query = lineRepository.GetQueryable()
            .Where(l => l.AccountId == accountId);

        if (to is not null)
            query = query.Where(l => l.JournalEntry.CreateDate <= to.Value);

        long debit = await query.SumAsync(l => l.Debit);
        long credit = await query.SumAsync(l => l.Credit);

        return debit - credit;
    }
}