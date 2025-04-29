using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Trade;
using Domain.Enumes.BaseEnum;
using Domain.Enumes.Payment;
using Domain.IRepository;
using Domain.ViewModel.Payment;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class PaymentService(
    IBaseRepository<Payment?> paymentRepository,
    IBaseRepository<BankAccount?> bankRepository,
    IMapper mapper) : IPaymentService
{
    public async Task<OperationResult> RegisterAsync(AddPaymentViewModel addPaymentViewModel)
    {
        var payment = mapper.Map<Payment>(addPaymentViewModel);
        await paymentRepository.AddAsync(payment);
        return OperationResult.Success;
    }

    public async Task<OperationResult> ConfirmAsync(string paymentId)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId);
        if (payment is null) return OperationResult.NotFound;

        payment.IsConfirm = true;
        payment.Status = PaymentStatus.Success;

        await paymentRepository.UpdateAsync(payment);
        return OperationResult.Success;
    }

    public async Task<OperationResult> CancelAsync(string paymentId)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId);
        if (payment is null) return OperationResult.NotFound;

        payment.IsConfirm = false;
        payment.Status = PaymentStatus.Canceled;

        await paymentRepository.UpdateAsync(payment);
        return OperationResult.Success;
    }

    public async Task<PaginatedList<PaymentViewModel>> ListByInvoiceAsync(string invoiceId, int page = 1, int size = 20)
    {
        var query = paymentRepository.GetQueryable()
            .Where(p => p.InvoiceId == invoiceId);

        int total = await query.CountAsync();

        var list = await query.OrderByDescending(p => p.Date)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(p => mapper.Map<PaymentViewModel>(p))
            .ToListAsync();

        return new PaginatedList<PaymentViewModel>(list, total, page, size);
    }

    public async Task<OperationResult> AddBankAccountAsync(AddBankAccountViewModel addBankAccountViewModel)
    {
        var entity = mapper.Map<BankAccount>(addBankAccountViewModel);
        await bankRepository.AddAsync(entity);
        return OperationResult.Success;
    }

    public async Task<IEnumerable<BankAccountViewModel>> GetBankAccountsAsync()
    {
        var list = await bankRepository.GetAllAsync();
        return mapper.Map<IEnumerable<BankAccountViewModel>>(list);
    }
}