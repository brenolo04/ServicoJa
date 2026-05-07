using ServicoJa.Domain.Enums;

namespace ServicoJa.Application.UseCases.OrdemServico;

public record ObterTodosOrdemServicosResponse(IEnumerable<OrdemServicosSaida> OrdemServicos, int TotalRegistros);

public record OrdemServicosSaida
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