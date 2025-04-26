namespace Domain.ViewModel.Invoice;

public class InvoiceLineViewModel
{
    public string? Id { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public long UnitPrice { get; set; }
    public long VatRate { get; set; }
}