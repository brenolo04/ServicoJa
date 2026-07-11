using FluentResults;
using ServicoJa.Domain.Interfaces.Services;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Endereco;

public class EnderecoOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;
    private readonly IEnderecoService _enderecoService;

    public EnderecoOrdemServicoHandler(IOrdemServicoRepository ordemServicoRepository, IEnderecoService enderecoService)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _enderecoService = enderecoService;
    }

    public async Task<Result<EnderecoOrdemServicoResponse>> ExecuteAsync(long idOrdemServico, long idPerfilRequest, EnderecoOrdemServicoRequest request)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if (ordemServico is null)
            return Result.Fail(new EntidadeVaziaError("Ordem de serviço", idOrdemServico));

        if (ordemServico.IdPerfilSolicitante != idPerfilRequest && !ordemServico.SolicitanteAnonimo)
            return Result.Fail(new DomainError("Não é o solicitante do serviço", idOrdemServico));

        var enderecoExterno = await _enderecoService.EnderecoPorCep(request.Cep);

        if (enderecoExterno is null)
            return Result.Fail(new DomainError("CEP inválido", idOrdemServico));

        var endereco = new Domain.ValueObjects.Endereco(enderecoExterno.Logradouro, enderecoExterno.Bairro, enderecoExterno.Localidade, request.Cep, request.Numero);
        var result = ordemServico.VincularEndereco(endereco);

        if(result.IsFailed)
            return Result.Fail(result.Errors);

        await _ordemServicoRepository.SalvarAsync();

        return Result.Ok(new EnderecoOrdemServicoResponse(
            ordemServico.Endereco.Cep,
            ordemServico.Endereco.Cidade,
            ordemServico.Endereco.Bairro,
            ordemServico.Endereco.Rua,
            ordemServico.Endereco.Numero
        ));
    }
}
