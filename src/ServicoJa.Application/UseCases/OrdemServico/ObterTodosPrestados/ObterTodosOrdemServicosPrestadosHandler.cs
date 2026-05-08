using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.ObterTodosPrestados;

public class ObterTodosOrdemServicosPrestadosHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ObterTodosOrdemServicosPrestadosHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<ObterTodosOrdemServicosResponse?> ExecuteAsync(long idPerfilRequest, int paginaAtual, int tamanhoPagina)
    {
        var ordemServicos = await _ordemServicoRepository.ObterTodosOrdemServicosPrestadosAsync(idPerfilRequest, paginaAtual, tamanhoPagina);
        var totalRegistros = await _ordemServicoRepository.TotalPaginasOrdemServicosPrestadosAsync(idPerfilRequest);

        return ordemServicos.ParaObterTodosOrdemServicosPrestadosResponse(totalRegistros);
    }
}

