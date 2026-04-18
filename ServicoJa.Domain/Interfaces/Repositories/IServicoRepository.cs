using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IServicoRepository
{

    Task<Servico> CriarServico(Servico servico);
    Task<IEnumerable<Servico>> ObterTodos(long idPerfil);
    Task<Servico> ObterServicoPorId(long id, long idPerfil);
    Task<Servico> AtualizarServico(Servico servico);
    Task<Servico> ExcluirServico(long id);

}
