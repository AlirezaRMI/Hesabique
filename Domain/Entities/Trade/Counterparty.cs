using Domain.Entities.Common;

namespace Domain.Entities.Trade;

public class Counterparty : BaseEntity
{


    public required string Name { get; set; }
    public string? Phone { get; set; }

    #region Relation

    public string TenantId { get; set; }
    public Tenant.Tenant Tenant { get; set; } = null!;

    #endregion
}