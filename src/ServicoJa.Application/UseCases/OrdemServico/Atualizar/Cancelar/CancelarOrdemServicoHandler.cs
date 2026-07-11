using FluentResults;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Cancelar;

public class CancelarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public CancelarOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<Result<StatusResponse>> ExecuteAsync(long idOrdemServico, long idPerfilRequest)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return Result.Fail(new EntidadeVaziaError("Ordem de serviço", idOrdemServico));

        if (ordemServico.IdPerfilPrestador != idPerfilRequest && ordemServico.IdPerfilSolicitante != idPerfilRequest)
            return Result.Fail(new DomainError("Não está envolvido na ordem de serviço", idOrdemServico));

        ordemServico.CancelarOrdemServico();

        await _ordemServicoRepository.SalvarAsync();

        return Result.Ok(new StatusResponse(ordemServico.Status));
    }
}
