using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.ViewModel.Ledger;

public class JournalEntryViewModel
{
    public string? Id { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Reference { get; set; }

    public object Lines { get; set; }
    
    
}