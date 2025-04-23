using Domain.Entities.Common;

namespace Domain.Entities.Ledger;

public class JournalLine : BaseEntity
{
    public string JournalEntryId { get; set; }


    public long Debit { get; set; }
    public long Credit { get; set; }
    public string? Memo { get; set; }

    #region Relation

    public Account Account { get; set; } = null!;
    public JournalEntry JournalEntry { get; set; } = null!;
    public string? AccountId { get; set; }

    #endregion
}