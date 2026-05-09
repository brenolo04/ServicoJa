using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Executar;

public class ExecutarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ExecutarOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository)
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

        ordemServico.ExecutarOrdemServico();

        await _ordemServicoRepository.SalvarAsync();

        return ordemServico.ParaStatusResponse();
    }
}
