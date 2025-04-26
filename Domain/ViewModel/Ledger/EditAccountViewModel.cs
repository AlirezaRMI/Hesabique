using Domain.Enumes.Ledger;

namespace Domain.ViewModel.Ledger;

public class EditAccountViewModel
{
    public required string AccountCode { get; set; }

    public required string Name { get; set; }

    public AccountType Type { get; set; }

    public string ParentId { get; set; }

    public string Id { get; set; }
}