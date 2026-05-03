using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.Servico.ObterTodos;

public class ObterTodosServicosHandler
{
    private readonly IServicoRepository _servicoRepository;
    public ObterTodosServicosHandler(IServicoRepository repository)
    {
        _servicoRepository = repository;
    }

    public async Task<ObterTodosServicosResponse?> ExecuteAsync(long idPerfil, int paginaAtual, int tamanhoPagina)
    { 
        var servicos = await _servicoRepository.ObterTodosAsync(idPerfil, paginaAtual, tamanhoPagina);
        var totalRegistros = await _servicoRepository.TotalPaginasAsync(idPerfil);

        if (servicos is null)
            return null;

        return servicos.ParaObterTodosServicosResponse(totalRegistros);
    }
}
