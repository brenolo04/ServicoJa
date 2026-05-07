using ServicoJa.Domain.Interfaces.Repositories;
using ServicoJa.Domain.Models;
using ServicoJa.Infra.Config;

namespace ServicoJa.Infra.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly ServicoJaDbContext _context;
    public PerfilRepository(ServicoJaDbContext context)
    {
        _context = context;
    }

    public async Task<Perfil?> ObterPerfilPorIdAsync(long idPerfil)
        => _context.Perfis.FirstOrDefault(x => x.Id == idPerfil);
}
