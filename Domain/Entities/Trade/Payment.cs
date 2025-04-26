using Domain.Entities.Common;
using Domain.Enumes.Payment;
using Domain.Enumes.Trade;

namespace Domain.Entities.Trade;

public class Payment : BaseEntity
{
    public required string InvoiceId  { get; set; }
    
    public Invoice Invoice { get; set; } = null!;

    public PaymentStatus Status { get; set; }
    public bool IsConfirm { get; set; }
    public PaymentMethod Method { get; set; }
    public long Amount   { get; set; }
    public DateTime Date    { get; set; }

    public string? Refrence { get; set; }

    public string? ChequeId   { get; set; }
    public Cheque? Cheque   { get; set; }  
}
