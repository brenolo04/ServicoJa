using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.OrdemServico.ObterPorId;

public class ObterOrdemServicoPorIdHandler 
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;

    public ObterOrdemServicoPorIdHandler(IOrdemServicoRepository ordemServicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
    }

    public async Task<ObterOrdemServicoPorIdResponse?> ExecuteAsync(long idOrdemServico, long idPerfilRequest)
    { 
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        return ordemServico switch
        {
            null => null,
            { IdPerfilPrestador: var p } when p == idPerfilRequest => ordemServico.ParaObterPorIdOrdemServicoResponse(),
            { IdPerfilSolicitante: var p } when p == idPerfilRequest => ordemServico.ParaObterPorIdOrdemServicoResponse(),
            _ => null
        };
    }
}
