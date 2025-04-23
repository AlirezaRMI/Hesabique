using Domain.Entities.Common;

namespace Domain.Entities.Tenant;

public class Tenant : BaseEntity
{
    public required string Name { get; set; }

    #region Relation

    public ICollection<User> Users { get; set; } = [];
    public ICollection<FeatureFlag> FeatureFlags { get; set; } = [];

    #endregion
}