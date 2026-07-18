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
        
        var podeAtualizarEnderecoResult = ordemServico.PodeAtualizarEndereco(idPerfilRequest);

        if (podeAtualizarEnderecoResult.IsFailed)
            return Result.Fail(podeAtualizarEnderecoResult.Errors);

        var enderecoExterno = await _enderecoService.EnderecoPorCep(request.Cep);

        if (enderecoExterno is null)
            return Result.Fail(new DomainError("CEP inválido", idOrdemServico));

        var endereco = new Domain.ValueObjects.Endereco(enderecoExterno.Logradouro, enderecoExterno.Bairro, enderecoExterno.Localidade, request.Cep, request.Numero);
        var vincularEnderecoResult = ordemServico.VincularEndereco(endereco);

        if(vincularEnderecoResult.IsFailed)
            return Result.Fail(vincularEnderecoResult.Errors);

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
