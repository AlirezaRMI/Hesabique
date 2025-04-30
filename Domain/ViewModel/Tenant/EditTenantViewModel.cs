namespace Domain.ViewModel.Tenant;

public class EditTenantViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string Job { get; set; }
}