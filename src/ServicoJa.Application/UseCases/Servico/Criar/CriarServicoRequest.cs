namespace ServicoJa.Application.UseCases.Servico.Criar;

public record CriarServicoRequest(
    string Nome,
    string Descricao,
    decimal Valor
);