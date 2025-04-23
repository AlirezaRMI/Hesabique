using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class UserRoles
{
    public string UserId { get; set; }
    public string RoleId { get; set; }

    #region Relation

    public Role Role { get; set; }
    public User User { get; set; }
    #endregion
}
