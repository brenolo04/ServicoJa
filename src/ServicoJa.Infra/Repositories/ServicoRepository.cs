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

    public async Task<Servico?> ObterServicoPorIdAsync(long idServico, long idPerfil)
        => await _context.Servicos.FirstOrDefaultAsync(x => x.Id == idServico && x.IdPerfil == idPerfil);

    public async Task<IEnumerable<Servico>> ObterTodosAsync(long idPerfil, int paginaAtual, int tamanhoPagina)
        => await _context.Servicos
            .AsNoTracking()
            .Take(tamanhoPagina)
            .Skip((paginaAtual - 1) * tamanhoPagina)
            .Where(x => x.IdPerfil == idPerfil)
            .ToListAsync();

    public async Task<int> TotalPaginasAsync(long idPerfil)
        => await _context.Servicos
            .AsNoTracking()
            .Where(servico => servico.IdPerfil == idPerfil)
            .CountAsync();

    public async Task SalvarAsync()
        => await _context.SaveChangesAsync();
}
