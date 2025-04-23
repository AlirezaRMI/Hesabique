using Domain.Entities.Common;

namespace Domain.Entities.Partner;

public class Partner : BaseEntity
{
        
        public string TenantId { get; set; }
     

        public required string FullName { get; set; }
        public long SharePercent { get; set; }

        #region Relation

        public ICollection<PartnerSettlement> Settlements { get; set; } = [];
        public Tenant.Tenant Tenant { get; set; } = null!;
        #endregion
        
}