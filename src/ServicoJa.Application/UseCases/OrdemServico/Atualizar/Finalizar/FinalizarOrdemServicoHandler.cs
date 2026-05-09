using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Finalizar;

public class FinalizarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public FinalizarOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<StatusResponse?> ExecuteAsync(long idOrdemServico, long idPerfilRequest)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return null;

        if (ordemServico.IdPerfilPrestador != idPerfilRequest)
            return null;

        ordemServico.FinalizarOrdemServico();

        await _ordemServicoRepository.SalvarAsync();

        return ordemServico.ParaStatusResponse();
    }
}
