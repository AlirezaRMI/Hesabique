using Domain.Enumes.Trade;

namespace Domain.ViewModel.Payment;

public class AddPaymentViewModel
{
    public string Id { get; set; }

    public string ChequeId { get; set; } = null!;
    public long Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentMethod Method { get; set; }
    public string? Reference { get; set; }
    public string? UserId { get; set; }
}