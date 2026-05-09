using ServicoJa.Application.UseCases.OrdemServico;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.Endereco;
using ServicoJa.Application.UseCases.OrdemServico.Atualizar.SolicitanteAnonimo;
using ServicoJa.Application.UseCases.OrdemServico.Criar;
using ServicoJa.Application.UseCases.OrdemServico.ObterPorId;
using ServicoJa.Domain.Models;

namespace ServicoJa.Application.Extensions;

public static class OrdemServicoExtensions
{
    public static CriarOrdemServicoResponse ParaCriarOrdemServicoResponse(this OrdemServico ordemServico)
        => new
        (
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
        );

    public static ObterOrdemServicoPorIdResponse ParaObterPorIdOrdemServicoResponse(this OrdemServico ordemServico)
        => new
        (
            ordemServico.IdServico,
            ordemServico.PerfilPrestador.Nome,
            ordemServico.SolicitanteAnonimo ? ordemServico.NomeSolicitante : ordemServico.PerfilSolicitante.Nome,
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
        );

    public static ObterTodosResponse ParaObterTodosOrdemServicosPrestadosResponse(this IEnumerable<OrdemServico> ordemServico, int totalRegistros)
    {
        var ordemServicosSaida = ordemServico.Select(os => 
            new OrdemServicoSaida
            (
                os.Id,
                os.Servico.Nome, 
                os.SolicitanteAnonimo ? os.NomeSolicitante! : os.PerfilSolicitante!.Nome,
                os.Endereco.Cep, 
                os.Endereco.Cidade, 
                os.Endereco.Bairro, 
                os.Endereco.Rua, 
                os.Endereco.Numero, 
                os.DataMarcado, 
                os.DataFinalizado, 
                os.DataCriacao, 
                os.Status
            )
        );

        return new ObterTodosResponse(ordemServicosSaida, totalRegistros);
    }

    public static ObterTodosResponse ParaObterTodosOrdemServicosSolicitadosResponse(this IEnumerable<OrdemServico> ordemServico, int totalRegistros)
    {
        var ordemServicosSaida = ordemServico.Select(os =>
            new OrdemServicoSaida
            (
                os.Id,
                os.Servico.Nome,
                os.SolicitanteAnonimo ? os.NomeSolicitante! : os.PerfilPrestador!.Nome,
                os.Endereco.Cep,
                os.Endereco.Cidade,
                os.Endereco.Bairro,
                os.Endereco.Rua,
                os.Endereco.Numero,
                os.DataMarcado,
                os.DataFinalizado,
                os.DataCriacao,
                os.Status
            )
        );

        return new ObterTodosResponse(ordemServicosSaida, totalRegistros);
    }

    public static StatusResponse ParaStatusResponse(this OrdemServico ordemServico)
        => new(ordemServico.Status);

    public static SolicitanteAnonimoResponse ParaSolicitanteAnonimoResponse(this OrdemServico ordemServico)
        => new(ordemServico.NomeSolicitante!);

    public static EnderecoOrdemServicoResponse ParaEnderecoOrdemServicoResponse(this OrdemServico ordemServico)
        => new(ordemServico.Endereco.Cep, ordemServico.Endereco.Cidade, ordemServico.Endereco.Bairro, ordemServico.Endereco.Rua, ordemServico.Endereco.Numero);
}