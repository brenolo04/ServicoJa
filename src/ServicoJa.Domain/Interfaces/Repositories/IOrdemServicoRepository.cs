using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IOrdemServicoRepository
{
    Task CriarOrdemServicoAsync(OrdemServico ordemServico);
    Task<IEnumerable<OrdemServico>> ObterTodosServicosPrestadosAsync(long idPerfil);
    Task<IEnumerable<OrdemServico>> ObterTodosServicosSolicitadosAsync(long idPerfil);
    Task<OrdemServico?> ObterOrdemServicoPorIdAsync(long id);
    Task SalvarAsync();
}
