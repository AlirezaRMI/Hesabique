using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities.Trade;
using Domain.Enumes.BaseEnum;
using Domain.IRepository;
using Domain.ViewModel.Payment;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations;

public class PaymentService(
    IBaseRepository<Payment> paymentRepository,
    IBaseRepository<BankAccount> bankRepository) : IPaymentService
{
    public async Task<OperationResult> RegisterAsync(AddPaymentViewModel addPaymentViewModel)
    {
        // var payment = new Payment
        // {
        //     Id = Guid.NewGuid().ToString("N"),
        //     InvoiceId = addPaymentViewModel.InvoiceId,
        //     Amount = addPaymentViewModel.Amount,
        //     Method = addPaymentViewModel.Method,
        //     Date = addPaymentViewModel.Date,
        //     ChequeId = addPaymentViewModel.ChequeId
        // };
        //
        // await paymentRepository.AddAsync(payment);
        return OperationResult.Success;
    }

    public async Task<OperationResult> ConfirmAsync(string paymentId)
    {
        var payment = await paymentRepository.GetByIdAsync(paymentId);
        if (payment is null) return OperationResult.NotFound;
        await paymentRepository.UpdateAsync(payment);
        return OperationResult.Success;
    }

    public async Task<OperationResult> CancelAsync(string paymentId)
    {
        // var payment = await paymentRepository.GetByIdAsync(paymentId);
        // if (payment is null) return OperationResult.NotFound;
        //
        // payment.IsConfirmed = false;
        // payment.Status = PaymentStatus.Canceled;
        // await paymentRepository.UpdateAsync(payment);
        return OperationResult.Success;
    }

    public async Task<PaginatedList<PaymentViewModel>> ListByInvoiceAsync(string invoiceId, int page = 1, int size = 20)
    {
        var query = paymentRepository.GetQueryable()
            .Where(p => p.InvoiceId == invoiceId);

        int total = await query.CountAsync();

        // var list = await query.OrderByDescending(p => p.Date)
        //     .Skip((page - 1) * size)
        //     .Take(size)
        //     .Select(p => new PaymentViewModel
        //     (
        //         Id: p.Id,
        //         InvoiceId: p.InvoiceId,
        //         Amount: p.Amount,
        //         Method: p.Method.ToString(),
        //         IsConfirmed: p.IsConfirmed,
        //         Date: p.Date,
        //         PaymentRef: p.PaymentRef
        //     )).ToListAsync();
        //
        // return new PaginatedList<PaymentViewModel>(list, total, page, size);
        return null;
    }

    public async Task<OperationResult> AddBankAccountAsync(AddBankAccountViewModel addBankAccountViewModel)
    {
        // var entity = new BankAccount
        // {
        //     Id = Guid.NewGuid().ToString("N"),
        //     Name = addBankAccountViewModel.Name,
        //     AccountNumber = addBankAccountViewModel.AccountNumber,
        //     Iban = addBankAccountViewModel.Iban,
        //     TenantId = addBankAccountViewModel.TenantId
        // };
        // await bankRepository.AddAsync(entity);
        return OperationResult.Success;
    }

    public async Task<IEnumerable<BankAccountViewModel>> GetBankAccountsAsync()
    {
        // var list = await bankRepository.GetAllAsync();
        // return list.Select(b => new BankAccountViewModel
        // (
        //     Id: b.Id,
        //     Name: b.Name,
        //     AccountNumber: b.AccountNumber,
        //     Iban: b.Iban
        // ));
        return null;
    }
}