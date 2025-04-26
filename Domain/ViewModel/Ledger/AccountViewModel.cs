using Domain.Enumes.Ledger;

namespace Domain.ViewModel.Ledger;

public class AccountViewModel()
{
    public string AccountId { get; set; }
    
    public required string AccountCode { get; set; }

    public required string Name { get; set; }

    public AccountType Type { get; set; }
}