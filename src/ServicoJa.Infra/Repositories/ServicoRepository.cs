using Microsoft.EntityFrameworkCore;
using ServicoJa.Domain.Models;
using ServicoJa.Domain.Repositories;
using ServicoJa.Infra.Config;

namespace ServicoJa.Infra.Repositories;

public class ServicoRepository : IServicoRepository
{
    private readonly ServicoJaDbContext _context;

    public ServicoRepository(ServicoJaDbContext context) 
        => _context = context;

    public async Task CriarServicoAsync(Servico servico)
        => await _context.Servicos.AddAsync(servico);

    public async Task<Servico?> ObterServicoPorIdAsync(long id)
        => await _context.Servicos.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Servico>> ObterTodosAsync(long idPerfil)
        => await _context.Servicos
            .AsNoTracking()
            .Where(x => x.IdPerfil == idPerfil)
            .ToListAsync();

    public async Task SalvarAsync()
        => await _context.SaveChangesAsync();
}
