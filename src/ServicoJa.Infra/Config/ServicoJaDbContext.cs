using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServicoJa.Infra.Config;

public class ServicoJaDbContext(DbContextOptions<ServicoJaDbContext> opt)
    : IdentityDbContext<UsuarioIdentity, IdentityRole<long>, long>(opt)
{
    public DbSet<UsuarioIdentity> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ServicoJaDbContext).Assembly);
    }
}

public class UsuarioIdentity : IdentityUser<long> { }
