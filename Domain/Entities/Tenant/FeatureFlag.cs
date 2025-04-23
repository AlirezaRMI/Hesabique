using Domain.Entities.Common;

namespace Domain.Entities.Tenant;

public class FeatureFlag :BaseEntity
{
    public required string Key { get; set; }
    public bool Enabled { get; set; }

    #region Relation

    public Tenant Tenant { get; set; } = null!;
    public string TenantId { get; set; }
    #endregion
}