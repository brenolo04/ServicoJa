using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Interfaces.Services;
using ServicoJa.Domain.Repositories;

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

    public async Task<EnderecoOrdemServicoResponse?> ExecuteAsync(long idOrdemServico, long idPerfilRequest, EnderecoOrdemServicoRequest request)
    {
        var ordemServico = await _ordemServicoRepository.ObterOrdemServicoPorIdAsync(idOrdemServico);

        if(ordemServico is null || ordemServico.IdPerfilSolicitante != idPerfilRequest) return null;
        
        var enderecoExterno = await _enderecoService.EnderecoPorCep(request.Cep);

        if (enderecoExterno is null)
            return null;

        var endereco = new Domain.ValueObjects.Endereco(enderecoExterno.Logradouro, enderecoExterno.Bairro, enderecoExterno.Localidade, request.Cep, request.Numero);
        ordemServico.VincularEndereco(endereco);

        await _ordemServicoRepository.SalvarAsync();

        return ordemServico.ParaEnderecoOrdemServicoResponse();
    }
}
