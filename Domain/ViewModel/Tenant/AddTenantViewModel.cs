using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.ViewModel.Tenant;

public class AddTenantViewModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Job { get; set; }
    
}