using System.Security.Claims;
using Application.Extensions;
using Application.Services.Interfaces;
using Domain.Enum.Transeation;
using Domain.ViewModel.Transaction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class TransactionController(ITransactionService transactionService) : BaseController
{
    [HttpGet,Authorize]
    public async Task<IActionResult> Index(int page = 1)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int pageSize = 5;

        var transactions = await transactionService.GetUserTransactionsPagingAsync(userId, page, pageSize);
        var totalCount = await transactionService.GetUserTransactionsCountAsync(userId);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        return View();
    }
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");   
    }
    [HttpPost,ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTransaction(AddTransactionViewModel model)
    {
        if (ModelState.IsValid)
        {
            AddTransactionResult result = await transactionService.CreateAsync(model,User.GetUserId());
            switch (result)
            {
                case AddTransactionResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                    return RedirectToAction("Index");
                case AddTransactionResult.Fail:
                    TempData[ErrorMessage] = "خطا در انجام عملیات";
                    return RedirectToAction("Index");
                case AddTransactionResult.Error:
                    TempData[ErrorMessage] = "خطای نا شناخته";
                    return RedirectToAction("Index");
            }
        }
        

        return View("Index");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmTransaction(string id)
    {
        var result = await transactionService.ConfirmAsync(id);
        switch (result)
        {
            case MineTransaction.Success:
                TempData["SuccessMessage"] = "تراکنش با موفقیت تایید شد";
                break;
            case MineTransaction.Fail:
                TempData["ErrorMessage"] = "خطا در تایید تراکنش";
                break;
            case MineTransaction.Unknown:
                TempData["WarningMessage"] = "تراکنش پیدا نشد";
                break;
            
        }  
    
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> DeleteTransaction(string id)
    {
        MineTransaction result = await transactionService.DeleteAsync(id);
        switch (result)
        {
            case MineTransaction.Success:
                TempData[SuccessMessage] = "حذف با موفقیت انجام شد";
                return RedirectToAction("Index");
            case MineTransaction.Fail:
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد";
                return RedirectToAction("Index");
            case MineTransaction.Unknown:
                TempData[WarningMessage] = "کاربری یافت نشد";
                return RedirectToAction("Index");
            
        }
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> EditTransaction(string id)
    {
 
        var transaction = await transactionService.FindAsync(id);
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (transaction == null)
        {
            TempData["ErrorMessage"] = "تراکنش پیدا نشد";
            return RedirectToAction("Index","Transaction");
        }

        if (transaction.UserId != userId)
        {
            TempData["ErrorMessage"] = "دسترسی غیر مجاز!";
            return RedirectToAction("Logout");
        }

        return View(transaction);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditTransaction(EditeTransactionViewModel transaction, string id)
    {
        var userId =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var transactionEntity = await transactionService.FindAsync(id);
        if (transactionEntity == null)
        {
            TempData[WarningMessage] = "تراکنش یافت نشد";
            return RedirectToAction("Index","Transaction");
        }

        if (transactionEntity.UserId != userId)
        {
            TempData[ErrorMessage] = "دسترسی غیر مجاز!";
            return RedirectToAction("Logout"); 
        }

        if (ModelState.IsValid && transaction.Id == id)
        {
            var result = await transactionService.UpdateAsync(transaction, userId);
            switch (result)
            {
                case MineTransaction.Success:
                    return RedirectToAction("Index");
                case MineTransaction.Fail:
                    TempData[ErrorMessage] = "خطا در انجام عملیات";
                    return RedirectToAction("Index");
                case MineTransaction.Unknown:
                    TempData[WarningMessage] = "تراکنش یافت نشد";
                    return RedirectToAction("Index");
            }
        }
     
        return View();
    }
}