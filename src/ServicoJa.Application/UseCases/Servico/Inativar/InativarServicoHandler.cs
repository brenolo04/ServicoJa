using FluentResults;
using ServicoJa.Application.UseCases.Servico.ObterPorId;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;

namespace ServicoJa.Application.UseCases.Servico.Inativar;

public class InativarServicoHandler
{
    private readonly IServicoRepository _servicoRepository;
    public InativarServicoHandler(IServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }

    public async Task<Result<ObterServicoPorIdResponse>> ExecuteAsync(long idServico, long idPerfil)
    {
        var servico = await _servicoRepository.ObterServicoPorIdEPerfilAsync(idServico, idPerfil);

        if (servico is null)
            return Result.Fail(new EntidadeVaziaError("Serviço", idServico));
        
        servico.Inativar();        
        await _servicoRepository.SalvarAsync();

        return Result.Ok(new ObterServicoPorIdResponse(servico.Id, servico.Nome, servico.Descricao, servico.Valor, servico.Inativo, servico.DataCriado));
    }
}
