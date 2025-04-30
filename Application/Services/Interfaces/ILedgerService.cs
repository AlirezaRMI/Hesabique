using Application.Helpers;
using Domain.Enumes;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Ledger;

namespace Application.Services.Interfaces;

public interface ILedgerService
{
    Task<IEnumerable<AccountViewModel>> GetAccountsAsync();

    Task<OperationResult> AddAccountAsync(AddAccountViewModel addAccountViewModel);

    Task<OperationResult> UpdateAccountAsync(EditAccountViewModel editAccountViewModel);

    Task<OperationResult> DeleteAccountAsync(string accountId);

    Task<OperationResult> PostJournalAsync(AddJournalEntryViewModel addJournalEntryViewModel,string tenantId);

    Task<PaginatedList<JournalEntryViewModel>> GetEntriesAsync(DateTime? from, DateTime? to,
        int page = 1, int size = 20);

    Task<List<JournalEntryViewModel>> ListAsync(string? tenantId);
    Task<long> GetAccountBalanceAsync(string accountId, DateTime? to = null);
}