

namespace ServicoJa.Application.UseCases.OrdemServico.Atualizar.Endereco;

public record EnderecoOrdemServicoResponse
(
    string Cep,
    string Cidade,
    string Bairro,
    string Rua,
    string? Numero
);
