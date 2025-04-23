using Data.Repository;
using Domain.IRepository;
using Microsoft.Extensions.DependencyInjection;
namespace Ioc;

public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        return services;
    }
}