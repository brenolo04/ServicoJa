using FluentResults;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.SolicitanteAnonimo;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.SolicitanteAnonimo;

public class SolicitanteAnonimoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public SolicitanteAnonimoHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<Result<SolicitanteAnonimoResponse>> ExecuteAsync(long idOrdemServico, long idPerfilRequest, SolicitanteAnonimoRequest request)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return Result.Fail(new EntidadeVaziaError("Ordem de serviço", idOrdemServico));

        if (ordemServico.IdPerfilPrestador != idPerfilRequest)
            return Result.Fail(new DomainError("Não é o prestador do serviço", idOrdemServico));

        if (!ordemServico.SolicitanteAnonimo)
            return Result.Fail(new DomainError("Solicitante não é anônimo", idOrdemServico));

        var result = ordemServico.AtualizarSolicitanteAnonimo(request.Nome);

        if(result.IsFailed)
            return Result.Fail(result.Errors);

        await _ordemServicoRepository.SalvarAsync();

        return Result.Ok(new SolicitanteAnonimoResponse(ordemServico.NomeSolicitante!));
    }
}
