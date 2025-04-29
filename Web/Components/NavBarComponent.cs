using Microsoft.AspNetCore.Mvc;

namespace Web.Components;

public class NavBar : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        await Task.CompletedTask;
        
        return View("NavBarComponent");
    }
}