using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ServicoJa.Domain.Repositories;
using ServicoJa.Infra.Config;
using ServicoJa.Infra.Repositories;
using System.Text;

namespace ServicoJa.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ServicoJaDbContext>(opt =>
            opt.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddTransient<IServicoRepository, ServicoRepository>();
        services.AddTransient<IOrdemServicoRepository, OrdemServicoRepository>();

        return services;
    }

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<UsuarioIdentity>()
            .AddEntityFrameworkStores<ServicoJaDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters.ValidAudience = configuration["JwtConfiguration:Audience"];
            options.TokenValidationParameters.ValidIssuer = configuration["JwtConfiguration:Issuer"];
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfiguration:SecretKey"]!));
        });

        services.AddAuthorization();

        return services;
    }
}
