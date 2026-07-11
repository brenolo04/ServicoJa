using FluentResults;
using FluentResults;
using ServicoJa.Domain.Errors;
using ServicoJa.Domain.Repositories;
using ServicoJa.Application.UseCases.OrdemServico;

namespace ServicoJa.Application.UseCases.OrdemServico.ObterTodosPrestados;

public class ObterTodosOrdemServicosPrestadosHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ObterTodosOrdemServicosPrestadosHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<Result<ObterTodosResponse>> ExecuteAsync(long idPerfilRequest, int paginaAtual, int tamanhoPagina)
    {
        var ordemServicos = await _ordemServicoRepository.ObterTodosOrdemServicosPrestadosAsync(idPerfilRequest, paginaAtual, tamanhoPagina);
        var totalRegistros = await _ordemServicoRepository.TotalPaginasOrdemServicosPrestadosAsync(idPerfilRequest);

        if (ordemServicos.Count() == 0)
            return Result.Ok().WithReason(new ListaVaziaSuccess("Ordem de serviço"));

        return Result.Ok(new ObterTodosResponse(
            ordemServicos.Select(os => new OrdemServicoSaida(
                os.Id,
                os.Servico.Nome,
                os.SolicitanteAnonimo ? os.NomeSolicitante! : os.PerfilSolicitante!.Nome,
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

