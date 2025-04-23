using Application.Helpers;
using Domain.Enumes;
using Domain.ViewModel.Ledger;

namespace Application.Services.Interfaces;

public interface ILedgerService
{

    Task<IEnumerable<AccountViewModel>> GetAccountsAsync();

    Task<MineResult> AddAccountAsync(AddAccountViewModel addAccountViewModel);

    Task<MineResult> UpdateAccountAsync(EditAccountViewModel editAccountViewModel);

    Task<MineResult> DeleteAccountAsync(string accountId);

    Task<MineResult> PostJournalAsync(AddJournalEntryViewModel addJournalEntryViewModel);

    Task<PaginatedList<JournalEntryViewModel>> GetEntriesAsync(DateTime? from, DateTime? to,
        int page = 1, int size = 20);

    Task<decimal> GetAccountBalanceAsync(string accountId, DateTime? to = null);
}

