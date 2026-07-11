using FluentResults;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;

namespace ServicoJa.Application.UseCases.Servico.ObterPorId;

public class ObterServicoPorIdHandler
{
    private readonly IServicoRepository _servicoRepository;

    public ObterServicoPorIdHandler(IServicoRepository repository)
    {
        _servicoRepository = repository;    
    }

    public async Task<Result<ObterServicoPorIdResponse?>> ExecuteAsync(long idServico, long idPerfil)
    {
        var servico = await _servicoRepository.ObterServicoPorIdEPerfilAsync(idServico, idPerfil);

        if (servico is null)
            return Result.Fail(new EntidadeVaziaError("Serviço", idServico));

        return Result.Ok(new ObterServicoPorIdResponse(
            servico.Id,
            servico.Nome,
            servico.Descricao,
            servico.Valor,
            servico.Inativo,
            servico.DataCriado
        ))!;
    }
}
