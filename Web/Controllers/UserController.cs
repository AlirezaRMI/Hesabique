using Application.Services.Interfaces;
using Domain.Enum.User;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class UserController(IUserService userService) : BaseController
{
 
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> EditeUser()
    {
      
        var user = await userService.FindByUsernameAsync(User.Identity.Name);

        if (user == null)
        {
            TempData["ErrorMessage"] = "کاربر یافت نشد.";
            return RedirectToAction("Index", "Home");
        }

        var model = new EditeUserViewModel(user);
        return View(model);
    }
    
    [HttpPost,ValidateAntiForgeryToken,Authorize]
    public async Task<IActionResult> EditeUser( [Bind("Id,UserName,Address,AccountCode")] EditeUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await userService.EditUserAsync(model);

            switch (result)
            {
                case EditeResult.UserNotFound:
                    TempData["ErrorMessage"] = "کاربری با این مشخصات یافت نشد.";
                    return RedirectToAction("Index", "Home");

                case EditeResult.Error:
                    TempData["ErrorMessage"] = "خطایی در هنگام ویرایش رخ داد.";
                    return View(model);

                case EditeResult.Success:
                    TempData["SuccessMessage"] = "ویرایش با موفقیت انجام شد.لطفا برای اعمال ویرایش مجدد با اطلاعات جدید وارد شوید";
                    return RedirectToAction("Logout", "Transaction"); 
             
            }
            return RedirectToAction("Index" , "Home");
    
        }

        TempData["ErrorMessage"] = "لطفاً اطلاعات را به درستی وارد کنید.";
        return View(model);
    }
}