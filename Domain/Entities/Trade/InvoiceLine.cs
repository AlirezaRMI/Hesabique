using Domain.Entities.Common;

namespace Domain.Entities.Trade;

public class InvoiceLine: BaseEntity
{
    public string InvoiceId { get; set; }


    public required string Description { get; set; }
    public long Qty { get; set; }
    public long UnitPrice { get; set; }
    public long VatRate { get; set; }

    #region Relation

    public Invoice Invoice { get; set; } = null!;

    #endregion
}