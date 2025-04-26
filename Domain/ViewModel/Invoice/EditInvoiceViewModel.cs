using Domain.Enumes.Trade;

namespace Domain.ViewModel.Invoice;

public class EditInvoiceViewModel
{
    public string Id { get; set; }
    public DateTime IssueDate { get; set; }
    public InvoiceType Type { get; set; }
    public string CounterpartyId { get; set; }
    
    public List<EditInvoiceLineViewModel> Lines { get; set; } = new();
}
