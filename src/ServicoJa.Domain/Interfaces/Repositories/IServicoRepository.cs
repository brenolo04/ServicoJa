using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IServicoRepository
{

    Task CriarServicoAsync(Servico servico);
    Task<IEnumerable<Servico>?> ObterTodosAsync(long idPerfil);
    Task<Servico?> ObterServicoPorIdAsync(long id);
    Task SalvarAsync();

}
