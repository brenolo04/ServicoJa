namespace ServicoJa.Application.UseCases.Servico.Criar;

public record CriarServicoResponse(
    long Id,
    string Nome,
    string Descricao,
    decimal Valor,
    bool Inativo,
    DateTime DataCriado
);
