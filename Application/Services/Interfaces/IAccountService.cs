using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Ledger;

namespace Application.Services.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<AccountTreeNodeViewModel>> GetTreeAsync();

    Task<AccountViewModel> GetByIdAsync(string accountId);

    Task<long> GetBalanceAsync(string accountId, DateTime? to = null);

    Task<OperationResult> AddAsync(AddAccountViewModel vm);

    Task<OperationResult> UpdateAsync(EditAccountViewModel vm);

    Task<OperationResult> DeleteAsync(string accountId);
}