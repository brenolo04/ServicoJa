namespace ServicoJa.Application.UseCases.Servico.ObterPorId;

public record ObterServicoPorIdResponse(
    long Id,
    string Nome,
    string Descricao,
    decimal Valor,
    bool Inativo,
    DateTime DataCriado
);
