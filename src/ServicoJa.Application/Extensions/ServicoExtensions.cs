using ServicoJa.Application.UseCases.Servico.Criar;
using ServicoJa.Application.UseCases.Servico.ObterPorId;
using ServicoJa.Application.UseCases.Servico.ObterTodos;
using ServicoJa.Domain.Models;

namespace ServicoJa.Application.Extensions;

public static class ServicoExtensions
{
    public static ObterServicoPorIdResponse ParaObterServicoPorIdResponse(this Servico servico)
        => new(
            servico.Id,
            servico.Nome,
            servico.Descricao,
            servico.Valor,
            servico.Inativo,
            servico.DataCriado
        );

    public static ObterTodosServicosResponse ParaObterTodosServicosResponse(this IEnumerable<Servico> servicos, int totalRegistros)
    {
        var servicosSaida = servicos.Select(servico => new ServicosSaida(servico.Id, servico.Nome, servico.Descricao));

        return new ObterTodosServicosResponse(servicosSaida, totalRegistros);
    }
}
