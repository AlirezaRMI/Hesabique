using Domain.Enumes.Trade;

namespace Domain.ViewModel.Invoice;

public class InvoiceFilterViewModel
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public InvoiceType? Type { get; set; }
    public string? Search { get; set; }
}