using System.Security.AccessControl;
using Domain.Enumes.Ledger;

namespace Domain.ViewModel.Ledger;

public class AddAccountViewModel
{

    public  string AccountCode { get; set; }
    
    public required string? TenantId { get; set; }
    public  string Name { get; set; }

    public AccountType Type { get; set; }

    public string? ParentId { get; set; }
    
}