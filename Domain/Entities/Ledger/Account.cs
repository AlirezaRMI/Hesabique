using Domain.Entities.Common;
using Domain.Enumes.Ledger;

namespace Domain.Entities.Ledger;

public class Account : BaseEntity
{
    public string TenantId { get; set; }
   

    public required string AccountCode { get; set; }
    public required string Name { get; set; }
    public AccountType Type { get; set; }

    #region Relation

    public string? ParentId { get; set; }
    public Account? Parent { get; set; }
    public ICollection<Account?> Children { get; set; } = [];
    public Tenant.Tenant Tenant { get; set; } = null!;

    #endregion
}