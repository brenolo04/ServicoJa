using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServicoJa.Infra.Config;

namespace ServicoJa.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddDbConfig(this IServiceCollection services, string connectionString)
    {

        services.AddDbContext<ServicoJaDbContext>(opt =>
            opt.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddDependencias(this IServiceCollection services)
    {
        return services;
    }
}
