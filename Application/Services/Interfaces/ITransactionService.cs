using Application.Helpers;
using Domain.Enum.Transeation;
using Domain.ViewModel.Transaction;

namespace Application.Services.Interfaces;

public interface ITransactionService
{
    Task<AddTransactionResult> CreateAsync(AddTransactionViewModel addTransactionViewModel, string? userId);

    Task<MineTransaction> UpdateAsync(EditeTransactionViewModel editeTransactionViewModel, string? userId);

    Task<TransactionViewModel?> FindAsync(string id);


    Task<PaginatedList<TransactionViewModel>> ListAsync(string userId, int page = 1, int pageSize = 20,
        TransactionStatus? status = null);

    Task<MineTransaction> ConfirmAsync(string id);

    Task<MineTransaction> DeleteAsync(string id);
    
    Task<int> GetUserTransactionsCountAsync(string userId);
    Task<List<TransactionViewModel>> GetUserTransactionsPagingAsync(string userId, int page, int pageSize);
}