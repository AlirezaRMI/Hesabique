using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Tenant;

namespace Application.Services.Interfaces;

public interface ITenantService
{
    Task<OperationResult> AddTenantAsync(AddTenantViewModel model);
    Task<OperationResult> EditTenantAsync(EditTenantViewModel model);
    Task<OperationResult> DeleteTenantAsync(string id);
    Task<TenantViewModel?> GetTenantByIdAsync(string id);
    Task<List<TenantViewModel>> GetAllTenantsAsync();
}