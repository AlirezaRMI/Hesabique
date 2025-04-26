using Domain.Entities.Common;
using Domain.Entities.Ledger;
namespace Domain.Entities;

public class JournalEntry : BaseEntity
{
    public string TenantId { get; set; }
   

    public DateTime CreateDate { get; set; }
    public string? Reference { get; set; }

    #region Relation

    public ICollection<JournalLine> Lines { get; set; } = [];
    public Tenant.Tenant Tenant { get; set; } = null!;

    #endregion
}