using Domain.Entities.Common;

namespace Domain.Entities.Ledger;

public class JournalLine : BaseEntity
{

    public long? Debit { get; set; }
    public long? Credit { get; set; }
    public string? Memo { get; set; }

    #region Relation

    public string JournalEntryId { get; set; }

    public Account Account { get; set; } = null!;
    public JournalEntry JournalEntry { get; set; } = null!;
    public string? AccountId { get; set; }

    #endregion
}