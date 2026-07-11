using FluentResults;
using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Errors;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.Servico.ObterTodos;

public class ObterTodosServicosHandler
{
    private readonly IServicoRepository _servicoRepository;
    public ObterTodosServicosHandler(IServicoRepository repository)
    {
        _servicoRepository = repository;
    }

    public async Task<Result<ObterTodosServicosResponse>> ExecuteAsync(long idPerfil, int paginaAtual, int tamanhoPagina)
    { 
        var servicos = await _servicoRepository.ObterTodosAsync(idPerfil, paginaAtual, tamanhoPagina);
        var totalRegistros = await _servicoRepository.TotalPaginasAsync(idPerfil);

        if (servicos.Count() == 0)
            return Result.Ok().WithReason(new ListaVaziaSuccess("Serviço"));

        var servicosSaida = servicos.Select(s => new ServicosSaida(s.Id,s.Nome,s.Descricao));

        return Result.Ok(new ObterTodosServicosResponse(servicosSaida, totalRegistros));
    }
}
