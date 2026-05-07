using ServicoJa.Application.Extensions;
using ServicoJa.Domain.Interfaces.Repositories;
using ServicoJa.Domain.Interfaces.Services;
using ServicoJa.Domain.Repositories;

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

    public async Task<CriarOrdemServicoResponse?> ExecuteAsync(CriarOrdemServicoRequest request)
    {
        var servico = await _servicoRepository.ObterServicoPorIdAsync(request.IdServico);
        var perfil = await _perfilRepository.ObterPerfilPorIdAsync(request.IdPerfilSolicitante);

        if (servico is null || perfil is null) 
            return null;

        var ordemServico = string.IsNullOrEmpty(request.NomeSolicitante) 
            ? new Domain.Models.OrdemServico(servico.IdPerfil, request.IdPerfilSolicitante, request.IdServico, request.DataMarcado) 
            : new Domain.Models.OrdemServico(servico.IdPerfil, request.NomeSolicitante, request.IdServico, request.DataMarcado);

        var enderecoExterno = await _enderecoService.EnderecoPorCep(request.Cep);

        if (enderecoExterno is null)
            return null;

        var endereco = new Domain.ValueObjects.Endereco(enderecoExterno.Logradouro, enderecoExterno.Bairro, enderecoExterno.Localidade, request.Cep, request.Numero);

        ordemServico.VincularEndereco(endereco);

        await _ordemServicoRepository.CriarOrdemServicoAsync(ordemServico);
        await _ordemServicoRepository.SalvarAsync();

        return ordemServico.ParaCriarOrdemServicoResponse();
    }

}
