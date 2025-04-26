using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Trade;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Invoice;
using Domain.IRepository;
using Domain.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class InvoiceService(
    IBaseRepository<Invoice> repository,
    IBaseRepository<InvoiceLine> lineRepository,
    IMapper mapper) : IInvoiceService
{
    public async Task<OperationResult> CreateAsync(AddInvoiceViewModel addInvoiceViewModel)
    {
        if (!addInvoiceViewModel.Lines.Any())
            return OperationResult.ValidationError;
        var invoice = mapper.Map<Invoice>(addInvoiceViewModel);
        invoice.Total = invoice.Lines.Sum(l => l.Quantity * l.UnitPrice);

        await repository.AddAsync(invoice);
        return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateAsync(EditInvoiceViewModel editInvoiceViewModel)
    {
        var invoice = await repository.GetQueryable()
            .Include(i => i.Lines)
            .SingleOrDefaultAsync(i => i.Id == editInvoiceViewModel.Id);

        if (invoice is null) return OperationResult.NotFound;

        mapper.Map(editInvoiceViewModel, invoice);
        invoice.Total = invoice.Lines.Sum(l => l.Quantity * l.UnitPrice);

        await repository.UpdateAsync(invoice);
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

        return invoice is null ? null : mapper.Map<InvoiceViewModel>(invoice);
    }

    public async Task<PaginatedList<InvoiceViewModel>> ListAsync(
        InvoiceFilterViewModel filter, int page = 1, int size = 20)
    {
        IQueryable<Invoice> query = repository.GetQueryable();

        if (filter.FromDate is not null)
            query = query.Where(i => i.IssueDate >= filter.FromDate);

        if (filter.ToDate is not null)
            query = query.Where(i => i.IssueDate <= filter.ToDate);

        if (filter.Type is not null)
            query = query.Where(i => i.Type == filter.Type);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(i => i.Id != null && i.Id.Contains(filter.Search));

        int total = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.IssueDate)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(i => mapper.Map<InvoiceViewModel>(i))
            .ToListAsync();

        return new PaginatedList<InvoiceViewModel>(items, total, page, size);
    }

    public async Task<OperationResult> AddLineAsync(string invoiceId, AddInvoiceLineViewModel addInvoiceLineViewModel)
    {
        var invoice = await repository.GetByIdAsync(invoiceId);
        if (invoice is null) return OperationResult.NotFound;

        var line = mapper.Map<InvoiceLine>(addInvoiceLineViewModel);
        line.InvoiceId = invoiceId;

        await lineRepository.AddAsync(line);

        invoice.Total += addInvoiceLineViewModel.Quantity * addInvoiceLineViewModel.UnitPrice;
        await repository.UpdateAsync(invoice);

        return OperationResult.Success;
    }

    public async Task<OperationResult> RemoveLineAsync(string invoiceLineId)
    {
        var line = await lineRepository.GetByIdAsync(invoiceLineId);
        if (line is null) return OperationResult.NotFound;

        var invoice = await repository.GetByIdAsync(line.InvoiceId);
        if (invoice is null) return OperationResult.Error;

        invoice.Total -= line.Quantity * line.UnitPrice;

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

        invoice.Total = invoice.Lines.Sum(l => l.Quantity * l.UnitPrice);
        await repository.UpdateAsync(invoice);

        return invoice.Total;
    }
}