using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IOrdemServicoRepository
{
    Task<OrdemServico> CriarOrdemServico(OrdemServico ordemServico);
    Task<IEnumerable<OrdemServico>> ObterTodosComoPrestador(long idPerfil);
    Task<IEnumerable<OrdemServico>> ObterTodosComoSolicitante(long idPerfil);
    Task<OrdemServico> ObterOrdemServicoPorId(long id, long idPerfil);
    Task<OrdemServico> AtualizarOrdemServico(OrdemServico ordemServico);
    Task<OrdemServico> ExcluirOrdemServico(long id);
}
