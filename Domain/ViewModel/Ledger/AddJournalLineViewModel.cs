namespace Domain.ViewModel.Ledger;

public class AddJournalLineViewModel
{

    public string AccountId { get; set; } = null!;
    
    public long? Debit { get; set; }
    
    public long? Credit { get; set; }
    
    public string? Memo { get; set; }
}