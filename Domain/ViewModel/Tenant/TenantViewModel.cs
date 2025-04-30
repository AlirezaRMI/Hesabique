namespace Domain.ViewModel.Tenant;

public class TenantViewModel
{
    public string Id { get; set; } = null!;
    public string Job { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}