using FluentResults;
using ServicoJa.Domain.Repositories;

namespace ServicoJa.Application.UseCases.Servico.Criar;

public class CriarServicoHandler
{
    private readonly IServicoRepository _servicoRepository;
    public CriarServicoHandler(IServicoRepository repository)
    {
        _servicoRepository = repository;
    }

    public async Task<Result<CriarServicoResponse>> ExecuteAsync(CriarServicoRequest input, long idPerfil)
    {
        var result = Domain.Models.Servico.Criar(idPerfil, input.Nome, input.Descricao, input.Valor);

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        await _servicoRepository.CriarServicoAsync(result.Value);
        await _servicoRepository.SalvarAsync();

        return Result.Ok(new CriarServicoResponse(
            result.Value.Id,
            result.Value.Nome,
            result.Value.Descricao,
            result.Value.Valor,
            result.Value.Inativo,
            result.Value.DataCriado));
    }
}
