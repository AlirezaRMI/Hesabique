using Application.Helpers;
using Domain.Enumes;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel;
using Domain.ViewModel.Invoice;

namespace Application.Services.Interfaces;

public interface IInvoiceService
{
    Task<OperationResult> CreateAsync(AddInvoiceViewModel addInvoiceViewModel);
    Task<OperationResult> UpdateAsync(EditInvoiceViewModel editInvoiceViewModel);
    Task<OperationResult> DeleteAsync(string invoiceId);

    Task<InvoiceViewModel?> FindAsync(string invoiceId);

    Task<PaginatedList<InvoiceViewModel>> ListAsync(InvoiceFilterViewModel filter,
        int page = 1, int size = 20);


    Task<OperationResult> AddLineAsync(string invoiceId, AddInvoiceLineViewModel addInvoiceLineViewModel);

    Task<OperationResult> RemoveLineAsync(string invoiceLineId);
    Task<long> RecalculateAsync(string invoiceId);
}