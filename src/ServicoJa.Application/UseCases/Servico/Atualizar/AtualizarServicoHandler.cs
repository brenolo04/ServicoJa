using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.Servico.Atualizar;

public class AtualizarServicoHandler
{
    private readonly IServicoRepository _servicoRepository;
    public AtualizarServicoHandler(IServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }

    public async Task<AtualizarServicoResponse?> ExecuteAsync(long idServico, long idPerfil, AtualizarServicoRequest request)
    {
        
        var servico = await _servicoRepository.ObterServicoPorIdAsync(idServico);

        if (servico is null || servico.IdPerfil != idPerfil)
            return null;

        servico.AtualizarServico(request.Nome, request.Descricao, request.Valor);

        await _servicoRepository.SalvarAsync();

        return new(servico.Id, servico.Nome, servico.Descricao, servico.Valor);
    }
}
