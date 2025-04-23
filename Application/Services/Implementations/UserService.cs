using Application.Helpers;
using Application.Services.Interfaces;
using Data.Context;
using Domain.Entities;
using Domain.Enum.Transeation;
using Domain.Enum.User;
using Domain.IRepository;
using Domain.ViewModel.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public class UserService(
    IBaseRepository<User> userRepo,
    HesabiqueContext ctx,
    ILogger<UserService> logger) : IUserService
{
    public async Task AddUserAsync(AddUserViewModel vm)
    {
        await userRepo.AddAsync(vm.GenerateUser());
    }

    public async Task<EditeResult> EditUserAsync(EditeUserViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Id))
            return EditeResult.UserNotFound;

        var user = await userRepo.GetQueryable()
            .SingleOrDefaultAsync(u => u.Id == vm.Id);

        if (user is null) return EditeResult.UserNotFound;

        user.UserName    = vm.UserName;
        user.Address     = vm.Address;
        user.AccountCode = vm.AccountCode;

        await userRepo.UpdateAsync(user);
        return EditeResult.Success;
    }
    

    public async Task<long> GetBalanceAsync(string userId)
    {
        var inc = await ctx.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Increase)
            .SumAsync(t => t.Price);

        var dec = await ctx.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Decrease)
            .SumAsync(t => t.Price);

        return inc - dec;
    }
    
    public async Task<RegisterResult> RegisterAsync(RegisterViewModel vm)
    {
        if (await FindByUsernameAsync(vm.UserName) is not null)
            return RegisterResult.UserAlreadyExists;

        var user = new User
        {
            UserName    = vm.UserName,
            Address     = vm.Address,
            Password    = PasswordHash.EncodePasswordMd5(vm.Password),
            Status      = Status.Active,
            IsActive    = false,
            CreateDate  = DateOnly.FromDateTime(DateTime.Now),
            AccountCode = vm.AccountCode
        };

        await userRepo.AddAsync(user);
        return RegisterResult.Success;
    }

    public async Task<(LoginResult Result, UserViewModel? User)> LoginAsync(LoginUserViewModel vm)
    {
        try
        {
            var user = await userRepo.GetQueryable()
                .SingleOrDefaultAsync(u => u.UserName == vm.UserName);

            if (user is null ||
                user.Password != PasswordHash.EncodePasswordMd5(vm.Password))
                return (LoginResult.NotFound, null);

            if (user.Status != Status.Active)
                return (LoginResult.NotActive, null);

            return (LoginResult.Success, new UserViewModel(user));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Login error for {User}", vm.UserName);
            return (LoginResult.Error, null);
        }
    }

    /*────────────── عملیات خواندنی ──────────────*/

    public async Task<UserViewModel?> FindByUsernameAsync(string username)
    {
        var user = await userRepo.GetQueryable()
                                 .SingleOrDefaultAsync(u => u.UserName == username.Trim());
        return user is null ? null : new UserViewModel(user);
    }

    public async Task<UserViewModel?> FindByIdAsync(string userId)
    {
        var user = await userRepo.GetQueryable()
                                 .SingleOrDefaultAsync(u => u.Id == userId);
        return user is null ? null : new UserViewModel(user);
    }

    public async Task<IEnumerable<UserViewModel>> ListAsync(
        int page = 1, int pageSize = 20, string? search = null)
    {
        var query = userRepo.GetQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => u.UserName!.Contains(search));

        return await query.Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .Select(u => new UserViewModel(u))
                          .ToListAsync();
    }
}
