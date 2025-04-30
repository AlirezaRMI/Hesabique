using Application.Services.Interfaces;
using AutoMapper;
using Data.Repository;
using Domain.Entities.Tenant;
using Domain.Enumes.BaseEnum;
using Domain.IRepository;
using Domain.ViewModel.Tenant;

namespace Application.Services.Implementations;

public class TenantService(
    IBaseRepository<Tenant> tenantRepository,
    IMapper mapper
) : ITenantService
{
    public async Task<OperationResult> AddTenantAsync(AddTenantViewModel model)
    {
        var tenant = mapper.Map<Tenant>(model);
        await tenantRepository.AddAsync(tenant);
        return OperationResult.Success;
    }

    public async Task<OperationResult> EditTenantAsync(EditTenantViewModel model)
    {
        var tenant = await tenantRepository.GetByIdAsync(model.Id);
        if (tenant == null) return OperationResult.NotFound;

        tenant = mapper.Map(model, tenant);
        await tenantRepository.UpdateAsync(tenant);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteTenantAsync(string id)
    {
        var tenant = await tenantRepository.GetByIdAsync(id);
        if (tenant == null) return OperationResult.NotFound;

        var result = await tenantRepository.DeleteAsync(tenant);
        return result ? OperationResult.Success : OperationResult.Error;
    }

    public async Task<TenantViewModel?> GetTenantByIdAsync(string id)
    {
        var tenant = await tenantRepository.GetByIdAsync(id);
        return tenant == null ? null : mapper.Map<TenantViewModel>(tenant);
    }

    public async Task<List<TenantViewModel>> GetAllTenantsAsync()
    {
        var tenants = await tenantRepository.GetAllAsync();
        return tenants.Select(t => mapper.Map<TenantViewModel>(t)).ToList();
    }
}