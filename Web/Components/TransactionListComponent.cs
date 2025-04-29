using System.Security.Claims;
using Application.Extensions;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Components;


public class Transaction(ITransactionService service, IHttpContextAccessor httpContext, IUserService userService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int page = 1)
    {
        if (page < 1)
            page = 1;
        var userId = httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        int pageSize = 5;

        int skip = (page - 1) * pageSize;
        var transactions = await service.GetUserTransactionsPagingAsync(userId, page, pageSize);
        var totalCount = await service.GetUserTransactionsCountAsync(userId);
        var balance = await userService.GetUserBalanceAsync(userId);

        if (transactions.Any())
            transactions.First().TotalPrice = balance;

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return View("TransactionListComponent", transactions);
    }
}
