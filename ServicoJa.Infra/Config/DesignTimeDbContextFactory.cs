using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ServicoJa.Infra.Config;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ServicoJaDbContext>
{
    public ServicoJaDbContext CreateDbContext(string[] args)
    {
        var connectionString = "User ID=postgres;Password=master;Host=172.19.112.1;Port=5432;Database=ServicoJaDev;";

        var options = new DbContextOptionsBuilder<ServicoJaDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new ServicoJaDbContext(options);
    }
}
