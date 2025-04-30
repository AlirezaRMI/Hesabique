
namespace Domain.ViewModel.Ledger;

public class AddJournalEntryViewModel
{
    public string? TenantId { get; set; }
    public List<AddJournalLineViewModel> Lines { get; set; } = new();
    public string? Refrence { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
}
