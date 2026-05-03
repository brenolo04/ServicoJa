using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServicoJa.Domain.Models;

namespace ServicoJa.Infra.Config;

public class ServicoJaDbContext(DbContextOptions<ServicoJaDbContext> opt)
    : IdentityDbContext<UsuarioIdentity, IdentityRole<long>, long>(opt)
{
    public DbSet<Perfil> Perfis { get; init; }
    public DbSet<Servico> Servicos { get; init; }
    public DbSet<OrdemServico> OrdemServicos { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ServicoJaDbContext).Assembly);
    }
}

public class UsuarioIdentity : IdentityUser<long> { }
