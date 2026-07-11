using FluentResults;
using FluentResults;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Aprovar;

public class AprovarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public AprovarOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository)
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

        var result = ordemServico.AprovarOrdemServico();

        if(result.IsFailed)
            return Result.Fail(result.Errors);

        await _ordemServicoRepository.SalvarAsync();

        return Result.Ok(new StatusResponse(ordemServico.Status));
    }
}
