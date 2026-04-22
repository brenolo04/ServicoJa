using Microsoft.EntityFrameworkCore;
using ServicoJa.Domain.Models;
using ServicoJa.Domain.Repositories;
using ServicoJa.Infra.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<IEnumerable<OrdemServico>> ObterTodosServicosPrestadosAsync(long idPerfil)
        => await _context.OrdemServicos
            .AsNoTracking()
            .Where(x => x.IdPerfilPrestador == idPerfil)
            .ToListAsync();

    public async Task<IEnumerable<OrdemServico>> ObterTodosServicosSolicitadosAsync(long idPerfil)
        => await _context.OrdemServicos
            .AsNoTracking()
            .Where(x => x.IdPerfilSolicitante == idPerfil)
            .ToListAsync();

    public async Task SalvarAsync()
        => await _context.SaveChangesAsync();
}
