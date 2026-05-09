using ServicoJa.Domain.Enums;

namespace ServicoJa.Application.UseCases.OrdemServico.ObterPorId;

public record ObterOrdemServicoPorIdResponse
(
    long IdServico,
    string NomePrestador,
    string? NomeSolicitante,
    bool SolicitanteAnonimo,
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
