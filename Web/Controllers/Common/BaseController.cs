using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class BaseController : Controller
{
    protected readonly string SuccessMessage = "SuccessMessage";
    protected string InfoMessage = "InfoMessage";
    protected readonly string WarningMessage = "WarningMessage";
    protected readonly string ErrorMessage = "ErrorMessage";
}