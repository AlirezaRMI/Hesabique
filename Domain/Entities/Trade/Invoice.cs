using Domain.Entities.Common;
using Domain.Enumes.Trade;

namespace Domain.Entities.Trade;

public class Invoice : BaseEntity
{


    public InvoiceType Type { get; set; }
    public DateTime IssueDate { get; set; }

    public string CounterpartyId { get; set; }
    

    public long Total { get; set; }

    #region Relation
    public Counterparty Counterparty { get; set; } = null!;
    public ICollection<InvoiceLine> Lines { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public string TenantId { get; set; }
    public Tenant.Tenant Tenant { get; set; } = null!;

    #endregion
}