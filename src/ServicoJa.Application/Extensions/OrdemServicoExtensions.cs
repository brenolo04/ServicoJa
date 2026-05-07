using ServicoJa.Application.UseCases.OrdemServico;
using ServicoJa.Application.UseCases.OrdemServico.Criar;
using ServicoJa.Application.UseCases.OrdemServico.ObterPorId;
using ServicoJa.Domain.Models;

namespace ServicoJa.Application.Extensions;

public static class OrdemServicoExtensions
{
    public static CriarOrdemServicoResponse ParaCriarOrdemServicoResponse(this OrdemServico ordemServico)
        => new
        (
            ordemServico.IdServico,
            ordemServico.IdPerfilPrestador,
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
            ordemServico.IdPerfilPrestador,
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

    public static ObterTodosOrdemServicosResponse ParaObterTodosOrdemServicosPrestadosResponse(this IEnumerable<OrdemServico> ordemServico, int totalRegistros)
    {
        var ordemServicosSaida = ordemServico.Select(os => 
            new OrdemServicosSaida
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

        return new ObterTodosOrdemServicosResponse(ordemServicosSaida, totalRegistros);
    }

    public static ObterTodosOrdemServicosResponse ParaObterTodosOrdemServicosSolicitadosResponse(this IEnumerable<OrdemServico> ordemServico, int totalRegistros)
    {
        var ordemServicosSaida = ordemServico.Select(os =>
            new OrdemServicosSaida
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

        return new ObterTodosOrdemServicosResponse(ordemServicosSaida, totalRegistros);
    }
}