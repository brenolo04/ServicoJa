using FluentResults;
using FluentResults;
using ServicoJa.Domain.Errors;
using ServicoJa.Domain.Repositories;
using ServicoJa.Application.UseCases.OrdemServico;

namespace ServicoJa.Application.UseCases.OrdemServico.ObterTodosSolicitados;

public class ObterTodosOrdemServicosSolicitadosHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ObterTodosOrdemServicosSolicitadosHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<Result<ObterTodosResponse>> ExecuteAsync(long idPerfilRequest, int paginaAtual, int tamanhoPagina)
    {
        var ordemServicos = await _ordemServicoRepository.ObterTodosOrdemServicosSolicitadosAsync(idPerfilRequest, paginaAtual, tamanhoPagina);
        var totalRegistros = await _ordemServicoRepository.TotalPaginasOrdemServicosSolicitadosAsync(idPerfilRequest);

        if (ordemServicos.Count() == 0)
            return Result.Ok().WithReason(new ListaVaziaSuccess("Ordem de serviço"));

        return Result.Ok(new ObterTodosResponse(
            ordemServicos.Select(os => new OrdemServicoSaida(
                os.Id,
                os.Servico.Nome,
                os.SolicitanteAnonimo ? os.NomeSolicitante! : os.PerfilPrestador!.Nome,
                os.Endereco.Cep,
                os.Endereco.Cidade,
                os.Endereco.Bairro,
                os.Endereco.Rua,
                os.Endereco.Numero,
                os.DataMarcado,
                os.DataFinalizado,
                os.DataCriacao,
                os.Status
            )),
            totalRegistros
        ));
    }
}
