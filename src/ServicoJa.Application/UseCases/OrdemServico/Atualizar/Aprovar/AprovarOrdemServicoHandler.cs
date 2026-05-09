using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Aprovar;

public class AprovarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public AprovarOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository)
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

        ordemServico.AprovarOrdemServico();

        await _ordemServicoRepository.SalvarAsync();

        return ordemServico.ParaStatusResponse();
    }
}
