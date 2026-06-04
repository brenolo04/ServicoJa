namespace ServicoJa.Application.UseCases.Servico.Atualizar;

public record AtualizarServicoResponse(
    long Id, 
    string Nome, 
    string Descricao,
    decimal Valor);
