using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Data.Context;
using Domain.Entities;
using Domain.Enum.Transeation;
using Domain.Enum.User;
using Domain.IRepository;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public class UserService(
    IBaseRepository<User?> userRepository,
    HesabiqueContext context,
    ILogger<UserService> logger,
    IMapper mapper,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager) : IUserService
{
    public async Task AddUserAsync(AddUserViewModel addUserViewModel)
    {
        var user = mapper.Map<User>(addUserViewModel);
        await userRepository.AddAsync(user);
    }

    public async Task<EditeResult> EditUserAsync(EditeUserViewModel editeUserViewModel)
    {
        if (string.IsNullOrWhiteSpace(editeUserViewModel.Id))
            return EditeResult.UserNotFound;

        var user = await userRepository.GetQueryable()
            .SingleOrDefaultAsync(u => u.Id == editeUserViewModel.Id);

        if (user is null) return EditeResult.UserNotFound;

        mapper.Map(editeUserViewModel, user);

        await userRepository.UpdateAsync(user);
        return EditeResult.Success;
    }

    public async Task<long> GetBalanceAsync(string userId)
    {
        var inc = await context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Increase)
            .SumAsync(t => t.Price);

        var dec = await context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Decrease)
            .SumAsync(t => t.Price);

        return inc - dec;
    }

    [Obsolete("Obsolete")]
    public async Task<RegisterResult> RegisterAsync(RegisterViewModel registerViewModel)
    {
        if (await FindByUsernameAsync(registerViewModel.UserName) is not null)
            return RegisterResult.UserAlreadyExists;

        var user = mapper.Map<User>(registerViewModel);
        user.Password = PasswordHash.EncodePasswordMd5(registerViewModel.Password);
        user.Status = Status.Active;
        user.IsActive = true;
        user.Id = Guid.NewGuid().ToString();
        user.TenantId = user.Id;    
        user.CreateDate = DateOnly.FromDateTime(DateTime.Now);
        
        return RegisterResult.Success;
    }

    [Obsolete("Obsolete")]
    public async Task<(LoginResult, UserViewModel?)> LoginAsync(LoginUserViewModel loginUserViewModel)
    {
        try
        {
            var user = await userRepository.GetQueryable().SingleOrDefaultAsync(x => x.UserName == loginUserViewModel.UserName);
            if (user is null || user.Password != PasswordHash.EncodePasswordMd5(loginUserViewModel.Password))
                return (LoginResult.NotFound, null);

            if (user.Status != Status.Active)
                return (LoginResult.NotActive, null);

            return (LoginResult.Success, new(user));
        }
        catch (Exception e)
        {
            logger.LogError($"when user by user name {loginUserViewModel.UserName} occurred error :" + e.Message);
            return (LoginResult.Error, null);
        }
    }

    public async Task<UserViewModel?> FindByUsernameAsync(string username)
    {
        var user = await userRepository.GetQueryable()
            .SingleOrDefaultAsync(u => u.UserName == username.Trim());
        return user is null ? null : mapper.Map<UserViewModel>(user);
    }

    public async Task<UserViewModel?> FindByIdAsync(string userId)
    {
        var user = await userRepository.GetQueryable()
            .SingleOrDefaultAsync(u => u.Id == userId);
        return user is null ? null : mapper.Map<UserViewModel>(user);
    }

    public async Task<long> GetUserBalanceAsync(string id)
    {
        var increase = await context.Transactions
            .Where(t => t.UserId == id && t.Type == TransactionType.Increase)
            .SumAsync(t => t.Price);

        var decrease = await context.Transactions
            .Where(t => t.UserId == id && t.Type == TransactionType.Decrease)
            .SumAsync(t => t.Price);

        return increase - decrease;
    }

    public async Task<IEnumerable<UserViewModel>> ListAsync(
        int page = 1, int pageSize = 20, string? search = null)
    {
        var query = userRepository.GetQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => u.UserName!.Contains(search));

        var list = await query.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return mapper.Map<IEnumerable<UserViewModel>>(list);
    }
}