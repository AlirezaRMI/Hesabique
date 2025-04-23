using Domain.Entities.Common;

namespace Domain.Entities.Partner;

public class PartnerSettlement : BaseEntity
{


    public DateTime PeriodEnd { get; set; }
    public long ProfitShare { get; set; }
    public long Withdrawn { get; set; }
    
    public long Balance => ProfitShare - Withdrawn;

    #region Relation

    public string PartnerId { get; set; }
    public Partner Partner { get; set; } = null!;

    #endregion
}