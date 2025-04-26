using Domain.Entities.Common;

namespace Domain.Entities.Trade;

public class BankAccount : BaseEntity
{


    public required string? Name { get; set; }
    public required string AccountNumber { get; set; }
    public string? Iban { get; set; }

    #region Relation

    public string? TenantId { get; set; }
    public Tenant.Tenant Tenant { get; set; } = null!;
    public ICollection<Cheque>   Cheques   { get; set; } = []; 
    #endregion
}