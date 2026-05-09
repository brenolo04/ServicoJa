using ServicoJa.Domain.Enums;

namespace ServicoJa.Application.UseCases.OrdemServico;

public record ObterTodosResponse(IEnumerable<OrdemServicoSaida> OrdemServicos, int TotalRegistros);

public record OrdemServicoSaida
(
    long IdOrdemServico,
    string NomeServico,
    string NomeSolicitante,
    string Cep,
    string Cidade,
    string Bairro,
    string Rua,
    string? Numero,
    DateTime DataMarcado,
    DateTime? DataFinalizado,
    DateTime DataCriacao,
    EStatusServico Status
 );