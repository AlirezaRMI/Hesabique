using Domain.Entities.Common;
using Domain.Entities.Ledger;

namespace Domain.Entities.Tenant;

public class Tenant : BaseEntity
{
    public required string Name { get; set; }

    public required string Job { get; set; }
    
    public string? Description { get; set; }

    public ICollection<Account> Accounts { get; set; } = [];

    #region Relation

    public ICollection<User> Users { get; set; } = [];
    public ICollection<FeatureFlag> FeatureFlags { get; set; } = [];

    #endregion
}