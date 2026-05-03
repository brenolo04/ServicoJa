using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.Servico.ObterPorId;

public class ObterServicoPorIdHandler
{
    private readonly IServicoRepository _servicoRepository;

    public ObterServicoPorIdHandler(IServicoRepository repository)
    {
        _servicoRepository = repository;    
    }

    public async Task<ObterServicoPorIdResponse?> ExecuteAsync(long idServico, long idPerfil)
    {
        var servico = await _servicoRepository.ObterServicoPorIdAsync(idServico, idPerfil);

        if (servico is null)
            return null;

        return servico.ParaObterServicoPorIdResponse();
    }
}
