using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Cancelar;

public class CancelarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public CancelarOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<StatusResponse?> ExecuteAsync(long idOrdemServico, long idPerfilRequest)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return null;

        if (ordemServico.IdPerfilPrestador != idPerfilRequest && ordemServico.IdPerfilSolicitante != idPerfilRequest)
            return null;

        ordemServico.CancelarOrdemServico();

        await _ordemServicoRepository.SalvarAsync();

        return ordemServico.ParaStatusResponse();
    }
}
