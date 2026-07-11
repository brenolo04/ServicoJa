using FluentResults;
using FluentResults;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;
using ServicoJa.Application.UseCases.OrdemServico.ObterPorId;

namespace ServicoJa.Application.UseCases.OrdemServico.ObterPorId;

public class ObterOrdemServicoPorIdHandler 
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ObterOrdemServicoPorIdHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<Result<ObterOrdemServicoPorIdResponse>> ExecuteAsync(long idOrdemServico, long idPerfilRequest)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return Result.Fail(new EntidadeVaziaError("Ordem de serviço", idOrdemServico));

        if (ordemServico.IdPerfilPrestador != idPerfilRequest && ordemServico.IdPerfilSolicitante != idPerfilRequest)
            return Result.Fail(new EntidadeVaziaError("Ordem de serviço", idOrdemServico));

        return Result.Ok(new ObterOrdemServicoPorIdResponse(
            ordemServico.IdServico,
            ordemServico.PerfilPrestador.Nome,
            ordemServico.SolicitanteAnonimo ? ordemServico.NomeSolicitante : ordemServico.PerfilSolicitante.Nome,
            ordemServico.SolicitanteAnonimo,
            ordemServico.Endereco.Cep,
            ordemServico.Endereco.Cidade,
            ordemServico.Endereco.Bairro,
            ordemServico.Endereco.Rua,
            ordemServico.Endereco.Numero,
            ordemServico.DataMarcado,
            ordemServico.DataFinalizado,
            ordemServico.DataCriacao,
            ordemServico.Status
        ));
    }
}
