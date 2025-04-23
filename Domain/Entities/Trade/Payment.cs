using Domain.Entities.Common;
using Domain.Enumes.Trade;

namespace Domain.Entities.Trade;

public class Payment : BaseEntity
{
    public string InvoiceId  { get; set; }
    public Invoice Invoice { get; set; } = null!;

    public PaymentMethod Method { get; set; }
    public decimal Amount   { get; set; }
    public DateTime Date    { get; set; }

    public string? ChequeId   { get; set; }
    public Cheque? Cheque   { get; set; }  
}
