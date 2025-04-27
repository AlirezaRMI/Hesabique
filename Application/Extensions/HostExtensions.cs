using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0) where TContext : DbContext
    {
        if (retry != null)
        {
            int retryForAvailability = retry.Value;

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TContext>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("شروع مهاجرت دیتابیس {ContextName}...", typeof(TContext).Name);
                context.Database.Migrate();
                logger.LogInformation("مهاجرت دیتابیس {ContextName} با موفقیت انجام شد.", typeof(TContext).Name);
            }
            catch (Exception e)
            {
                logger.LogError(e, "خطا در مهاجرت دیتابیس {ContextName}", typeof(TContext).Name);
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(1000);
                    MigrateDatabase<TContext>(host, retryForAvailability);
                }
            }
        }

        return host;
    }
}