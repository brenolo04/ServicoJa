using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IOrdemServicoRepository
{
    Task<OrdemServico> CriarOrdemServico(OrdemServico ordemServico);
    Task<IEnumerable<OrdemServico>> ObterTodosServicosPrestados(long idPerfil);
    Task<IEnumerable<OrdemServico>> ObterTodosServicosSolicitados(long idPerfil);
    Task<OrdemServico> ObterOrdemServicoPorId(long id);
    Task<OrdemServico> AtualizarOrdemServico(OrdemServico ordemServico);
}
