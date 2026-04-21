using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IServicoRepository
{

    Task<Servico> CriarServico(Servico servico);
    Task<IEnumerable<Servico>> ObterTodos(long idPerfil);
    Task<Servico> ObterServicoPorId(long id);
    Task<Servico> AtualizarServico(Servico servico);

}
