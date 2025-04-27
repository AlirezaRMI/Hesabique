using Domain.Entities;
using Domain.Enum.User;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task AddUserAsync(AddUserViewModel addUserViewModel);
    Task<EditeResult> EditUserAsync(EditeUserViewModel editeUserViewModel);
    Task<long> GetBalanceAsync(string userId);
    Task<RegisterResult> RegisterAsync(RegisterViewModel registerViewModel);

    Task<(LoginResult Result, UserViewModel? User)> LoginAsync(LoginUserViewModel loginUserViewModel);

    Task<UserViewModel?> FindByUsernameAsync(string username);

    Task<UserViewModel?> FindByIdAsync(string userId);

    Task<IEnumerable<UserViewModel>> ListAsync(int page = 1, int pageSize = 20, string? search = null);
}