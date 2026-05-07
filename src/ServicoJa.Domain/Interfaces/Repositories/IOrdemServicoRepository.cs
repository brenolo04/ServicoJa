using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IOrdemServicoRepository
{
    Task CriarOrdemServicoAsync(OrdemServico ordemServico);
    Task<IEnumerable<OrdemServico>> ObterTodosOrdemServicosPrestadosAsync(long idPerfil, int paginaAtual, int tamanhoPagina);
    Task<IEnumerable<OrdemServico>> ObterTodosOrdemServicosSolicitadosAsync(long idPerfil, int paginaAtual, int tamanhoPagina);
    Task<OrdemServico?> ObterOrdemServicoPorIdAsync(long id);
    Task<int> TotalPaginasOrdemServicosPrestadosAsync(long idPerfil);
    Task<int> TotalPaginasOrdemServicosSolicitadosAsync(long idPerfil);
    Task SalvarAsync();
}
