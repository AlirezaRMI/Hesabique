using Application.Helpers;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Payment;

namespace Application.Services.Interfaces;

public interface IPaymentService
{
    Task<OperationResult> RegisterAsync(AddPaymentViewModel addPaymentViewModel);
    Task<OperationResult> ConfirmAsync(string paymentId);
    Task<OperationResult> CancelAsync(string paymentId);

    Task<PaginatedList<PaymentViewModel>> ListByInvoiceAsync(string invoiceId,
        int page = 1, int size = 20);
    Task<OperationResult> AddBankAccountAsync(AddBankAccountViewModel addBankAccountViewModel);
    Task<IEnumerable<BankAccountViewModel>> GetBankAccountsAsync();
}