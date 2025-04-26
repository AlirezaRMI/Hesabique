using Domain.Enumes.Trade;

namespace Domain.ViewModel.Payment;

public class PaymentViewModel
{
    public string? Id { get; set; }
    public required string InvoiceId { get; set; }
    public long Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime Date { get; set; }
    public string? Refrence { get; set; }
}
