using Domain.Enumes.Ledger;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.ViewModel.Ledger;

public class AccountViewModel()
{
    public string AccountId { get; set; }
    public required string TenantId { get; set; }
    public required string AccountCode { get; set; }

    public string ParentId { get; set; }
    public required string Name { get; set; }

    public AccountType Type { get; set; }
}