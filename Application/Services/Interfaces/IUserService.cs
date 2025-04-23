using Domain.Entities;
using Domain.Enum.User;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces;

/// <summary>
/// عملیات مربوط به کاربر (ثبت نام، ورود، مدیریت پروفایل)  
/// *تمام متدها <see langword="async"/> هستند و در صورت رخداد خطا
///  استثنای منحصربه‌فرد ServiceException پرتاب می‌کنند.*  
/// </summary>
public interface IUserService
{
    Task AddUserAsync(AddUserViewModel vm);


    Task<EditeResult> EditUserAsync(EditeUserViewModel vm);
    
    Task<long> GetBalanceAsync(string userId);
    
    Task<RegisterResult> RegisterAsync(RegisterViewModel vm);
    
    Task<(LoginResult Result, UserViewModel? User)> LoginAsync(LoginUserViewModel vm);
    
    Task<UserViewModel?> FindByUsernameAsync(string username);
    
    Task<UserViewModel?> FindByIdAsync(string userId);
    
    Task<IEnumerable<UserViewModel>> ListAsync(int page = 1, int pageSize = 20, string? search = null);
}