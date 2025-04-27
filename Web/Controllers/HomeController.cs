using System.Diagnostics;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers;

// [Authorize] - Temporarily removed for debugging
public class HomeController(IUserService userService, ILogger<HomeController> logger) : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        logger.LogInformation("User {UserName} accessed Index page", User.Identity?.Name);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}