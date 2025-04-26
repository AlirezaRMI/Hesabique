namespace Domain.ViewModel.Ledger;

public class AddJournalEntryViewModel
{
    public List<AddJournalLineViewModel> Lines { get; set; } = new();
    public string Refrence { get; set; }
    public DateTime CreateDate { get; set; }
}