using System.Security.Claims;
using Application.Services.Interfaces;
using Domain.Enumes.BaseEnum;
using Domain.ViewModel.Ledger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
public class JournalController(ILedgerService ledgerService) : Controller
{
    [HttpGet]
    public IActionResult CreateJournal()
    {
        var model = new AddJournalEntryViewModel
        {
            Lines = new List<AddJournalLineViewModel>
            {
                new(),
            }
        };
        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateJournal(AddJournalEntryViewModel model)
    {
        var tenantId=User.FindFirst(ClaimTypes.Role)?.Value;

        if (!ModelState.IsValid || !model.Lines.Any())
        {
            TempData["ErrorMessage"] = "لطفاً اطلاعات را به‌درستی وارد کنید.";
            return View(model);
        }

        model.TenantId = tenantId;
        if (tenantId != null)
        {
            model.TenantId = tenantId;

            var result = await ledgerService.PostJournalAsync(model, tenantId);

            if (result == OperationResult.Success)
            {
                TempData["SuccessMessage"] = "سند با موفقیت ثبت شد.";
                return RedirectToAction("Index");
            }
        }

        TempData["ErrorMessage"] = "ثبت سند با شکست مواجه شد!";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tenantId = User.FindFirst("TenantId")?.Value;
        var list = await ledgerService.ListAsync(tenantId);
        return View(list);
    }
}