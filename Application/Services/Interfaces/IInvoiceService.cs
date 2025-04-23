using Application.Helpers;
using Domain.Enumes;
using Domain.ViewModel;
using Domain.ViewModel.Invoice;

namespace Application.Services.Interfaces;

public interface IInvoiceService
{
    Task<MineResult> CreateAsync(AddInvoiceViewModel addInvoiceViewModel);
    Task<MineResult> UpdateAsync(EditInvoiceViewModel editInvoiceViewModel);
    Task<MineResult> DeleteAsync(string invoiceId);

    Task<InvoiceViewModel?> FindAsync(string invoiceId);

    Task<PaginatedList<InvoiceViewModel>> ListAsync(InvoiceFilterViewModel filter,
        int page = 1, int size = 20);


    Task<MineResult> AddLineAsync(string invoiceId, AddInvoiceLineViewModel addInvoiceLineViewModel);

    Task<MineResult> RemoveLineAsync(string invoiceLineId);
    Task<long> RecalculateAsync(string invoiceId);
}