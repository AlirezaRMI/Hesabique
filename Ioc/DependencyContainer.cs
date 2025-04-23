using Application.Services.Implementations;
using Application.Services.Interfaces;
using Data.Repository;
using Domain.IRepository;
using Microsoft.Extensions.DependencyInjection;
namespace Ioc;

public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITransactionService, TransactionService>();
        return services;
    }
}