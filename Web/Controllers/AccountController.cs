using System.Security.Claims;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Ledger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
public class AccountController(IAccountService accountService, IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accounts = await accountService.GetTreeAsync();
        return View(accounts);
    }

    [HttpGet]
    public IActionResult CreateAccount()
    {
        return View(new AddAccountViewModel
        {
            TenantId = User.FindFirst("TenantId")?.Value
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAccount(AddAccountViewModel model)
    {
        var tenantId=User.FindFirst(ClaimTypes.Role)?.Value;
        model.TenantId = tenantId;
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "لطفا مقادیر را به صورت صحیح وارد کنید";
            return View(model);
        }

      
        var result = await accountService.AddAsync(model);
        if (result == OperationResult.Success)
        {
            TempData["SuccessMessage"] = "حساب با موفقیت ایجاد شد.";
            return RedirectToAction(nameof (Index));
        }

        if (result == OperationResult.ValidationError)
        {
            TempData["ErrorMessage"] = "کد حساب تکراری است.";
            return View(model);
        }

        TempData["ErrorMessage"] = "ایجاد حساب با خطا مواجه شد.";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditAccount(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var model = await accountService.GetByIdAsync(id);
        var editModel = mapper.Map<EditAccountViewModel>(model);
        return View(editModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAccount(EditAccountViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await accountService.UpdateAsync(model);
        if (result == OperationResult.Success)
        {
            TempData["SuccessMessage"] = "حساب با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = "ویرایش حساب با خطا مواجه شد.";
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var result = await accountService.DeleteAsync(id);

        if (result == OperationResult.Success)
            TempData["SuccessMessage"] = "حساب با موفقیت حذف شد.";
        else if (result == OperationResult.ValidationError)
            TempData["WarningMessage"] = "این حساب در حال استفاده است و قابل حذف نیست.";
        else
            TempData["ErrorMessage"] = "حذف حساب با خطا مواجه شد.";

        return RedirectToAction(nameof(Index));
    }
}