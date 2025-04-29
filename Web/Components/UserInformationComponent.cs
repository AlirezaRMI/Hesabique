using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Web.Components
{
    public class UserInformation(IUserService userService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Content("اطلاعات کاربر یافت نشد.");
            }

            var user = await userService.FindByUsernameAsync(username);
            if (user == null)
            {
                return Content("کاربر موجود نیست."); 
            }

            await Task.CompletedTask;
            return View("UserInformationComponent",user);
        }
    }

}
