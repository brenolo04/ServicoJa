using ServicoJa.Domain.Models;

namespace ServicoJa.Domain.Repositories;

public interface IServicoRepository
{
    Task CriarServicoAsync(Servico servico);
    Task<IEnumerable<Servico>> ObterTodosAsync(long idPerfil, int paginaAtual, int tamanhoPagina);
    Task<Servico?> ObterServicoPorIdAsync(long idServico, long idPerfil);
    Task<int> TotalPaginasAsync(long idPerfil);
    Task SalvarAsync();
}
