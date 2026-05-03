using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.Servico.Criar;

public class CriarServicoHandler
{
    private readonly IServicoRepository _servicoRepository;
    public CriarServicoHandler(IServicoRepository repository)
    {
        _servicoRepository = repository;
    }

    public async Task<CriarServicoResponse> ExecuteAsync(CriarServicoRequest input, long idPerfil)
    {
        var servico = new Domain.Models.Servico(idPerfil, input.Nome, input.Descricao, input.Valor);

        await _servicoRepository.CriarServicoAsync(servico);
        await _servicoRepository.SalvarAsync();

        return servico.ParaCriarServicoResponse();      
    }
}
