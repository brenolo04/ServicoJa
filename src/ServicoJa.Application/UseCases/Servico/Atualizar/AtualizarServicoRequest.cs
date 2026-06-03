namespace ServicoJa.Application.UseCases.Servico.Atualizar;

public record AtualizarServicoRequest(
    string Nome, 
    string Descricao, 
    decimal Valor);
