using Microsoft.EntityFrameworkCore;
using ServicoJa.Domain.Models;
using ServicoJa.Domain.Repositories;
using ServicoJa.Infra.Config;
using System;
using System.Collections.Generic;
namespace ServicoJa.Infra.Repositories;

public class OrdemServicoRepository : IOrdemServicoRepository
{
    private readonly ServicoJaDbContext _context;

    public OrdemServicoRepository(ServicoJaDbContext context)
    {
        _context = context;
    }

    public async Task CriarOrdemServicoAsync(OrdemServico ordemServico)
        => await _context.OrdemServicos.AddAsync(ordemServico);

    public async Task<OrdemServico?> ObterOrdemServicoPorIdAsync(long id)
        => await _context.OrdemServicos.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<OrdemServico>> ObterTodosOrdemServicosPrestadosAsync(long idPerfil, int paginaAtual, int tamanhoPagina)
        => await _context.OrdemServicos
            .AsNoTracking()
            .Take(tamanhoPagina)
            .Skip((paginaAtual - 1) * tamanhoPagina)
            .Where(os => os.IdPerfilPrestador == idPerfil)
            .Include(os => os.PerfilSolicitante)
            .Include(os => os.Servico)
            .ToListAsync();

    public async Task<IEnumerable<OrdemServico>> ObterTodosOrdemServicosSolicitadosAsync(long idPerfil, int paginaAtual, int tamanhoPagina)
        => await _context.OrdemServicos
            .AsNoTracking()
            .Take(tamanhoPagina)
            .Skip((paginaAtual - 1) * tamanhoPagina)
            .Where(os => os.IdPerfilSolicitante == idPerfil)
            .Include(os => os.PerfilPrestador)
            .Include(os => os.Servico)
            .ToListAsync();

    public async Task<int> TotalPaginasOrdemServicosPrestadosAsync(long idPerfil)
        => await _context.OrdemServicos
            .AsNoTracking()
            .Where(os => os.IdPerfilPrestador == idPerfil)
            .CountAsync();

    public async Task<int> TotalPaginasOrdemServicosSolicitadosAsync(long idPerfil)
        => await _context.OrdemServicos
            .AsNoTracking()
            .Where(os => os.IdPerfilSolicitante == idPerfil)
            .CountAsync();

    public async Task SalvarAsync()
        => await _context.SaveChangesAsync();
}
