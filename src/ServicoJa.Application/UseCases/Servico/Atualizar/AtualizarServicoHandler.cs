using FluentResults;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;

namespace ServicoJa.Application.UseCases.Servico.Atualizar;

public class AtualizarServicoHandler
{
    private readonly IServicoRepository _servicoRepository;
    public AtualizarServicoHandler(IServicoRepository servicoRepository)
    {
        _servicoRepository = servicoRepository;
    }

    public async Task<Result<AtualizarServicoResponse>> ExecuteAsync(long idServico, long idPerfil, AtualizarServicoRequest request)
    {
        var servico = await _servicoRepository.ObterServicoPorIdAsync(idServico);

        if (servico is null)
            return Result.Fail(new EntidadeVaziaError("Serviço", idServico));

        if (servico.IdPerfil != idPerfil)
            return Result.Fail(new EntidadeVaziaError("Serviço", idServico));

        var result = servico.AtualizarServico(request.Nome, request.Descricao, request.Valor);

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        await _servicoRepository.SalvarAsync();

        return Result.Ok(new AtualizarServicoResponse(servico.Id, servico.Nome, servico.Descricao, servico.Valor));
    }
}
