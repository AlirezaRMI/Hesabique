using Domain.Enumes.Trade;

namespace Domain.ViewModel.Invoice;

public class InvoiceViewModel
{
    public string? Id { get; set; }
    public DateTime IssueDate { get; set; }
    public InvoiceType Type { get; set; }
    public string CounterpartyName { get; set; }
    public long Total { get; set; }
    public List<InvoiceLineViewModel> Lines { get; set; } = new();
}