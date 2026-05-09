using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.SolicitanteAnonimo;

public class SolicitanteAnonimoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public SolicitanteAnonimoHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<SolicitanteAnonimoResponse?> ExecuteAsync(long idOrdemServico, long idPerfilRequest, SolicitanteAnonimoRequest request)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return null;

        if (ordemServico.IdPerfilPrestador != idPerfilRequest)
            return null;

        if (!ordemServico.SolicitanteAnonimo)
            return null;

        ordemServico.AtualizarSolicitanteAnonimo(request.Nome);

        await _ordemServicoRepository.SalvarAsync();

        return ordemServico.ParaSolicitanteAnonimoResponse();
    }
}
