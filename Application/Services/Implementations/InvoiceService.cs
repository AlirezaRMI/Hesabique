using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities.Trade;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel;
using Domain.ViewModel.Invoice;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class InvoiceService(
    IBaseRepository<Invoice> repository,
    IBaseRepository<InvoiceLine> lineRepository) : IInvoiceService
{
    public async Task<OperationResult> CreateAsync(AddInvoiceViewModel addInvoiceViewModel)
    {
        // if (addInvoiceViewModel.Lines is null || addInvoiceViewModel.Lines == "")
        //     return OperationResult.ValidationError;
        //
        // var invoice = new Invoice
        // {
        //     Id = Guid.NewGuid().ToString("N"),
        //     IssueDate = addInvoiceViewModel,
        //     Type = addInvoiceViewModel.,
        //     CounterpartyId = addInvoiceViewModel.,
        //     Total = addInvoiceViewModel.Lines.(l => l.Qty * l.UnitPrice),
        //     Lines = addInvoiceViewModel.Lines.(l => new InvoiceLine
        //     {
        //         Id = Guid.NewGuid().ToString("N"),
        //         Description = l.Description,
        //         Qty = l.Qty,
        //         UnitPrice = l.UnitPrice,
        //         VatRate = l.VatRate
        //     }).ToList()
        // };
        //
        // await invRepo.AddAsync(invoice);
         return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateAsync(EditInvoiceViewModel editInvoiceViewModel)
    {
        var invoice = await repository.GetQueryable()
            .Include(i => i.Lines)
            .SingleOrDefaultAsync(i => i.Id == editInvoiceViewModel.Id);

        // if (invoice is null) return OperationResult.NotFound;
        //
        // invoice.IssueDate = editInvoiceViewModel.IssueDate;
        // invoice.Type = editInvoiceViewModel.Type;
        // invoice.CounterpartyId = editInvoiceViewModel.CounterpartyId;
        //
        // invoice.Total = invoice.Lines.Sum(l => l.Qty * l.UnitPrice);
        //
        // await invRepo.UpdateAsync(invoice);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteAsync(string invoiceId)
    {
        var invoice = await repository.GetByIdAsync(invoiceId);
        if (invoice is null) return OperationResult.NotFound;

        await repository.DeleteAsync(invoice);
        return OperationResult.Success;
    }
    

    public async Task<InvoiceViewModel?> FindAsync(string invoiceId)
    {
        var invoice = await repository.GetQueryable()
            .Include(i => i.Lines)
            .SingleOrDefaultAsync(i => i.Id == invoiceId);

        // return invoice is null
        //     ? null
        //     : new InvoiceViewModel
        //     (
        //         Id: invoice.Id,
        //         Total: invoice.Total,
        //         Type: invoice.Type.ToString(),
        //         Date: invoice.IssueDate,
        //         Lines: invoice.Lines.Select(l => new InvoiceLineViewModel
        //         (
        //             Id: l.Id,
        //             Description: l.Description,
        //             Qty: l.Qty,
        //             UnitPrice: l.UnitPrice,
        //             VatRate: l.VatRate
        //         )).ToList()
        //     );
        return null;
    }

    public async Task<PaginatedList<InvoiceViewModel>> ListAsync(
        InvoiceFilterViewModel filter, int page = 1, int size = 20)
    {
        // var query = repository.GetQueryable();
        //
        // if (filter.FromDate is not null)
        //     query = query.Where(i => i.IssueDate >= filter.FromDate);
        //
        // if (filter.ToDate is not null)
        //     query = query.Where(i => i.IssueDate <= filter.ToDate);
        //
        // if (filter.Type is not null)
        //     query = query.Where(i => i.Type == filter.Type);
        //
        // if (!string.IsNullOrWhiteSpace(filter.Search))
        //     query = query.Where(i => i.Id.Contains(filter.Search));
        //
        // int total = await query.CountAsync();
        //
        // var items = await query
        //     .OrderByDescending(i => i.IssueDate)
        //     .Skip((page - 1) * size)
        //     .Take(size)
        //     .Select(i => new InvoiceViewModel
        //     (
        //         Id: i.Id,
        //         Total: i.Total,
        //         Type: i.Type.ToString(),
        //         Date: i.IssueDate,
        //         Lines: new List<InvoiceLineViewModel>() // برای خلاصه‌‌نمایش
        //     ))
        //     .ToListAsync();
        // return new PaginatedList<InvoiceViewModel>(items, total, page, size);
        return null;
    }
    

    public async Task<OperationResult> AddLineAsync(string invoiceId, AddInvoiceLineViewModel addInvoiceLineViewModel)
    {
        var invoice = await repository.GetByIdAsync(invoiceId);
        if (invoice is null) return OperationResult.NotFound;

        // var line = new InvoiceLine
        // {
        //     Id = Guid.NewGuid().ToString("N"),
        //     InvoiceId = invoiceId,
        //     Description = vm.Description,
        //     Qty = vm.Qty,
        //     UnitPrice = vm.UnitPrice,
        //     VatRate = vm.VatRate
        // };
        //
        // await lineRepository.AddAsync(line);
        //
        // invoice.Total += vm.Qty * vm.UnitPrice;
        // await invRepo.UpdateAsync(invoice);

        return OperationResult.Success;
    }

    public async Task<OperationResult> RemoveLineAsync(string invoiceLineId)
    {
        var line = await lineRepository.GetByIdAsync(invoiceLineId);
        if (line is null) return OperationResult.NotFound;

        var invoice = await repository.GetByIdAsync(line.InvoiceId!);
        if (invoice is null) return OperationResult.Error;

        invoice.Total -= line.Qty * line.UnitPrice;

        await lineRepository.DeleteAsync(line);
        await repository.UpdateAsync(invoice);

        return OperationResult.Success;
    }

    public async Task<long> RecalculateAsync(string invoiceId)
    {
        var invoice = await repository.GetQueryable()
                          .Include(i => i.Lines)
                          .SingleOrDefaultAsync(i => i.Id == invoiceId)
                      ?? throw new KeyNotFoundException("Invoice not found.");

        invoice.Total = invoice.Lines.Sum(l => l.Qty * l.UnitPrice);
        await repository.UpdateAsync(invoice);

        return invoice.Total;
    }
}