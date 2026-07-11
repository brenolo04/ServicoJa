using FluentResults;
using FluentResults;
using ServicoJa.Domain.Interfaces.Repositories;
using ServicoJa.Domain.Interfaces.Services;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;
using ServicoJa.Application.UseCases.OrdemServico.Criar;
using ServicoJa.Domain.Interfaces.Services;
using ServicoJa.Domain.Repositories;
using ServicoJa.Domain.Results;

namespace ServicoJa.Application.UseCases.OrdemServico.Criar;

public class CriarOrdemServicoHandler
{
    private readonly IOrdemServicoRepository _ordemServicoRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IPerfilRepository _perfilRepository;
    private readonly IEnderecoService _enderecoService;

    public CriarOrdemServicoHandler
    (
        IOrdemServicoRepository ordemServicoRepository, 
        IServicoRepository servicoRepository,
        IPerfilRepository perfilRepository,
        IEnderecoService enderecoService
    )
    {
        _ordemServicoRepository = ordemServicoRepository;
        _servicoRepository = servicoRepository;
        _perfilRepository = perfilRepository;
        _enderecoService = enderecoService;
    }

    public async Task<Result<CriarOrdemServicoResponse>> ExecuteAsync(CriarOrdemServicoRequest request)
    {
        var servico = await _servicoRepository.ObterServicoPorIdAsync(request.IdServico);
        var perfil = await _perfilRepository.ObterPerfilPorIdAsync(request.IdPerfilSolicitante);

        if (servico is null && perfil is null && string.IsNullOrEmpty(request.NomeSolicitante))
            return Result.Fail(new EntidadeVaziaError("Serviço", request.IdServico));

        if (servico!.Inativo)
            return Result.Fail(new DomainError("Serviço inativo", request.IdServico));

        var ordemServico = string.IsNullOrEmpty(request.NomeSolicitante) 
            ? new Domain.Models.OrdemServico(servico!.IdPerfil, request.IdPerfilSolicitante, request.IdServico, request.DataMarcado) 
            : new Domain.Models.OrdemServico(servico!.IdPerfil, request.NomeSolicitante, request.IdServico, request.DataMarcado);

        var enderecoExterno = await _enderecoService.EnderecoPorCep(request.Cep);

        if (enderecoExterno is null)
            return Result.Fail(new DomainError("CEP inválido", 0));

        var endereco = new Domain.ValueObjects.Endereco(enderecoExterno.Logradouro, enderecoExterno.Bairro, enderecoExterno.Localidade, request.Cep, request.Numero);

        var result = ordemServico.VincularEndereco(endereco);

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        await _ordemServicoRepository.CriarOrdemServicoAsync(ordemServico);
        await _ordemServicoRepository.SalvarAsync();

        return Result.Ok(new CriarOrdemServicoResponse(
            ordemServico.Id,
            ordemServico.IdServico,
            ordemServico.IdPerfilSolicitante,
            ordemServico.NomeSolicitante,
            ordemServico.SolicitanteAnonimo,
            ordemServico.Endereco.Cep,
            ordemServico.Endereco.Cidade,
            ordemServico.Endereco.Bairro,
            ordemServico.Endereco.Rua,
            ordemServico.Endereco.Numero,
            ordemServico.DataMarcado,
            ordemServico.DataFinalizado,
            ordemServico.DataCriacao,
            ordemServico.Status
        ));
    }
}
