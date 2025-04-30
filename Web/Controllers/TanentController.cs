using Application.Services.Interfaces;
using AutoMapper;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Tenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
public class TenantController(ITenantService tenantService,IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tenants = await tenantService.GetAllTenantsAsync();
        return View(tenants);
    }

    [HttpGet]
    public IActionResult CreateTenant()
    {
        return View(new AddTenantViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTenant(AddTenantViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await tenantService.AddTenantAsync(model);
        if (result == OperationResult.Success)
            return RedirectToAction("Index");

        TempData["ErrorMessage"] = "ایجاد مستاجر با خطا مواجه شد.";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditTenant(string id)
    {
        var tenant = await tenantService.GetTenantByIdAsync(id);
        var editModel =mapper.Map<EditTenantViewModel>(tenant);
        return View(editModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTenant(EditTenantViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await tenantService.EditTenantAsync(model);
        if (result == OperationResult.Success)
            return RedirectToAction("Index");

        TempData["ErrorMessage"] = "ویرایش مستاجر با خطا مواجه شد.";
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTenant(string id)
    {
        var result = await tenantService.DeleteTenantAsync(id);
        return RedirectToAction("Index");
    }
}