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
        var servico = new Domain.Models.Servico(idPerfil, input.Nome, input.Descricao, input.Valor);

        await _servicoRepository.CriarServicoAsync(servico);
        await _servicoRepository.SalvarAsync();

        return Result.Ok(new CriarServicoResponse(
            servico.Id,
            servico.Nome,
            servico.Descricao,
            servico.Valor,
            servico.Inativo,
            servico.DataCriado));
    }
}
