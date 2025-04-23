using Domain.Entities.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string? RoleTitle { get; set; }

    #region Relation

    public List<UserRoles>? UserRoles { get; set; }

    #endregion
}