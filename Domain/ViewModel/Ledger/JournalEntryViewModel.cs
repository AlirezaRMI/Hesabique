using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.ViewModel.Ledger;

public class JournalEntryViewModel
{
    public string? TenantId { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Reference { get; set; }

    public List<JournalLineViewModel> Lines { get; set; } = new();


}