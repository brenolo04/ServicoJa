using FluentResults;
using FluentResults;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Executar;

public class ExecutarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ExecutarOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<Result<StatusResponse>> ExecuteAsync(long idOrdemServico, long idPerfilRequest)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return Result.Fail(new EntidadeVaziaError("Ordem de serviço", idOrdemServico));

        if (ordemServico.IdPerfilPrestador != idPerfilRequest)
            return Result.Fail(new DomainError("Não é o prestador do serviço", idOrdemServico));

        var result = ordemServico.ExecutarOrdemServico();

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        await _ordemServicoRepository.SalvarAsync();

        return Result.Ok(new StatusResponse(ordemServico.Status));
    }
}
