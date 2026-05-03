using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicoJa.Application.UseCases.Servico.Criar;
using ServicoJa.Application.UseCases.Servico.ObterPorId;
using ServicoJa.Application.UseCases.Servico.ObterTodos;
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
        services.AddScoped<CriarServicoHandler>();
        services.AddScoped<ObterServicoPorIdHandler>();
        services.AddScoped<ObterTodosServicosHandler>();

        services.AddScoped<IServicoRepository, ServicoRepository>();
        services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();

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

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira apenas o token JWT no campo abaixo. Exemplo: eyJhbGci... "
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
