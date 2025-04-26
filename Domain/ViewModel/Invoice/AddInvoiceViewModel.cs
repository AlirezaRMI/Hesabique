using Domain.Enumes.Trade;
using Domain.ViewModel.Invoice;

namespace Domain.ViewModel;

public class AddInvoiceViewModel
{
    public DateTime IssueDate { get; set; }
    public InvoiceType Type { get; set; }
    public string CounterpartyId { get; set; }
    public List<AddInvoiceLineViewModel> Lines { get; set; } = new();
}