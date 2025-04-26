using Domain.Entities;
using Domain.Enum.User;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces;

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