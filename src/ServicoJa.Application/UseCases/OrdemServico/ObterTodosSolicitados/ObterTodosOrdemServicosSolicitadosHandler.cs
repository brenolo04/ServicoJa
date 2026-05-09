using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.ObterTodosSolicitados;

public class ObterTodosOrdemServicosSolicitadosHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ObterTodosOrdemServicosSolicitadosHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<ObterTodosResponse?> ExecuteAsync(long idPerfilRequest, int paginaAtual, int tamanhoPagina)
    {
        var ordemServicos = await _ordemServicoRepository.ObterTodosOrdemServicosSolicitadosAsync(idPerfilRequest, paginaAtual, tamanhoPagina);
        var totalRegistros = await _ordemServicoRepository.TotalPaginasOrdemServicosSolicitadosAsync(idPerfilRequest);

        return ordemServicos.ParaObterTodosOrdemServicosSolicitadosResponse(totalRegistros);
    }
}
